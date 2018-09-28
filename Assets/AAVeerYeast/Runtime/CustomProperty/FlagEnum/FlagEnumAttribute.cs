using UnityEngine;

public class FlagEnumAttribute : PropertyAttribute
{
    public string enumName;

    public FlagEnumAttribute() { }

    public FlagEnumAttribute(string name)
    {
        enumName = name;
    }
}
