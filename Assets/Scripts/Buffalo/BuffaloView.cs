using DefaultNamespace;
using UnityEngine;

namespace Buffalo
{
    public class BuffaloView
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsHeadDown = Animator.StringToHash("IsHeadDown");
        private Animator animator;
        private Animator bodyAnimator;
        private Animator headAnimator;
        private Transform headTransform;
        public bool IsNeckRotation;
        public BuffaloView(Animator animator, Animator bodyAnimator, Animator headAnimator, Transform headTransform)
        {
            this.animator = animator;
            this.bodyAnimator = bodyAnimator;
            this.headAnimator = headAnimator;
            this.headTransform = headTransform;
            IsNeckRotation = false;
        }
        
        public void SetWalkSpeed(float realSpeed)
        {
            float speed = RealSpeedToAnimSpeed(realSpeed);
            bodyAnimator.SetFloat(Speed, speed);
        }
        
        private float RealSpeedToAnimSpeed(float realSpeed)
        {
            float speed = realSpeed / GameConst.BuffaloViewWalkMaxSpeedThreshold;
            if (speed > 1f)
            {
                return 1f;
            }
            else if(speed < GameConst.BuffaloViewWalkMinSpeedThreshold)
            {
                return GameConst.BuffaloViewWalkMinSpeedThreshold;
            }
            return realSpeed / GameConst.BuffaloViewWalkMaxSpeedThreshold;
        }

        public void SetHeadRotation(float angleFromForwardToTarget)
        {
            if (!IsNeckRotation)
            {
                headTransform.localRotation = Quaternion.Euler(0f, Mathf.Lerp(headTransform.rotation.y, 0, Time.deltaTime), 0f);
                return;
            }
            if (angleFromForwardToTarget > GameConst.BuffaloViewNeckMaxAngle)
            {
                angleFromForwardToTarget = GameConst.BuffaloViewNeckMaxAngle;
            }
            else if (angleFromForwardToTarget < -GameConst.BuffaloViewNeckMaxAngle)
            {
                angleFromForwardToTarget = -GameConst.BuffaloViewNeckMaxAngle;
            }
            headTransform.localRotation = Quaternion.Euler(0f, Mathf.Lerp(headTransform.rotation.y, -angleFromForwardToTarget, Time.deltaTime * 50f), 0f);
        }

        public void PlayAttackAnimation()
        {
            headAnimator.SetTrigger(Attack);
        }
        
        public void SetIsHeadDown(bool isHeadDown)
        {
            headAnimator.SetBool(IsHeadDown, isHeadDown);
        }
    }
}