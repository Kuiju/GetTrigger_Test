using System;
using System.Collections.Generic;
using UnityEngine;
using VeerYeast;

namespace VeerDependentTask
{
    public class DependentTaskManager : Singleton<DependentTaskManager>
    {
        private Dictionary<string, DependentTaskRegion> _TaskRegionDic = new Dictionary<string, DependentTaskRegion>();

        internal void OnTaskFinished(string taskRegionId, string taskName)
        {
            if (!_TaskRegionDic.ContainsKey(taskRegionId))
                return;
            _TaskRegionDic[taskRegionId].OnTaskFinished(taskName);
        }

        internal void CheckExecuteTask(string taskRegionId)
        {
            if (!_TaskRegionDic.ContainsKey(taskRegionId))
                return;
            _TaskRegionDic[taskRegionId].CheckExecuteTask();
        }

        internal bool IsTaskFinished(string taskRegionId, string taskName)
        {
            if (!_TaskRegionDic.ContainsKey(taskRegionId))
                return false;
            return _TaskRegionDic[taskRegionId].IsTaskFinished(taskName);
        }

        public void RegisterFinishedTaskTag(string taskRegionId, string taskTag)
        {
            CheckCreateRegion(taskRegionId);

            _TaskRegionDic[taskRegionId].AddFinishedTaskTag(taskTag);
        }

        private void CheckCreateRegion(string taskRegionId)
        {
            if (!_TaskRegionDic.ContainsKey(taskRegionId))
            {
                DependentTaskRegion region = new DependentTaskRegion();
                region.RegionId = taskRegionId;
                _TaskRegionDic.Add(taskRegionId, region);
            }
        }

        internal void RegisterTask(DependentTask taskBase)
        {
            if (taskBase == null ||
                taskBase.Internal == null ||
                string.IsNullOrEmpty(taskBase.Internal.TaskRegionId) ||
                string.IsNullOrEmpty(taskBase.Internal.TaskName))
                return;

            string regionId = taskBase.Internal.TaskRegionId;
            CheckCreateRegion(regionId);

            _TaskRegionDic[regionId].AddTask(taskBase);
        }
    }
}
