using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Logic
{
    public class ButtonManger : MonoBehaviour
    {
        public Image Obutton;
        public Image Xbutton;

        Color selectcolor = new Color(0.008721969f, 0.2641509f, 0.0114556f);

        public void Start()
        {
            Click_Obutton();
        }

        public void Click_Obutton()
        {
            DataBase.instance.switch_o_x = true;
            Obutton.color = selectcolor;
            Xbutton.color = Color.white;
        }

        public void Click_Xbutton()
        {
            DataBase.instance.switch_o_x = false;
            Xbutton.color = selectcolor;
            Obutton.color = Color.white;
        }
    }
}