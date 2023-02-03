using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils.Animation;

namespace Core.Extensions
{
    public static class AnimatorExtensions
    {
        public static bool TrySetTrigger(this Animator self, int triggerHash)
        {
            if (self == null || !self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
            {
                return false;
            }

            try
            {
                self.SetTrigger(triggerHash);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public static bool TrySetBool(this Animator self, int boolHash, bool value)
        {
            if (self == null || !self.isActiveAndEnabled || !self.gameObject.activeInHierarchy)
            {
                return false;
            }
            
            try
            {
                self.SetBool(boolHash, value);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public static async UniTask WaitForAnimationEnd(this Animator animator, string animationTag, bool shouldWaitForTransitionEnd = true, int layer = 0)
        {
            await new WaitForAnimationStart(animator, animationTag, layer);
            await new WaitForAnimationEnd(animator, animationTag, layer);
            
            if (shouldWaitForTransitionEnd)
            {
                await new WaitForAnimationTransitionEnd(animator, layerIndex: layer);
            }
        }
    }
}