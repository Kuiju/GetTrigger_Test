using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VeerYeast
{
    public class VeerSystemUtils
    {
        public const string DEVICE_NAME_KEY = "device_name";
        public const string DEVICE_TYPE_KEY = "device_type";
        public const string DEVICE_MODEL_KEY = "device_model";
        public const string DEVICE_UID_KEY = "device_uid";

        public static JSONObject GetSystemEnvJson(JSONObject json = null)
        {
            if (json == null)
                json = JSONObject.Create("{}");

            json.AddField(DEVICE_NAME_KEY, SystemInfo.deviceName);
            json.AddField(DEVICE_TYPE_KEY, SystemInfo.deviceType.ToString());
            json.AddField(DEVICE_MODEL_KEY, SystemInfo.deviceModel);
            json.AddField(DEVICE_UID_KEY, SystemInfo.deviceUniqueIdentifier);

            return json;
        }
    }
}