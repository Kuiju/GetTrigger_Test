using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CoroutineUtils
{
    #region Basic

    public static IEnumerator OnFrameEnd(UnityAction action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }

    public static IEnumerator WaitForFrame(int frameCount, UnityAction action)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }
        action();
    }

    public static IEnumerator WaitForFrameOnFrameEnd(int frameCount, UnityAction action)
    {
        for (int i = 0; i < frameCount - 1; i++)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        action();
    }

    public static IEnumerator WaitForSecond(float second, UnityAction action)
    {
        yield return new WaitForSeconds(second);
        action();
    }

    public static IEnumerator WaitForFrameThenSecond(int frameCount, float second, UnityAction action)
    {
        for (int i = 0; i < frameCount - 1; i++)
        {
            yield return null;
        }
        yield return new WaitForSeconds(second);
        action();
    }

    public static IEnumerator LoopExecuteBySecondInterval(float intervalSecond, UnityAction action)
    {
        while (true)
        {
            action();
            yield return new WaitForSeconds(intervalSecond);
        }
    }

    public static IEnumerator LoopExecuteBySecondIntervalWithTime(int executeTime, float intervalSecond, UnityAction action)
    {
        int i = 0;
        while (i < executeTime)
        {
            i++;
            action();
            yield return new WaitForSeconds(intervalSecond);
        }
    }

    public static IEnumerator LoopExecuteByFrameInterval(int intervalFrame, UnityAction action)
    {
        while (true)
        {
            action();
            for (int i = 0; i < intervalFrame; i++)
            {
                yield return null;
            }
        }
    }

    public static IEnumerator LoopExecuteByFrameIntervalWithTime(int executeTime, int intervalFrame, UnityAction action)
    {
        int i = 0;
        while (i < executeTime)
        {
            i++;
            action();
            for (int j = 0; j < intervalFrame; j++)
            {
                yield return null;
            }
        }
    }

    #endregion
}
