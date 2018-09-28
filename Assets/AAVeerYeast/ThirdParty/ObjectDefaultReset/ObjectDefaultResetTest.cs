namespace ObjectDefaultResetTest
{
    public static class StaticDefaultReset
    {
        [DefaultReset("static reset")]
        public static string StaticStringField = "static default";
    }

    public class NoStaticDefaultReset
    {
        [DefaultReset("static reset")]
        public static string StaticStringField = "static default";

        [DefaultReset("reset")]
        public string StringField = "default";

        [DefaultReset]
        public int IntField = 999;
    }

    public class ObjectDefaultResetTest : GUITestBase
    {
        public void Test_DefReset()
        {
            VeerDebug.Log(StaticDefaultReset.StaticStringField);
            ObjectDefaultReset.DefaultReset(typeof(StaticDefaultReset));
            VeerDebug.Log(StaticDefaultReset.StaticStringField);

            NoStaticDefaultReset inst = new NoStaticDefaultReset();
            VeerDebug.Log(NoStaticDefaultReset.StaticStringField);
            VeerDebug.Log(inst.StringField);
            VeerDebug.Log(inst.IntField);
            inst.DefaultReset();
            VeerDebug.Log(NoStaticDefaultReset.StaticStringField);
            VeerDebug.Log(inst.StringField);
            VeerDebug.Log(inst.IntField);
        }
    }
}