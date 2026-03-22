using GFramework.Core.Abstractions.State;
using GFramework.Core.Coroutine.Instructions;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.enums.resources;
using GFrameworkGodotTemplate.scripts.utility;
using Godot;

namespace GFrameworkGodotTemplate.scripts.intro;

[ContextAware]
[Log]
public partial class Intro : Node2D, IController
{
    [GetNode] private AnimationPlayer _animationPlayer = null!;

    [GetNode] private Sprite2D _sprite = null!;

    private IGodotTextureRegistry _textureRegistry = null!;

    /// <summary>
    ///     节点准备就绪时的回调方法
    ///     在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        __InjectGetNodes_Generated();
        _textureRegistry = this.GetUtility<IGodotTextureRegistry>()!;
        // 延迟调用 Run 方法，确保在当前帧结束后执行
        CallDeferred(nameof(Run));
    }

    /// <summary>
    ///     执行动画播放逻辑的方法
    ///     启动协程以播放动画
    /// </summary>
    private void Run()
    {
        // 启动协程执行动画播放逻辑
        PlayAnimationCoroutine().RunCoroutine();
    }

    /// <summary>
    ///     播放一个交替淡入淡出的动画序列。
    ///     该协程会依次播放"fade_in"和"fade_out"动画，并在每次播放后等待3秒。
    /// </summary>
    private IEnumerator<IYieldInstruction> PlayAnimationCoroutine()
    {
        if (!GameConstants.Development.Equals(this.GetEnvironment<IEnvironment>()?.Name, StringComparison.Ordinal))
        {
            // 播放淡入动画
            _animationPlayer.Play("fade_in");
            yield return new Delay(3);

            // 播放淡出动画
            _animationPlayer.Play("fade_out");
            yield return new Delay(3);
            _sprite.Texture = _textureRegistry.Get(nameof(TextureKey.GodotStart)) as Texture2D;
            _sprite.Scale = new Vector2(1f, 1f);
            yield return new Delay(0.1);
            // 再次播放淡入动画
            _animationPlayer.Play("fade_in");
            yield return new Delay(3);

            // 最后播放淡出动画
            _animationPlayer.Play("fade_out");
            yield return new Delay(3);
        }

        // 切换到主菜单
        yield return this.GetSystem<IStateMachineSystem>()!
            .ChangeToAsync<MainMenuState>().AsCoroutineInstruction();
        yield return new Delay(0.1);
    }
}