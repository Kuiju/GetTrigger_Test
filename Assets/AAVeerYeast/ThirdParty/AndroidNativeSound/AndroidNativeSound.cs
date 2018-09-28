using UnityEngine;

namespace VeerYeast
{
    public class AndroidNativeSound : Singleton<AndroidNativeSound>
    {
        private AndroidJavaObject _NativeSoundObject = null;
        public AndroidJavaObject NativeSoundObject
        {
            get
            {
                if (_NativeSoundObject == null)
                {
                    InitAndroidNativeSound();
                }
                return _NativeSoundObject;
            }
        }

        void InitAndroidNativeSound()
        {
#if UNITY_ANDROID
            AndroidJavaClass unityActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activityObj = unityActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _NativeSoundObject = new AndroidJavaObject("veer.veeryeast.com.androidnativesound.AndroidNativeSound", 200, activityObj);
#endif
        }

        public float GetVolume()
        {
#if UNITY_ANDROID
            return NativeSoundObject.Call<float>("GetVolume");
#else
            return 1;
#endif
        }

        public void SetVolume(float val)
        {
#if UNITY_ANDROID
            NativeSoundObject.Call("SetVolume", val);
#endif
        }

        /// <summary>
        /// soundPath 对于 StreamingAssets 目录下 声音文件 如 "Audio/click_1.wav"
        /// </summary>
        public void PlaySound(string soundPath)
        {
#if UNITY_ANDROID
            NativeSoundObject.Call("PlaySound", soundPath);
#endif
        }
    }
}