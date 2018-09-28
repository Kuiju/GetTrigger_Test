using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

namespace VeerYeast
{
    public class EditorLibrary
    {
        public static string CreateDirectory(string path)
        {
            Console.LogWarning(Path.GetDirectoryName(path));
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            return path;
        }

        public static string GetResourcePath(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return path.Remove(0, 7);
        }
    }
}