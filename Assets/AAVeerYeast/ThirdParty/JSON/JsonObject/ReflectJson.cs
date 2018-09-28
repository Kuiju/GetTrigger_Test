using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ReflectJson
{
    // 基础类型 ToJsonObject 方法的代理
    public delegate void BasicTypeToJsonFunction(JSONObject root, string key, object value);

    // 自定义 Class ToJsonObject 方法的代理
    public delegate JSONObject ClassTypeToJsonFunction(object value);

    // Init Tag
    private static bool _Inited = false;

    // 基础类型 与 ToJson 方法的映射
    private static Dictionary<Type, BasicTypeToJsonFunction> _BasicTypeToJsonFunctionMap = new Dictionary<Type, BasicTypeToJsonFunction>();

    // 自定义类型 与 ToJson 方法的映射
    private static Dictionary<Type, ClassTypeToJsonFunction> _ClassTypeToJsonFunctionMap = new Dictionary<Type, ClassTypeToJsonFunction>();

    // 递归注册方法时 避免冲突
    private static List<Type> IsCreatingFuncTypeList = new List<Type>();

    // 扩展方法
    public static JSONObject ToJsonObject(this object obj)
    {
        return ToJsonObject_Internal(obj);
    }

    // 核心方法
    private static JSONObject ToJsonObject_Internal(object target)
    {
        if (!_Inited)
            Init();

        // 检查空值
        if (target == null)
            return null;

        // 检查是否为 类的实例
        Type targetType = target.GetType();
        if (!targetType.IsClass)
        {
            VeerDebug.LogWarning(" 不支持对非类的实例进行 ToJsonObject 操作 : " + targetType.Name);
            return null;
        }

        // register
        CheckRegister(targetType);

        // 检查是否已有 ToJson方法
        if (_ClassTypeToJsonFunctionMap.ContainsKey(targetType))
        {
            ClassTypeToJsonFunction existingFunction = _ClassTypeToJsonFunctionMap[targetType];
            if (existingFunction == null)
            {
                VeerDebug.LogWarning(" 对无法支持的类型进行 ToJsonObject 操作 : " + targetType.Name);
                return null;
            }
            // 使用已有方法进行 ToJson
            return _ClassTypeToJsonFunctionMap[targetType](target);
        }
        else
        {
            return null;
        }
    }

    private static void Init()
    {
        if (_Inited)
            return;
        _Inited = true;

        // 绑定针对 Basic 类型的 序列化方法
        _BasicTypeToJsonFunctionMap.Add(typeof(Int16), Int16ToJsonObject);
        _BasicTypeToJsonFunctionMap.Add(typeof(UInt16), UInt16ToJsonObject);
        _BasicTypeToJsonFunctionMap.Add(typeof(Int32), Int32ToJsonObject);
        _BasicTypeToJsonFunctionMap.Add(typeof(UInt32), UInt32ToJsonObject);
        _BasicTypeToJsonFunctionMap.Add(typeof(Int64), LongToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(UInt64), UInt64ToJsonObject);
        _BasicTypeToJsonFunctionMap.Add(typeof(Byte), ByteToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(SByte), SByteToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Single), FloatToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Double), DoubleToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Decimal), DecimalToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Char), CharToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Boolean), BoolToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(String), StringToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(DateTime), DateTimeToJson);

        _BasicTypeToJsonFunctionMap.Add(typeof(Vector2), Vector2ToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Vector3), Vector3ToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Vector4), Vector4ToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Quaternion), QuaternionToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Rect), RectToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Color), ColorToJson);
        _BasicTypeToJsonFunctionMap.Add(typeof(Color32), Color32ToJson);
    }

    private static ClassTypeToJsonFunction CreateTypeSerializeFunction(Type targetType)
    {
        IsCreatingFuncTypeList.Add(targetType);

        ClassTypeToJsonFunction toJsonObjectFunction = null;

        if (targetType.IsEnum)
        {
#if UNITY_EDITOR
            VeerDebug.LogWarning(" 目前不支持 Enum 类型 ToJsonObject ...");
#endif
            toJsonObjectFunction = null;
        }
        else if (targetType.IsArray || targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
        {
            toJsonObjectFunction = CreateTypeSerializeFunctionForArrayList(targetType);
        }
        else if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            toJsonObjectFunction = CreateTypeSerializeFunctionForDictionary(targetType);
        }
        else if (targetType.IsClass)
        {
            toJsonObjectFunction = CreateTypeSerializeFunctionForClass(targetType);
        }

        // 注册方法
        RegisterClassTypeSerializeFunction(targetType, toJsonObjectFunction);

        IsCreatingFuncTypeList.Remove(targetType);

        return toJsonObjectFunction;
    }

    private static ClassTypeToJsonFunction CreateTypeSerializeFunctionForArrayList(Type targetType)
    {
        // 1.check type
        Type arrayListElementType = null;
        if (targetType.IsArray)
            arrayListElementType = targetType.GetElementType();
        else if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
            arrayListElementType = targetType.GetGenericArguments()[0];
        if (arrayListElementType == null)
        {
            VeerDebug.LogWarning(" CreateTypeSerializeFunctionForArrayList but target type is not arrar or list : " + targetType);
            return null;
        }

        // 2.check register element type
        CheckRegister(arrayListElementType);

        // 3.create function
        ClassTypeToJsonFunction function = (object arrayListObj) =>
        {
            JSONObject jsonArray = JSONObject.arr;

            if (arrayListObj == null)
                return jsonArray;

            IEnumerable elementArray = arrayListObj as IEnumerable;

            if (_BasicTypeToJsonFunctionMap.ContainsKey(arrayListElementType))
            {
                foreach (var element in elementArray)
                    _BasicTypeToJsonFunctionMap[arrayListElementType](jsonArray, null, element);
            }
            else if (_ClassTypeToJsonFunctionMap.ContainsKey(arrayListElementType))
            {
                ClassTypeToJsonFunction toJsonFunc = _ClassTypeToJsonFunctionMap[arrayListElementType];
                if (toJsonFunc != null)
                    foreach (var element in elementArray)
                        jsonArray.Add(toJsonFunc(element));
            }

            return jsonArray;
        };

        return function;
    }

    private static ClassTypeToJsonFunction CreateTypeSerializeFunctionForDictionary(Type targetType)
    {
        // 1.check type
        if (!targetType.IsGenericType || targetType.GetGenericTypeDefinition() != typeof(Dictionary<,>))
        {
            VeerDebug.LogWarning(" CreateTypeSerializeFunctionForDictionary but target type is not dictionary : " + targetType);
            return null;
        }

        // 2.check dictionary key type is string
        Type keyType = targetType.GetGenericArguments()[0];
        if (keyType != typeof(string))
        {
            VeerDebug.LogWarning(" ToJsonObject 不支持 KeyType 不是 string 的 Dictionary : " + targetType);
            return null;
        }

        // 3.check register value type
        Type dicValueType = targetType.GetGenericArguments()[1];
        CheckRegister(dicValueType);

        // 4.create function
        ClassTypeToJsonFunction function = (object dictionaryObj) =>
        {
            JSONObject jsonDictionary = JSONObject.obj;

            if (dictionaryObj == null)
                return jsonDictionary;

            IDictionary iDictionary = (IDictionary)dictionaryObj;
            IDictionaryEnumerator enumerator = iDictionary.GetEnumerator();

            if (_BasicTypeToJsonFunctionMap.ContainsKey(dicValueType))
            {
                while (enumerator.MoveNext())
                    _BasicTypeToJsonFunctionMap[dicValueType](jsonDictionary, enumerator.Key.ToString(), enumerator.Value);
            }
            else if (_ClassTypeToJsonFunctionMap.ContainsKey(dicValueType))
            {
                ClassTypeToJsonFunction toJsonFunc = _ClassTypeToJsonFunctionMap[dicValueType];
                if (toJsonFunc != null)
                    while (enumerator.MoveNext())
                        jsonDictionary.AddField(enumerator.Key.ToString(), toJsonFunc(enumerator.Value));
            }

            return jsonDictionary;
        };

        return function;
    }

    private static ClassTypeToJsonFunction CreateTypeSerializeFunctionForClass(Type targetType)
    {
        // 1.check type
        if (!targetType.IsClass)
        {
            VeerDebug.LogWarning(" CreateTypeSerializeFunctionForClass but target type is not Class : " + targetType);
            return null;
        }

        // 2.找到全部 targetType 需要 ToJson 的字段
        List<FieldInfo> needToJsonFieldInfos = new List<FieldInfo>();
        BindingFlags flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        foreach (FieldInfo field in targetType.GetFields(flag))
        {
            Type fieldType = field.GetType();
            if (_BasicTypeToJsonFunctionMap.ContainsKey(fieldType))
            {
                needToJsonFieldInfos.Add(field);
            }
            else if (fieldType.IsClass)
            {
                needToJsonFieldInfos.Add(field);
            }
        }

        // 3.register all types
        foreach (FieldInfo field in needToJsonFieldInfos)
        {
            Type fieldType = field.FieldType;
            if (!IsCreatingFuncTypeList.Contains(fieldType))
                CheckRegister(fieldType);
        }

        // 4.create function
        ClassTypeToJsonFunction function = (object classInstance) =>
        {
            JSONObject json = JSONObject.obj;

            if (classInstance == null)
                return JSONObject.nullJO;

            foreach (var field in needToJsonFieldInfos)
            {
                object fieldValue = field.GetValue(classInstance);
                Type fieldType = field.FieldType;

                if (_BasicTypeToJsonFunctionMap.ContainsKey(fieldType))
                {
                    _BasicTypeToJsonFunctionMap[fieldType](json, field.Name, fieldValue);
                }
                else if (_ClassTypeToJsonFunctionMap.ContainsKey(fieldType))
                {
                    ClassTypeToJsonFunction toJsonFunc = _ClassTypeToJsonFunctionMap[fieldType];
                    if (toJsonFunc != null)
                        json.AddField(field.Name, toJsonFunc(fieldValue));
                }
            }

            return json;
        };

        return function;
    }

    private static void CheckRegister(Type targetType)
    {
        if (_BasicTypeToJsonFunctionMap.ContainsKey(targetType) ||
            _ClassTypeToJsonFunctionMap.ContainsKey(targetType))
            return;

        CreateTypeSerializeFunction(targetType);

#if UNITY_EDITOR
        if (!_ClassTypeToJsonFunctionMap.ContainsKey(targetType))
            VeerDebug.LogWarning(" CreateTypeSerializeFunction 不支持该类型的 ToJsonObject 方法 : " + targetType);
#endif
    }

    public static void RegisterClassTypeSerializeFunction(Type classType, ClassTypeToJsonFunction classTypeSerializeFunction, bool bOverride = false)
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
            _ClassTypeToJsonFunctionMap.SetAddValue(classType, classTypeSerializeFunction);
        }
        else
        {
            if (_ClassTypeToJsonFunctionMap.ContainsKey(classType))
                return;
            else
                _ClassTypeToJsonFunctionMap.Add(classType, classTypeSerializeFunction);
        }
    }

    #region Basic Type To Json

    public static void Int16ToJsonObject(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Int16))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (Int16)value);
        else if (root.IsArray)
            root.Add((Int16)value);
    }

    public static void UInt16ToJsonObject(JSONObject root, string key, object value)
    {
        if (value == null || !(value is UInt16))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (UInt16)value);
        else if (root.IsArray)
            root.Add((UInt16)value);
    }

    public static void Int32ToJsonObject(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Int32))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (Int32)value);
        else if (root.IsArray)
            root.Add((Int32)value);
    }

    public static void UInt32ToJsonObject(JSONObject root, string key, object value)
    {
        if (value == null || !(value is UInt32))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (UInt32)value);
        else if (root.IsArray)
            root.Add((UInt32)value);
    }

    public static void UInt64ToJsonObject(JSONObject root, string key, object value)
    {
        if (value == null || !(value is UInt64))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (UInt64)value);
        else if (root.IsArray)
            root.Add((UInt64)value);
    }

    public static void FloatToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is float))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (float)value);
        else if (root.IsArray)
            root.Add((float)value);
    }

    public static void LongToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is long))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (long)value);
        else if (root.IsArray)
            root.Add((int)value);
    }

    public static void DoubleToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is double))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (double)value);
        else if (root.IsArray)
            root.Add((double)value);
    }

    public static void BoolToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is bool))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (bool)value);
        else if (root.IsArray)
            root.Add((bool)value);
    }

    public static void ByteToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Byte))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (Byte)value);
        else if (root.IsArray)
            root.Add((Byte)value);
    }

    public static void SByteToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is SByte))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (SByte)value);
        else if (root.IsArray)
            root.Add((SByte)value);
    }

    public static void CharToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Char))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, (Char)value + "");
        else if (root.IsArray)
            root.Add((SByte)value + "");
    }

    public static void DecimalToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Decimal))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, Double.Parse(value.ToString()));
        else if (root.IsArray)
            root.Add(Double.Parse(value.ToString()));
    }

    public static void Vector2ToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Vector2))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Vector2)value).x);
            jSON.Add(((Vector2)value).y);
            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Vector2)value).x);
            jSON.Add(((Vector2)value).y);
            root.Add(jSON);
        }
    }

    public static void Vector3ToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Vector3))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Vector3)value).x);
            jSON.Add(((Vector3)value).y);
            jSON.Add(((Vector3)value).z);
            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Vector3)value).x);
            jSON.Add(((Vector3)value).y);
            jSON.Add(((Vector3)value).z);
            root.Add(jSON);
        }
    }

    public static void Vector4ToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Vector4))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Vector4)value).x);
            jSON.Add(((Vector4)value).y);
            jSON.Add(((Vector4)value).z);
            jSON.Add(((Vector4)value).w);

            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Vector4)value).x);
            jSON.Add(((Vector4)value).y);
            jSON.Add(((Vector4)value).z);
            jSON.Add(((Vector4)value).w);

            root.Add(jSON);
        }
    }

    public static void QuaternionToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Quaternion))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Quaternion)value).x);
            jSON.Add(((Quaternion)value).y);
            jSON.Add(((Quaternion)value).z);
            jSON.Add(((Quaternion)value).w);

            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Quaternion)value).x);
            jSON.Add(((Quaternion)value).y);
            jSON.Add(((Quaternion)value).z);
            jSON.Add(((Quaternion)value).w);
            root.Add(jSON);
        }
    }

    public static void RectToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Rect))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Rect)value).x);
            jSON.Add(((Rect)value).y);
            jSON.Add(((Rect)value).width);
            jSON.Add(((Rect)value).height);

            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Rect)value).x);
            jSON.Add(((Rect)value).y);
            jSON.Add(((Rect)value).width);
            jSON.Add(((Rect)value).height);
            root.Add(jSON);
        }
    }

    public static void ColorToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Color))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Color)value).a);
            jSON.Add(((Color)value).b);
            jSON.Add(((Color)value).g);
            jSON.Add(((Color)value).r);

            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Color)value).a);
            jSON.Add(((Color)value).b);
            jSON.Add(((Color)value).g);
            jSON.Add(((Color)value).r);
            root.Add(jSON);
        }
    }

    public static void Color32ToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is Color32))
            return;

        if (root.IsObject && !string.IsNullOrEmpty(key))
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Color32)value).a);
            jSON.Add(((Color32)value).b);
            jSON.Add(((Color32)value).g);
            jSON.Add(((Color32)value).r);

            root.AddField(key, jSON);
        }
        else if (root.IsArray)
        {
            JSONObject jSON = JSONObject.arr;
            jSON.Add(((Color32)value).a);
            jSON.Add(((Color32)value).b);
            jSON.Add(((Color32)value).g);
            jSON.Add(((Color32)value).r);
            root.Add(jSON);
        }
    }

    public static void StringToJson(JSONObject root, string key, object value)
    {
        if (value != null && !(value is string))
            return;

        string realValue = value as string;
        if (string.IsNullOrEmpty(realValue))
            realValue = "";

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, realValue);
        else if (root.IsArray)
            root.Add(realValue);
    }

    public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public static void DateTimeToJson(JSONObject root, string key, object value)
    {
        if (value == null || !(value is DateTime))
            return;

        DateTime dateTime = (DateTime)value;

        if (root.IsObject && !string.IsNullOrEmpty(key))
            root.AddField(key, dateTime.ToString(DATE_TIME_FORMAT));
        else if (root.IsArray)
            root.Add(dateTime.ToString(DATE_TIME_FORMAT));
    }

    #endregion
}