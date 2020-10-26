using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Lobby");
            }
        }

        public void SaveMaps()
        {
            string json = JsonMapper.ToJson(DataBase.instance.maps);
            File.WriteAllText(Application.persistentDataPath + "/currmap.json", json);
        }

        public bool LoadMaps()
        {
            if (!File.Exists(Application.persistentDataPath + "/currmap.json"))
                return false;

            string json = File.ReadAllText(Application.persistentDataPath + "/currmap.json");
            DataBase.instance.maps = JsonMapper.ToObject<string[]>(json);
            return true;
        }

        public void SetLivesCount(int num)
        {
            DataBase.instance.lives = new LifePrefab[num];
        }

        public void SetLife(int num)
        {
            foreach (LifePrefab lp in DataBase.instance.lives)
                lp.SetTrue();

            for (int i = 0; i < DataBase.instance.lives.Length - DataBase.instance.leftLife; i++)
            {
                DataBase.instance.lives[DataBase.instance.lives.Length - 1 - i].SetFalse();
            }
        }

        public void MinusLives()
        {

            int count = 0;
            for (int i = 0; i < DataBase.instance.lives.Length; i++)
            {
                if (DataBase.instance.lives[i].isLife)
                    count++;
            }

            DataBase.instance.lives[count - 1].SetFalse();
            DataBase.instance.leftLife = count - 1;
            if (count == 1)
            {
                PlayerDie();
                return; // 사망함
            }

        }

        public void PlayerDie()
        {

            StartCoroutine(DieEffect());

        }

        IEnumerator DieEffect()
        {
            DataBase.instance.playerDieLifeEffect();
            float time = 0;
            while (time < 2f)
            {
                time += Time.deltaTime;
                DataBase.instance.isStop = true;
                yield return null;
            }
            for (int i = 0; i < DataBase.instance.maps.Length; i++)
            {
                switch (DataBase.instance.maps[i])
                {
                    case "11":
                        DataBase.instance.maps[i] = "01";
                        break;
                    case "10":
                        DataBase.instance.maps[i] = "00";
                        break;
                }
            }
            SaveMaps();
            DataBase.instance.leftLife = 0;
            SceneManager.LoadScene("Lobby");
        }

        public void CheckClear()
        {
            if (DataBase.instance.leftPixel.Count == 0)
            {
                StartCoroutine(ClearThisMap());
                DataBase.instance.isStop = true;
            }
        }

        IEnumerator ClearThisMap()
        {
            DataBase.instance.playerClearLineEffect();
            DataBase.instance.leftLife = 3;
            SaveCollection();
            yield return new WaitForSeconds(2.5f);

            File.Delete(Application.persistentDataPath + "/currmap.json");
            PlayerPrefs.DeleteAll();

            FindObjectOfType<Screen>().InitAD();
            //SceneManager.LoadScene("Main");

            yield return null;
        }

        public void SaveCollection()
        {
            string path = Application.persistentDataPath + "collection/";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                di.Create();

            FileInfo[] fis = di.GetFiles("*.data");

            int idx = fis.Length;

            Texture2D tex = new Texture2D(390,390);
            for (int j = 0; j<15; j++)
            {
                for (int i = 0; i < 15; i++)
                {
                    Color green = new Color(0, 0.47f, 0.06f, 1);
                    Color blue = new Color(0.075f, 0.64f, 0.63f, 1);
                    switch (DataBase.instance.maps[i + (j * 15)])
                    {
                        case "00":
                            _setPixel(tex, i, 14-j, blue);
                            break;
                        case "10":
                            _setPixel(tex, i, 14-j, blue);
                            break;
                        case "01":
                            _setPixel(tex, i, 14-j, green);
                            break;
                        case "11":
                            _setPixel(tex, i, 14-j, green);
                            break;
                    }
                }
            }
            tex.Apply();
            byte[] save = tex.EncodeToPNG();
            File.WriteAllBytes(path + idx.ToString("D6") + ".data", save);
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