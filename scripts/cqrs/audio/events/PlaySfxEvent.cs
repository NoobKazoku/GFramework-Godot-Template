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

using GFrameworkGodotTemplate.scripts.enums.audio;
using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.audio.events;

/// <summary>
/// 音效播放事件
/// 当需要播放特定类型的音效时触发此事件
/// 该事件实现了INotification接口，可在CQRS架构中作为通知消息使用
/// </summary>
public class PlaySfxEvent : INotification
{
    /// <summary>
    /// 获取要播放的音效类型
    /// </summary>
    public SfxType SfxType { get; init; }
}