using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Audio;
using Person;
using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class ScoreManager
    { 
        public static async UniTask CheckTackleDog(Transform playerTransform, Player player,Func<Vector2, float> GetScore, Action<float> AddScore, PlaySceneSoundManager playSceneSoundManager,CancellationToken cancel)
        {
            while (true)
            {
                if (cancel.IsCancellationRequested) return;
                if (player.isScoreGetable)
                {
                    float score = GetScore(VectorUtils.ToXZ(playerTransform.position));
                    if (score <= 0.01f)
                    {
                    }
                    else if (score <= GameConst.DodgeScoreUpSmallValue + 0.01f)
                    {
                        playSceneSoundManager.PlayJoySmallSound();
                    }
                    else if (score <= GameConst.DodgeScoreUpGigValue + 0.01f)
                    {
                        playSceneSoundManager.PlayJoyBigSound();

                    }

                    AddScore(score);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(GameConst.DodgeScoreUpInterval), cancellationToken: cancel);
            }
        }
        
    }
}