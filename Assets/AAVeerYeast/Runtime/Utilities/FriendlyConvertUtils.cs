using System.Collections;
using System.Collections.Generic;

public class FriendlyConvertUtils
{
    #region Time

    public static string MillisecondsToMSString(int ms)
    {
        System.DateTime ts = new System.DateTime(0);
        ts = ts.AddMilliseconds(ms);
        return ts.ToString("mm:ss");
    }

    public static string MillisecondsToMSStringStyle2(int ms)
    {
        System.DateTime ts = new System.DateTime(0);
        ts = ts.AddMilliseconds(ms);
        return ts.Minute + "'" + ts.AddMinutes(-ts.Minute).Second + "''";
    }

    public static string SecondsToMSString(double seconds)
    {
        System.DateTime ts = new System.DateTime(0);
        ts = ts.AddSeconds(seconds);
        return ts.ToString("mm:ss");
    }

    #endregion

    #region Display Count

    public static string ToDisplayCount(ulong count)
    {
        if (count >= 1000000)
            return ((float)count / 1000000f).ToString("F1") + "m";
        else if (count >= 1000)
            return ((float)count / 1000f).ToString("F1") + "k";
        else
            return count.ToString();
    }

    public static string ToDisplayCount(int count)
    {
        if (count >= 1000000)
            return ((float)count / 1000000f).ToString("F1") + "m";
        else if (count >= 1000)
            return ((float)count / 1000f).ToString("F1") + "k";
        else
            return count.ToString();
    }

    #endregion

    #region File / Dir Size

    public const float KB_SIZE = 1024.0f;
    public const float MB_SIZE = 1024.0f * 1024.0f;
    public const float GB_SIZE = 1024.0f * 1024.0f * 1024.0f;

    public static string ToFriendlySizeString(int size)
    {
        return ToFriendlySizeString((long)(size));
    }

    public static string ToFriendlySizeString(long size)
    {
        if (size / KB_SIZE < 1)
        {
            return size.ToString() + "B";
        }
        if (size / MB_SIZE < 1)
        {
            return (size / KB_SIZE).ToString("F1") + "KB";
        }
        if (size / GB_SIZE < 1)
        {
            return (size / MB_SIZE).ToString("F1") + "MB";
        }
        return (size / GB_SIZE).ToString("F1") + "GB";
    }

    #endregion
}
