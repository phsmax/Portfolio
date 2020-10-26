using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class LifePrefab : MonoBehaviour
    {
        Sprite Life_True;
        Sprite Life_False;
        public bool isLife;


        void Awake()
        {
            Life_True = Resources.Load("03Sprites/UI_Life_True", typeof(Sprite)) as Sprite;
            Life_False = Resources.Load("03Sprites/UI_Life_False", typeof(Sprite)) as Sprite;
            DataBase.instance.playerDieLifeEffect += Die;
        }

        public void SetTrue()
        {
            GetComponent<Image>().sprite = Life_True;
            isLife = true;
        }

        public void SetFalse()
        {
            GetComponent<Image>().sprite = Life_False;
            isLife = false;
        }

        public void Die()
        {
            StartCoroutine(DieEffect());
        }

        IEnumerator DieEffect()
        {
            float time = 0;
            while (time < 2f)
            {
                time += Time.deltaTime;
                this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5, time / 2f);
                yield return null;
            }

            yield return null;
        }
    }
}