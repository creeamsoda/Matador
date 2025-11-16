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
        [System.NonSerialized] public Transform buffaloTransform;
        private bool isDodging = false;
        InputAction moveAction;
        InputAction dodgeAction;
        [SerializeField] private PlayerInput playerInput;
        private UniTask handleLongPressTask;
        protected override void Start()
        {
            base.Start();
            
            moveAction = playerInput.actions["Move"];
            dodgeAction = playerInput.actions["Dodge"];
            handleLongPressTask = UniTask.CompletedTask;
            dodgeAction.performed += _ =>
            {
                InputUtil.HandleLongPress(() =>DodgeAndManageFlag(moveAction), ShowCloth, HideCloth, dodgeAction,
                    GameConst.DodgeClothThreshold, CancellationToken.None);
            };
        }
        protected override void Update()
        {
            base.Update();
            // 移動処理
            Vector2 moveDir = moveAction.ReadValue<Vector2>();
            if (!isDodging)
                Move(moveDir * GameConst.PlayerSpeed);
            Debug.Log("isShowingCloth: " + isShowingCloth);
        }

        private async void DodgeAndManageFlag(InputAction moveAction)
        {
            if (isDodging) return;
            isDodging = true;
            await Dodge(moveAction);
            isDodging = false;
        }
    }
}