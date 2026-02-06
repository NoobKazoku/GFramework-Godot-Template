using GFramework.Core.Abstractions.state;
using GFramework.Core.extensions;
using GFramework.Core.state;
using GFramework.Game.Abstractions.ui;
using GFrameworkGodotTemplate.scripts.tests;

namespace GFrameworkGodotTemplate.scripts.core.state.impls;

/// <summary>
/// 游戏进行中状态
/// </summary>
public class PlayingState : ContextAwareStateBase
{
    public override void OnEnter(IState? from)
    {
        this.GetSystem<IUiRouter>()!.Replace(HomeUi.UiKeyStr);
    }
}