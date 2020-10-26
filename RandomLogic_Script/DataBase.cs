using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

namespace Logic
{
    public class DataBase : MonoBehaviour
    {
        public static DataBase instance;

        public bool debugmode;

        public LifePrefab[] lives;
        public int leftLife
        {
            get {
                if (PlayerPrefs.GetInt("LEFTLIFE", 3) == 0)
                    return 3;
                return PlayerPrefs.GetInt("LEFTLIFE", 3); 
            }
            set { PlayerPrefs.SetInt("LEFTLIFE", value); }
        }

        public ColumnPrefab[] colums = new ColumnPrefab[15];
        public RowPrefab[] rows = new RowPrefab[15];
        public PixelPrefab[] pixels = new PixelPrefab[225];
        public string[] maps = new string[225];

        public bool switch_o_x = true;
        public bool isTouch = true;
        public bool isStop = false;

        public Action playerDieLifeEffect;
        public Action playerClearLineEffect;

        public List<PixelPrefab> leftPixel = new List<PixelPrefab>();

        public Action allPixelFalse;
        public bool isDragging = false;


        private void Awake()
        {
            instance = this;
        }

        public void DebugMode()
        {
            debugmode = !debugmode;
        }


    }
}