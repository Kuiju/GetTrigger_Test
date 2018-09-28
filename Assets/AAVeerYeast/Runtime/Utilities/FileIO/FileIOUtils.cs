using System.IO;
using UnityEngine;

namespace VeerYeast
{
    public static class FileIOUtils
    {
        public static void SaveTextFile(string fileFullPath, string str)
        {
            FileInfo t = new FileInfo(fileFullPath);
            t.Directory.Create();
            StreamWriter sw = t.CreateText();
            sw.Write(str);
            sw.Close();
            sw.Dispose();
        }

        public static bool LoadTextFile(string fileFullPath, out string rst)
        {
            FileInfo t = new FileInfo(fileFullPath);
            if (!t.Exists)
            {
                rst = null;
                return false;
            }

            StreamReader sr = File.OpenText(fileFullPath);
            rst = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return true;
        }
    }
}