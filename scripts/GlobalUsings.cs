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

// 全局导入LanguageExt函数式编程库的核心组件，包括通用类型、效果系统和预定义函数
global using LanguageExt;
global using LanguageExt.Common;
global using LanguageExt.Effects;
global using LanguageExt.Pretty;
global using static LanguageExt.Prelude;

// 全局导入GFramework
global using GFramework.Core.extensions;
global using GFramework.Core.coroutine.extensions;