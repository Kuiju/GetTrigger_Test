using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtils
{
    //获得不同平台下的完整 StreamingAssets 文件位置,用于 System.IO.StreamWriter
    public static string GetStreamingAssetsFullPath(string Path)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/StreamingAssets" + Path;
#elif UNITY_STANDALONE_WIN || UNITY_WSA
        return Application.dataPath + "/StreamingAssets" + Path;
#elif UNITY_IPHONE
        return Application.dataPath + "/Raw" + Path;    
#elif UNITY_ANDROID
        return Application.dataPath + "!/assets" + Path;
#endif
    }

    public static string CombinePath(params string[] path)
    {
        if (path.Length < 1)
        {
            return "";
        }

        string rst = "";
        for (int i = 0; i < path.Length; i++)
        {
            rst += path[i].Trim('/', '\\') + "/";
        }

        rst = rst.Trim('/', '\\');
        return rst;
    }

    public static string TrimEndDirectorySeparator(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return "";
        }
        if (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
        {
            return path.Remove(path.Length - 1);
        }
        else
        {
            return path;
        }
    }
}
