using System;
using UniRx;
using UnityEngine;

namespace Buffalo.Brain
{
    public class StayBuffaloThink : IBuffaloBrain
    {
        private int count = 0;
        private Subject<IBuffaloBrain> onChangedBrain = new Subject<IBuffaloBrain>();
        private IObservable<IBuffaloBrain> OnChangedBrain => onChangedBrain;
        public StayBuffaloThink(BuffaloBrainArgs args)
        {
        }
        public Vector2 Process(BuffaloBrainArgs args)
        {
            if (count > 2)
                onChangedBrain.OnNext(new WalkBuffaloThink(args));
            count++;
            return Vector2.zero;
        }
        public void Dispose()
        {
            onChangedBrain.Dispose();
        }

        public void SubscribeOnChangedBrain(Action<IBuffaloBrain> action)
        {
            OnChangedBrain.Subscribe(action);
        }
    }
}