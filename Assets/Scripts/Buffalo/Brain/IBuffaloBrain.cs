using System;
using UniRx;
using UnityEngine;

namespace Buffalo.Brain
{
    public interface IBuffaloBrain
    {
        private static readonly float WallRepulsiveRate;
        private static readonly float WallRepulsiveArea;
        public Vector2 Process(BuffaloBrainArgs args);
        public void SubscribeOnChangedBrain(Action<IBuffaloBrain> action);
    }
}