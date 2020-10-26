using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class Banner : MonoBehaviour
    {
        string bannerID = "";
        string testbannerID = "";

        string testDevice = "";

        public BannerView banner;

        // Start is called before the first frame update
        void Start()
        {
            InitAD();

        }

        void InitAD()
        {
            string id = bannerID;

            banner = new BannerView(id, AdSize.SmartBanner, AdPosition.Bottom);

            AdRequest request = new AdRequest.Builder().AddTestDevice(testDevice).Build();

            banner.LoadAd(request);
            banner.Show();
        }

    }
}