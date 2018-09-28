using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening
{
    public enum AbortMethod
    {
        // will call OnKill
        JustKill,

        // snap to target with OnKill callback
        ForceCompleteWithKillCallback,

        // snap to target with OnComplete callback
        ForceCompleteWithOnCompleteCallback,

        // snap to target with OnComplete and then OnKill callback
        ForceCompleteWithOnCompleteAndOnKill
    }

    public class TweenWarpper
    {
        public Tween Tween = null;

        public static TweenWarpper Create(Tween tween)
        {
            TweenWarpper tweenWarpper = new TweenWarpper();
            tweenWarpper.Set(tween);
            return tweenWarpper;
        }

        public void Set(Tween tween)
        {
            if (tween == null)
            {
                return;
            }

            if (Tween != null)
            {
                Abort(AbortMethod.JustKill);
            }

            Tween = tween;
        }

        public void Complete(bool withCallback)
        {
            if (Tween == null || !Tween.IsPlaying())
            {
                return;
            }

            Tween.Complete(withCallback);
        }

        public void Abort(AbortMethod abortMethod)
        {
            if (Tween == null || !Tween.IsPlaying())
            {
                return;
            }

            if (abortMethod == AbortMethod.JustKill)
            {
                Tween.Kill();
            }
            else if (abortMethod == AbortMethod.ForceCompleteWithKillCallback)
            {
                Tween.OnComplete(null);
                Tween.Complete();
            }
            else if (abortMethod == AbortMethod.ForceCompleteWithOnCompleteCallback)
            {
                Tween.OnKill(null);
                Tween.Complete();
            }
            else if (abortMethod == AbortMethod.ForceCompleteWithOnCompleteAndOnKill)
            {
                Tween.Kill(true);
            }

            Tween = null;
        }
    }
}