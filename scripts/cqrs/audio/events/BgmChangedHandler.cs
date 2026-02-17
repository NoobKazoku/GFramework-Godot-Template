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

using GFramework.Core.cqrs.notification;
using GFrameworkGodotTemplate.scripts.core.audio.system;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.events;

/// <summary>
/// 背景音乐变更事件处理器
/// 负责处理BgmChangedEvent事件，通过音频系统切换背景音乐
/// 继承自AbstractNotificationHandler，专门处理背景音乐切换相关的通知消息
/// </summary>
public class BgmChangedHandler : AbstractNotificationHandler<BgmChangedEvent>
{
    private IAudioSystem? _audioSystem;

    /// <summary>
    /// 处理背景音乐变更事件
    /// 根据事件中的背景音乐类型切换到对应的背景音乐
    /// </summary>
    /// <param name="notification">背景音乐变更事件通知对象</param>
    /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
    /// <returns>异步任务，表示音乐切换操作的完成</returns>
    public override ValueTask Handle(BgmChangedEvent notification, CancellationToken cancellationToken)
    {
        _audioSystem ??= this.GetSystem<IAudioSystem>()!;
        _audioSystem.PlayBgm(notification.BgmType);
        return ValueTask.CompletedTask;
    }
}