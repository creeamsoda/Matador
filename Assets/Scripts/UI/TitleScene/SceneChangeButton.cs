using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.UI.TitleScene
{
    public class SceneChangeButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        public void SceneChange()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}