using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FastJsonTest
{
    // 关于 FastJson 的局限性
    // 1.序列化 包含循环引用的对象 会引发崩溃 Test_Example1
    // 2.反序列化 不会恢复 相同引用 Test_Example3
    // 3.List<BaseClass> 反序列化无法自动识别为子类 Test_Example4

    // 序列化形式 1 : SimpleJson 由 SimpleJsonAttribute 控制序列化内容及别名
    [JsonTarget]
    public class ClassA
    {
        private string a_tag = "I am A";

        [JsonTarget("private-string")]
        private string a_string = "this is A string";

        [JsonTarget]
        private float a_float = 404.404f;

        [JsonTarget("my-a")]
        private ClassA a_next_a = null;

        [JsonTarget]
        private ClassB a_next_b = null;

        [JsonTarget("my-c")]
        private ClassC a_next_c = null;

        private int a_int = 65535;

        private bool a_bool = true;

        private GameObject a_gameobj = null;

        public void SetA(ClassA InClassA)
        {
            a_next_a = InClassA;
        }

        public void SetB(ClassB InClassB)
        {
            a_next_b = InClassB;
        }

        public void SetC(ClassC InClassC)
        {
            a_next_c = InClassC;
        }

        public void SetGameObject(GameObject InGameObject)
        {
            a_gameobj = InGameObject;
        }
    }

    // 序列化形式 2 : Fast Json 完全深度序列化
    public class ClassB
    {
        string b_tag = "I am B";

        string b_string = "this is B string";

        int b_int = 65535;

        float b_float = 404.404f;

        bool b_bool = true;
    }

    // 序列化形式 3 : 完全自定义
    // 该例子的 反序列化 会失败，反序列化时将找不到字段 my_glorious_c_tag
    public class ClassC
    {
        public string c_tag = "I am C";
        public string c_string = "this is C string";
        public int c_int = 65535;
        public float c_float = 404.404f;

        public static void ToJson(StringBuilder sb, object obj)
        {
            if (obj != null)
            {
                sb.Append("{\"my_glorious_c_tag\":\"" + (obj as ClassC).c_tag + "\"}");
            }
            else
            {
                sb.Append("null");
            }
        }
    }

    // 测试 List Array Dic 的序列化
    [JsonTarget]
    public class ClassD
    {
        [JsonTarget("c_list")]
        public List<ClassC> class_c_list;

        [JsonTarget("c_array")]
        public ClassC[] class_c_array;

        [JsonTarget("c_dic")]
        public Dictionary<string, ClassC> class_c_dic;
    }

    // 测试 子类反序列化
    public class ClassE
    {
        public List<ClassB> class_b_list = new List<ClassB>();
    }

    public class ClassF : ClassB
    {
        public string f_tag = "I am f";
        public string f_string = "this is f string";
        public int f_int = 777;
    }

    public class ClassG : ClassB
    {
        public string g_tag = "I am g";
    }

    // 测试 反序列化 JSONObject
    [JsonTarget]
    public class ClassH
    {
        [JsonTarget("h_json")]
        public JSONObject json;
    }

    // 测试 反序列化 JSONObject 暂存为 string
    public class ClassI
    {
        public string str;
    }

    public class FastJsonTest : GUITestBase
    {
        private void Test_Example1()
        {
            ClassA tempA = new ClassA();

            // 循环嵌套会直接造成崩溃
            //tempA.SetNext(tempA);

            ClassA tempAnotherA = new ClassA();
            tempA.SetA(tempAnotherA);

            ClassB tempB = new ClassB();
            tempA.SetB(tempB);

            ClassC tempC = new ClassC();
            tempA.SetC(tempC);

            tempA.SetGameObject(this.gameObject);

            string jsonStr = FastJson.Serialize(tempA);
            Debug.Log(jsonStr);
        }

        private void Test_Example2()
        {
            ClassA tempA = new ClassA();

            // 循环嵌套会直接造成崩溃
            //tempA.SetNext(tempA);

            ClassA tempAnotherA = new ClassA();
            tempA.SetA(tempAnotherA);

            ClassB tempB = new ClassB();
            tempA.SetB(tempB);

            ClassC tempC = new ClassC();
            tempA.SetC(tempC);

            tempA.SetGameObject(this.gameObject);

            // 测试 反序列化
            string jsonStr = FastJson.Serialize(tempA);
            ClassA deserializeA = FastJson.Deserialize<ClassA>(jsonStr);

            Debug.Log(FastJson.Serialize(deserializeA));
        }

        private void Test_Example3()
        {
            ClassD tempD = new ClassD();
            tempD.class_c_list = new List<ClassC>();
            tempD.class_c_array = new ClassC[3];
            tempD.class_c_dic = new Dictionary<string, ClassC>();

            ClassC c1 = new ClassC();
            c1.c_tag = "c1";
            tempD.class_c_list.Add(c1);
            tempD.class_c_array[0] = c1;
            tempD.class_c_dic.Add("c0", c1);

            ClassC c2 = new ClassC();
            c2.c_tag = "c2";
            tempD.class_c_list.Add(c2);
            tempD.class_c_array[1] = c2;
            tempD.class_c_dic.Add("c1", c2);

            ClassC c3 = new ClassC();
            c3.c_tag = "c3";
            tempD.class_c_list.Add(c3);
            tempD.class_c_array[2] = c3;
            tempD.class_c_dic.Add("c2", c3);

            string jsonStr = FastJson.Serialize(tempD);
            Debug.Log(jsonStr);

            // 反序列化后其 list array dic 内容不再是相同引用，考虑限制 List 和 Array 的反序列化
            ClassD deserializeD = FastJson.Deserialize<ClassD>(jsonStr);
            Debug.Log(FastJson.Serialize(deserializeD));
        }

        private void Test_Example4()
        {
            ClassE e = new ClassE();
            ClassB b = new ClassB();
            ClassF f = new ClassF();
            ClassG g = new ClassG();

            e.class_b_list.Add(b);
            e.class_b_list.Add(f);
            e.class_b_list.Add(g);

            // 测试 反序列化
            string jsonStr = FastJson.Serialize(e);
            Debug.Log(jsonStr);
            ClassE deserializeE = FastJson.Deserialize<ClassE>(jsonStr);
            Debug.Log(FastJson.Serialize(deserializeE));
        }

        private void Test_Example5()
        {
            Debug.Log("Test_Example5 begin ...");

            // 测试 Error Json
            ClassA a = null;
            try
            {
                FastJson.Deserialize<ClassA>("error json string ...");
                Debug.Log("deserialize success ...");
            }
            catch
            { Debug.Log("deserialize failed ..."); }

            if (a == null)
                Debug.Log("when given error json ClassA is null...");
            else
                Debug.Log("when given error json ClassA is new ClassA ...");

            // 测试 Default(T)
            a = null;
            try
            {
                FastJson.Deserialize<ClassA>("{}");
                Debug.Log("deserialize success ...");
            }
            catch
            { Debug.Log("deserialize failed ..."); }

            if (a == null)
                Debug.Log("default ClassA is null...");
            else
                Debug.Log("default ClassA is new ClassA ...");
        }


        private void Test_Example6()
        {
            string testJson = "{\"h_json\":{\"subKey1\":\"subValue1\",\"subKey2\":{\"subKey21\":\"subValue21\"},\"subKey3\":[\"subValue31\",\"subValue32\"]}}";
            ClassH h = FastJson.Deserialize<ClassH>(testJson);
            Debug.Log(h.json);
        }

        private void Test_Example7()
        {
            // 验证 MethodInfo.MakeGenericMethod() Type类驱动泛型方法
            ClassF f = new ClassF();
            f.f_int = 666;
            string jsonStr = FastJson.Serialize(f);
            Debug.Log(jsonStr);

            MethodInfo mi = typeof(FastJson).GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(ClassF));
            ClassB deserializeF = mi.Invoke(null, new object[] { jsonStr }) as ClassB;

            Debug.Log(FastJson.Serialize(deserializeF));
            Debug.Log(FastJson.Serialize(deserializeF as ClassF));
        }

        private void Test_Example8()
        {
            // 测试 反序列化 JSONObject 暂存为 string
            ClassI i = FastJson.Deserialize<ClassI>("{\"str\":{\"temp_key\":\"temp_value\"}}");
        }
    }
}