using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static partial class FastJson
{
    //delegate object JsonObjectDeserializer(Type type, JsonObjectParser parser);

    //static Dictionary<Type, JsonObjectDeserializer> jsonObjectDeserializerCache;

    //public class JsonObjectParser
    //{
    //    //int count;
    //    //readonly int textLength;
    //    private readonly JSONObject _Json;

    //    public JsonObjectParser(JSONObject json)
    //    {
    //        _Json = json;
    //    }
    //}

    //// Use this for initialization
    //public static void Dada()
    //{
    //    JSONObject a = JSONObject.Create("{}");
    //    VideoInfo ii = Deserialize2<VideoInfo>(a);

    //    VideoInfo ii2 = Deserialize<VideoInfo>("dasdada");
    //}

    //// Update is called once per frame
    //public static void Haha()
    //{

    //}

    //public static T Deserialize2<T>(JSONObject json)
    //{
    //    JsonObjectParser parser = new JsonObjectParser(json);
    //    return (T)_DeserializeJSONObject(typeof(T), parser);
    //}


    //static object _DeserializeJSONObject(Type type, JsonObjectParser parser)
    //{
    //    JsonObjectDeserializer deserializer = _GetJsonObjectDeserializer(type);
    //    if (deserializer != null)
    //    {
    //        return deserializer(type, parser);
    //    }
    //    return _GetDefault(type);
    //}

    //static JsonObjectDeserializer _GetJsonObjectDeserializer(Type type)
    //{
    //    JsonObjectDeserializer deserializer;

    //    if (jsonObjectDeserializerCache.TryGetValue(type, out deserializer))
    //    {
    //        return deserializer;
    //    }

    //    MethodInfo method = type.GetMethod("FromJsonObject", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase, null, deserializerArgs, null);

    //    if (method != null)
    //    {
    //        deserializer = (JsonObjectDeserializer)Delegate.CreateDelegate(typeof(JsonObjectDeserializer), method);
    //    }
    //    else if (IsSimpleJsonType(type))
    //    {
    //        deserializer = JsonObjectSimpleDeserialize;
    //    }
    //    else if (!type.IsAbstract)
    //    {
    //        if (type.IsGenericType)
    //        {
    //            Type genericType = type.GetGenericTypeDefinition();
    //            Type[] args = genericType.GetGenericArguments();

    //            if (genericType == typeof(List<>))
    //            {
    //                if (_GetDeserializer(args[0]) != null)
    //                {
    //                    deserializer = _DeserializeList;
    //                }
    //            }
    //            else if (genericType == typeof(HashSet<>))
    //            {
    //                if (_GetDeserializer(args[0]) != null)
    //                {
    //                    deserializer = _DeserializeHashSet;
    //                }
    //            }
    //            else if (genericType == typeof(Dictionary<,>) || genericType == typeof(SortedDictionary<,>))
    //            {
    //                if (_GetDeserializer(args[0]) != null && _GetDeserializer(args[1]) != null)
    //                {
    //                    deserializer = _DeserializeDictionary;
    //                }
    //            }
    //            else
    //            {
    //                deserializer = _DeserializeObject;
    //            }
    //        }
    //        else if (type.IsArray)
    //        {
    //            if (_GetDeserializer(type.GetElementType()) != null)
    //            {
    //                deserializer = _DeserializeArray;
    //            }
    //        }
    //        else if (type.IsEnum)
    //        {
    //            deserializer = _DeserializeEnum;
    //        }
    //        else if (type.IsClass || type.IsValueType)
    //        {
    //            deserializer = _DeserializeObject;
    //        }
    //    }

    //    jsonObjectDeserializerCache.Add(type, deserializer);
    //    return deserializer;
    //}


    //static public object JsonObjectSimpleDeserialize(Type type, JsonObjectParser parser)
    //{
    //    return null;
    //    //if (parser.ReadIfNull())
    //    //{
    //    //    return null;
    //    //}

    //    //    Dictionary<string, FieldInfo> fieldInfoDic = new Dictionary<string, FieldInfo>();
    //    //    FieldInfo[] fieldInfos = type.GetFields(BINDING_FLAGS);
    //    //    int fieldCount = fieldInfos.Length;
    //    //    for (int i = 0; i < fieldCount; i++)
    //    //    {
    //    //        FieldInfo fieldInfo = fieldInfos[i];
    //    //        if (!fieldInfo.IsDefined(typeof(SimpleJsonAttribute), true))
    //    //        {
    //    //            continue;
    //    //        }

    //    //        string name = SimpleJsonAttribute.GetName(fieldInfo);
    //    //        if (string.IsNullOrEmpty(name))
    //    //        {
    //    //            name = fieldInfo.Name;
    //    //        }

    //    //        fieldInfoDic.Add(name, fieldInfo);
    //    //    }

    //    //    parser.ReadSpecifyChar('{');

    //    //    object obj = Activator.CreateInstance(type);

    //    //    while (true)
    //    //    {
    //    //        char ch = parser.PeekWithTrim();

    //    //        if (ch == '"')
    //    //        {
    //    //            string serializedFieldName = parser.ReadString();
    //    //            parser.ReadSpecifyChar(':');

    //    //            FieldInfo fieldInfo = null;

    //    //            if (fieldInfoDic.ContainsKey(serializedFieldName))
    //    //            {
    //    //                fieldInfo = fieldInfoDic[serializedFieldName];
    //    //                fieldInfoDic.Remove(serializedFieldName);
    //    //            }

    //    //            if (fieldInfo != null)
    //    //            {
    //    //                fieldInfo.SetValue(obj, _Deserialize(fieldInfo.FieldType, parser));
    //    //            }
    //    //            else
    //    //            {
    //    //                parser.SkipValue();
    //    //            }

    //    //            ch = parser.ReadWithTrim();

    //    //            if (ch == '}')
    //    //            {
    //    //                break;
    //    //            }
    //    //            else if (ch == ',')
    //    //            {
    //    //                continue;
    //    //            }
    //    //        }
    //    //        else if (ch == '}')
    //    //        {
    //    //            parser.Read();
    //    //            break;
    //    //        }

    //    //        goto failed;
    //    //    }

    //    //    return obj;

    //    //failed:
    //    //throw parser.CreateFormatException();
    //}


}
