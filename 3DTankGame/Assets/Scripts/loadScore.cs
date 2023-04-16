using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class loadScore : MonoBehaviour
{
    public TextMeshProUGUI rTeamNames;
    public TextMeshProUGUI rTeamKills;
    public TextMeshProUGUI rTeamScores;

    public TextMeshProUGUI gTeamNames;
    public TextMeshProUGUI gTeamKills;
    public TextMeshProUGUI gTeamScores;

    public TextMeshProUGUI winTeam;

    void Start()
    {
        rTeamNames.text = Score.redTeamNames;
        rTeamKills.text = Score.redTeamKills;
        rTeamScores.text = Score.redTeamScores;
        
        gTeamNames.text = Score.greenTeamNames;
        gTeamKills.text = Score.greenTeamKills;
        gTeamScores.text = Score.greenTeamScores;

        winTeam.text = Score.winTeam;
    }

}

public static class Score
{
    public static string redTeamNames = "";
    public static string redTeamKills = "";
    public static string redTeamScores = "";

    public static string greenTeamNames = "";
    public static string greenTeamKills = "";
    public static string greenTeamScores = "";
    
    public static string winTeam = "";

}
