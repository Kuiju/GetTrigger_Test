using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using UnityEngine;

public class TestSimpleHttpReq : MonoBehaviour
{
    SimpleHttpReq req = null;

    private void OnGUI()
    {
        if (GUILayout.Button("Check"))
        {
            if (req == null)
            {
                VeerDebug.Log("is null");
            }
            else
            {
                VeerDebug.Log("is exist");
            }
        }
        if (GUILayout.Button("Create"))
        {
            req = new SimpleHttpReq(new System.Uri("https://qvcdn.veervr.tv/ie/scene2.jpg?sign=efb3c091d9ed64c5d4c5219ede1934e3&t=5dfc0f00"), (result, res, reqId) =>
            {
                if (result == SimpleHttpResult.Abort)
                {
                    VeerDebug.Log("callback abort");
                }
                else if (result == SimpleHttpResult.FailedForNetwork)
                {
                    VeerDebug.Log("callback failed");
                }
                else if (result == SimpleHttpResult.Success)
                {
                    VeerDebug.Log("callback success");
                }
            });
        }
        if (GUILayout.Button("Send"))
        {
            req.Send();
        }
        if (GUILayout.Button("Abort"))
        {
            req.Abort();
        }
    }
}
