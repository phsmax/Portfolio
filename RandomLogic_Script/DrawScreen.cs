using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class DrawScreen : MonoBehaviour
    {
        public static DrawScreen instance;

        public GameObject MainField;
        public GameObject LifeField;

        public GameObject MainTop;
        public GameObject MainLeft;
        public GameObject MainBody;
        public GameObject MainBodySR_Line;
        public GameObject MainBodySC_Line;
        public GameObject MainBodyLR_Line;
        public GameObject MainBodyLC_Line;

        [HideInInspector]
        public GameObject MainStartPos;
        [HideInInspector]
        public GameObject UI_Pixels;
        [HideInInspector]
        public GameObject UI_Numrows;
        [HideInInspector]
        public GameObject UI_Numcolumns;
        [HideInInspector]
        public GameObject UI_Life;
        [HideInInspector]
        public GameObject ColumnLine;
        [HideInInspector]
        public GameObject RowLine;
        [HideInInspector]
        public GameObject RLine;
        [HideInInspector]
        public GameObject CLine;

        private void Awake()
        {
            instance = this;

            MainStartPos = MainField.transform.GetChild(0).gameObject;
            UI_Pixels = Resources.Load("04Prefabs/UI_Pixels", typeof(GameObject)) as GameObject;
            UI_Numrows = Resources.Load("04Prefabs/UI_Numrows", typeof(GameObject)) as GameObject;
            UI_Numcolumns = Resources.Load("04Prefabs/UI_Numcolumns", typeof(GameObject)) as GameObject;
            UI_Life = Resources.Load("04Prefabs/UI_Life", typeof(GameObject)) as GameObject;
            RowLine = Resources.Load("04Prefabs/RowLine", typeof(GameObject)) as GameObject;
            ColumnLine = Resources.Load("04Prefabs/ColumnLine", typeof(GameObject)) as GameObject;
            RLine = Resources.Load("04Prefabs/RLine", typeof(GameObject)) as GameObject;
            CLine = Resources.Load("04Prefabs/CLine", typeof(GameObject)) as GameObject;
        }

       

        public void SetLifeUI(int num)
        {
            GameManager.instance.SetLivesCount(num);
            for (int i = 0; i < num; i++)
            {
                GameObject go = Instantiate(UI_Life, LifeField.transform);
                DataBase.instance.lives[i] = go.GetComponent<LifePrefab>();
                go.GetComponent<LifePrefab>().SetTrue();
            }
            GameManager.instance.SetLife(DataBase.instance.leftLife);
        }

        public void SetMainUI()
        {
            SetMainUI_Top();
            SetMainUI_Left();
            SetMainUI_Pixels();
            SetBoldLine();
        }

        public void SetMainUI_Top()
        {
            for (int i = 0; i < 15; i++)
            {
                GameObject go = Instantiate(UI_Numcolumns, MainTop.transform);
                go.GetComponent<ColumnPrefab>().idx = i;
                DataBase.instance.colums[i] = go.GetComponent<ColumnPrefab>();
                go.GetComponent<ColumnPrefab>().SetDeafault();

            }
        }

        public void SetMainUI_Left()
        {
            for (int i = 0; i < 15; i++)
            {
                GameObject go = Instantiate(UI_Numrows, MainLeft.transform);
                go.GetComponent<RowPrefab>().idx = i;
                DataBase.instance.rows[i] = go.GetComponent<RowPrefab>();
                go.GetComponent<RowPrefab>().SetDeafault();
            }
        }

        public void SetMainUI_Pixels()
        {
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    GameObject go = Instantiate(UI_Pixels, MainBody.transform);
                    PixelPrefab pp = go.GetComponent<PixelPrefab>();
                    pp.idx = x + (y * 15);
                    pp.data = DataBase.instance.maps[x + (y * 15)];
                    if (pp.data == "01")
                    {
                        DataBase.instance.leftPixel.Add(pp);
                        DataBase.instance.rows[y].pixel.Add(pp);
                        DataBase.instance.rows[y].thisLinePixel.Add(pp);
                        DataBase.instance.colums[x].pixel.Add(pp);
                        DataBase.instance.colums[x].thisLinePixel.Add(pp);
                    }
                    else if (pp.data == "00")
                    {
                        DataBase.instance.rows[y].nopixel.Add(pp);
                        DataBase.instance.rows[y].thisLinePixel.Add(pp);
                        DataBase.instance.colums[x].nopixel.Add(pp);
                        DataBase.instance.colums[x].thisLinePixel.Add(pp);
                    }
                    DataBase.instance.pixels[x + (y * 15)] = pp;

                    GameObject rline = Instantiate(RLine, MainBodySR_Line.transform);
                    GameObject cline = Instantiate(CLine, MainBodySC_Line.transform);
                }
            }
        }

        public void SetBoldLine()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject row = Instantiate(RowLine, MainBodyLR_Line.transform);
                GameObject col = Instantiate(ColumnLine, MainBodyLC_Line.transform);
                
            }
        }

    }
}