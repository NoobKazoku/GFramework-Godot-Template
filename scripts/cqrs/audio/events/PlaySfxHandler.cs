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
/// 音效播放事件处理器
/// 负责处理PlaySfxEvent事件，通过音频系统播放音效
/// 继承自AbstractNotificationHandler，专门处理音效播放相关的通知消息
/// </summary>
public class PlaySfxHandler : AbstractNotificationHandler<PlaySfxEvent>
{
    private IAudioSystem? _audioSystem;

    /// <summary>
    /// 处理音效播放事件
    /// 根据事件中的音效类型播放对应的音效
    /// </summary>
    /// <param name="notification">音效播放事件通知对象</param>
    /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
    /// <returns>异步任务，表示音效播放操作的完成</returns>
    public override ValueTask Handle(PlaySfxEvent notification, CancellationToken cancellationToken)
    {
        _audioSystem ??= this.GetSystem<IAudioSystem>()!;
        _audioSystem.PlaySfx(notification.SfxType);
        return ValueTask.CompletedTask;
    }
}