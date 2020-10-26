using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private static ScoreManager SM;
    public int ScoreCurrent;
    int ScoreMAX;
    

    public static ScoreManager _SM
    {

        get
        {
            if (SM == null)
            {
                GameObject Scoremanager = new GameObject("Scoremanager");
                Scoremanager.AddComponent<ScoreManager>();
            }
            return SM;
        }
    }

    private void Awake()
    {
        SM = this;
        ScoreMAX = PlayerPrefs.GetInt("MAX", 0 );
        GameObject.Find("Score").GetComponent<Text>().text = "" + ScoreCurrent;
        GameObject.Find("ScoreMAX").GetComponent<Text>().text = "" + ScoreMAX;
    }

    public void AddScore() // 점수 100점추가
    {
        ScoreCurrent += 100;
        if (ScoreCurrent > ScoreMAX)
        {
            ScoreMAX = ScoreCurrent;
            PlayerPrefs.SetInt("MAX", ScoreMAX);
        }
        GameObject.Find("Score").GetComponent<Text>().text = "" + ScoreCurrent;
        GameObject.Find("ScoreMAX").GetComponent<Text>().text = "" + ScoreMAX;
    }

    public void AddScore(int _score) // 점수 int값만큼 추가
    {
        ScoreCurrent += _score;
        if (ScoreCurrent > ScoreMAX)
        {
            ScoreMAX = ScoreCurrent;
            PlayerPrefs.SetInt("MAX", ScoreMAX);
        }
        GameObject.Find("Score").GetComponent<Text>().text = "" + ScoreCurrent;
        GameObject.Find("ScoreMAX").GetComponent<Text>().text = "" + ScoreMAX;
    }

}
