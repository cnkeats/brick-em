using UnityEngine;

public class UIManager : MonoBehaviour
{
    private TMPro.TextMeshProUGUI shotsRemainingText;
    private static TMPro.TextMeshProUGUI scoreText;
    private static TMPro.TextMeshProUGUI scoreAddText;
    private static TMPro.TextMeshProUGUI levelText;

    private void Awake()
    {
        shotsRemainingText = GameObject.Find("ShotsRemainingText").GetComponent<TMPro.TextMeshProUGUI>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        scoreAddText = GameObject.Find("ScoreAddText").GetComponent<TMPro.TextMeshProUGUI>();
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
        int prevScore = int.Parse(scoreText.text.Replace(",", ""));
        int prevScoreAdd = int.Parse(scoreAddText.text.Replace(",", "").Replace("+", ""));

        scoreText.text = score.ToString("N0");
        scoreAddText.text = string.Format("+ {0}", (score - prevScore + prevScoreAdd).ToString("N0"));

        scoreAddText.gameObject.GetComponent<ScoreAddFade>().RestartFade();
    }

    public static void UpdateWithLevel(Level level)
    {
        levelText = GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>();
        levelText.text = level?.levelMetadata.name ?? "-";
    }
}
