using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Buffalo.Brain;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Buffalo
{
    public class Buffalo : MovableObject
    {
        private readonly float tackleSpan = 1f;
        private Subject<Unit> onWantToTackle = new Subject<Unit>();
        public IObservable<Unit> OnWantToTackle => onWantToTackle;
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        
        private Subject<Unit> onTackleStart = new Subject<Unit>();
        public IObservable<Unit> OnTackleStart => onTackleStart;
        
        private Subject<Unit> onTacklEnd = new Subject<Unit>();
        public IObservable<Unit> OnTackleEnd => onTacklEnd;
        private Subject<Flag> onGetFlag = new Subject<Flag>();
        public IObservable<Flag> OnGetFlag => onGetFlag;
        
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Animator buffaloAnimator;
        [SerializeField] private Animator buffaloBodyAnimator;
        [SerializeField] private Animator buffaloHeadAnimator;
        [SerializeField] private Transform headTransform;
        public BuffaloView buffaloView;
        protected bool isBig = false;
        
        public float SpeedUpRate { get; set; } = 1f;
        
        private IBuffaloBrain currentBrain;

        protected override void Start()
        {
            base.Start();
            buffaloView = new BuffaloView(buffaloAnimator, buffaloBodyAnimator, buffaloHeadAnimator,headTransform);
            // Walk(cancelTokenSource.Token);
            currentBrain = new StayBuffaloThink(new BuffaloBrainArgs(VectorUtils.ToXZ(transform.position), velocity,
                VectorUtils.ToXZ(playerTransform.position), buffaloView, isBig));
            currentBrain.Dispose();
            currentBrain = new WalkBuffaloThink(new BuffaloBrainArgs(VectorUtils.ToXZ(transform.position), velocity,
                VectorUtils.ToXZ(playerTransform.position), buffaloView, isBig));
            currentBrain.SubscribeOnChangedBrain(SubscribeOnChangedBrain);
        }
        
        private void SubscribeOnChangedBrain(IBuffaloBrain brain)
        {
            IBuffaloBrain prevBrain = currentBrain;
            prevBrain.Dispose();
            currentBrain = brain;
            currentBrain.SubscribeOnChangedBrain(SubscribeOnChangedBrain);
            if (brain is TackleBuffaloThink tackleBrain)
            {
                tackleBrain.OnTackleStart.Subscribe(_ =>
                {
                    onTackleStart.OnNext(Unit.Default);
                });
                tackleBrain.OnTackleEnd.Subscribe(_ =>
                {
                    onTacklEnd.OnNext(Unit.Default);
                });
            }
        }

        protected override void Update()
        {
            base.Update();
            // if (true)
            // {
            //     onWantToTackle.OnNext(Unit.Default);
            // }
            //
            // if (CheckGetFlag() != null)
            // {
            //     onGetFlag.OnNext(CheckGetFlag());
            // }
            Vector2 nextVelocity = currentBrain.Process(new BuffaloBrainArgs(VectorUtils.ToXZ(transform.position), velocity, VectorUtils.ToXZ(playerTransform.position), buffaloView, isBig));
            direction = GetTurnAmount(nextVelocity);
            buffaloView.SetWalkSpeed(nextVelocity.magnitude);
            buffaloView.SetHeadRotation(Vector2.SignedAngle(VectorUtils.ToXZ(transform.forward), VectorUtils.ToXZ(playerTransform.position) - VectorUtils.ToXZ(transform.position)));
            buffaloView.SetIsHeadDown(BuffaloUtils.IsPlayerNearHead(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(transform.forward), VectorUtils.ToXZ(playerTransform.position)));
            Move(nextVelocity);
        }

        public Vector2? GetAttackVector(Vector2 targetPosition)
        {
            if ( /*isAttacking && */VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), targetPosition) <
                                    GameConst.BuffaloAttackRadius * GameConst.BuffaloAttackRadius
                                    && Vector2.Angle(VectorUtils.ToXZ(transform.forward),
                                        (targetPosition - VectorUtils.ToXZ(transform.position))) <
                                    GameConst.BuffaloAttackAngle)
            {
                buffaloView.PlayAttackAnimation();
                return (targetPosition - VectorUtils.ToXZ(transform.position)).normalized * (GameConst.BuffaloAttackPower * (isBig ? 2f : 1f));
            }else if (isBig && isInBigBody(targetPosition))
            {
                return (targetPosition - VectorUtils.ToXZ(transform.position)).normalized * (GameConst.BuffaloAttackPower * 2f);
            }
            return null;
        }

        private bool isInBigBody(Vector2 targetPosition)
        {
            Vector2 buffaloBackCenterPos = VectorUtils.ToXZ(-transform.forward * GameConst.BigBuffaloBodyLength + transform.position);
            float ShaeiVectorLength = Vector2.Dot(VectorUtils.ToXZ(transform.forward), VectorUtils.ToXZ(targetPosition - buffaloBackCenterPos));
            if (0 < ShaeiVectorLength && ShaeiVectorLength < GameConst.BigBuffaloBodyLength)
            {
                if (VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.forward) * ShaeiVectorLength,
                        (targetPosition - buffaloBackCenterPos)) <
                    GameConst.BigBuffaloBodyWidth * GameConst.BigBuffaloBodyWidth)
                {
                    return true;
                } 
            }else if (VectorUtils.SqrDistance(buffaloBackCenterPos, targetPosition) <
                      GameConst.BigBuffaloBodyWidth * GameConst.BigBuffaloBodyWidth)
            {
                return true;
            }else if (VectorUtils.SqrDistance(
                          buffaloBackCenterPos + VectorUtils.ToXZ(transform.forward * GameConst.BigBuffaloBodyLength),
                          targetPosition) <
                      GameConst.BigBuffaloBodyWidth * GameConst.BigBuffaloBodyWidth)
            {
                return true;
            }
            return false;
        }

        private Vector2 getWalkTargetDirection()
        {
            Vector2 targetDirection = new Vector2(Random.Range(-GameConst.BuffaloWalkArea.x, GameConst.BuffaloWalkArea.x),Random.Range(-GameConst.BuffaloWalkArea.y, GameConst.BuffaloWalkArea.y));

            return targetDirection;
        }
        
        protected Quaternion GetTurnAmount(Vector2 targetVelocity)
        {
            float angle = Vector2.SignedAngle(VectorUtils.ToXZ(-transform.forward), targetVelocity);
            float rotateAmount;
            if (0 <= angle)
            {
                rotateAmount = Mathf.Min(angle, GameConst.BuffaloRotateSpeed * Time.deltaTime);
            }
            else
            {
                rotateAmount = Mathf.Max(angle, - GameConst.BuffaloRotateSpeed * Time.deltaTime);
            }
            return Quaternion.Euler(0, rotateAmount+transform.rotation.eulerAngles.y, 0);
        }
        
        [CanBeNull]
        private Flag CheckGetFlag()
        {
            if (FlagsInField.Count <= 0) return null;
            foreach (var flag in FlagsInField)
            {
                float distanceToFlag = (VectorUtils.ToXZ(transform.position) - VectorUtils.ToXZ(flag.position)).sqrMagnitude;
                Debug.Log("Distance to Flag: " + distanceToFlag);
                if (distanceToFlag < GameConst.FragGetableDistance * GameConst.FragGetableDistance)
                {
                    return flag;
                }
            }

            return null;
        }

        public float GetTackleScoreUp(Vector2 playerPosition)
        {
            if (VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), playerPosition) <
                GameConst.DodgeScoreUpGigRadius * GameConst.DodgeScoreUpGigRadius
                && Vector2.Angle(velocity, (playerPosition - VectorUtils.ToXZ(transform.position))) <
                GameConst.DodgeScoreUpBigAngle)
            {
                return GameConst.DodgeScoreUpGigValue;
            }else if(VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), playerPosition) <
                GameConst.DodgeScoreUpSmallRadius * GameConst.DodgeScoreUpSmallRadius
                && Vector2.Angle(velocity, (playerPosition - VectorUtils.ToXZ(transform.position))) <
                GameConst.DodgeScoreUpSmallAngle)
            {
                return GameConst.DodgeScoreUpSmallValue;
            }else{
                return 0;
            }
        }
    }
}