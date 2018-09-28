using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VeerYeast;

public class JSONObjectTest : GUITestBase
{
    private void Start()
    {
        VeerDebugHelper.Instance.InitDebug();
    }

    // 正确使用 Json Array
    private void Test_Example1()
    {
        JSONObject json = JSONObject.Create();
        json.Add(true);
        json.Add(404.404f);
        json.Add(404);
        json.Add("veer");
        Debug.Log(json.ToString());
    }

    // 正确使用 目前索引器不会自动扩充 JsonArray 大小
    private void Test_Example1_2()
    {
        JSONObject json = JSONObject.Create();
        json.Add(0);
        json.Add(1);
        json.Add(2);
        json.Add(3);
        json.Add("veer_4");
        json[0] = true;
        json[1] = 404.404f;
        json[2] = 404;
        json[3] = "veer";
        Debug.Log(json.ToString());
    }

    // 错误使用 观察Debug
    private void Test_Example2()
    {
        JSONObject json = JSONObject.Create();
        json.Add(true);
        json.Add(404.404f);
        json.SetField("tK1", "tV1");
        json.SetField("tK2", "tV2");
        Debug.Log(json.ToString());
    }

    // 测试 索引运算符 []
    private void Test_Example2_2()
    {
        JSONObject json = JSONObject.Create();
        json["tK1"] = "tV1";
        json["tK2"] = true;
        json["tK3"] = 404.404f;
        json["tK4"] = 65535;
        json["tK5"] = JSONObject.Create("{\"subKey\":\"subValue\"}");
        Debug.Log("json   : " + json.ToString());

        string tV1 = json["tK1"];
        bool tV2 = json["tK2"];
        float tV3 = json["tK3"];
        int tV4 = json["tK4"];
        JSONObject tV5 = json["tK5"];

        JSONObject rejson = JSONObject.Create();
        rejson["tK1"] = tV1;
        rejson["tK2"] = tV2;
        rejson["tK3"] = tV3;
        rejson["tK4"] = tV4;
        rejson["tK5"] = tV5;
        Debug.Log("rejson : " + rejson.ToString());
    }

    // 错误使用 观察Debug
    private void Test_Example3()
    {
        JSONObject json = JSONObject.Create("{\"a\":{\"b\":{\"c\":{}}}}");
        json.Add(true);
        json.Add(404.404f);
        json.SetField("tK1", "tV1");
        json.SetField("tK2", 123);
        Debug.Log(json.ToString());
    }

    // 正确使用 观察 Json 深度
    private void Test_Example4()
    {
        JSONObject json = JSONObject.Create("{\"a\":{\"b\":{\"c\":{}}},\"d\":{\"e\":{}}}", 1);
        Debug.Log(json.ToString());

        if (json.HasField("a"))
            Debug.Log("a is exist");
        else
            Debug.Log("a is not exist");

        if (json.HasField("b"))
            Debug.Log("b is exist");
        else
            Debug.Log("b is not exist");
    }

    // 正确使用 观察 Json 深度
    private void Test_Example5()
    {
        JSONObject json = JSONObject.Create("{\"a\":{\"b\":{\"c\":{}}},\"d\":{\"e\":{}}}", 0);
        Debug.Log(json.ToString());

        if (json.HasField("a"))
            Debug.Log("a is exist");
        else
            Debug.Log("a is not exist");

        if (json.HasField("b"))
            Debug.Log("b is exist");
        else
            Debug.Log("b is not exist");
    }

    // 正确使用
    private void Test_Example6()
    {
        JSONObject json = JSONObject.Create("[1,2,3]");
        json.Add(4);
        json.Add(5);
        json.Add(6);
        Debug.Log(json.ToString());
    }

    // 错误使用 观察Debug
    private void Test_Example7()
    {
        JSONObject json = JSONObject.Create("[1,2,3]");
        json.Add(4);
        json.Add(5);
        json.Add(6);
        json.SetField("tK1", 7);
        json.SetField("tK2", 8);
        Debug.Log(json.ToString());
    }

    // ------------ 测试 转义 & 去转义 ------------

    public class TestJsonConvert
    {
        public string StringField;
    }

    public TextAsset OuterJsonText = null;

    public static string TestInnerJsonText = "string with \\n as \n and a c# \\\" as \" and a c# \\\\ as \\";
    public string TestEscapeJsonText { get { return JsonUtils.JsonStringEscape(TestInnerJsonText); } }
    public string TestEscapeUnescapeJsonText { get { return JsonUtils.JsonStringUnescape(TestEscapeJsonText); } }

