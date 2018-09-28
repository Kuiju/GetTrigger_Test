using System;
using System.Collections.Generic;
using VeerYeast;

namespace VeerDependentTask
{
    public class DependentTaskTest : GUITestBase
    {
        public void Test_DependentTask()
        {
            VeerDebugHelper.Instance.bSaveLog = true;
            VeerDebugHelper.Instance.InitDebug();

            string taskName = null;
            List<string> taskDependent = null;

            taskName = "A";
            taskDependent = new List<string>();
            CreateTask(taskName, taskDependent);

            taskName = "B";
            taskDependent = new List<string>() { "A" };
            CreateTask(taskName, taskDependent);

            taskName = "C";
            taskDependent = new List<string>() { "A" };
            CreateTask(taskName, taskDependent);

            taskName = "D";
            taskDependent = new List<string>() { "A", "C" };
            CreateTask(taskName, taskDependent);

            taskName = "E";
            taskDependent = new List<string>() { "B" };
            CreateTask(taskName, taskDependent);

            taskName = "F";
            taskDependent = new List<string>() { "B", "C", "D" };
            CreateTask(taskName, taskDependent);

            taskName = "G";
            taskDependent = new List<string>() { "E", "F" };
            CreateTask(taskName, taskDependent);
        }

        private void CreateTask(string taskName, List<string> taskDependent)
        {
            DependentTask task = new DependentTask().SetTaskRegion("null").SetTaskName(taskName).SetTaskAction((TaskExecuteCallback callback) =>
            {
                VeerDebug.Log(" task start : " + taskName);
                float duration = UnityEngine.Random.Range(1f, 4f);
                this.StartCoroutine(CoroutineUtils.WaitForSecond(duration, () =>
                {
                    VeerDebug.Log(" task finished : " + taskName + " , duration : " + duration);
                    callback(true, null, null);
                }));
            });

            if (taskDependent != null)
            {
                for (int i = 0; i < taskDependent.Count; i++)
                    task.AddDependentTask(taskDependent[i]);
            }

            task.RegisterTask();
        }
    }
}
