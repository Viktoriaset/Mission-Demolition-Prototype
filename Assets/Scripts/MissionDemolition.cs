using System;
using TMPro;
using UnityEngine;

public enum GameMode
{
    idle, 
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in inspector")]
    public TextMeshProUGUI uitLevel;
    public TextMeshProUGUI uitShots;
    public TextMeshProUGUI uitButton;

    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show slingshot";

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
            Destroy(pTemp);

        castle = Instantiate(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    private void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots taken: " + shotsTaken;
    }

    private void Update()
    {
        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;

            SwitchView("Show Both");

            Invoke("NextLevel", 2f);
        }
    }

    private void NextLevel()
    {
        level++;
        level = level == levelMax ? 0 : level;

        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }

        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