    public string TestOuterJsonText { get { return OuterJsonText.text; } }
    public string TestEscapeOuterJsonText { get { return JsonUtils.JsonStringEscape(TestOuterJsonText); } }
    public string TestUnescapeOuterJsonText { get { return JsonUtils.JsonStringUnescape(TestOuterJsonText); } }

    private void Test_1_InnerEqualOuter()
    {
        VeerDebug.Log("InnerEqualOuter : " + (TestInnerJsonText == TestOuterJsonText).ToString());
    }

    private void Test_2_ShowInnerJsonText()
    {
        VeerDebug.Log("Inner : " + TestInnerJsonText);
    }

    private void Test_3_ShowEscapedJsonText()
    {
        VeerDebug.Log("Inner Escape : " + TestEscapeJsonText);
    }

    private void Test_4_ShowEscapeUnescapeJsonText()
    {
        VeerDebug.Log("Inner Unescape : " + TestEscapeUnescapeJsonText);
    }

    private void Test_5_OriginalEqualUnescape()
    {
        string ori = TestInnerJsonText;
        string une = TestEscapeUnescapeJsonText;
        VeerDebug.Log("Original Unescape Length : " + TestInnerJsonText.Length + "  " + TestEscapeUnescapeJsonText.Length);

        for (int i = 0; i < ori.Length; i++)
            if (ori[i] != une[i])
                VeerDebug.Log(i.ToString() + " : " + ori[i] + " " + une[i]);

        VeerDebug.Log("OriginalEqualUnescape : " + (TestInnerJsonText == TestEscapeUnescapeJsonText).ToString());
    }

    private void Test_6_ShowOuterJsonText()
    {
        VeerDebug.Log("Outer : " + TestOuterJsonText);
    }

    private void Test_7_JsonObjectCreate()
    {
        JSONObject json = JSONObject.obj;
        json.AddField("StringField", TestInnerJsonText);

        string jsonString = json.ToString();
        VeerDebug.Log("json to string : " + jsonString);

        JSONObject jsonParse = JSONObject.Create(jsonString);
        VeerDebug.Log("json parsed : " + jsonParse.ToString());
    }

    private void Test_8_FastJsonMyText()
    {
        string jsonString = "{\"StringField\":\"ha\\\"ha\\\"ha\"}";
        VeerDebug.Log("json string : " + jsonString);

        TestJsonConvert test = FastJson.Deserialize<TestJsonConvert>(jsonString);
        VeerDebug.Log("fast json string field : " + test.StringField);

        string serialize = FastJson.Serialize(test);
        VeerDebug.Log("fast json serialize : " + serialize);
    }

    private void Test_9_FastJsonSerialize()
    {
        JSONObject json = JSONObject.obj;
        json.AddField("StringField", TestInnerJsonText);

        string jsonString = json.ToString();
        VeerDebug.Log("json to string : " + jsonString);

        TestJsonConvert test = FastJson.Deserialize<TestJsonConvert>(jsonString);
        VeerDebug.Log("fast json string field : " + test.StringField);

        string serialize = FastJson.Serialize(test);
        VeerDebug.Log("fast json serialize : " + serialize);
    }

    public JSONObject TestJsonByInnerJsonText { get { return JSONObject.Create(TestInnerJsonText); } }
    public JSONObject TestJsonByEscapeInnerJsonText { get { return JSONObject.Create(TestEscapeJsonText); } }
    public JSONObject TestJsonByUnescapeInnerJsonText { get { return JSONObject.Create(TestEscapeUnescapeJsonText); } }

    private void Test_JsonByInnerTextToString()
    {
        VeerDebug.Log(TestJsonByInnerJsonText.ToString());
    }

    private void Test_JsonByEscapeInnerTextToString()
    {
        VeerDebug.Log(TestJsonByEscapeInnerJsonText.ToString());
    }

    private void Test_JsonByUnescapeInnerTextToString()
    {
        VeerDebug.Log(TestJsonByUnescapeInnerJsonText.ToString());
    }

    public JSONObject TestJsonByOuterJsonText { get { return JSONObject.Create(TestOuterJsonText); } }
    public JSONObject TestJsonByEscapeOuterJsonText { get { return JSONObject.Create(TestEscapeOuterJsonText); } }
    public JSONObject TestJsonByUnescapeOuterJsonText { get { return JSONObject.Create(TestUnescapeOuterJsonText); } }

    private void Test_JsonByOuterTextToString()
    {
        VeerDebug.Log(TestJsonByOuterJsonText.ToString());
    }

    private void Test_JsonByEscapeOuterTextToString()
    {
        VeerDebug.Log(TestJsonByEscapeOuterJsonText.ToString());
    }

    private void Test_JsonByUnescapeOuterTextToString()
    {
        VeerDebug.Log(TestJsonByUnescapeOuterJsonText.ToString());
    }
}