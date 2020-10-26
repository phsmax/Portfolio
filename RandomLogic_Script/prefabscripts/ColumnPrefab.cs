using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{

    public class ColumnPrefab : MonoBehaviour
    {
        Text text;
        public int idx;
        public List<int> num = new List<int>();
        public List<PixelPrefab> pixel = new List<PixelPrefab>();
        public List<PixelPrefab> nopixel = new List<PixelPrefab>();
        public List<PixelPrefab> thisLinePixel = new List<PixelPrefab>();



        private void Awake()
        {
            text = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            SetFix();
        }

        public void SetThisLineTrue()
        {
            foreach (PixelPrefab pp in thisLinePixel)
                pp.isThisPixelTouch = true;
        }

        public void SetThisLineFalse()
        {
            foreach (PixelPrefab pp in thisLinePixel)
                pp.isThisPixelTouch = false;
        }

        public void SetFix()
        {
            if (pixel.Count > 0)
                return;

            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            for (int i = 0; i < nopixel.Count; i++)
            {
                nopixel[i].ShowThisPixel();
            }
        }

        public void SetDeafault()
        {
            int count = 0;
            int count0 = 0;
            for (int i = 0; i < 15; i++)
            {

                if (DataBase.instance.maps[idx + (i * 15)] == "00" || DataBase.instance.maps[idx + (i * 15)] == "10")
                {
                    if (i != 0 && count0 == 0)
                        num.Add(count);
                    count0++;
                    count = 0;
                }
                else if (DataBase.instance.maps[idx + (i * 15)] == "01" || DataBase.instance.maps[idx + (i * 15)] == "11")
                {
                    count0 = 0;
                    count++;
                }
            }
            if (num.Count == 0 && count == 0)
                num.Add(0);
            if (count != 0)
                num.Add(count);
            SetNum(num);
        }

        public void SetNum(List<int> nums)
        {
            int count = nums.Count;
            string txt = "";
            for (int i = 0; i < count; i++)
            {
                txt += nums[i].ToString();
                if (i != count - 1)
                    txt += "\n";
            }
            text.text = txt;
        }
    }
}