using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestTweenWarpper : MonoBehaviour
{
    public Tween Tween1 = null;
    public Tween Tween2 = null;

    public TweenWarpper TweenWarpper = null;

    public float TestFloat = 0;

    private void OnGUI()
    {
        //TestOriginTween();
        TestTweenWapper();
    }

    private void TestTweenWapper()
    {
        if ((GUILayout.Button("Create TweenWarpper")))
        {
            TweenWarpper = new TweenWarpper();
        }
        if ((GUILayout.Button("Play Tween")))
        {
            TestFloat = 0;
            TweenWarpper.Set(DOTween
                             .To(() => { return TestFloat; }, (f) => { TestFloat = f; }, 100, 5)
                             .OnComplete(() => { VeerDebug.Log("tween complete ..."); })
                             .OnKill(() => { VeerDebug.Log("tween kill ..."); }));
        }
        if ((GUILayout.Button("Just Kill")))
        {
            TweenWarpper.Abort(AbortMethod.JustKill);
        }
        if ((GUILayout.Button("Force With OnKill")))
        {
            TweenWarpper.Abort(AbortMethod.ForceCompleteWithKillCallback);
        }
        if ((GUILayout.Button("Force With OnComplete")))
        {
            TweenWarpper.Abort(AbortMethod.ForceCompleteWithOnCompleteCallback);
        }
        if ((GUILayout.Button("Force With OnKill and OnComplete")))
        {
            TweenWarpper.Abort(AbortMethod.ForceCompleteWithOnCompleteAndOnKill);
        }
    }

    private void TestOriginTween()
    {
        if ((GUILayout.Button("Check Tween")))
        {
            if (Tween1 == null)
            {
                VeerDebug.Log("tween is null ...");
            }
            else
            {
                VeerDebug.Log("tween is exist ...");
            }
        }
        if ((GUILayout.Button("Create Tween 1")))
        {
            TestFloat = 0;
            Tween1 = DOTween.To(() => { return TestFloat; }, (f) => { TestFloat = f; }, 100, 5)
                            .OnComplete(() => { VeerDebug.Log("tween complete ..."); })
                            .Pause();
        }
        if ((GUILayout.Button("Create Tween 2")))
        {
            TestFloat = 0;
            Tween2 = DOTween.To(() => { return TestFloat; }, (f) => { TestFloat = f; }, 100, 5)
                            .OnComplete(() => { VeerDebug.Log("tween complete ..."); })
                            .Pause();
        }
        if ((GUILayout.Button("Play Tween 1")))
        {
            Tween1.Play();
        }
        if ((GUILayout.Button("Play Tween 2")))
        {
            Tween2.Play();
        }
        if ((GUILayout.Button("Complete Tween 1")))
        {
            Tween1.Complete();
        }
        if ((GUILayout.Button("Kill Tween 1")))
        {
            Tween1.Kill();
        }
    }
}
