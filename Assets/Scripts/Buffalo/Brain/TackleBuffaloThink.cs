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
        
        private static readonly Subject<IBuffaloBrain> onChangedBrain = new Subject<IBuffaloBrain>();
        public static readonly IObservable<IBuffaloBrain> OnChangedBrain = onChangedBrain;
        
        public TackleBuffaloThink(){}

        public Vector2 Process(BuffaloBrainArgs args)
        {
            if (!isStartedTackle)
            {
                if (Vector2.Angle(args.velocity, (args.targetPosition - args.position)) < GameConst.BuffaloTackleStartAngle)
                {
                    isStartedTackle = true;
                }
                return Vector2.Lerp(args.velocity, (args.targetPosition - args.position).normalized * (GameConst.BuffaloTackleStaySpeed * 100f), GameConst.BuffaloTackleStayLerpRate * Time.deltaTime);
            }
            else if (!isStartedOverRun)
            {
                if (VectorUtils.SqrDistance(args.targetPosition, args.position) < GameConst.BuffaloTackleOverRunDistance * GameConst.BuffaloTackleOverRunDistance || Vector2.Angle(args.velocity, (args.targetPosition - args.position)) > GameConst.BuffaloTackleStartAngle)
                {
                    isStartedOverRun = true;
                }
                
                tackleSpeed += GameConst.BuffaloTackleAcceleration * Time.deltaTime;
                return Vector2.Lerp(args.velocity,
                    (args.targetPosition - args.position).normalized * tackleSpeed, GameConst.BuffaloTackleLerpRate * Time.deltaTime);
            }
            else
            {
                if (true)
                {
                    // TODO ここにタックル終了処理
                }
                
                tackleSpeed += GameConst.BuffaloTackleAcceleration * Time.deltaTime;
                return Vector2.Lerp(args.velocity,
                    args.velocity.normalized * tackleSpeed, GameConst.BuffaloTackleStayLerpRate * Time.deltaTime);
            }
        }
        
        public void SubscribeOnChangedBrain(Action<IBuffaloBrain> action) => onChangedBrain.Subscribe(action);
    }
}