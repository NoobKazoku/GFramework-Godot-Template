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

namespace GFrameworkGodotTemplate.scripts.enums.scene;

/// <summary>
/// 场景卸载策略
/// </summary>
public enum SceneUnloadPolicy
{
    /// <summary>立即销毁</summary>
    Destroy,

    /// <summary>隐藏但保留（可复用）</summary>
    Hide,

    /// <summary>延迟销毁（等待动画完成）</summary>
    Deferred
}