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

using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.scene.events;

/// <summary>
/// 场景根节点就绪事件
/// 用于通知系统场景根节点已经准备完成
/// 该事件实现了INotification接口，可在CQRS架构中作为通知消息使用
/// </summary>
public class SceneRootReadyEvent : INotification;