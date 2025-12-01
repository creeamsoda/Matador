using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Audio
{
    public class UISoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip titleBgm;
        [SerializeField] private AudioClip resultBgm;
        
        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "Title") BGMManager.instance.SetBgm(titleBgm);
            else if (SceneManager.GetActiveScene().name == "Result") BGMManager.instance.SetBgm(resultBgm);
        }
    }
}