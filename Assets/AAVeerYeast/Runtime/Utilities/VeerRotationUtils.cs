using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VeerRotationUtils
{
    public static Quaternion QuaternionFromList(List<float> list)
    {
        if (list == null)
            return Quaternion.identity;
        if (list.Count == 3)
            return QuaternionFromEulerList(list);
        if (list.Count == 4)
            return QuaternionFromQuaternionList(list);
        return Quaternion.identity;
    }

    public static Quaternion QuaternionFromEulerList(List<float> list)
    {
        if (list == null || list.Count != 3)
            return Quaternion.identity;
        return Quaternion.Euler(list[0], list[1], list[2]);
    }

    public static Quaternion QuaternionFromQuaternionList(List<float> list)
    {
        if (list == null || list.Count != 4)
            return Quaternion.identity;
        return new Quaternion(list[0], list[1], list[2], list[3]);
    }

    /// <summary>
    /// 解析形如 "0.000601,7.072153,0,YXZ" 的旋转信息
    /// </summary>
    public static Quaternion ParseWebOrientationString(string veerWebOrientationString, bool bIgnoreX = false, bool bIgnoreY = false, bool bIgnoreZ = false)
    {
        Quaternion result = Quaternion.identity;
        if (string.IsNullOrEmpty(veerWebOrientationString))
            return result;

        try
        {
            string[] str = veerWebOrientationString.Split(new char[] { ',' });
            if (str.Length == 4)
            {
                string k = str[3].ToLower();
                Quaternion q = Quaternion.identity;
                Quaternion x = Quaternion.identity;
                Quaternion y = Quaternion.identity;
                Quaternion z = Quaternion.identity;

                x = Quaternion.AngleAxis(-float.Parse(str[0]) * Mathf.Rad2Deg, Vector3.right);
                y = Quaternion.AngleAxis(-float.Parse(str[1]) * Mathf.Rad2Deg, Vector3.up);
                z = Quaternion.AngleAxis(float.Parse(str[2]) * Mathf.Rad2Deg, Vector3.forward);

                for (int i = 0; i < 3; i++)
                {
                    if (k[i] == 'x' && !bIgnoreX)
                        q *= x;
                    else if (k[i] == 'y' && !bIgnoreY)
                        q *= y;
                    else if (k[i] == 'z' && !bIgnoreZ)
                        q *= z;
                }

                // songlingyi
                // Web 端设置 Setting 初始角度 时 使用的是 Camera 方向
                // 而 VR 端使用球的旋转来设
                // 因此此处要对 Quaternion 取反
                result = Quaternion.Inverse(q);
            }
        }
        catch (System.Exception e)
        {
            VeerDebug.Log(e.Message);
            VeerDebug.Log(e.StackTrace);
            return Quaternion.identity;
        }

        return result;
    }
}
