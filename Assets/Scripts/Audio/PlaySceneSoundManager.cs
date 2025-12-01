using UnityEngine;

namespace DefaultNamespace.Audio
{
    public class PlaySceneSoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip gameBgm;
        [SerializeField] private AudioClip gameBgm2;
        [SerializeField] private AudioSource dodgeAS;
        [SerializeField] private AudioSource damageAS;
        [SerializeField] private AudioSource buffaloNormal1AS;
        [SerializeField] private AudioSource buffaloNormal2AS;
        [SerializeField] private AudioSource buffaloAngryAS;
        [SerializeField] private AudioSource buffaloChargeAS;
        [SerializeField] private AudioSource joyBigAS;
        [SerializeField] private AudioSource joySmallAS;

        private void Start()
        {
            BGMManager.instance.SetVolume(AudioConst.PlaySceneBgm1Volume);
            BGMManager.instance.SetPitch(1f);
            BGMManager.instance.SetBgm(gameBgm);
        }
        
        public void PlayGameBgm2()
        {
            BGMManager.instance.SetVolume(AudioConst.PlaySceneBgm2Volume);
            BGMManager.instance.SetPitch( AudioConst.PlaySceneBgmPitch);
            BGMManager.instance.SetBgm(gameBgm2);
        }

        public void PlayDodgeSound()
        {
            dodgeAS.PlayOneShot(dodgeAS.clip);
        }

        public void PlayDamageSound()
        {
            damageAS.PlayOneShot(damageAS.clip);
            joyBigAS.Stop();
            joySmallAS.Stop();
            buffaloAngryAS.PlayOneShot(buffaloAngryAS.clip);
        }

        public void PlayBuffaloNormal1Sound()
        {
            buffaloNormal1AS.PlayOneShot(buffaloNormal1AS.clip);
        }

        public void PlayBuffaloNormal2Sound()
        {
            buffaloNormal2AS.PlayOneShot(buffaloNormal2AS.clip);
        }

        public void PlayBuffaloAngrySound()
        {
            buffaloAngryAS.PlayOneShot(buffaloAngryAS.clip);
        }

        public void PlayBuffaloChargeSound()
        {
            buffaloChargeAS.PlayOneShot(buffaloChargeAS.clip);
        }

        public void PlayJoyBigSound()
        {
            joyBigAS.PlayOneShot(joyBigAS.clip);
        }

        public void PlayJoySmallSound()
        {
            joySmallAS.PlayOneShot(joySmallAS.clip);
        }
    }
}