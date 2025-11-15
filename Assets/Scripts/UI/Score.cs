using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemiesManager enemiesManager = FindAnyObjectByType<EnemiesManager>();
        TMP_Text textUi = GetComponent<TMP_Text>();

        int score = enemiesManager.GetNbEnemies();

        textUi.text = score.ToString();

        if(Utilities.maxScore < score)
        {
            Utilities.maxScore = score;
        }
    }

}
