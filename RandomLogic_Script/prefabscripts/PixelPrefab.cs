using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class PixelPrefab : MonoBehaviour
    {
        Sprite Blank;
        Sprite Full;
        Sprite NO;
        GameObject incorrectPrefab;
        public int idx;
        public string data;
        public bool isThisPixelTouch = false;

        void Awake()
        {
            Blank = Resources.Load("03Sprites/UI_Pixels_Blank", typeof(Sprite)) as Sprite;
            Full = Resources.Load("03Sprites/UI_Pixels_YES", typeof(Sprite)) as Sprite;
            NO = Resources.Load("03Sprites/UI_Pixels_NO", typeof(Sprite)) as Sprite;
            incorrectPrefab = Resources.Load("04Prefabs/Incorrect", typeof(GameObject)) as GameObject;
        }

        private void Start()
        {
            switch (data)
            {
                case "10":
                    SetNO();
                    break;
                case "11":
                    SetYES();
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                OnClickUpPixel();
        }

        public void SetBlank()
        {
            GetComponent<Image>().sprite = Blank;
        }

        public void SetYES()
        {
            GetComponent<Image>().sprite = Full;
            DataBase.instance.leftPixel.Remove(this);
            //pp.idx = x + (y * 15);
            int x = idx % 15;
            int y = idx / 15;
            DataBase.instance.colums[x].pixel.Remove(this);
            DataBase.instance.colums[x].SetFix();
            DataBase.instance.rows[y].pixel.Remove(this);
            DataBase.instance.rows[y].SetFix();
        }

        public void SetNO()
        {
            GetComponent<Image>().sprite = NO;
        }

        public void OnClickDownPixel()
        {
            if (DataBase.instance.isDragging)
                return;

            DataBase.instance.isDragging = true;
            int x = idx % 15;
            int y = idx / 15;
            DataBase.instance.colums[x].SetThisLineTrue();
            DataBase.instance.rows[y].SetThisLineTrue();
        }

        public void OnClickUpPixel()
        {
            DataBase.instance.allPixelFalse();
            DataBase.instance.isDragging = false;

        }

        public void Click()
        {
            // 최초 터치지점 정함

            if (!DataBase.instance.isTouch || DataBase.instance.isStop || !isThisPixelTouch)
                return;

            DataBase.instance.isTouch = false;

            if (data == "10" || data == "11")
            {
                DataBase.instance.isTouch = true;
                return;
            }

            if (!DataBase.instance.debugmode)
            {
                if (!Compare_CurrSwitch())
                {
                    GameManager.instance.MinusLives();
                    GameObject go = Instantiate(incorrectPrefab, this.transform);
                    go.GetComponent<RectTransform>().position = this.transform.GetComponent<RectTransform>().position;
                }
            }
            ShowThisPixel();

            GameManager.instance.SaveMaps();
            GameManager.instance.CheckClear();

            DataBase.instance.isTouch = true;

        }

        public void ShowThisPixel()
        {
            switch (data)
            {
                case "00":
                    SetNO();
                    DataBase.instance.maps[idx] = "10";
                    data = "10";
                    break;
                case "01":
                    SetYES();
                    DataBase.instance.maps[idx] = "11";
                    data = "11";
                    break;
            }
        }

        bool Compare_CurrSwitch()
        {
            if (DataBase.instance.switch_o_x)
            {
                if (data == "01")
                {
                    return true;
                }
            }
            else
            {
                if (data == "00")
                {
                    return true;
                }
            }
            return false;
        }
    }
}