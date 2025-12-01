using System;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using Utils;

namespace Buffalo.Brain
{
    public class TackleBuffaloThink : IBuffaloBrain
    {
        private bool isStartedTackle = false;
        private bool isStartedOverRun = false;
        private float tackleSpeed = GameConst.BuffaloTackleStaySpeed;
        
        private readonly Subject<IBuffaloBrain> onChangedBrain = new Subject<IBuffaloBrain>();
        public IObservable<IBuffaloBrain> OnChangedBrain => onChangedBrain;
        
        private readonly Subject<Unit> onTackleStart = new Subject<Unit>();
        public  IObservable<Unit> OnTackleStart => onTackleStart;
        private readonly Subject<Unit> onTackleEnd = new Subject<Unit>();
        public  IObservable<Unit> OnTackleEnd => onTackleEnd;
        
        public TackleBuffaloThink(){}

        public Vector2 Process(BuffaloBrainArgs args)
        {
            if (!isStartedTackle)
            {
                args.buffaloView.IsNeckRotation = true;
                if (Vector2.Angle(args.velocity, (args.targetPosition - args.position)) < GameConst.BuffaloTackleStartAngle)
                {
                    onTackleStart.OnNext(Unit.Default);
                    isStartedTackle = true;
                }
                return Vector2.Lerp(args.velocity, (args.targetPosition - args.position).normalized * (GameConst.BuffaloTackleStaySpeed), GameConst.BuffaloTackleStayLerpRate * Time.deltaTime);
            }
            else if (!isStartedOverRun)
            {
                args.buffaloView.IsNeckRotation = true;
                if (VectorUtils.SqrDistance(args.targetPosition, args.position) < GameConst.BuffaloTackleOverRunStartDistance * GameConst.BuffaloTackleOverRunStartDistance || Vector2.Angle(args.velocity, (args.targetPosition - args.position)) > GameConst.BuffaloTackleStartAngle)
                {
                    isStartedOverRun = true;
                }
                
                tackleSpeed += GameConst.BuffaloTackleAcceleration * Time.deltaTime * (args.isBig ? 0.5f : 1f);
                return Vector2.Lerp(args.velocity,
                    (args.targetPosition - args.position).normalized * tackleSpeed, GameConst.BuffaloTackleLerpRate * Time.deltaTime);
            }
            else
            {
                args.buffaloView.IsNeckRotation = false;
                if (BuffaloUtils.GetFrontSqrDistanceFromStageEnd(args.position, args.velocity) < GameConst.BuffaloTackleOverRunEndDistanceAgainstWall * GameConst.BuffaloTackleOverRunEndDistanceAgainstWall)
                {
                    onTackleEnd.OnNext(Unit.Default);
                    onChangedBrain.OnNext(new RapidDecelerationBuffaloThink());
                }
                
                tackleSpeed += GameConst.BuffaloTackleAcceleration * Time.deltaTime * (args.isBig ? 0.5f : 1f);
                return Vector2.Lerp(args.velocity,
                    args.velocity.normalized * tackleSpeed, GameConst.BuffaloTackleStayLerpRate * Time.deltaTime);
            }
        }
        
        public void SubscribeOnChangedBrain(Action<IBuffaloBrain> action) => onChangedBrain.Subscribe(action);

        public void Dispose()
        {
            onChangedBrain.Dispose();
            onTackleStart.Dispose();
            onTackleEnd.Dispose();
        }
    }
}