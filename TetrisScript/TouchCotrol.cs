using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCotrol : MonoBehaviour,IDragHandler,IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    Blockmanager BM;
    bool candown = true;
    bool dragCut = false;
    float cooltime = 0.03f;
    Vector2 originpos;
    Vector2 pos;
    public float sensi = 0.165f;
    public int vsensi = 50;


    public void OnBeginDrag(PointerEventData eventData) 
    {
        pos = Camera.main.ScreenToWorldPoint(eventData.position);
        dragCut = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (BM.end)
            return;

        Vector2 bpos = Camera.main.ScreenToWorldPoint(eventData.position);

        if (eventData.delta.y < -vsensi && dragCut) // 아래로 그으면 바로 떨어짐
        {
            if (BM.MoveCondition(1))
                BM.cooltime = 0;
            dragCut = false;
            return;
        }
        else if (eventData.delta.y < 0 && candown && pos.y > eventData.position.y)
        {
            BM.DownMove();
            candown = false;
        }

        // 천천히 좌우이동하면 센시가 조정됨
        if (Mathf.Abs(eventData.delta.x) < 20) 
            sensi = sensi * 1.7f;

        // 좌로이동
        if (pos.x > bpos.x + sensi && !(Mathf.Abs(eventData.delta.y) > 10)) 
        {
            int repeat = (int)((pos.x - bpos.x + sensi)/0.33f);
            if (repeat == 0)
                repeat = 1;
            for(int i = 0; i<repeat; i++)
                BM.LeftMove();
            pos = Camera.main.ScreenToWorldPoint(eventData.position);
        }
        // 우로이동
        else if (pos.x < bpos.x - sensi && !(Mathf.Abs(eventData.delta.y) > 10)) 
        {
            int repeat = (int)((bpos.x - sensi - pos.x) / 0.33f);
            if (repeat == 0)
                repeat = 1;
            for (int i = 0; i < repeat; i++)
                BM.RightMove();
            pos = Camera.main.ScreenToWorldPoint(eventData.position);
        }

        sensi = PlayerPrefs.GetFloat("SENSISAVE", 0.165f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originpos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if((originpos - eventData.position).sqrMagnitude < 300)
        {
            BM.UpButton();
        }
    }

    IEnumerator DownCool()
    {
        while (true)
        {
            yield return new WaitWhile(() => candown);
            if (!candown)
            {
                yield return new WaitForSeconds(cooltime);
                candown = true;
            }
            yield return null;
        }
    }


    void Start()
    {
        BM = FindObjectOfType<Blockmanager>();
        StartCoroutine(DownCool());
    }

}