using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VeerYeast
{
    public class FPSCalc
    {
        public float RecordTime = 1.0f;
        private List<float> _DeltaTimeList = new List<float>();
        private float _Time = 0;
        private int _FrameCount = 0;

        public int FPS { get { return (int)(_FrameCount / RecordTime); } }

        public FPSCalc(float recordTime)
        {
            RecordTime = recordTime;
        }

        public void FPSCalcUpdate(float delta)
        {
            _DeltaTimeList.Add(delta);
            _Time += delta;
            _FrameCount++;
            while (_Time > RecordTime)
            {
                if (_DeltaTimeList.Count == 0)
                    break;

                float first = _DeltaTimeList[0];
                _Time -= first;
                _FrameCount--;
                _DeltaTimeList.RemoveAt(0);
            }
        }
    }
}
