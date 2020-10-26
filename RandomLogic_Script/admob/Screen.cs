using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class Screen : MonoBehaviour
    {

        string ScreenID = "";
        string testScreenID = "";

        string testDevice = "";

        public InterstitialAd screenAD;

        // Start is called before the first frame update
        void Start()
        {

        }

        void NextScene(object sender, EventArgs args)
        {
            SceneManager.LoadScene("Main");

        }

        public void InitAD()
        {
            string id = ScreenID;

            screenAD = new InterstitialAd(id);

            AdRequest request = new AdRequest.Builder().AddTestDevice(testDevice).Build();

            screenAD.LoadAd(request);
            screenAD.OnAdClosed += NextScene;
            screenAD.OnAdFailedToLoad += NextScene;

            StartCoroutine(ShowScreenAD());
        }


        IEnumerator ShowScreenAD()
        {
            while (!screenAD.IsLoaded())
            {
                yield return null;
            }

            screenAD.Show();
        }

    }
}