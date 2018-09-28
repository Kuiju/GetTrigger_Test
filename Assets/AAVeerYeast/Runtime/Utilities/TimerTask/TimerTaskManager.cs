using System;
using System.Collections.Generic;
using VeerYeast;

public class TimerTaskManager : Singleton<TimerTaskManager>
{
    private static int _timerTaskId = int.MinValue;
    private static int _TimerTaskId { get { return _timerTaskId++; } }

    private static List<TimerTask> _RunningTimerTaskList = new List<TimerTask>();

    public static void RunTimerTask(TimerTask timerTask, float delayTime, Action action, bool loop = false)
    {
        if (timerTask == null)
        {
            VeerDebug.LogError(" can not RunTimerTask with a null timerTask input ...");
            return;
        }

        if (timerTask.bTaskRunning)
        {
            timerTask.StopTask();
            return;
        }

        if (delayTime <= 0)
        {
            VeerDebug.LogError(" can not RunTimerTask with strange delay time ...");
            return;
        }

#if UNITY_EDITOR

        if (_RunningTimerTaskList.Contains(timerTask))
        {
            VeerDebug.LogError(" strange ... a timer task in _RunningTimerTaskList is not running ...");
            return;
        }

#endif

        timerTask.bTaskRunning = true;
        timerTask.CoroutineTaskId = _TimerTaskId;
        timerTask.bPaused = false;
        timerTask.bLoop = loop;
        timerTask.TaskDelayTime = delayTime;
        timerTask.TaskTimer = delayTime;
        timerTask.TaskAction = action;

        _RunningTimerTaskList.Add(timerTask);
    }

    public static void StopTimerTask(TimerTask timerTask)
    {
        if (_RunningTimerTaskList.Contains(timerTask))
        {
            timerTask.bTaskRunning = false;
            _RunningTimerTaskList.Remove(timerTask);
        }
    }

    public static void TimerTaskManagerUpdate(float deltaTime)
    {
        List<TimerTask> finishedTimerTask = null;
        foreach (var task in _RunningTimerTaskList)
        {
            if (task == null)
            {
                if (finishedTimerTask == null)
                {
                    finishedTimerTask = new List<TimerTask>();
                }
                finishedTimerTask.Add(task);
                continue;
            }

            if (!task.bPaused)
            {
                task.TaskTimer -= deltaTime;
            }

            if (task.TaskTimer <= 0)
            {
                if (finishedTimerTask == null)
                {
                    finishedTimerTask = new List<TimerTask>();
                }
                finishedTimerTask.Add(task);
            }
        }

        if (finishedTimerTask == null)
        {
            return;
        }

        foreach (var task in finishedTimerTask)
        {
            if (task != null)
            {
                task.bTaskRunning = false;
                if (task.TaskAction != null)
                {
                    task.TaskAction();
                }
            }

            if (task.bLoop)
            {
                task.bTaskRunning = true;
                task.TaskTimer = task.TaskDelayTime;
            }
            else
                _RunningTimerTaskList.Remove(task);
        }
    }
}
