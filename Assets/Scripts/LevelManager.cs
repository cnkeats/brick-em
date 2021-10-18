using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;

    public Level currentLevel;
    public GameObject currentLevelObject;

    private PlayerController player;
    private BallLauncher ballLauncher;

    private string[] levelList = { "Level_SingleMino", "Level_Basics", "Level_0", "Level_1", "Level_2" };
    private int currentLevelIndex = -1;

    public static List<Mino> currentBreakableMinos = new List<Mino>();
    public static List<Ball> currentProjectiles = new List<Ball>();

    private bool waitingToLoad = false;

    public void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        ballLauncher = FindObjectOfType<BallLauncher>();

        if (currentLevelObject != null)
        {
            currentLevelIndex = Array.FindIndex(levelList, n => n.Equals(currentLevelObject.name));
            currentLevel = LoadLevelByIndex(currentLevelIndex);
            player.UpdateCurrentLevel(currentLevel);
        }
        else
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevelButtonClick()
    {
        LoadNextLevel();
    }

    [ContextMenu("Load next level")]
    public Level LoadNextLevel()
    {
        currentLevelIndex += 1;
        return LoadLevelByIndex(currentLevelIndex);
    }

    public Level LoadLevelByIndex(int levelIndex)
    {
        string nextLevelName = levelList[levelIndex % levelList.Length];

        Debug.Log(string.Format("Loading index {0} - {1}", levelIndex, nextLevelName));

        Type levelType = Type.GetType(nextLevelName);
        Level level = Activator.CreateInstance(levelType) as Level;

        if (level == null)
        {
            Debug.Log(string.Format("Failed to load level {0}!", nextLevelName));
            return null;
        }

        Transform parent = (currentLevelObject != null) ? currentLevelObject.transform.parent : GameObject.Find("GameArea").transform;

        if (Application.isEditor)
        {
            DestroyImmediate(currentLevelObject);

            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                DestroyImmediate(ball.gameObject);
            }
        }
        else
        {
            Destroy(currentLevelObject);

            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                Destroy(ball.gameObject);
            }
        }

        currentLevel = level;

        // Create level objects
        currentLevelObject = Instantiate(level.levelContent);
        currentLevelObject.transform.parent = parent;
        currentLevelObject.name = level.levelMetadata.prefabName;
        currentLevelObject.tag = "Level";

        // Update UI
        UIManager.UpdateWithLevel(currentLevel);

        if (player != null)
        {
            // Update player data
            player.UpdateCurrentLevel(currentLevel);
            player.LevelAdvance();
        }

        waitingToLoad = false;
        return currentLevel;
    }

    public void Update()
    {
        if (!waitingToLoad)
        {
            CheckForLevelFinished();
        }

        if (currentProjectiles.Count == 0 && !ballLauncher.isActiveAndEnabled)
        {
            ballLauncher.gameObject.SetActive(true);
        }
    }

    private void CheckForLevelFinished()
    {
        if (currentProjectiles.Count > 0)
        {
            return;
        }

        int minosRemaining = currentBreakableMinos.Count;
        if (minosRemaining == 0)
        {
            Debug.Log("Level complete!");

            Invoke("LoadNextLevel", 2.5f);
            waitingToLoad = true;
            return;
        }

        int shotsInQueue = PlayerController.shotQueue.Count;
        int levelShotLimit = currentLevel.levelMetadata.shotsAllowed;
        if (player.usedShots >= levelShotLimit && shotsInQueue == 0)
        {
            Debug.Log("Level failed! :(");
            Invoke("LoadNextLevel", 2.5f);
            waitingToLoad = true;
        }
    }
}