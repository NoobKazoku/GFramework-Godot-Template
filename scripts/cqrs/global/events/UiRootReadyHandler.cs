// Copyright (c) 2026 GeWuYou
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using GFramework.Core.Abstractions.state;
using GFramework.Core.cqrs.notification;
using GFramework.Godot.scene;
using GFrameworkGodotTemplate.scripts.core.state.impls;
using GFrameworkGodotTemplate.scripts.core.utils;
using GFrameworkGodotTemplate.scripts.enums.scene;

namespace GFrameworkGodotTemplate.scripts.cqrs.global.events;

/// <summary>
/// UI根节点就绪事件处理器
/// 负责处理UiRootReadyEvent事件，当UI根节点准备就绪时触发状态机切换
/// 继承自AbstractNotificationHandler，专门处理UI就绪相关的通知消息
/// </summary>
public class UiRootReadyHandler : AbstractNotificationHandler<UiRootReadyEvent>
{
    private IStateMachineSystem? _stateMachine;

    /// <summary>
    /// 处理UI根节点就绪事件
    /// 当接收到UiRootReadyEvent通知时，将状态机切换到启动状态
    /// </summary>
    /// <param name="notification">UI根节点就绪事件通知对象</param>
    /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
    /// <returns>异步任务，表示状态切换操作的完成</returns>
    public override async ValueTask Handle(UiRootReadyEvent notification, CancellationToken cancellationToken)
    {
        // 检查是否应该进入主菜单状态，如果是则注册UI根节点就绪事件来切换到主菜单状态
        if (ShouldEnterMainMenu())
        {
            // 获取状态机系统实例并切换到启动状态
            await (_stateMachine ??= this.GetSystem<IStateMachineSystem>()!)
                .ChangeToAsync<BootStartState>().ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     判断当前场景是否为主菜单场景，决定是否需要进入主菜单状态
    /// </summary>
    /// <returns>如果当前场景是主菜单场景则返回true，否则返回false</returns>
    private bool ShouldEnterMainMenu()
    {
        var tree = GameUtil.GetTree();
        var currentScene = tree.CurrentScene;

        if (currentScene == null)
            return false;

        var scenePath = currentScene.SceneFilePath;
        return string.Equals(scenePath, this.GetUtility<IGodotSceneRegistry>()!.Get(nameof(SceneKey.Main)).GetPath(),
            StringComparison.Ordinal);
    }
}