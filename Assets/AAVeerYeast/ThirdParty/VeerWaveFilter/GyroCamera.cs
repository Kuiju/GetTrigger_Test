using UnityEngine;
using UnityEngine.UI;

namespace VeerWaveFilter
{
    public class GyroCamera : MonoBehaviour
    {
        public Text HitCountInput;
        public Text Debug;

        public GameObject CamParent;
        public MovingAverageQuaternion MovingAver;
        public bool bUseAver = false;

        private Quaternion _QuatMult;
        private Quaternion _QuatMap;

        private void Start()
        {
            CamParent.transform.position = transform.position;
            transform.parent = CamParent.transform;

            Input.gyro.enabled = true;

            CamParent.transform.eulerAngles = new Vector3(90, 0, 0);
            _QuatMult = new Quaternion(0, 0, 1, 0);
        }

        public void InitMovingAverage()
        {
            MovingAver = new MovingAverageQuaternion();
            MovingAver.InitMovingAverageQuaternion(int.Parse(HitCountInput.text), Quaternion.identity);
        }

        public void SwitchUseAver()
        {
            bUseAver = !bUseAver;
        }

        private void Update()
        {
            if (!bUseAver || MovingAver == null || MovingAver.SampleCount <= 1)
            {
                _QuatMap = Input.gyro.attitude;
                Quaternion qt = _QuatMap * _QuatMult;
                transform.localRotation = qt;
            }
            else
            {
#if UNITY_EDITOR
                _QuatMap = Quaternion.identity;
#else
            _QuatMap = Input.gyro.attitude;
#endif
                Quaternion qt = _QuatMap * _QuatMult;
                MovingAver.PushSample(qt);
                transform.localRotation = MovingAver.GetMovingAverage();
            }

            Debug.text = Input.gyro.attitude.eulerAngles.ToString() + "\n" + transform.localRotation.eulerAngles.ToString();
        }
    }
}