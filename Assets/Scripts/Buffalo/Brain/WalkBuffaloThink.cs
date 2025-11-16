using System;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Buffalo.Brain
{
    public class WalkBuffaloThink : IBuffaloBrain
    {
        private static readonly float WallRepulsiveRate = 10f;
        private static readonly float WallRepulsiveArea = 100f;

        private static readonly float destinationChangeDistance = 5f;
        private static readonly float destinationYBlurAmount = 5f;
        private Vector2 destination;

        private static readonly Subject<IBuffaloBrain> onChangedBrain = new Subject<IBuffaloBrain>();
        public static readonly IObservable<IBuffaloBrain> OnChangedBrain = onChangedBrain;

        public WalkBuffaloThink(BuffaloBrainArgs args)
        {
            destination = DecideDestination(args.position);
            Debug.Log("next destination = " + destination);
        }
        
        public Vector2 Process(BuffaloBrainArgs args)
        {
            
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Return))
                onChangedBrain.OnNext(new TackleBuffaloThink()); 
#endif
            if (VectorUtils.SqrDistance(args.position, destination) <
                destinationChangeDistance * destinationChangeDistance)
            {
                destination = DecideDestination(args.position);
                Debug.Log("next destination = " + destination);
            }

            Vector2 velocity = Vector2.Lerp(args.velocity, (destination - args.position).normalized * GameConst.BuffaloWalkSpeed, GameConst.BuffaloWalkLerpRate * Time.deltaTime);
            return velocity;
        }

        private Vector2 DecideDestination(Vector2 position)
        {
            if (position.y > GameConst.StageEnd.y * 0.5f)
            {
                return new Vector2(GameConst.BuffaloWalkArea.x * Random.Range(-1f, 1f),
                    GameConst.BuffaloWalkArea.y);
            }
            else if (position.y < -GameConst.StageEnd.y * 0.5f)
            {
                return new Vector2(GameConst.BuffaloWalkArea.x * Random.Range(-1f, 1f),
                    -GameConst.BuffaloWalkArea.y);
            }
            else
            {
                return new Vector2(GameConst.BuffaloWalkArea.x * Random.Range(-1f, 1f),
                    Mathf.Sign(Random.Range(-1f, 1f))*GameConst.BuffaloWalkArea.y);
            }
        }

        public void SubscribeOnChangedBrain(Action<IBuffaloBrain> action)
        {
            OnChangedBrain.Subscribe(action);
        }
    }
}