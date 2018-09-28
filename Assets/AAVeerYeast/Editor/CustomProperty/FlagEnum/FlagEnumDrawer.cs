using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FlagEnumAttribute))]
public class FlagEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FlagEnumAttribute flagEnumAttribute = (FlagEnumAttribute)attribute;

        string propName = flagEnumAttribute.enumName;
        if (string.IsNullOrEmpty(propName))
        {
            propName = property.name;
        }

        property.intValue = EditorGUI.MaskField(position, propName, property.intValue, property.enumDisplayNames);
    }
}
