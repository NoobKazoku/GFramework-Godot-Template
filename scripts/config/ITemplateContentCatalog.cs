namespace GFrameworkGodotTemplate.scripts.config;

public interface ITemplateContentCatalog : IUtility
{
    CommonTextConfig GetCommonText();

    MenuTextConfig GetMenuText();

    RuntimeProfileConfig GetRuntimeProfile();

    string GetCurrentLanguageId();

    void Reload();
}
