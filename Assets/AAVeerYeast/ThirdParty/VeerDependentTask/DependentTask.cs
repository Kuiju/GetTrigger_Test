using System;
using System.Collections.Generic;
using UnityEngine;

namespace VeerDependentTask
{
    public class DependentTask
    {
        public DependentTaskInternal Internal = null;

        public TaskExecuteAction TaskAction = null;

        public DependentTask()
        {
            Internal = new DependentTaskInternal();
        }

        public void ExecuteTask()
        {
            if (Internal == null)
                return;

            if (TaskAction == null)
            {
                Internal.bTaskRunning = false;
                Internal.bTaskFinished = true;
                Internal.bTaskFinishedSuccess = true;
                DependentTaskManager.Instance.OnTaskFinished(Internal.TaskRegionId, Internal.TaskName);
                return;
            }

            Internal.bTaskRunning = true;
            TaskAction.Invoke((bool outSuccess, string outExtraInfo, object outExtraObj) =>
            {
                Internal.bTaskRunning = false;
                Internal.bTaskFinished = true;
                Internal.bTaskFinishedSuccess = outSuccess;
                Internal.bTaskExecuteExtraInfo = outExtraInfo;
                Internal.bTaskExecuteExtraObject = outExtraObj;

                // when task finished flush action
                TaskAction = null;

                DependentTaskManager.Instance.OnTaskFinished(Internal.TaskRegionId, Internal.TaskName);
            });
        }

        public DependentTask SetTaskRegion(string taskRegion)
        {
            Internal.TaskRegionId = taskRegion;
            return this;
        }

        public DependentTask SetTaskName(string taskName)
        {
            Internal.TaskName = taskName;
            return this;
        }

        public DependentTask AddDependentTask(string taskName)
        {
            Internal.AddDependentTask(taskName);
            return this;
        }

        public DependentTask SetTaskAction(TaskExecuteAction action)
        {
            TaskAction = action;
            return this;
        }

        public bool CheckAllDependentTaskFinished()
        {
            return Internal.CheckAllDependentTaskFinished();
        }

        public DependentTask RegisterTask()
        {
            DependentTaskManager.Instance.RegisterTask(this);
            return this;
        }
    }
}
