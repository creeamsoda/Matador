using DefaultNamespace;
using UnityEngine;
using Utils;

namespace Buffalo.BuffaloDiff
{
    public class CenterBuffalo : ScheduledBuffalo
    {
        private bool isOutOfCage = false;
        protected override void OnBooted()
        {
            //鳴く？
            isBig = true;
        }

        protected override void Start()
        {
        
        }

        protected override void Update()
        {
            if(!isBooted) return;
            if (isFirstBoot) 
            {
                OnBooted();
                isFirstBoot = false;
            }
            if (!isOutOfCage)
            {
                ForceMove((new Vector2(0, 13f) - VectorUtils.ToXZ(transform.position)).normalized *
                          GameConst.BuffaloWalkSpeed);
                if (transform.position.z - 13f < 0.1f)
                {
                    isOutOfCage = true;
                    base.Start();
                }
                return;
            }
            base.Update();
        }
    }
}