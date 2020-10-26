using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    int ScoreMAX;

    private void Awake()
    {
        ScoreMAX = PlayerPrefs.GetInt("MAX", 0);
        GameObject.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = "" + ScoreMAX;
    }
    
    public void StartButtonClick()
    {
        SceneManager.LoadScene("tetris");
        
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void MaxButtonON()
    {
        GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(true);
    }
    public void MaxButtonOFF()
    {
        GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(false);
    }
}
