using System;

public class TimerTask
{
    /// <summary>
    /// songlingyi 禁止直接修改
    /// </summary>
    public int CoroutineTaskId = int.MinValue;

    /// <summary>
    /// songlingyi 禁止直接修改
    /// </summary>
    public bool bTaskRunning = false;

    /// <summary>
    /// songlingyi 禁止直接修改
    /// </summary>
    public bool bPaused = false;

    public bool bLoop = false;

    /// <summary>
    /// songlingyi 禁止直接修改
    /// </summary>
    public float TaskDelayTime = float.MinValue;

    public float TaskTimer = float.MinValue;

    public Action TaskAction = null;

    public void ResetTaskTimer(float time)
    {
        TaskDelayTime = time;
        TaskTimer = time;
    }

    public void StopTask()
    {
        TimerTaskManager.StopTimerTask(this);
    }

    public void Pause()
    {
        bPaused = true;
    }

    public void Run()
    {
        bPaused = false;
    }
}
