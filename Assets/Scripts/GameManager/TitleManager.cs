using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;
        [SerializeField] private GameObject tutrialWindow;
        private InputAction submitAction;

        private void Start()
        {
            submitAction = playerInput.actions["Submit"];
        }

        void Update()
        {
            if (submitAction.triggered)
            {
                if(tutrialWindow.activeSelf)
                    SceneManager.LoadScene("MainScene");
                else
                {
                    tutrialWindow.SetActive(true);
                }
            }
        }
    }
}