using System.Collections;
using UnityEngine;
using Utils;

namespace Buffalo
{
    public class Buffalo : MovableObject
    {
        private readonly float tackleSpan = 1f;
        private readonly float tackleSpeedMagnitude = 10f;
        
        private IEnumerator Tackle(Vector3 targetPos)
        { 
            float tackleDuration = 0;
            Vector2 tackleSpeed = VectorUtils.ToXZ(targetPos - transform.position).normalized * tackleSpeedMagnitude;

            while(true)
            {
                tackleDuration += Time.deltaTime;
                if (tackleDuration >= tackleSpan) break;
                Move(tackleSpeed);
                yield return null;
            }
        }
    }
}