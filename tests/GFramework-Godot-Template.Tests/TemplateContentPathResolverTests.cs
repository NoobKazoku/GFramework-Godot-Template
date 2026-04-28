using System.Reflection;
using GFrameworkGodotTemplate.scripts.config;

namespace GFramework_Godot_Template.Tests;

public sealed class TemplateContentPathResolverTests
{
    [Theory]
    [InlineData("  config\\menu_text\\\\  ", "config/menu_text")]
    [InlineData("res://", "res://")]
    [InlineData("user://", "user://")]
    [InlineData("C:\\", "C:/")]
    public void NormalizePath_NormalizesSeparatorsAndWhitespace(string input, string expected)
    {
        var normalizedPath = InvokeNormalizePath(input);

        Assert.Equal(expected, normalizedPath);
    }

    [Fact]
    public void CombinePath_NormalizesRootAndRelativeSegments()
    {
        var combinedPath = InvokeCombinePath("res://config/", "\\menu_text\\en.yaml/");

        Assert.Equal("res://config/menu_text/en.yaml", combinedPath);
    }

    [Theory]
    [InlineData("res://config/", "res://config")]
    [InlineData("user://cache///", "user://cache")]
    [InlineData("/tmp/template/", "/tmp/template")]
    public void NormalizePath_TrimsTrailingSlashes_ForNonRootPaths(string input, string expected)
    {
        var normalizedPath = InvokeNormalizePath(input);

        Assert.Equal(expected, normalizedPath);
    }

    private static string InvokeNormalizePath(string path)
    {
        var method = typeof(TemplateContentPathResolver).GetMethod(
            "NormalizePath",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);
        return Assert.IsType<string>(method!.Invoke(null, new object?[] { path }));
    }

    private static string InvokeCombinePath(string rootPath, string relativePath)
    {
        var method = typeof(TemplateContentPathResolver).GetMethod(
            "CombinePath",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);
        return Assert.IsType<string>(method!.Invoke(null, new object?[] { rootPath, relativePath }));
    }
}
