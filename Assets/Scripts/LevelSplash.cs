using System.Linq;
using UnityEngine;

public class LevelSplash : MonoBehaviour
{
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI blurb;
    private TMPro.TextMeshProUGUI score;

    private LevelManager levelManager;
    private PlayerController player;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();

        TMPro.TextMeshProUGUI[] texts = transform.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        title = texts.Where(t => t.gameObject.name.Equals("LevelTitle")).First();
        blurb = texts.Where(t => t.gameObject.name.Equals("LevelBlurb")).First();
        score = texts.Where(t => t.gameObject.name.Equals("Score")).First();
    }

    private void OnEnable()
    {
        title.text = levelManager.currentLevel.levelMetadata.name;
        blurb.text = levelManager.currentLevel.levelMetadata.blurb;
        score.text = player.GetScore().ToString("N0");
    }
}
