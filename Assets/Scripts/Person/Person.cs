using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using Utils;

namespace Person
{
    public class Person : MovableObject
    {
        [SerializeField] protected Buffalo.Buffalo buffalo;
        protected bool isShowingCloth = false;
        [SerializeField] protected GameObject cloth;
        
        protected override void Start()
        {
            base.Start();
            buffalo.OnWantToTackle.Subscribe(_ =>
            {
                if (!buffalo.IsTackling && isShowingCloth)
                {
                    buffalo.Tackle(transform.position);
                }
            });
        }

        protected override void Update()
        {
            base.Update();
            Turn();
        }

        protected void Turn()
        {
            Vector2 toBuffalo = VectorUtils.ToXZ(buffalo.transform.position - transform.position);
            Debug.Log("toBuffalo"+toBuffalo);
            float angle = Vector2.SignedAngle(VectorUtils.ToXZ(transform.forward), toBuffalo);
            Debug.Log("angle"+angle);
            float rotateAmount;
            if (0 <= angle)
            {
                rotateAmount = Mathf.Min(angle, GameConst.PlayerRotateSpeed * Time.deltaTime);
            }
            else
            {
                rotateAmount = Mathf.Max(angle, - GameConst.PlayerRotateSpeed * Time.deltaTime);
            }
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y-rotateAmount, 0);
        }

        protected async UniTask Dodge()
        {
            // 左側にかいひする
            Vector2 dodgeDirection = new Vector2(- Mathf.Cos(transform.rotation.eulerAngles.y), Mathf.Sin(transform.rotation.eulerAngles.y));
            float dodegeDuration = 0f;
            while (true)
            {
                dodegeDuration += Time.deltaTime;
                if (dodegeDuration >= GameConst.DodgeDurationMax)break;
                Move(dodgeDirection * GameConst.DodgeSpeed);
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
    }
}