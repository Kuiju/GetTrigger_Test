using System.Collections;
using System.Collections.Generic;

public static class JsonUtils
{
    public static string JsonStringEscape(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        // songlingyi 不要调整转义顺序
        s = s
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r");
        return s;
    }

    public static string JsonStringUnescape(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        // songlingyi 不要调整转义顺序
        s = s
            // 解决 特殊字符
            .Replace("\\\\u0026", "&")
            // 逆序 去转义
            .Replace("\\r", "\r")
            .Replace("\\n", "\n")
            .Replace("\\\"", "\"")
            .Replace("\\\\", "\\")
            // 解决 去转义 的次序问题 ( 如 \\n 中间的 \ 被错误地向后组合为 \n )
            .Replace("\\\r", "\\r")
            .Replace("\\\n", "\\n")
            // 解决奇怪的临时问题 (目前 Downloaded Content 的 Url 解析经常出现 \/)
            .Replace("\\/", "/");
        return s;
    }
}
