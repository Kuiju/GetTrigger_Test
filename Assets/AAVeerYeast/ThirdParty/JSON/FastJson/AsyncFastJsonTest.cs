using System.Collections;
using System.Collections.Generic;
using U3D.Threading.Tasks;
using UnityEngine;

public class AsyncFastJsonTest : GUITestBase
{
    public TextAsset Text;

    //public void Test_NormalAsyncFastJsonText()
    //{
    //    Task task = FastJson.AsyncDeserializeJson<VideoInfo>("{}", null);
    //    StartCoroutine(CoroutineUtils.WaitForFrame(1, () =>
    //    {
    //        task.CheckAbort();
    //    }));
    //}

    //public void Test_ErrorAsyncFastJsonText()
    //{
    //    Task task = FastJson.AsyncDeserializeJson<VideoInfo>("error json text", null);
    //    task.CheckAbort();
    //}

    //public void Test_AsyncDeserializeJson()
    //{
    //    Task task = FastJson.AsyncDeserializeJson<List<VideoInfo>>(Text.text, (success, videoInfoList) =>
    //    {
    //        if (!success)
    //        {
    //            return;
    //        }
    //    });
    //}
}
