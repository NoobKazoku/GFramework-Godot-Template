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

using GFramework.Game.scene;
using GFramework.Game.scene.handler;
using GFramework.Godot.scene;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.scene;

/// <summary>
///     场景路由器类，负责管理游戏场景的切换和路由功能
///     继承自SceneRouterBase基类，提供场景根节点访问和初始化功能
/// </summary>
public class SceneRouter : SceneRouterBase
{
    /// <summary>
    ///     获取场景根节点
    ///     将基类的Root属性转换为Node类型返回
    /// </summary>
    public Node? SceneRoot => Root as Node;

    /// <summary>
    ///     注册场景转换处理器
    ///     重写基类方法，注册日志记录转换处理器
    ///     用于在场景切换过程中记录相关日志信息
    /// </summary>
    protected override void RegisterHandlers()
    {
        RegisterHandler(new LoggingTransitionHandler());
        var sceneRegistry = this.GetUtility<IGodotSceneRegistry>()!;
        RegisterAroundHandler(
            new SceneTransitionAnimationHandler(() => SceneTransitionManager.Instance!, sceneRegistry.GetAll()));
    }
}