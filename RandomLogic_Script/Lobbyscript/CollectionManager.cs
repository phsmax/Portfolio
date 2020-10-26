using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

namespace Logic
{
    public class CollectionManager : MonoBehaviour
    {
        public List<Sprite> clearCollection = new List<Sprite>();
        public Transform Content;
        public GameObject CollectionContentPrefab;
        public GameObject CollectionCanvas;
        public GameObject CollectionEmptyPrefab;

        public GameObject CountImage;
        public Text countText;

        private void Start()
        {
            SetCount();
            ConvertOldVersion();
        }

        public void SetCount()
        {
            int count = CountCollection();
            if (count == 0)
                return;

            CountImage.SetActive(true);
            countText.text = "+" + count;
            StartCoroutine(CountEffect());
        }

        IEnumerator CountEffect()
        {
            Image img = CountImage.GetComponent<Image>();
            Color c1 = new Color(1, 1, 1, 0.22f);
            Color c2 = new Color(1, 1, 1, 0.42f);
            float timing = 0.7f;
            while (true)
            {
                float time = 0;
                while (time < timing)
                {
                    img.color = Color.Lerp(c1, c2, time / timing);
                    time += Time.deltaTime;
                    yield return null;
                }
                time = 0;
                while (time < timing)
                {
                    img.color = Color.Lerp(c2, c1, time / timing);
                    time += Time.deltaTime;
                    yield return null;
                }
            }
        }



        public void ClickCollection()
        {
            CollectionCanvas.SetActive(true);

            if (clearCollection.Count < CountCollection())
                LoadCollection();

            if (clearCollection.Count == 0)
                Instantiate(CollectionEmptyPrefab, Content);

            foreach (Sprite st in clearCollection)
            {
                GameObject go = Instantiate(CollectionContentPrefab, Content);
                go.GetComponent<Image>().sprite = st;
            }
        }

        public int CountCollection()
        {
            string path = Application.persistentDataPath + "collection/";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                return 0;

            FileInfo[] fis = di.GetFiles("*.data");

            return fis.Length;
        }

        public void LoadCollection()
        {
            clearCollection.Clear();
            int idx = 0;
            while (File.Exists(Application.persistentDataPath + "collection/" + idx.ToString("D6") + ".data"))
            {
                string path = Application.persistentDataPath + "collection/" + idx.ToString("D6") + ".data";
                byte[] byteTexture = System.IO.File.ReadAllBytes(path);
                if (byteTexture.Length > 0)
                {
                    Texture2D texture = new Texture2D(0, 0);
                    texture.LoadImage(byteTexture);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    clearCollection.Add(sprite);
                }
                idx++;
            }
        }
        void ConvertOldVersion()
        {
            string path = Application.persistentDataPath + "collection/";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                di.Create();

            FileInfo[] fis = di.GetFiles("*.json");

            int x = 0;
            while (File.Exists(path + x.ToString("D6") + ".json"))
            {
                string json = File.ReadAllText(path + x.ToString("D6") + ".json");
                string[] data = JsonMapper.ToObject<string[]>(json);
                Texture2D tex = new Texture2D(390, 390);
                for (int j = 0; j < 15; j++)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Color green = new Color(0, 0.47f, 0.06f, 1);
                        Color blue = new Color(0.075f, 0.64f, 0.63f, 1);
                        switch (data[i + (j * 15)])
                        {
                            case "00":
                                _setPixel(tex, i, 14 - j, blue);
                                break;
                            case "10":
                                _setPixel(tex, i, 14 - j, blue);
                                break;
                            case "01":
                                _setPixel(tex, i, 14 - j, green);
                                break;
                            case "11":
                                _setPixel(tex, i, 14 - j, green);
                                break;
                        }
                    }
                }
                tex.Apply();
                byte[] save = tex.EncodeToPNG();
                File.WriteAllBytes(path + x.ToString("D6") + ".data", save);
                File.Delete(path + x.ToString("D6") + ".json");
                x++;

            }
        }
        void _setPixel(Texture2D t2d, int x, int y, Color color)
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    t2d.SetPixel(i + x * 26, j + y * 26, color);
                }
            }
        }
    }
}
