using GFramework.Core.Abstractions.controller;
using GFramework.Core.Abstractions.state;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.ui;
using GFrameworkGodotTemplate.scripts.cqrs.game.command;
using GFrameworkGodotTemplate.scripts.cqrs.menu.command;
using GFrameworkGodotTemplate.scripts.cqrs.pause_menu.command.input;
using GFrameworkGodotTemplate.scripts.enums.ui;
using Godot;

namespace GFrameworkGodotTemplate.scripts.pause_menu;

[ContextAware]
[Log]
public partial class PauseMenu : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    /// <summary>
    ///     页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;

    private IStateMachineSystem _stateMachineSystem = null!;

    /// <summary>
    ///     获取恢复游戏按钮节点
    /// </summary>
    private Button ResumeButton => GetNode<Button>("%ResumeButton");

    /// <summary>
    ///     获取保存游戏按钮节点
    /// </summary>
    private Button SaveButton => GetNode<Button>("%SaveButton");

    /// <summary>
    ///     获取加载游戏按钮节点
    /// </summary>
    private Button LoadButton => GetNode<Button>("%LoadButton");

    /// <summary>
    ///     获取选项按钮节点
    /// </summary>
    private Button OptionsButton => GetNode<Button>("%OptionsButton");

    /// <summary>
    ///     获取主菜单按钮节点
    /// </summary>
    private Button MainMenuButton => GetNode<Button>("%MainMenuButton");

    /// <summary>
    ///     获取退出游戏按钮节点
    /// </summary>
    private Button QuitButton => GetNode<Button>("%QuitButton");

    /// <summary>
    ///     Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.PauseMenu);


    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<Control>(this, UiKeyStr, UiLayer.Modal);
        return _page;
    }

    /// <summary>
    ///     节点就绪时调用的方法，用于初始化UI和设置事件处理器
    /// </summary>
    public override void _Ready()
    {
        SetupEventHandlers();
        _stateMachineSystem = this.GetSystem<IStateMachineSystem>()!;
    }

    public override void _Input(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel") || !Visible) return;

        this.SendCommand(new ResumeGameWithClosePauseMenuCommand(new ClosePauseMenuCommandInput
        {
            Handle = GetPage().Handle!.Value
        }));
        AcceptEvent();
    }

    /// <summary>
    ///     设置按钮点击事件处理器
    ///     为各个按钮绑定相应的命令发送逻辑
    /// </summary>
    private void SetupEventHandlers()
    {
        // 绑定恢复游戏按钮点击事件
        ResumeButton.Pressed += () =>
        {
            this.SendCommand(new ResumeGameWithClosePauseMenuCommand(new ClosePauseMenuCommandInput
            {
                Handle = GetPage().Handle!.Value
            }));
        };
        // 绑定保存游戏按钮点击事件
        SaveButton.Pressed += () =>
        {
            // 在此保存游戏
            this.SendCommand(new ResumeGameWithClosePauseMenuCommand(new ClosePauseMenuCommandInput
            {
                Handle = GetPage().Handle!.Value
            }));
            _log.Debug("保存游戏");
        };
        // 绑定加载游戏按钮点击事件
        LoadButton.Pressed += () => { _log.Debug("加载游戏"); };
        // 绑定选项按钮点击事件
        OptionsButton.Pressed += () => { this.RunCommandCoroutine(new OpenOptionsMenuCommand()); };

        // 绑定返回主菜单按钮点击事件
        MainMenuButton.Pressed += () =>
        {
            this.SendCommand(new ResumeGameWithClosePauseMenuCommand(new ClosePauseMenuCommandInput
            {
                Handle = GetPage().Handle!.Value
            }));
            _stateMachineSystem.ChangeToAsync<MainMenuState>().ToCoroutineEnumerator().RunCoroutine();
        };

        // 绑定退出游戏按钮点击事件
        QuitButton.Pressed += () => this.RunCommandCoroutine(new ExitGameCommand());
    }
}