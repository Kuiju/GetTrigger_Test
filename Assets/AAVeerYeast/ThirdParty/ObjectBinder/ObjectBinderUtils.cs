using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ObjectBinderUtils
{
    public static void BindObjectRecursive(Component root)
    {
        if (root == null)
        {
            VeerDebug.LogWarning(" BindObjectRecursive root is null ...");
            return;
        }

        Type rootType = root.GetType();

        FieldInfo[] fieldInfos = rootType.GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic);

        bool bFieldIsComponentOrGameObject = false;

        foreach (var f in fieldInfos)
        {
            if (f.FieldType.IsSubclassOf(typeof(Component)))
                bFieldIsComponentOrGameObject = true;
            else if (f.FieldType.Equals(typeof(GameObject)))
                bFieldIsComponentOrGameObject = false;
            else
                continue;

            foreach (var attr in f.GetCustomAttributes(true))
            {
                if (attr is ObjectBinderAttribute)
                {
                    f.SetValue(root, null);

                    ObjectBinderAttribute objectBinder = attr as ObjectBinderAttribute;

                    string attrObjectName = objectBinder.ObjectName;
                    if (string.IsNullOrEmpty(attrObjectName))
                    {
                        VeerDebug.LogWarning(" ObjectBinderAttribute attrObjectName is null or empty ... " + f.Name);
                        continue;
                    }

#if UNITY_EDITOR
                    if (!attrObjectName.StartsWith(VeerYeastConst.Auto_Bind_Tag))
                    {
                        VeerDebug.LogError(" ObjectBinderAttribute attrObjectName is not start with " + VeerYeastConst.Auto_Bind_Tag + " ... " + f.Name);
                        continue;
                    }
#endif

                    GameObject targetGameObject = root.gameObject.GetChildGoByName(attrObjectName, true);
                    if (targetGameObject == null)
                    {
                        VeerDebug.LogWarning(" ObjectBinderAttribute targetGameObject is null ... " + f.Name);
                        continue;
                    }

                    if (!bFieldIsComponentOrGameObject)
                    {
                        f.SetValue(root, targetGameObject);
                    }
                    else
                    {
                        Component targetMonoBehaviour = targetGameObject.GetComponent(f.FieldType) as Component;
                        if (targetMonoBehaviour == null)
                        {
                            VeerDebug.LogWarning(" ObjectBinderAttribute targetMonoBehaviour is null ... " + f.Name);
                            continue;
                        }

                        f.SetValue(root, targetMonoBehaviour);
                    }
                }

                if (attr is ObjectBinderRootAttribute)
                {
                    Component rootMonoBehaviour = f.GetValue(root) as Component;
                    if (rootMonoBehaviour == null)
                    {
#if UNITY_EDITOR
                        VeerDebug.LogWarning(" ObjectBinderRootAttribute root is null ... " + f.Name);
#endif
                        continue;
                    }

                    BindObjectRecursive(rootMonoBehaviour);
                }
            }
        }
    }

    public static string GetObjectBinderAttributeObjectName(FieldInfo fieldInfo)
    {
        string objectName = "";
        foreach (var it in fieldInfo.GetCustomAttributes(true))
        {
            if (it is ObjectBinderAttribute)
            {
                string attrObjectName = (it as ObjectBinderAttribute).ObjectName;
                objectName = attrObjectName;
                break;
            }
        }
        return objectName;
    }
}
