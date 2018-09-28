using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConvertUtils
{
    public static int[] BytesToInts(byte[] bytes)
    {
        if (bytes == null) return null;
        if (bytes.Length == 0) return null;

        var size = bytes.Length / sizeof(int);
        var ints = new int[size];
        Buffer.BlockCopy(bytes, 0, ints, 0, ints.Length);

        return ints;
    }
}
