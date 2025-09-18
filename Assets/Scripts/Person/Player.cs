using System.Threading;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using Utils;

namespace Person
{
    public class Player : Person
    {
        private bool isDodging = false;
        protected override void Update()
        {
            base.Update();
            // 移動処理
            Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Move(moveDir * GameConst.PlayerSpeed);
            
            // 赤布の出し入れ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InputUtil.HandleLongPress(DodgeAndManageFlag, ShowCloth, HideCloth, KeyCode.Space, GameConst.DodgeClothThreshold, CancellationToken.None);
            }
            Debug.Log("isShowingCloth: " + isShowingCloth);
        }

        private async void DodgeAndManageFlag()
        {
            if (isDodging) return;
            isDodging = true;
            await Dodge();
            isDodging = false;
        }
    }
}