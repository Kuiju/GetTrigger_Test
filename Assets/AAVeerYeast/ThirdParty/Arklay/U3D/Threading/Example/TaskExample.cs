using System.Collections.Generic;
using U3D.Threading.Tasks;
using UnityEngine;

public class TaskExample : GUITestBase
{
    // 测试 Task.RunInMainThread 没什么用，正常阻塞
    public void Test_RunInMainThread()
    {
        // executed in the main thread
        VeerDebug.Log(" Test_RunInMainThread Start ...");
        Task.RunInMainThread(() =>
        {
            VeerDebug.Log(" Test_RunInMainThread ...");
            GameObject g = new GameObject("_auto_Test_RunInMainThread");
            int rst = SomeCalculate();
        });
        VeerDebug.Log(" Test_RunInMainThread End ...");
    }

    // 测试 Task.Run
    public void Test_Run()
    {
        // executed in the new thread
        // log 的 次序是不确定的，注意 异步编程 / 多线程编程 的常见问题
        VeerDebug.Log(" Test_Run Start ...");
        Task.Run(() =>
        {
            VeerDebug.Log(" Test_Run ...");
            // 在 Run 中操作 UnityEngine.Object 子类会引发 Crash
            //GameObject g = new GameObject("_auto_Test_Run");
        });
        VeerDebug.Log(" Test_Run End ...");
    }

    // 测试 Task.Run 返回后执行 Main Thread 回调方法
    public void Test_RunIntFunc()
    {
        Task<int>.Run(() =>
        {
            int rst = SomeCalculate();
            VeerDebug.Log(" in another thread rst : " + rst);
            return rst;
        }).ContinueInMainThreadWith((obj) =>
        {
            VeerDebug.Log(" in main thread rst : " + obj.Result);
            GameObject g = new GameObject("_auto_Test_RunIntFunc");
        });
    }

    // 测试开启 大量 多线程
    public List<string> TaskListChecker = null;
    public int TaskCountChecker = 0;

    public void Test_MultiThread()
    {
        List<Task> taskList = new List<Task>();
        TaskListChecker = new List<string>();
        TaskCountChecker = 500;
        for (int i = 1; i < 501; i++)
        {
            string index = i.ToString();
            TaskListChecker.Add(index);
            taskList.Add(Task<string>.Run(() =>
            {
                SomeCalculateSimple();
                return index;
            }).ContinueInMainThreadWith((obj) =>
            {
                TaskCountChecker--;

                if (obj == null)
                {
                    VeerDebug.Log(" time out : " + index);
                    return;
                }

                VeerDebug.Log(" thread over : " + obj.Result);
                TaskListChecker.Remove(obj.Result);
            }, 10f));
        }
    }

    public void Test_ShowTaskListChecker()
    {
        VeerDebug.Log(" TaskListChecker : " + TaskListChecker.ListToString());
        VeerDebug.Log(" TaskCountChecker : " + TaskCountChecker.ToString());
    }

    // 测试开启 多线程 Abort
    public void Test_TaskAbort()
    {
        List<Task> taskList = new List<Task>();
        TaskListChecker = new List<string>();
        TaskCountChecker = 500;
        for (int i = 1; i < 501; i++)
        {
            string index = i.ToString();
            TaskListChecker.Add(index);
            taskList.Add(Task<string>.Run(() =>
            {
                SomeCalculateSimple();
                return index;
            }).ContinueInMainThreadWith((obj) =>
            {
                TaskCountChecker--;

                if (obj == null)
                {
                    VeerDebug.Log(" time out : " + index);
                    return;
                }

                VeerDebug.Log(" thread over : " + obj.Result);
                TaskListChecker.Remove(obj.Result);
            }, 10f));
        }

        foreach (var task in taskList)
        {
            task.CheckAbort();
        }
    }

    private int SomeCalculateSimple()
    {
        int rst = 0;
        for (int i = 0; i < 1000; i++)
            rst++;
        return rst;
    }

    private int SomeCalculate()
    {
        int rst = 0;
        int count = 1000;
        for (int i = 0; i < count; i++)
            for (int ii = 0; ii < count; ii++)
                for (int iii = 0; iii < count; iii++)
                    rst++;
        return rst;
    }
}
