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
    [InlineData("..")]
    [InlineData("../en")]
    [InlineData("zh-cn/sub")]
    [InlineData("zh-cn\\sub")]
    [InlineData("C:/temp")]
    [InlineData("C:\\temp")]
    [InlineData("profile:default")]
    public void ValidateConfigIdentifier_RejectsPathTraversalAndSeparators(string input)
    {
        var exception = Assert.Throws<TargetInvocationException>(() =>
            InvokeValidateConfigIdentifier(input, "languageId"));

        Assert.IsType<ArgumentException>(exception.InnerException);
    }

    [Theory]
    [InlineData(" en ", "en")]
    [InlineData("zh-cn", "zh-cn")]
    [InlineData("default", "default")]
    public void ValidateConfigIdentifier_TrimsSafeIdentifiers(string input, string expected)
    {
        var normalizedValue = InvokeValidateConfigIdentifier(input, "profileId");

        Assert.Equal(expected, normalizedValue);
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

    private static string InvokeValidateConfigIdentifier(string value, string parameterName)
    {
        var method = typeof(TemplateContentPathResolver).GetMethod(
            "ValidateConfigIdentifier",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);
        return Assert.IsType<string>(method!.Invoke(null, new object?[] { value, parameterName }));
    }
}
