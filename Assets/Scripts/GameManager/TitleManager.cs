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

        [SerializeField] private GameObject GetGoogleSpreadSheetButton;

        private void Start()
        {
            submitAction = playerInput.actions["Submit"];
            GetGoogleSpreadSheetButton.SetActive(false);
#if UNITY_EDITOR
            GetGoogleSpreadSheetButton.SetActive(true);
#endif
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