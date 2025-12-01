namespace Buffalo.BuffaloDiff
{
    public class ScheduledBuffalo : Buffalo
    {
        public bool isBooted = false;
        protected bool isFirstBoot = true;

        protected virtual void OnBooted()
        {
            
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}