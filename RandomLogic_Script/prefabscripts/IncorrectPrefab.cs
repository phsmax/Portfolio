using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class IncorrectPrefab : MonoBehaviour
    {
        Image image;
        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();

            Handheld.Vibrate();
            StartCoroutine(DestroySelf());
        }

        IEnumerator DestroySelf()
        {
            DataBase.instance.isTouch = false;
            float time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                image.color = Color.Lerp(Color.red, Color.clear, time / 1f);
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            DataBase.instance.isTouch = true;
            Destroy(this.gameObject);
        }
    }
}