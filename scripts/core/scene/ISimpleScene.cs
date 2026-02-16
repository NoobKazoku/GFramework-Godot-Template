using GFramework.Game.Abstractions.scene;

namespace GFrameworkGodotTemplate.scripts.core.scene;

/// <summary>
///     简单场景接口
///     继承自IScene接口，提供默认的空实现
///     适用于不需要复杂场景生命周期管理的简单场景
/// </summary>
public interface ISimpleScene : IScene
{
    /// <summary>
    ///     场景加载异步方法
    ///     提供空实现，返回已完成的任务
    /// </summary>
    /// <param name="param">场景进入参数，可为空</param>
    /// <returns>表示异步操作完成的ValueTask</returns>
    ValueTask IScene.OnLoadAsync(ISceneEnterParam? param)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     场景进入异步方法
    ///     提供空实现，返回已完成的任务
    /// </summary>
    /// <returns>表示异步操作完成的ValueTask</returns>
    ValueTask IScene.OnEnterAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     场景暂停异步方法
    ///     提供空实现，返回已完成的任务
    /// </summary>
    /// <returns>表示异步操作完成的ValueTask</returns>
    ValueTask IScene.OnPauseAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     场景恢复异步方法
    ///     提供空实现，返回已完成的任务
    /// </summary>
    /// <returns>表示异步操作完成的ValueTask</returns>
    ValueTask IScene.OnResumeAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     场景退出异步方法
    ///     提供空实现，返回已完成的任务
    /// </summary>
    /// <returns>表示异步操作完成的ValueTask</returns>
    ValueTask IScene.OnExitAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     场景卸载异步方法
    ///     提供空实现，返回已完成的任务
    /// </summary>
    /// <returns>表示异步操作完成的ValueTask</returns>
    ValueTask IScene.OnUnloadAsync()
    {
        return ValueTask.CompletedTask;
    }
}