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

using GFramework.Core.system;
using GFrameworkGodotTemplate.scripts.enums.audio;
using global::GFrameworkGodotTemplate.global;

namespace GFrameworkGodotTemplate.scripts.core.audio.system;

/// <summary>
/// Godot音频系统实现类
/// 实现IAudioSystem接口，负责游戏音频的播放控制
/// 通过绑定AudioManager来执行具体的音频播放操作
/// </summary>
public class GodotAudioSystem : AbstractSystem, IAudioSystem
{
    private AudioManager? _manager;

    /// <summary>
    /// 获取音频管理器实例
    /// 如果管理器未绑定则抛出异常
    /// </summary>
    private AudioManager AudioManager =>
        _manager ?? throw new InvalidOperationException("AudioManager has not been bound.");

    /// <summary>
    /// 绑定音频管理器
    /// 将指定的音频管理器实例与当前音频系统关联
    /// </summary>
    /// <param name="audioManager">要绑定的音频管理器实例</param>
    /// <exception cref="InvalidOperationException">当音频管理器已经被绑定时抛出</exception>
    public void BindAudioManager(AudioManager audioManager)
    {
        // 检查是否已经绑定了音频管理器
        if (_manager is not null)
        {
            throw new InvalidOperationException("AudioManager has already been bound.");
        }

        _manager = audioManager;
    }

    /// <summary>
    /// 播放背景音乐
    /// 通过绑定的音频管理器播放指定类型的背景音乐
    /// </summary>
    /// <param name="bgmType">要播放的背景音乐类型</param>
    public void PlayBgm(BgmType bgmType)
    {
        AudioManager.PlayBgm(bgmType);
    }

    /// <summary>
    /// 播放音效
    /// 通过绑定的音频管理器播放指定类型的音效
    /// </summary>
    /// <param name="sfxType">要播放的音效类型</param>
    public void PlaySfx(SfxType sfxType)
    {
        AudioManager.PlaySfx(sfxType);
    }

    protected override void OnInit()
    {
        // ignore
    }
}