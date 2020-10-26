using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic {
    public class MapManager : MonoBehaviour
    {
        
        public void SetDataToPixels()
        { // 맵모양 확인하는 함수
            for (int i = 0; i < DataBase.instance.maps.Length; i++)
            {
                if (DataBase.instance.maps[i] == "11")
                    DataBase.instance.pixels[i].SetYES();
                else if (DataBase.instance.maps[i] == "10")
                    DataBase.instance.pixels[i].SetNO();
            }
        }


    }
}
