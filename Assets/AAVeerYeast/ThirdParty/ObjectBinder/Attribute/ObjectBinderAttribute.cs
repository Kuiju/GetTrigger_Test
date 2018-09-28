using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectBinderRootAttribute : Attribute
{
}

public class ObjectBinderAttribute : Attribute
{
    public string ObjectName { get; private set; }
    public bool bNullable { get; private set; }

#if UNITY_EDITOR
    private const string _StartWith = "bind_";
#endif

    public ObjectBinderAttribute(string objectName, bool nullable = false)
    {

#if UNITY_EDITOR
        if (!objectName.StartsWith(_StartWith))
            VeerDebug.LogError(" ObjectBinderAttribute objectName not start with " + _StartWith);
#endif

        ObjectName = objectName;
        bNullable = nullable;
    }
}
