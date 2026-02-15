// meta-name: 简单场景控制器类模板
// meta-description: 负责管理简单场景的生命周期和架构关联
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

using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.scene;
using GFramework.Godot.scene;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFrameworkGodotTemplate.scripts.core.scene;
using GFrameworkGodotTemplate.scripts.enums.scene;
using Godot;

[ContextAware]
[Log]
public partial class _CLASS_ : _BASE_, IController, ISceneBehaviorProvider, ISimpleScene
{
    /// <summary>
    /// 场景行为实例，用于管理具体的场景逻辑
    /// </summary>
    private ISceneBehavior? _scene;
    
    /// <summary>
    /// 获取场景键值字符串
    /// </summary>
    public static string SceneKeyStr => nameof(SceneKey._CLASS_);

    /// <summary>
    /// 获取场景行为实例
    /// 使用工厂模式创建场景行为，确保单例模式
    /// </summary>
    /// <returns>ISceneBehavior接口的场景行为实例</returns>
    public ISceneBehavior GetScene()
    {
        _scene ??= SceneBehaviorFactory.Create<_BASE_>(this, SceneKeyStr);
        return _scene;
    }

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
    }
}
