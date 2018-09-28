using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JSONObjectUtilsTest
{
    public enum EnumA
    {
        Unknow,
        EnumType1,
        EnumType2,
        EnumType3
    }

    public class ClassA
    {

        private Vector4 Vector4_1 = new Vector4();
        private Vector4 Vector4_2 = new Vector4();

        private Rect rect_1 = new Rect();
        private Color color_1 = new Color();
        private Quaternion Quaternion_1 = new Quaternion();
        public List<Vector4> list_Vector4 = new List<Vector4>();
        private Dictionary<string, Color> Dic_color = new Dictionary<string, Color>();
        public UInt16 Uint_1 = 12111;
        private UInt64 Uint_2 = 1111;
        public char char_1 = 'a';
        public SByte sbyte_1 = 12;
        public Byte byte_1 = 1;
        public long long_1 = 1111111111111111111;
        private Decimal Decimal_1 = 12.1m;
        // ulong 太长会出现科学计数法??
        public ulong ulong_1 = 11111222222222;

        public int Int_Field_1 = 1;
        public int Int_Field_2 = 2;
        public string String_Field = "string_in_class_a";
        public float Float_Field = 3.444f;
        public bool Bool_Field = true;

        public EnumA EnumA_Field = EnumA.EnumType1;

        public ClassB ClassB_Field = new ClassB();
        public ClassC ClassC_Field = new ClassC();

        public int[] Int_Array_Field = { 1, 3, 2, 3, 4, 5, 6, 7 };
        public List<int> Int_List_Field = new List<int> { 10, 20, 32, 123, 123, 14, 34, 3, 535 };
        public List<ClassB> ClassB_List_Field = new List<ClassB>();

        public Dictionary<string, int> Dic_Int_Field = new Dictionary<string, int>();

        public Dictionary<string, List<int>> Dic_Int_List_Field = new Dictionary<string, List<int>>();
        public Dictionary<string, List<string>> Dic_String_List_Field = new Dictionary<string, List<string>>();

        public ClassA()
        {
            Vector4_1.Set(1, 1, 12, 2);
            Vector4_2.Set(3, 3, 12, 2);
            rect_1.Set(12, 12, 12, 12);
            Quaternion_1.Set(1.2f, 1, 1, 1);
            color_1 = new Vector4(1, 1, 1, 1);

            list_Vector4.Add(Vector4_1);
            list_Vector4.Add(Vector4_2);
            Dic_color.Add("as", new Color(1, 1, 1, 2));
            Dic_color.Add("sd", new Color(1.1f, 2.2f, 3.3f, 4.4f));

            Dic_Int_Field.Add("a", 12);
            Dic_Int_Field.Add("b", 34);

            Dic_String_List_Field.Add("c", new List<string> { "q", "w", "e" });
            Dic_String_List_Field.Add("d", new List<string> { "r", "t", "y" });
            Dic_String_List_Field.Add("r", new List<string> { "r", "t", "y" });

            ClassB_List_Field.Add(new ClassB());
            ClassB_List_Field.Add(new ClassB());
            ClassB_List_Field.Add(new ClassB());

            ClassB_Field.String_Field = "e";

            Dic_Int_List_Field.Add("f", new List<int> { 10, 20 });
        }
    }

    public class ClassB
    {
        public string String_Field;
        public ClassC ClassC_Field_1 = new ClassC();
        public ClassC ClassC_Field_2 = null;
        public EnumA EnumA_Field = EnumA.EnumType2;
    }

    public class ClassC
    {
        public string String_Field = "class_c_string";
        public bool Bool_Field = false;
        public ClassB ClassB_Field = null;
    }

    public class JsonObjectUtilsTester : MonoBehaviour
    {
        void Start()
        {
            ClassA classA = new ClassA();
            JSONObject json = classA.ToJsonObject();
            VeerDebug.Log(json.ToString());
        }
    }
}
