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

// 全局命名空间导入 - 提供系统基础功能、集合操作、异步编程支持以及LanguageExt函数式编程库的功能

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;

// 全局导入GFramework
global using GFrameworkGodotTemplate.scripts.constants;
global using GFrameworkGodotTemplate.scripts.core.audio.system;
global using GFrameworkGodotTemplate.scripts.enums.audio;
global using GFramework.Godot.SourceGenerators.Abstractions;
global using ICommand = Mediator.ICommand;
global using GFramework.SourceGenerators.Abstractions.Logging;
global using GFramework.SourceGenerators.Abstractions.Rule;