using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class InputUtil
    {
        public static async UniTask HandleLongPress(Action shortPressAction, Action longPressAction, InputAction button, float longPressThreshold, CancellationToken cancel)
        {
            if (!button.IsPressed()) return;
            float pressDuration = 0f;
            while (button.IsPressed())
            {
                if (cancel.IsCancellationRequested) return;
                pressDuration += Time.deltaTime;
                if (pressDuration >= longPressThreshold)
                {
                    longPressAction?.Invoke();
                    return;
                }
                await UniTask.Yield();
            }
            if (cancel.IsCancellationRequested) return;
            if (pressDuration < longPressThreshold)
            {
                shortPressAction?.Invoke();
            }
        }
        
        public static async UniTask HandleLongPress(Action shortPressAction, Action longPressingAction, Action longPressEndAction, InputAction buttonAction, float longPressThreshold, CancellationToken cancel)
        {
            if (!buttonAction.IsPressed()) return;
            float pressDuration = 0f;
            while (buttonAction.IsPressed())
            {
                if (cancel.IsCancellationRequested) return;
                pressDuration += Time.deltaTime;
                if (pressDuration >= longPressThreshold)
                {
                    longPressingAction?.Invoke();
                }
                await UniTask.Yield();
            }
            if (cancel.IsCancellationRequested) return;
            if (pressDuration < longPressThreshold)
            {
                shortPressAction?.Invoke();
            }
            else
            {
                longPressEndAction?.Invoke();
            }
        }
    }
}