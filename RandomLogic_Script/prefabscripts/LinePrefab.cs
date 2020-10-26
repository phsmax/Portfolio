using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class LinePrefab : MonoBehaviour
    {
        private void Start()
        {
            DataBase.instance.playerClearLineEffect += Die;
        }

        public void Die()
        {
            StartCoroutine(DieEffect());
        }

        IEnumerator DieEffect()
        {
            float time = 0;
            Image img = GetComponent<Image>();
            while (time < 1.5f)
            {
                time += Time.deltaTime;
                img.color = Color.Lerp(Color.white, Color.clear, time / 1.5f);
                yield return null;
            }

            yield return null;
            DataBase.instance.playerClearLineEffect -= Die;

        }
    }
}