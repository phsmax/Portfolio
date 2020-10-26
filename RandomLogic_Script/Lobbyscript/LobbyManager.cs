using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Logic
{
    public class LobbyManager : MonoBehaviour
    {
        public GameObject continuebutton;
        public GameObject collectioncanvas;
        public GameObject content;

        public Text text;


        private void Start()
        {
            if (File.Exists(Application.persistentDataPath + "/currmap.json"))
                continuebutton.SetActive(true);
            else
                continuebutton.SetActive(false);

            text.text = UnityEngine.Screen.width.ToString() + " / " + UnityEngine.Screen.height.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !collectioncanvas.activeSelf)
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && collectioncanvas.activeSelf)
            {
                collectioncanvas.SetActive(false);
                Transform[] go = content.transform.GetComponentsInChildren<Transform>();
                for (int i = 1; i < go.Length; i++)
                {
                    Destroy(go[i].gameObject);
                }
            }
        }

        public void ContinueButton()
        {
            SceneManager.LoadScene("Main");
        }

        public void NewButton()
        {
            if (File.Exists(Application.persistentDataPath + "/currmap.json"))
                File.Delete(Application.persistentDataPath + "/currmap.json");

            PlayerPrefs.DeleteAll();

            SceneManager.LoadScene("Main");
        }
    }
}