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

using GFramework.Core.Abstractions.system;
using GFrameworkGodotTemplate.scripts.enums.audio;
using global::GFrameworkGodotTemplate.global;

namespace GFrameworkGodotTemplate.scripts.core.audio.system;

/// <summary>
/// 音频系统接口
/// 定义了游戏音频播放的核心功能，包括背景音乐和音效的播放控制
/// </summary>
public interface IAudioSystem : ISystem
{
    /// <summary>
    /// 绑定音频管理器
    /// 将指定的音频管理器实例与当前音频系统关联
    /// </summary>
    /// <param name="audioManager">要绑定的音频管理器实例</param>
    void BindAudioManager(AudioManager audioManager);

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="bgmType">背景音乐类型枚举值</param>
    void PlayBgm(BgmType bgmType);

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sfxType">音效类型枚举值</param>
    void PlaySfx(SfxType sfxType);
}