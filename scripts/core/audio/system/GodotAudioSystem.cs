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
using GFramework.SourceGenerators.Abstractions.logging;
using GFrameworkGodotTemplate.scripts.enums.audio;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.audio.system;

/// <summary>
/// Godot音频系统实现类
/// 实现IAudioSystem接口，负责游戏音频的播放控制
/// 通过绑定AudioManager来执行具体的音频播放操作
/// </summary>
[Log]
public partial class GodotAudioSystem : AbstractSystem, IAudioSystem
{
    private AudioManager? _manager;

    /// <summary>
    /// 获取音频管理器实例（仅在有效时返回，否则为 null）
    /// </summary>
    private AudioManager? CurrentManager =>
        (_manager is not null && GodotObject.IsInstanceValid(_manager) && _manager.IsInsideTree())
            ? _manager
            : null;

    /// <summary>
    /// 绑定音频管理器
    /// 将指定的音频管理器实例与当前音频系统关联
    /// </summary>
    /// <param name="audioManager">要绑定的音频管理器实例</param>
    /// <exception cref="ArgumentNullException">当 audioManager 为 null 时抛出</exception>
    /// <exception cref="InvalidOperationException">
    /// 当已有有效的 AudioManager 被绑定时抛出。
    /// 已绑定的 AudioManager 若已离开场景树或被销毁，将自动允许重新绑定。
    /// </exception>
    public void BindAudioManager(AudioManager audioManager)
    {
        if (audioManager is null)
            throw new ArgumentNullException(nameof(audioManager));

        if (_manager is not null)
        {
            var isValidInstance = GodotObject.IsInstanceValid(_manager);
            var isInTree = isValidInstance && _manager.IsInsideTree();

            if (isValidInstance && isInTree)
            {
                throw new InvalidOperationException("AudioManager has already been bound.");
            }

            // 旧实例失效，允许重新绑定
            _manager = null;
        }

        _manager = audioManager;

        // 自动解除绑定，避免悬垂引用
        if (GodotObject.IsInstanceValid(_manager))
        {
            var boundManager = _manager;
            boundManager.TreeExiting += () =>
            {
                if (ReferenceEquals(_manager, boundManager))
                {
                    _manager = null;
                }
            };
        }
    }

    /// <summary>
    /// 播放背景音乐（安全访问模式）
    /// 如果当前 AudioManager 无效，将忽略调用
    /// </summary>
    /// <param name="bgmType">要播放的背景音乐类型</param>
    public void PlayBgm(BgmType bgmType)
    {
        var manager = CurrentManager;
        if (manager != null)
        {
            manager.PlayBgm(bgmType);
        }
        else
        {
            _log.Warn($"PlayBgm skipped: AudioManager is not bound or has been destroyed.");
        }
    }

    /// <summary>
    /// 播放音效（安全访问模式）
    /// 如果当前 AudioManager 无效，将忽略调用
    /// </summary>
    /// <param name="sfxType">要播放的音效类型</param>
    public void PlaySfx(SfxType sfxType)
    {
        var manager = CurrentManager;
        if (manager != null)
        {
            manager.PlaySfx(sfxType);
        }
        else
        {
            _log.Warn($"PlaySfx skipped: AudioManager is not bound or has been destroyed.");
        }
    }

    protected override void OnInit()
    {
        // ignore
    }
}