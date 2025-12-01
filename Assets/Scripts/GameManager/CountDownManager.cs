using Cysharp.Threading.Tasks;
using DefaultNamespace.Audio;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class CountDownManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private Animator countDownAnimator;
        [SerializeField] private PlaySceneSoundManager playSceneSoundManager;

        public async UniTask PlayGo()
        {
            playSceneSoundManager.PlayBuffaloNormal1Sound();
            await UniTask.Delay(100);
            countDownAnimator.SetTrigger("Go!");
            
        }

        public async UniTask StartCountDownAndFinish()
        {
            countDownText.text = "3";
            countDownAnimator.SetTrigger("UpdateCount");
            await UniTask.Delay(1000);
            countDownText.text = "2";
            countDownAnimator.SetTrigger("UpdateCount");
            await UniTask.Delay(1000);
            countDownText.text = "1";
            countDownAnimator.SetTrigger("UpdateCount");
            await UniTask.Delay(1000);
            countDownText.text = "Finish!!";
            countDownAnimator.SetTrigger("Finish!");
            playSceneSoundManager.PlayBuffaloNormal1Sound();
            await UniTask.Delay(3000);
        }
    }
}