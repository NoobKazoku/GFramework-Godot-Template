using GFramework.Game.Abstractions.Config;
using GFramework.Game.Config;
using GFramework.Game.Config.Generated;
using GFrameworkGodotTemplate.scripts.config;

namespace GFramework_Godot_Template.Tests;

public sealed class TemplateConfigValidationTests
{
    private static readonly string RepoRoot = ResolveRepoRoot();

    [Theory]
    [InlineData("config/menu_text/en.yaml")]
    [InlineData("config/menu_text/zh-cn.yaml")]
    public void BundledMenuTextYaml_ValidatesAgainstCurrentSchema(string relativeYamlPath)
    {
        var schemaPath = Path.Combine(RepoRoot, "schemas", "menu_text.schema.json");
        var yamlPath = Path.Combine(RepoRoot, relativeYamlPath.Replace('/', Path.DirectorySeparatorChar));
        var yamlText = File.ReadAllText(yamlPath);

        YamlConfigTextValidator.Validate("menu_text", schemaPath, yamlPath, yamlText);
    }

    [Theory]
    [InlineData("config/common_text/en.yaml", "schemas/common_text.schema.json", "common_text")]
    [InlineData("config/common_text/zh-cn.yaml", "schemas/common_text.schema.json", "common_text")]
    [InlineData("config/runtime_profile/default.yaml", "schemas/runtime_profile.schema.json", "runtime_profile")]
    public void NewTemplateConfigTables_ValidateAgainstCurrentSchemas(
        string relativeYamlPath,
        string relativeSchemaPath,
        string tableName)
    {
        var schemaPath = Path.Combine(RepoRoot, relativeSchemaPath.Replace('/', Path.DirectorySeparatorChar));
        var yamlPath = Path.Combine(RepoRoot, relativeYamlPath.Replace('/', Path.DirectorySeparatorChar));
        var yamlText = File.ReadAllText(yamlPath);

        YamlConfigTextValidator.Validate(tableName, schemaPath, yamlPath, yamlText);
    }

    [Fact]
    public void ExtraYamlField_IsRejectedBySchemaValidation()
    {
        var schemaPath = Path.Combine(RepoRoot, "schemas", "menu_text.schema.json");
        var yamlPath = Path.Combine(RepoRoot, "config", "menu_text", "en.yaml");
        var yamlText = File.ReadAllText(yamlPath) + Environment.NewLine + "unexpectedField: boom" + Environment.NewLine;

        Assert.Throws<ConfigLoadException>(() =>
            YamlConfigTextValidator.Validate("menu_text", schemaPath, yamlPath, yamlText));
    }

    [Fact]
    public void InvalidSchemaFile_IsRejected()
    {
        var tempDirectory = Directory.CreateTempSubdirectory("template-config-schema-");
        try
        {
            var invalidSchemaPath = Path.Combine(tempDirectory.FullName, "menu_text.schema.json");
            var yamlPath = Path.Combine(RepoRoot, "config", "menu_text", "en.yaml");
            var yamlText = File.ReadAllText(yamlPath);
            File.WriteAllText(invalidSchemaPath, "{ invalid json");

            Assert.Throws<ConfigLoadException>(() =>
                YamlConfigTextValidator.Validate("menu_text", invalidSchemaPath, yamlPath, yamlText));
        }
        finally
        {
            tempDirectory.Delete(true);
        }
    }

    [Theory]
    [InlineData(
        """
        id: default
        defaultLanguageId: en
        uiTransitionDurationSeconds: 0
        notificationDurationSeconds: 2.5
        settingsPreviewDebounceMilliseconds: 120
        """)]
    [InlineData(
        """
        id: default
        defaultLanguageId: en
        uiTransitionDurationSeconds: 0.18
        notificationDurationSeconds: -1
        settingsPreviewDebounceMilliseconds: 120
        """)]
    [InlineData(
        """
        id: default
        defaultLanguageId: en
        uiTransitionDurationSeconds: 0.18
        notificationDurationSeconds: 2.5
        settingsPreviewDebounceMilliseconds: -1
        """)]
    public void RuntimeProfileSchema_RejectsNonPositiveTimingValues(string yamlText)
    {
        var schemaPath = Path.Combine(RepoRoot, "schemas", "runtime_profile.schema.json");
        var yamlPath = Path.Combine(RepoRoot, "config", "runtime_profile", "default.yaml");

        Assert.Throws<ConfigLoadException>(() =>
            YamlConfigTextValidator.Validate("runtime_profile", schemaPath, yamlPath, yamlText));
    }

