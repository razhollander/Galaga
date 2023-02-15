using System;
using Cysharp.Threading.Tasks;
using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.Scripts.Extensions
{
    public static class AnimationExtensions
    {
        public static UniTask PlayAndWait(this Animation animation, string animationName, float percentageToAwait = 1f)
        {
            try
            {
                animation.Play(animationName);
                var animationTime = GetAnimationLength(animation, animationName);
                var timeToWait = animationTime * percentageToAwait;
                return UniTask.Delay(TimeSpan.FromSeconds(timeToWait));
            }
            catch (Exception e)
            {
                LogService.LogError($"Failed to play animation: {animationName}, {e}");
                return UniTask.CompletedTask;
            }
        }

        public static async UniTask CrossFade(this Animation animation, string animationName, float crossFadeDuration)
        {
            try
            {
                while (crossFadeDuration > 0)
                {
                    animation.CrossFade(animationName, crossFadeDuration, PlayMode.StopSameLayer);
                    crossFadeDuration -= Time.deltaTime;
                    await UniTask.Yield();
                }
            }
            catch (Exception e)
            {
                LogService.LogError($"Failed to play animation: {animationName}, {e}");
            }
        }

        public static float GetAnimationLength(this Animation animation, string animationName)
        {
            var animationState = animation[animationName];
            return animationState.length * (1 / Mathf.Abs(animationState.speed));
        }
        
        public static float GetClipLength(this Animation animation, string clipName)
        {
            return animation.GetClip(clipName).length;
        }
    }
}