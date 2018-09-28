using UnityEngine;

namespace VeerYeast
{
    public class VeerDebugHelper : MonoSingleton<VeerDebugHelper>
    {
        [HideInInspector]
        public bool bEnableLog = true;

        [HideInInspector]
        public bool bSaveLog =
#if UNITY_EDITOR
            true;
#else
            false;
#endif

        [HideInInspector]
        public string SavePath = "Default";

        [HideInInspector]
        public string SaveFileName = "Default";

        [HideInInspector]
        public string SaveExt = "Default";

        public void InitDebug()
        {
            VeerDebug.SetEnableLog(bEnableLog);
            VeerDebug.SetDebugLogMode(VeerDebug.LogMode.ForbidTagMode);

#if UNITY_EDITOR
            if (bSaveLog)
            {
                if (SavePath == "Default" || string.IsNullOrEmpty(SavePath))
                {
                    SavePath = Application.dataPath + "/ZZLog";
                    SaveFileName = "log";
                    SaveExt = ".txt";
                }
            }
#endif

            VeerDebug.SetSaveLog(bSaveLog, SavePath, SaveFileName, SaveExt);
        }

        private void OnApplicationQuit()
        {
            VeerDebug.SetEnableLog(false);
            VeerDebug.SaveLogToFile();
        }
    }
}
