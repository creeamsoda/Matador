using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class ManageScoreText : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text scoreText;
        [SerializeField] private TextMeshProUGUI bonusText;
        [SerializeField] private GameManager gameManager;
        
        private void Update()
        {
            scoreText.text = $"Score: {GameManager.score}";
            bonusText.text = "";
            Debug.Log("ComboBonus: " + GameManager.GetComboBonus() + " WalkBonus: " + gameManager.GetWalkBonus() + "");
            if (GameManager.GetComboBonus() > 1)
            {
                bonusText.text += $"\nコンボボーナス ×{GameManager.GetComboBonus()}";
            }
            if (gameManager.GetWalkBonus() > 1)
            {
                bonusText.text += $"\n歩きボーナス ×{gameManager.GetWalkBonus()}";
            }
        }
    }
}