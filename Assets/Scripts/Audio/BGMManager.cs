using UnityEngine;

namespace DefaultNamespace.Audio
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager instance;
        
        [SerializeField] private AudioSource audioSource;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetBgm(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        
        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }
        public void SetPitch(float pitch)
        {
            audioSource.pitch = pitch;
        }
    }
}