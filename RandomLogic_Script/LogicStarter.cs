using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{

    public class LogicStarter : MonoBehaviour
    {
        public GameObject LogicCanvas;
        public CreateMap createmap;
        public DrawScreen drawScreen;
        public MapManager mapmanager;
        public GameObject lifeField;

        public GameObject MainTop;
        public GameObject MainLeft;
        public GameObject MainBody;
        public GameObject MainBodySR_Line;
        public GameObject MainBodySC_Line;
        public GameObject MainBodyLR_Line;
        public GameObject MainBodyLC_Line;

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            LogicCanvas.SetActive(true);
            DataBase.instance.leftPixel.Clear();
            DataBase.instance.switch_o_x = true;
            DataBase.instance.isTouch = true;
            DataBase.instance.isStop = false;
            createmap.CreateStart();
            drawScreen.SetLifeUI(3);
            drawScreen.SetMainUI();
            mapmanager.SetDataToPixels();
        }

        public void EndGame()
        {
            Transform[] life = lifeField.transform.GetComponentsInChildren<Transform>();
            DeleteChild(life);

            Transform[] maintop = MainTop.transform.GetComponentsInChildren<Transform>();
            DeleteChild(maintop);
            Transform[] mainleft = MainLeft.transform.GetComponentsInChildren<Transform>();
            DeleteChild(mainleft);
            Transform[] mainbody = MainBody.transform.GetComponentsInChildren<Transform>();
            DeleteChild(mainbody);
            Transform[] mainbodysr = MainBodySR_Line.transform.GetComponentsInChildren<Transform>();
            DeleteChild(mainbodysr);
            Transform[] mainbodysc = MainBodySC_Line.transform.GetComponentsInChildren<Transform>();
            DeleteChild(mainbodysc);
            Transform[] mainbodylr = MainBodyLR_Line.transform.GetComponentsInChildren<Transform>();
            DeleteChild(mainbodylr);
            Transform[] mainbodylc = MainBodyLC_Line.transform.GetComponentsInChildren<Transform>();
            DeleteChild(mainbodylc);

        }

        void DeleteChild(Transform[] tr, int startidx = 1)
        {
            for(int i = startidx;i<tr.Length; i++)
            {
                Destroy(tr[i].gameObject);
            }
        }
    }
}