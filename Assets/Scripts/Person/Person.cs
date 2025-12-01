using System;
using Buffalo.BuffaloDiff;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.Audio;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Person
{
    public class Person : MovableObject
    {
        [SerializeField] protected Transform buffaloTransform;
        [SerializeField] protected Transform buffaloRightTransform;
        [SerializeField] protected Transform buffaloLeftTransform;
        [SerializeField] protected Transform buffaloCenterTransform;
        [SerializeField] protected Buffalo.Buffalo buffalo;
        [SerializeField] protected RightBuffalo rightBuffalo;
        [SerializeField] protected LeftBuffalo leftBuffalo;
        [SerializeField] protected CenterBuffalo centerBuffalo;
        protected bool isShowingCloth = false;
        [SerializeField] protected GameObject cloth;
        [SerializeField] protected PlaySceneSoundManager playSceneSoundManager;
        
        protected override void Start()
        {
            base.Start();
            buffalo.OnWantToTackle.Subscribe(_ =>
            {
            });
        }

        protected override void Update()
        {
            base.Update();
            Turn();
        }

        protected void Turn()
        {
            Transform targetTransform = GetNearestBuffalo();
            
            Vector2 toBuffalo = VectorUtils.ToXZ(targetTransform.position - transform.position);
            float angle = Vector2.SignedAngle(VectorUtils.ToXZ(-transform.forward), toBuffalo);
            float rotateAmount;
            if (0 <= angle)
            {
                rotateAmount = Mathf.Min(angle, GameConst.PlayerRotateSpeed * Time.deltaTime);
            }
            else
            {
                rotateAmount = Mathf.Max(angle, - GameConst.PlayerRotateSpeed * Time.deltaTime);
            }
            direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y+rotateAmount, 0);
        }

        protected async UniTask Dodge(InputAction moveAction)
        {
            // スティック入力方向にかいひする
            Vector2 dodgeDirection = moveAction.ReadValue<Vector2>().normalized;
            float dodgeDuration = 0f;
            playSceneSoundManager.PlayDodgeSound();
            while (true)
            {
                dodgeDuration += Time.deltaTime;
                if (dodgeDuration >= GameConst.DodgeDurationMax)break;
                Move(dodgeDirection * GameConst.DodgeSpeed);
                await UniTask.Yield();
            }
            Vector2 dodgeVelocity = dodgeDirection * GameConst.DodgeSpeed;
            while (dodgeVelocity.sqrMagnitude > GameConst.DodgeStunEndThreshold*GameConst.DodgeStunEndThreshold)
            {
                dodgeVelocity = Vector2.Lerp(dodgeVelocity, Vector2.zero, Time.deltaTime * GameConst.DodgeStunRate);
                Move(dodgeVelocity);
                await UniTask.Yield();
            }
        }

        protected void ShowCloth()
        {
            if (!isShowingCloth) isShowingCloth = true;
            cloth.SetActive(true);
            // Animationをつける
        }

        protected void HideCloth()
        {
            isShowingCloth = false;
            cloth.SetActive(false);
        }

        protected Transform GetNearestBuffalo()
        {
            if (VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloTransform.position)) <
                VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloRightTransform.position))
                && VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloTransform.position)) <
                VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloLeftTransform.position))
                && VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloTransform.position)) <
                VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloCenterTransform.position)))
            {
                return buffaloTransform;
            }else if (VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloRightTransform.position)) <
                      VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloLeftTransform.position))
                      && VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloRightTransform.position)) <
                      VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloCenterTransform.position)))
            {
                return buffaloRightTransform;
            }else if (VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloLeftTransform.position)) <
                      VectorUtils.SqrDistance(VectorUtils.ToXZ(transform.position), VectorUtils.ToXZ(buffaloCenterTransform.position)))
            {
                return buffaloLeftTransform;
            }

            return buffaloCenterTransform;
        }
    }
}