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

using GFramework.Core.Abstractions.serializer;
using GFramework.Core.Abstractions.storage;
using GFramework.Game.Abstractions.data;
using GFramework.Game.data;
using GFrameworkGodotTemplate.scripts.setting.interfaces;

namespace GFrameworkGodotTemplate.scripts.setting;

/// <summary>
/// 设置数据仓库类，用于管理设置相关的数据存储和操作
/// </summary>
/// <param name="storage">存储接口实例，用于数据的持久化存储</param>
/// <param name="options">数据仓库配置选项，可选参数，默认为null</param>
public class SettingsDataRepository(IStorage? storage, IRuntimeTypeSerializer serializer, DataRepositoryOptions? options = null)
    : UnifiedSettingsRepository(storage,serializer, options), ISettingsDataRepository;
