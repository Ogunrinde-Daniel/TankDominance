using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Tank> redTeam;
    [SerializeField] private List<Tank> greenTeam;
    [SerializeField] private TextMeshProUGUI scores;


    public bool gameOver = false;
    public int killsToWin = 10;
    public int loadIndex;

    private bool isPaused = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrimObjects();
        int greenTeamKills = CalculateKills(greenTeam);
        int redTeamKills = CalculateKills(redTeam);
        scores.SetText(greenTeamKills + "\n" + redTeamKills);

        if (redTeamKills >= killsToWin)
        {
            gameOver = true;
            Score.winTeam = "Red Team Wins\n" + greenTeamKills + " : " + redTeamKills;
            constructScore();
            SceneManager.LoadScene(loadIndex);
        }
        if (greenTeamKills >= killsToWin)
        {
            gameOver = true;
            Score.winTeam = "Green Team Wins\n" + greenTeamKills + " : " + redTeamKills;
            constructScore();
            SceneManager.LoadScene(loadIndex);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (isPaused){
                Time.timeScale = 1; 
                isPaused = false;
            }
            else{
                Time.timeScale = 0;
                isPaused = true;
            }

        }
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }


    }
        

    private int CalculateKills(List<Tank> list)
    {
        int totalKills = 0;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            totalKills += list[i].kills;
        }
        return totalKills;
    }


    private void TrimObjects()
    {
        /*trim the object that don't currently exist*/
        for (int i = redTeam.Count - 1; i >= 0; i--)
        {
            if (redTeam[i] == null || redTeam[i].gameObject.activeSelf == false)
            {
                redTeam.RemoveAt(i);
                redTeam.RemoveAt(i);
            }
        }
        for (int i = greenTeam.Count - 1; i >= 0; i--)
        {
            if (greenTeam[i] == null || greenTeam[i].gameObject.activeSelf == false)
            {
                greenTeam.RemoveAt(i);
                greenTeam.RemoveAt(i);
            }
        }

    }

    private void constructScore()
    {
        StringBuilder str1 = new StringBuilder();
        StringBuilder str2 = new StringBuilder();
        StringBuilder str3 = new StringBuilder();
        str1.Append("Name\n\n");
        str2.Append("Score\n\n");
        str3.Append("Kills\n\n");
        for (int i = greenTeam.Count - 1; i >= 0; i--)
        {
            str1.Append(greenTeam[i].gameObject.name + "\n");
            str2.Append(greenTeam[i].score + "\n");
            str3.Append(greenTeam[i].kills + "\n");
        }
        Score.greenTeamNames = str1.ToString();
        Score.greenTeamScores = str2.ToString();
        Score.greenTeamKills = str3.ToString();

        //red team
        str1 = new StringBuilder();
        str2 = new StringBuilder();
        str3 = new StringBuilder();

        str1.Append("Name\n\n");
        str2.Append("Score\n\n");
        str3.Append("Kills\n\n");
        for (int i = redTeam.Count - 1; i >= 0; i--)
        {
            str1.Append(redTeam[i].gameObject.name + "\n");
            str2.Append(redTeam[i].score + "\n");
            str3.Append(redTeam[i].kills + "\n");
        }
        Score.redTeamNames = str1.ToString();
        Score.redTeamScores = str2.ToString();
        Score.redTeamKills = str3.ToString();

    }

}
