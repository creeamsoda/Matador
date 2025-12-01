using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ManageScoreUpText : MonoBehaviour
    {
        [SerializeField] GameObject scoreUpText;
        [SerializeField] GameObject scoreDownText;

        private void Update()
        {
            this.transform.rotation = Quaternion.Euler(60, 0, 0);
        }

        public void ShowScoreUpText(string text)
        {
            GameObject textInstance = Instantiate(scoreUpText, transform.position, Quaternion.identity, parent:this.transform);
            textInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            textInstance.GetComponent<TextMeshProUGUI>().SetText(text);
            DestroyAfterFade(textInstance).Forget();
        }
        
        public void ShowScoreDownText(string text)
        {
            GameObject textInstance = Instantiate(scoreDownText, transform.position, Quaternion.identity, parent:this.transform);
            textInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            textInstance.GetComponent<TextMeshProUGUI>().SetText(text);
            DestroyAfterFade(textInstance).Forget();
        }
        
        private async UniTask DestroyAfterFade(GameObject obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            Destroy(obj);
        }
    }
}