    [Fact]
    public async Task CommonTextWriteback_PreservesComments_AndRemainsSchemaValid()
    {
        var registry = await LoadConfigRegistryAsync(RepoRoot, CommonTextConfigBindings.TableName);
        var config = registry.GetCommonTextTable().Get("en");
        var generatedYaml = CommonTextConfigBindings.SerializeToYaml(config);
        var existingYaml = string.Join("\n", new[]
        {
            "id: en",
            "# Common dialog verbs remain grouped for translators.",
            "confirm: \"Okay\"",
            "cancel: \"Abort\"",
            string.Empty
        });

        var mergedYaml = FlatYamlWritebackPatcher.MergePreservingComments(existingYaml, generatedYaml);
        var yamlPath = Path.Combine(RepoRoot, "config", "common_text", "en.yaml");

        CommonTextConfigBindings.ValidateYaml(RepoRoot, yamlPath, mergedYaml);

        Assert.Contains("# Common dialog verbs remain grouped for translators.", mergedYaml, StringComparison.Ordinal);
        Assert.DoesNotContain("confirm: \"Okay\"", mergedYaml, StringComparison.Ordinal);
        Assert.Contains("confirm:", mergedYaml, StringComparison.Ordinal);
        Assert.Contains("applySucceeded:", mergedYaml, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RuntimeProfileWriteback_RoundTripsWithoutLosingFields()
    {
        var registry = await LoadConfigRegistryAsync(RepoRoot, RuntimeProfileConfigBindings.TableName);
        var sourceConfig = registry.GetRuntimeProfileTable().Get("default");
        var sourceYaml = RuntimeProfileConfigBindings.SerializeToYaml(sourceConfig);
        var temporaryProjectRoot =
            Path.Combine(Path.GetTempPath(), $"gframework-template-config-roundtrip-{Guid.NewGuid():N}");
        var temporaryConfigDirectory = Path.Combine(temporaryProjectRoot, "config", "runtime_profile");
        var temporarySchemaDirectory = Path.Combine(temporaryProjectRoot, "schemas");
        Directory.CreateDirectory(temporaryConfigDirectory);
        Directory.CreateDirectory(temporarySchemaDirectory);

        try
        {
            var temporaryConfigPath = Path.Combine(temporaryConfigDirectory, "default.yaml");
            var temporarySchemaPath = Path.Combine(temporarySchemaDirectory, "runtime_profile.schema.json");
            await File.WriteAllTextAsync(temporaryConfigPath, sourceYaml);
            File.Copy(
                Path.Combine(RepoRoot, "schemas", "runtime_profile.schema.json"),
                temporarySchemaPath,
                overwrite: true);

            var roundTripRegistry =
                await LoadConfigRegistryAsync(temporaryProjectRoot, RuntimeProfileConfigBindings.TableName);
            var roundTripConfig = roundTripRegistry.GetRuntimeProfileTable().Get("default");
            var roundTripYaml = RuntimeProfileConfigBindings.SerializeToYaml(roundTripConfig);

            RuntimeProfileConfigBindings.ValidateYaml(
                RepoRoot,
                Path.Combine(RepoRoot, "config", "runtime_profile", "default.yaml"),
                roundTripYaml);

            Assert.Equal(sourceYaml, roundTripYaml);
        }
        finally
        {
            if (Directory.Exists(temporaryProjectRoot))
            {
                Directory.Delete(temporaryProjectRoot, true);
            }
        }
    }

    private static async Task<IConfigRegistry> LoadConfigRegistryAsync(string rootPath, params string[] includedTableNames)
    {
        var registry = new ConfigRegistry();
        var loader = new YamlConfigLoader(rootPath)
            .RegisterAllGeneratedConfigTables(
                new GeneratedConfigRegistrationOptions
                {
                    IncludedTableNames = includedTableNames,
                    CommonTextComparer = StringComparer.OrdinalIgnoreCase,
                    MenuTextComparer = StringComparer.OrdinalIgnoreCase,
                    RuntimeProfileComparer = StringComparer.OrdinalIgnoreCase
                });

        await loader.LoadAsync(registry);
        return registry;
    }

    private static string ResolveRepoRoot()
    {
        for (var current = new DirectoryInfo(AppContext.BaseDirectory); current is not null; current = current.Parent)
        {
            var configDirectory = Path.Combine(current.FullName, "config");
            var schemaDirectory = Path.Combine(current.FullName, "schemas");
            if (Directory.Exists(configDirectory) && Directory.Exists(schemaDirectory))
                return current.FullName;
        }

        throw new DirectoryNotFoundException(
            $"Failed to locate repository root from '{AppContext.BaseDirectory}'. Expected 'config' and 'schemas' directories.");
    }
}
