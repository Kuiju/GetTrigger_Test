using System;
using System.Collections.Generic;
using UnityEngine;

namespace VeerDependentTask
{
    public static class DependentTaskUtils
    {
        private static int _DependentTaskId = int.MinValue;
        internal static int GetNewDependentTaskId { get { return _DependentTaskId++; } }
    }
}
