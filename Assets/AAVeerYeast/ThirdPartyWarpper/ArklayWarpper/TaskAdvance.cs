using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VeerYeast;

namespace U3D.Threading.Tasks
{
    /// <summary>
    /// TaskCoroutineHelper 用于辅助 Task 的 Timeout Coroutine 运作
    /// </summary>
    public class TaskCoroutineHelper : MonoSingleton<TaskCoroutineHelper> { }

    /// <summary>
    /// 该部分用于辅助 Task 多线程行为的 Timeout 管理
    /// Task 本身有一定的概率丢失线程
    /// 有任何问题请 @songlingyi
    /// </summary>
    public partial class Task<TResult> : Task
    {
        private List<Action<Task<TResult>>> _CallbackList = new List<Action<Task<TResult>>>();
        private List<Action<Task<TResult>>> _TimeoutCoroutineList = new List<Action<Task<TResult>>>();

        public Task ContinueInMainThreadWith(Action<Task<TResult>> continuationAction, float timeout)
        {
            _CallbackList.Add(continuationAction);

            Coroutine timeoutCoroutine = TaskCoroutineHelper.Instance.StartCoroutine(CoroutineUtils.WaitForSecond(timeout, () =>
            {
                if (_CallbackList.Contains(continuationAction))
                {
                    _CallbackList.Remove(continuationAction);
                    continuationAction(null);
                }
            }));

            Task task = ContinueInMainThreadWith((obj) =>
            {
                if (_CallbackList.Contains(continuationAction))
                {
                    _CallbackList.Remove(continuationAction);
                    TaskCoroutineHelper.Instance.StopCoroutine(timeoutCoroutine);
                    continuationAction(obj);
                }
            });

            return task;
        }
    }
}
