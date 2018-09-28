using System;
using System.Collections.Generic;
using UnityEngine;

namespace VeerDependentTask
{
    public class DependentTaskRegion
    {
        public string RegionId = "null";

        public Dictionary<string, DependentTask> UnfinishedTaskDic = new Dictionary<string, DependentTask>();
        public Dictionary<string, DependentTask> FinishedTaskDic = new Dictionary<string, DependentTask>();
        public List<string> FinishedTaskTag = new List<string>();

        internal void AddFinishedTaskTag(string taskTag)
        {
            FinishedTaskTag.AddIfNotContain(taskTag);
            CheckExecuteTask();
        }

        internal void AddTask(DependentTask taskBase)
        {
            UnfinishedTaskDic.SetAddValue(taskBase.Internal.TaskName, taskBase);

            if (taskBase.CheckAllDependentTaskFinished())
                taskBase.ExecuteTask();
        }

        internal void OnTaskFinished(string taskName)
        {
            if (!UnfinishedTaskDic.ContainsKey(taskName))
                return;

            DependentTask task = UnfinishedTaskDic[taskName];
            UnfinishedTaskDic.Remove(taskName);
            FinishedTaskDic.Add(taskName, task);

            CheckExecuteTask();
        }

        internal void CheckExecuteTask()
        {
            foreach (var p in UnfinishedTaskDic)
            {
                DependentTask task = p.Value;
                if (task.Internal.bTaskRunning)
                    continue;

#if UNITY_EDITOR
                if (task.Internal.bTaskFinished)
                {
                    VeerDebug.LogError(" a finished task is in unfinished task list : " + task.Internal.TaskName);
                    continue;
                }
#endif

                if (task.CheckAllDependentTaskFinished())
                    task.ExecuteTask();
            }
        }

        internal bool IsTaskFinished(string taskName)
        {
            if (FinishedTaskDic.ContainsKey(taskName))
                return true;

            if (FinishedTaskTag.Contains(taskName))
                return true;

            return false;
        }
    }
}
