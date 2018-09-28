using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VeerDependentTask
{
    public delegate void TaskExecuteAction(TaskExecuteCallback callback);
    public delegate void TaskExecuteCallback(bool outSuccess, string outExtraInfo, object outExtraObj);

    public class DependentTaskInternal
    {
        /// <summary> task inner identifier </summary>
        public int TaskInnerId { get; private set; }

        /// <summary> task region given by creator </summary>
        public string TaskRegionId = "null";

        /// <summary> task name given by creator </summary>
        public string TaskName = "null";

        public bool bTaskRunning { get; set; }

        public bool bTaskFinished { get; set; }

        public bool bTaskFinishedSuccess { get; set; }

        public string bTaskExecuteExtraInfo { get; set; }

        public object bTaskExecuteExtraObject { get; set; }

        /// <summary> dependent task </summary>
        public List<string> DependentTaskNameList { get; set; }

        public DependentTaskInternal()
        {
            TaskInnerId = DependentTaskUtils.GetNewDependentTaskId;
        }

        public void AddDependentTask(string taskName)
        {
            if (DependentTaskNameList == null)
                DependentTaskNameList = new List<string>();
            DependentTaskNameList.AddIfNotContain(taskName);
        }

        internal bool CheckAllDependentTaskFinished()
        {
            if (DependentTaskNameList == null)
                return true;

            for (int i = 0; i < DependentTaskNameList.Count; i++)
            {
                if (!DependentTaskManager.Instance.IsTaskFinished(TaskRegionId, DependentTaskNameList[i]))
                    return false;
            }

            return true;
        }
    }
}