using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Person
{
    public class Player : Person
    {
        private bool isUnControllable = false;
        public bool isScoreGetable = true;
        InputAction moveAction;
        InputAction dodgeAction;
        [SerializeField] private PlayerInput playerInput;
        private UniTask handleLongPressTask;
        private Subject<Unit> onAttacked = new Subject<Unit>();
        public IObservable<Unit> OnAttacked => onAttacked;
        
        [SerializeField] private Animator playerAnimator;
        private PlayerView playerView;
        public PlayerView PlayerView => playerView;
        
        private float lastDodgeTime;
        public bool wasSoonDodged => Time.time - lastDodgeTime < GameConst.DodgeBonusCoolTime;
        protected override void Start()
        {
            base.Start();
            
            playerView = new PlayerView(playerAnimator);
            
            moveAction = playerInput.actions["Move"];
            dodgeAction = playerInput.actions["Dodge"];
            handleLongPressTask = UniTask.CompletedTask;
            dodgeAction.performed += _ =>
            {
                if (isUnControllable) return;
                // InputUtil.HandleLongPress(() =>DodgeAndManageFlag(moveAction), ShowCloth, HideCloth, dodgeAction,
                    // GameConst.DodgeClothThreshold, CancellationToken.None);
                DodgeAndManageFlag(moveAction);
            };
            isScoreGetable = true;
        }
        protected override void Update()
        {
            base.Update();
            // 移動処理
            Vector2 moveDir = moveAction.ReadValue<Vector2>();
            if (!isUnControllable)
            {
                Vector2 moveSpeed = moveDir * GameConst.PlayerSpeed;
                playerView.SetWalkSpeed(moveSpeed.magnitude);
                Vector2? attackVector = buffalo.GetAttackVector(VectorUtils.ToXZ(transform.position));
                attackVector = rightBuffalo.GetAttackVector(VectorUtils.ToXZ(transform.position)) ?? attackVector;
                attackVector = leftBuffalo.GetAttackVector(VectorUtils.ToXZ(transform.position)) ?? attackVector;
                attackVector = centerBuffalo.GetAttackVector(VectorUtils.ToXZ(transform.position)) ?? attackVector;
                if (attackVector != null)
                {
                    onAttacked.OnNext(Unit.Default);
                    KnockBack((Vector2)attackVector);
                }
                else
                {
                    Move(moveSpeed);
                }
            }
        }

        private async void DodgeAndManageFlag(InputAction moveAction)
        {
            if (isUnControllable) return;
            lastDodgeTime = Time.time;
            isUnControllable = true;
            await Dodge(moveAction);
            lastDodgeTime = Time.time;
            isUnControllable = false;
        }

        private async UniTask KnockBack(Vector2 knockBackVector)
        {
            isUnControllable = true;
            isScoreGetable = false;
            playSceneSoundManager.PlayDamageSound();
            playerView.PlayDamagedAnimation();
            while (knockBackVector.sqrMagnitude > 0.01f)
            {
                Move(knockBackVector);
                knockBackVector *= GameConst.KnockBackDeceleration;
                await UniTask.Yield();
            }
            isScoreGetable = true;
            isUnControllable = false;
        }
    }
}