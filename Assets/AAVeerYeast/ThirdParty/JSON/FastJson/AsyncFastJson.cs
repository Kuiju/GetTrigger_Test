using System;
using U3D.Threading.Tasks;

/// <summary>
/// Fast Json Async Partial 有错误请 @songlingyi
/// </summary>
public static partial class FastJson
{
    #region Async Deserialize Json

    public static Task AsyncDeserializeJson<T>(JSONObject json, Action<bool, T> callback, float timeout = 6f)
    {
#if UNITY_EDITOR
        if (callback == null)
        {
            VeerDebug.LogError(" AsyncDeserializeJson callback is null...");
            return null;
        }
#endif

        Task task = Task<T>.Run(() =>
        {
            return Deserialize<T>(json);
        }).ContinueInMainThreadWith((obj) =>
        {
            AsyncDeserializeJsonCallback(callback, obj);
        }, timeout);
        return task;
    }

    public static Task AsyncDeserializeJson(JSONObject json, Type type, Action<bool, object> callback, float timeout = 6f)
    {
#if UNITY_EDITOR
        if (callback == null)
        {
            VeerDebug.LogError(" AsyncDeserializeJson callback is null...");
            return null;
        }
#endif

        Task task = Task<object>.Run(() =>
        {
            return Deserialize(json, type);
        }).ContinueInMainThreadWith((obj) =>
        {
            AsyncDeserializeJsonCallback(callback, obj);
        }, timeout);
        return task;
    }

    public static Task AsyncDeserializeJson<T>(string jsonText, Action<bool, T> callback, float timeout = 6f)
    {
#if UNITY_EDITOR
        if (callback == null)
        {
            VeerDebug.LogError(" AsyncDeserializeJson callback is null...");
            return null;
        }
#endif

        Task task = Task<T>.Run(() =>
        {
            return Deserialize<T>(jsonText);
        }).ContinueInMainThreadWith((obj) =>
        {
            AsyncDeserializeJsonCallback(callback, obj);
        }, timeout);
        return task;
    }

    private static void AsyncDeserializeJsonCallback<T>(Action<bool, T> callback, Task<T> obj)
    {
        if (obj == null)
        {
#if UNITY_EDITOR
            VeerDebug.Log(" AsyncDeserializeJson failed by timeout ...");
#endif
            callback(false, default(T));
            return;
        }
        else if (obj.IsFaulted || obj.IsAborted || !obj.IsCompleted)
        {
            callback(false, default(T));
            return;
        }
        else
        {
            callback(true, obj.Result);
            return;
        }
    }

    #endregion
}
