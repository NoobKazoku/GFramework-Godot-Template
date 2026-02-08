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

using GFramework.Core.extensions;
using GFramework.Core.query;
using GFramework.Game.Abstractions.setting;
using GFramework.Game.Abstractions.setting.data;
using GFrameworkGodotTemplate.scripts.setting.query.view;

namespace GFrameworkGodotTemplate.scripts.setting.query;

/// <summary>
///     获取当前设置的查询类
/// </summary>
public sealed class GetCurrentSettingsQuery : AbstractQuery<SettingsView>
{
    /// <summary>
    ///     执行获取当前设置的查询操作
    /// </summary>
    /// <returns>包含当前设置信息的SettingsView对象</returns>
    protected override SettingsView OnDo()
    {
        // 从模型中获取设置数据
        var model = this.GetModel<ISettingsModel>()!;
        // 再此可以校验设置数据
        // 构建并返回设置视图对象
        return new SettingsView
        {
            Audio = model.GetData<AudioSettings>(),
            Graphics = model.GetData<GraphicsSettings>(),
            Localization = model.GetData<LocalizationSettings>()
        };
    }
}