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


using GFramework.Game.Abstractions.setting.data;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.query.view;

/// <summary>
///     表示游戏设置视图的数据模型，包含音频和显示设置信息
/// </summary>
public sealed class SettingsView
{
    /// <summary>
    ///     音频设置配置对象，包含所有音频相关的设置选项
    /// </summary>
    public AudioSettings Audio { get; init; } = new();

    /// <summary>
    ///     图形设置配置对象，包含所有图形渲染相关的设置选项
    /// </summary>
    public GraphicsSettings Graphics { get; init; } = new();

    /// <summary>
    ///     本地化设置配置对象，包含所有语言和区域相关的设置选项
    /// </summary>
    public LocalizationSettings Localization { get; init; } = new();
}