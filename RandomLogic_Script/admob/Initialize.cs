using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class Initialize : MonoBehaviour
    {
        bool isinitialize = false;
        // Start is called before the first frame update
        void Start()
        {
            if(!isinitialize)
                MobileAds.Initialize((initStatus) => emptyfunc());

        }

        void emptyfunc()
        {
            isinitialize = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}