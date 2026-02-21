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

using GFramework.Core.Abstractions.coroutine;
using GFramework.Game.Abstractions.scene;
using GFramework.Godot.coroutine;
using GFramework.SourceGenerators.Abstractions.logging;
using GFrameworkGodotTemplate.scripts.enums.scene;
using global::GFrameworkGodotTemplate.global;
using Godot;

namespace GFrameworkGodotTemplate.scripts.core.scene;

/// <summary>
/// 场景过渡动画处理器，将 SceneTransitionManager 的过渡效果接入管道。
/// </summary>
[Log]
public partial class SceneTransitionAnimationHandler(
    Func<SceneTransitionManager> transitionManagerFunc,
    IReadOnlyDictionary<string, PackedScene> sceneMap)
    : ISceneAroundTransitionHandler
{
    private SceneTransitionManager TransitionManager => transitionManagerFunc.Invoke();
    public int Priority => 0; // 最高优先级，最外层包裹

    public bool ShouldHandle(SceneTransitionEvent @event)
        => !TransitionManager.IsTransitioning &&
           !string.Equals(@event.ToSceneKey, nameof(SceneKey.Boot), StringComparison.Ordinal);

    public async Task HandleAsync(
        SceneTransitionEvent @event,
        Func<Task> next,
        CancellationToken cancellationToken)
    {
        // honor external cancellation before starting any transition logic
        cancellationToken.ThrowIfCancellationRequested();
        // 正在过渡中，直接拦截，不往下传
        if (TransitionManager.IsTransitioning)
        {
            _log.Debug("Scene is transitioning, ignore new request.");
            return;
        }

        var toSceneKey = @event.ToSceneKey;

        // 没有目标场景 key，直接跳过过渡动画
        if (string.IsNullOrEmpty(toSceneKey))
        {
            _log.Debug("No target scene key, skip transition.");
            await next().ConfigureAwait(false);
            return;
        }

        // 将 next（场景切换核心逻辑）包装成协程，传给 PlayTransitionCoroutine
        IEnumerator<IYieldInstruction> SwitchCoroutine()
        {
            yield return next().AsCoroutineInstruction();
        }

        TransitionManager
            .PlayTransitionCoroutine(
                SwitchCoroutine(),
                () => sceneMap[toSceneKey].Instantiate()
            ).RunCoroutine();
    }
}