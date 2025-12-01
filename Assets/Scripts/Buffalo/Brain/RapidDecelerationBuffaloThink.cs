using System;
using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Buffalo.Brain
{
    public class RapidDecelerationBuffaloThink : IBuffaloBrain
    {
        private readonly Subject<IBuffaloBrain> onChangedBrain = new Subject<IBuffaloBrain>();
        private IObservable<IBuffaloBrain> OnChangedBrain => onChangedBrain;
        public Vector2 Process(BuffaloBrainArgs args)
        {
            args.buffaloView.IsNeckRotation = false;
            Vector2 nextVelocity = args.velocity * GameConst.BuffaloRapidDecelerationRate;
            if (nextVelocity.sqrMagnitude <
                GameConst.BuffaloRapidDecelerationEndSpeed * GameConst.BuffaloRapidDecelerationEndSpeed)
                onChangedBrain.OnNext(new WalkBuffaloThink(args));
            return nextVelocity;
        }

        public void SubscribeOnChangedBrain(Action<IBuffaloBrain> action)
        {
            OnChangedBrain.Subscribe(action);
        }
        public void Dispose()
        {
            onChangedBrain.Dispose();
        }
    }
}