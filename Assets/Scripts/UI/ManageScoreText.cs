using UnityEngine;

namespace DefaultNamespace.UI
{
    public class ManageScoreText : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text scoreText;
        [SerializeField] private GameManager gameManager;
        
        private void Update()
        {
            scoreText.text = $"Score: {gameManager.score}\nCombo: {gameManager.comboCount}";
        }
    }
}