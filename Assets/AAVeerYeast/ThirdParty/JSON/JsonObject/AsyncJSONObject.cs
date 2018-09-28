using System;
using U3D.Threading.Tasks;

/// <summary>
/// JSONObject Async partial 有错误请 @songlingyi
/// </summary>
public partial class JSONObject
{
    #region Async JSONObject Create

    public delegate void JSONObjectAsyncCreateCallback(bool success, JSONObject json);
    public static Task AsyncCreate(string jsonText, JSONObjectAsyncCreateCallback callback)
    {
#if UNITY_EDITOR
        if (callback == null)
        {
            VeerDebug.LogError(" JSONObject AsyncCreate callback is null...");
            return null;
        }
#endif

        Task task = Task<JSONObject>.Run(() =>
        {
            JSONObject json = null;
            try
            {
                json = Create(jsonText);
            }
            catch
            {
                VeerDebug.LogWarning(" create json failed ...");
                return null;
            }
            return json;
        }).ContinueInMainThreadWith((obj) =>
        {
            if (obj == null)
            {
                VeerDebug.LogWarning(" async json object create timeout, maybe lost thread ...");
                callback(false, null);
                return;
            }
            else if (obj.IsFaulted || obj.IsAborted || !obj.IsCompleted)
            {
                VeerDebug.LogWarning(" async json object create timeout, maybe lost thread ...");
                callback(false, null);
                return;
            }
            else
            {
                callback(true, obj.Result);
                return;
            }
        }, 8f);
        return task;
    }

    #endregion
}
