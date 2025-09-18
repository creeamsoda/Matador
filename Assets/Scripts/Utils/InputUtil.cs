using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class InputUtil
    {
        public static async UniTask HandleLongPress(Action shortPressAction, Action longPressAction, KeyCode keycode, float longPressThreshold, CancellationToken cancel)
        {
            if (!Input.GetKey(keycode)) return;
            float pressDuration = 0f;
            while (Input.GetKey(keycode))
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
        
        public static async UniTask HandleLongPress(Action shortPressAction, Action longPressingAction, Action longPressEndAction, KeyCode keycode, float longPressThreshold, CancellationToken cancel)
        {
            if (!Input.GetKey(keycode)) return;
            float pressDuration = 0f;
            while (Input.GetKey(keycode))
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