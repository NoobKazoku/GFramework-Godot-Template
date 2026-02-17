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

using GFrameworkGodotTemplate.scripts.cqrs.setting.query.view;
using Mediator;

namespace GFrameworkGodotTemplate.scripts.cqrs.setting.query;

/// <summary>
/// 获取当前设置信息的查询命令
/// 实现CQRS模式中的查询部分，用于获取应用程序的当前设置视图
/// </summary>
public sealed class GetCurrentSettingsQuery : IQuery<SettingsView>;