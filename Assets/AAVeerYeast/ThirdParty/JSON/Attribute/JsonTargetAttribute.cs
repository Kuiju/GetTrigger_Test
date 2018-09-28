using System;
using System.Reflection;

public class JsonTargetAttribute : Attribute
{
    public string Name { get; set; }

    public JsonTargetAttribute() { }

    public JsonTargetAttribute(string inName)
    {
        Name = inName;
    }

    public static string GetJsonName(Type inType)
    {
        string name = string.Empty;
        foreach (var it in inType.GetCustomAttributes(true))
        {
            if (it is JsonTargetAttribute)
            {
                string attrName = (it as JsonTargetAttribute).Name;
                if (!string.IsNullOrEmpty(attrName))
                {
                    name = attrName;
                }
                break;
            }
        }
        return name;
    }

    public static string GetJsonName(FieldInfo inFieldInfo)
    {
        string name = string.Empty;
        foreach (var it in inFieldInfo.GetCustomAttributes(true))
        {
            if (it is JsonTargetAttribute)
            {
                string attrName = (it as JsonTargetAttribute).Name;
                name = attrName;
                break;
            }
        }
        return name;
    }
}
