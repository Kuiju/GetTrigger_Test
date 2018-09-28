using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class ObjectDefaultReset
{
    public delegate void ClassDefaultResetFunc(object obj);

    private static bool _Inited = false;

    private static Dictionary<Type, ClassDefaultResetFunc> _ClassDefaultResetFuncMap = new Dictionary<Type, ClassDefaultResetFunc>();

    // 扩展方法
    public static void DefaultReset(this object obj) { DefaultReset_Internal(obj); }

    // 核心方法
    private static void DefaultReset_Internal(object obj)
    {
        if (!_Inited)
            Init();

        // 检查空值
        if (obj == null)
            return;

        Type targetType = null;

        // 检查是否为 Type 类型
        if (obj is Type)
        {
            targetType = obj as Type;

            // How to tell if a Type is a static class?
            // https://stackoverflow.com/questions/4145072/how-to-tell-if-a-type-is-a-static-class
            if (!targetType.IsClass || !targetType.IsAbstract || !targetType.IsSealed)
            {
                VeerDebug.LogWarning(" 不能对 非静态类 进行 DefaultReset_Internal(Type): " + targetType.Name);
                return;
            }
        }
        else
        {
            // 检查是否为 类的实例
            targetType = obj.GetType();
            if (!targetType.IsClass)
            {
                VeerDebug.LogWarning(" 不支持对非类的实例进行 Default Reset 操作 : " + targetType.Name);
                return;
            }
        }

        CheckRegister(targetType);

        // 检查是否已有 DefaultReset Func
        if (_ClassDefaultResetFuncMap.ContainsKey(targetType))
        {
            ClassDefaultResetFunc existingFunction = _ClassDefaultResetFuncMap[targetType];
            if (existingFunction == null)
            {
                VeerDebug.LogWarning(" 对无法支持的类型进行 DefaultReset 操作 : " + targetType.Name);
                return;
            }

            // 使用已有方法进行 DefaultReset
            existingFunction(obj);
        }
    }

    private static void Init()
    {
        if (_Inited)
            return;
        _Inited = true;
    }

    private static void CheckRegister(Type targetType)
    {
        if (_ClassDefaultResetFuncMap.ContainsKey(targetType))
            return;

        CreateTypeDefaultResetFunc(targetType);

#if UNITY_EDITOR
        if (!_ClassDefaultResetFuncMap.ContainsKey(targetType))
            VeerDebug.LogWarning(" ClassDefaultResetFunc 不支持该类型的 DefaultReset 方法 : " + targetType);
#endif
    }

    private static ClassDefaultResetFunc CreateTypeDefaultResetFunc(Type targetType)
    {
        ClassDefaultResetFunc defaultResetFunc = null;

        if (!targetType.IsClass)
        {
            RegisterClassTypeSerializeFunction(targetType, defaultResetFunc);
            return defaultResetFunc;
        }

        BindingFlags flag = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // get need reset field
        Dictionary<FieldInfo, object> resetFieldDefaultValueMap = new Dictionary<FieldInfo, object>();
        foreach (FieldInfo field in targetType.GetFields(flag))
        {
            object[] resetAttrs = field.GetCustomAttributes(typeof(DefaultResetAttribute), false);
            if (resetAttrs.Length == 0)
                continue;

            DefaultResetAttribute resetAttr = resetAttrs[0] as DefaultResetAttribute;
            Type fieldType = field.FieldType;

            if (resetAttr.DefaultResetValue != null)
                resetFieldDefaultValueMap.Add(field, resetAttr.DefaultResetValue);
            else
                resetFieldDefaultValueMap.Add(field, fieldType.TypeDefault());
        }

        // get need reset property
        Dictionary<PropertyInfo, object> resetPropertyDefaultValueMap = new Dictionary<PropertyInfo, object>();
        foreach (PropertyInfo property in targetType.GetProperties(flag))
        {
            object[] resetAttrs = property.GetCustomAttributes(typeof(DefaultResetAttribute), false);
            if (resetAttrs.Length == 0)
                continue;

            DefaultResetAttribute resetAttr = resetAttrs[0] as DefaultResetAttribute;
            Type propertyType = property.PropertyType;

            if (resetAttr.DefaultResetValue != null)
                resetPropertyDefaultValueMap.Add(property, resetAttr.DefaultResetValue);
            else
                resetPropertyDefaultValueMap.Add(property, propertyType.TypeDefault());
        }

        defaultResetFunc = (object obj) =>
        {
            // reset field
            foreach (var p in resetFieldDefaultValueMap)
            {
                FieldInfo field = p.Key as FieldInfo;
                object fieldDefaultValue = p.Value;

                if (field.IsStatic)
                    field.SetValue(null, fieldDefaultValue);
                else
                {
                    if (obj == null)
                        continue;
                    field.SetValue(obj, fieldDefaultValue);
                }
            }

            // reset property
            foreach (var p in resetPropertyDefaultValueMap)
            {
                PropertyInfo property = p.Key as PropertyInfo;
                object propertyDefaultValue = p.Value;

                if (property.GetGetMethod().IsStatic)
                    property.SetValue(null, propertyDefaultValue, null);
                else
                {
                    if (obj == null)
                        continue;
                    property.SetValue(obj, propertyDefaultValue, null);
                }
            }
        };

        RegisterClassTypeSerializeFunction(targetType, defaultResetFunc);

        return defaultResetFunc;
    }

    public static void RegisterClassTypeSerializeFunction(Type classType, ClassDefaultResetFunc classDefaultResetFunc, bool bOverride = false)
    {
        if (classType == null || !classType.IsClass)
        {
#if UNITY_EDITOR
            VeerDebug.LogWarning(" RegisterTypeSerializeFunction Type 为 null 或不是 Class ...");
#endif
            return;
        }

        if (bOverride)
        {
            _ClassDefaultResetFuncMap.SetAddValue(classType, classDefaultResetFunc);
        }
        else
        {
            if (_ClassDefaultResetFuncMap.ContainsKey(classType))
                return;
            else
                _ClassDefaultResetFuncMap.Add(classType, classDefaultResetFunc);
        }
    }

    public static object TypeDefault(this Type targetType)
    {
        return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
    }
}