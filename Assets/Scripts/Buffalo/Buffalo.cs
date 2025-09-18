using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public bool IsTackling { get; private set; } = false;
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        
        private Subject<Unit> onTacklEnd = new Subject<Unit>();
        public IObservable<Unit> OnTackleEnd => onTacklEnd;
        
        private Subject<Flag> onGetFlag = new Subject<Flag>();
        public IObservable<Flag> OnGetFlag => onGetFlag;
        
        public float SpeedUpRate { get; set; } = 1f;

        protected override void Start()
        {
            base.Start();
            Walk(cancelTokenSource.Token);
        }

        protected override void Update()
        {
            base.Update();
            if (true)
            {
                onWantToTackle.OnNext(Unit.Default);
            }

            if (CheckGetFlag() != null)
            {
                onGetFlag.OnNext(CheckGetFlag());
            }
        }

        private async UniTask Walk(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) return;
                Vector2 targetPosition = getWalkTargetDirection();
                Debug.Log("targetPosition: " + targetPosition);
                float walkForOneTargetDuration = 0f;
                float walkForOneTargetSpan = Random.Range(1f, 3f);
                while (walkForOneTargetDuration < walkForOneTargetSpan)
                {
                    if (token.IsCancellationRequested) return;
                    walkForOneTargetDuration += Time.deltaTime;
                    float speed = Mathf.Lerp(velocity.magnitude, GameConst.BuffaloWalkSpeed*SpeedUpRate, 1.5f * Time.deltaTime);
                    Turn(targetPosition);
                    Move(VectorUtils.ToXZ(transform.forward) * speed);
                    if ((VectorUtils.ToXZ(transform.position) - targetPosition).sqrMagnitude < 2f * 2f) break;
                    await UniTask.Yield();
                }
            }
        }

        private Vector2 getWalkTargetDirection()
        {
            Vector2 targetDirection = new Vector2(Random.Range(-GameConst.BuffaloWalkArea.x, GameConst.BuffaloWalkArea.x),Random.Range(-GameConst.BuffaloWalkArea.y, GameConst.BuffaloWalkArea.y));

            return targetDirection;
        }
        
        public async UniTask Tackle(Vector3 targetPos)
        { 
            cancelTokenSource.Cancel();
            IsTackling = true;
            Debug.Log("targetPos: " + targetPos);
            Vector2 targetPosXZ = VectorUtils.ToXZ(targetPos);
            float tackleDuration = 0f;
            float turnDuration = 0f;
            while (turnDuration < 1f)
            {
                turnDuration += Time.deltaTime;
                Turn(targetPosXZ);
                await UniTask.Yield();
            }

            while (true)
            {
                tackleDuration += Time.deltaTime;
                if (tackleDuration >= tackleSpan) break;
                Turn(targetPosXZ);
                Move(GameConst.BuffaloTackleSpeed * SpeedUpRate * VectorUtils.ToXZ(transform.forward));
                await UniTask.Yield();
            }
            onTacklEnd.OnNext(Unit.Default);
            IsTackling = false;
            cancelTokenSource = new CancellationTokenSource();
            Walk(cancelTokenSource.Token);
        }
        
        protected void Turn(Vector2 targetDirection)
        {
            Vector2 toTarget = targetDirection - VectorUtils.ToXZ(transform.position);
            Debug.Log("toBuffalo"+toTarget);
            float angle = Vector2.SignedAngle(VectorUtils.ToXZ(transform.forward), toTarget);
            Debug.Log("angle"+angle);
            float rotateAmount;
            if (0 <= angle)
            {
                rotateAmount = Mathf.Min(angle, GameConst.BuffaloRotateSpeed * Time.deltaTime);
            }
            else
            {
                rotateAmount = Mathf.Max(angle, - GameConst.BuffaloRotateSpeed * Time.deltaTime);
            }
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y-rotateAmount, 0);
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
    }
}