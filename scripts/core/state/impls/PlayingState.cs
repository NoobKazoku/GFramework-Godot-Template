using GFramework.Core.Abstractions.state;
using GFramework.Core.extensions;
using GFramework.Core.state;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.tests;

namespace GFrameworkGodotTemplate.scripts.core.state.impls;

/// <summary>
///     游戏进行中状态
///     表示游戏当前处于运行阶段的状态管理类。
///     继承自ContextAwareStateBase，用于处理游戏运行时的逻辑。
/// </summary>
public class PlayingState : ContextAwareStateBase
{
    /// <summary>
    ///     进入当前状态时调用的方法。
    ///     替换当前UI为HomeUi界面。
    /// </summary>
    /// <param name="from">进入当前状态前的状态实例，可能为null。</param>
    public override void OnEnter(IState? from)
    {
        // 获取UI路由系统并替换当前UI为HomeUi
        this.GetSystem<IUiRouter>()!.Replace(HomeUi.UiKeyStr);
    }
}