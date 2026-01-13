using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.ui;
using Godot;

[ContextAware]
[Log]
public partial class TestMainMenu : Control, IController, IUiPage
{
    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
      
    }

    public void OnEnter(IUiPageEnterParam? param)
    {
        _log.Info("测试主菜单 OnEnter");
    }
}