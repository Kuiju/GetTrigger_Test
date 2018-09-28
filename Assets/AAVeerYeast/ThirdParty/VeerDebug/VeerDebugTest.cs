using System.Collections;
using System.Collections.Generic;
using VeerYeast;

public class VeerDebugTest : GUITestBase
{
    public void Test_InitVeerDebug()
    {
        VeerDebugHelper.Instance.InitDebug();

        VeerDebug.SetDebugLogMode(VeerDebug.LogMode.ForbidTagMode);

        VeerDebug.AddStandaloneCacheTag("TagA");
        //VeerDebug.AddStandaloneCacheTag("TagB");
        VeerDebug.AddStandaloneCacheTag("TagC");
        VeerDebug.AddStandaloneCacheTag("TagD");

        VeerDebug.AddForbidTag("TagD");
    }

    public void Test_Debug()
    {
        VeerDebug.Log(GetRandomString());
        VeerDebug.Log(GetRandomString());
        VeerDebug.Log("TagA", GetRandomString());
        VeerDebug.Log("TagA", GetRandomString());
        VeerDebug.Log("TagB", GetRandomString());
        VeerDebug.Log("TagB", GetRandomString());
        VeerDebug.Log("TagC", GetRandomString());
        VeerDebug.Log("TagC", GetRandomString());
        VeerDebug.Log("TagD", GetRandomString());
        VeerDebug.Log("TagD", GetRandomString());
    }

    public string GetRandomString()
    {
        return UnityEngine.Random.Range(10001, 99999).ToString();
    }
}
