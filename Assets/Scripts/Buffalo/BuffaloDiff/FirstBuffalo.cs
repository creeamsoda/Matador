using DefaultNamespace;
using UnityEngine;
using Utils;

namespace Buffalo.BuffaloDiff
{
    public class FirstBuffalo : ScheduledBuffalo
    {
        private bool isOutOfCage = false;
        private float stayTime = 3f;
        protected override void OnBooted()
        {
            //鳴く？
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
                if (stayTime > 0)
                {
                    stayTime -= Time.deltaTime;
                    return;
                }
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