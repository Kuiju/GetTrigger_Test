using UnityEngine;

namespace VeerWaveFilter
{
    public class MovingAverageVector3
    {
        public int Hits = 30;
        public int CurrentHitCount = 0;
        public float ResetThreshold = 30;
        public Vector3[] HitBuffer;
        public int CurrentBufferIndex = 0;
        public Vector3 LastHit = Vector3.zero;
        public Vector3 CurrentSum = Vector3.zero;

        public void InitMovingAverage()
        {
            HitBuffer = new Vector3[Hits];
        }

        public void PushHit(Vector3 inHit)
        {
            if (IsBeyondThreshold(inHit))
            {
                ResetMovingAverage();
            }

            if (CurrentHitCount < Hits)
            {
                CurrentHitCount++;
                LastHit = inHit;
                CurrentSum += inHit;
            }
            else
            {
                LastHit = inHit;
                CurrentSum -= HitBuffer[CurrentBufferIndex];
                CurrentSum += inHit;
            }

            HitBuffer[CurrentBufferIndex] = inHit;
            CurrentBufferIndex++;
            if (CurrentBufferIndex >= Hits)
            {
                CurrentBufferIndex = 0;
            }
        }

        private void ResetMovingAverage()
        {
            CurrentHitCount = 0;
            LastHit = Vector3.zero;
            CurrentSum = Vector3.zero;
        }

        private bool IsBeyondThreshold(Vector3 inHit)
        {
            return Mathf.Abs(inHit.x - LastHit.x) > ResetThreshold
                || Mathf.Abs(inHit.y - LastHit.y) > ResetThreshold
                || Mathf.Abs(inHit.z - LastHit.z) > ResetThreshold;
        }

        public Vector3 GetMovingAverage()
        {
            if (CurrentHitCount < Hits)
            {
                return LastHit;
            }
            else
            {
                return CurrentSum / Hits;
            }
        }
    }
}