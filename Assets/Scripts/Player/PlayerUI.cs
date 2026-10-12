using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NbVie;
    [SerializeField] private TextMeshProUGUI textScore;
        public void UpdateScore(int score)
    {
        textScore.text = "Score: " + score.ToString();
    }

    public void UpdateHealth(int health)
    {
        NbVie.text = "Nombre de vies: " + health.ToString();
    }

}
