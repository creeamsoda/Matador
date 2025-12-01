using DefaultNamespace;
using UnityEngine;

namespace Person
{
    public class PlayerView
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private Animator animator;
        
        public PlayerView(Animator animator)
        {
            this.animator = animator;
        }
        
        public void SetWalkSpeed(float realSpeed)
        {
            animator.SetFloat(Speed, RealSpeedToAnimSpeed(realSpeed));
        }

        private float RealSpeedToAnimSpeed(float realSpeed)
        {
            float speed = realSpeed / GameConst.PlayerSpeed;
            if (speed > 1f)
            {
                return 1f;
            }
            return speed;
        }
        
        public void PlayDamagedAnimation()
        {
            animator.SetTrigger(Damaged);
        }
    }
}