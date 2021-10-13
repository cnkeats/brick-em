using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private TMPro.TextMeshProUGUI shotsRemainingText;
    private static TMPro.TextMeshProUGUI scoreText;
    private static TMPro.TextMeshProUGUI levelText;

    private void Awake()
    {
        shotsRemainingText = GameObject.Find("ShotsRemainingText").GetComponent<TMPro.TextMeshProUGUI>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        levelText = GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Update()
    {
        shotsRemainingText.text = PlayerController.shotQueue.Count.ToString();
    }

    public void DisplayShotQueue()
    {
        foreach (GameObject shot in PlayerController.shotQueue)
        {
            
        }
    }

    public static void UpdateScoreDisplay(int score)
    {
        scoreText.text = score.ToString("N0");
    }

    public static void UpdateWithLevel(Level level)
    {
        levelText.text = level.levelMetadata.name;
    }
}
