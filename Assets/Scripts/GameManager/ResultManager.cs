using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] TextMeshProUGUI maxComboText;
        private InputAction backAction;
        private void Start()
        {
            scoreText.text = $"Score : {GameManager.score}";
            backAction = playerInput.actions["Submit"];
            maxComboText.text = $"最大コンボ : {GameStats.maxCombo}\n被ダメージ合計 : {GameStats.damegeSum}";
        }
        
        private void Update()
        {
            if (backAction.triggered) SceneManager.LoadScene("Title");
        }
    }
}