using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    Blockmanager BM;
    int useCost = 500;

    void Start()
    {
        BM = FindObjectOfType<Blockmanager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager._SM.ScoreCurrent >= useCost)
            GetComponent<Button>().interactable = true;

    }

    public void ClickButton() // 되돌리기 버튼
    {
        for (int i = 0; i < 4; i++)
        {
            if (BM.returnList.Count > 0)
            {
                Destroy(BM.returnList.Pop());
            }
        }
        while(BM.returnList.Count > 0 && BM.returnList.Peek() == null)
        {
            BM.returnList.Pop();
        }

        ScoreManager._SM.AddScore(-useCost);
        GetComponent<Button>().interactable = false;
    }
}
