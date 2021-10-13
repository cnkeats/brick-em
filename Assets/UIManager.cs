using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private TMPro.TextMeshProUGUI shotsRemainingText;

    private void Awake()
    {
        shotsRemainingText = GameObject.Find("ShotsRemainingText").GetComponent<TMPro.TextMeshProUGUI>();
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
        GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString("N0");
    }
}
