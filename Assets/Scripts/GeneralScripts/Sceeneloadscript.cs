using System.Collections;
using System.Collections.Generic;
//using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceeneloadscript : MonoBehaviour
{
    void Start()
    {
        //if (!FB.IsInitialized)
          //  FB.Init(OnInitComplete);
        //else
          //  FB.ActivateApp();

        //GameAnalyticsSDK.GameAnalytics.Initialize();
        SceneManager.LoadScene("MainScene");

    }

    //void OnInitComplete()
    //{
      //  if (FB.IsInitialized)
        //{
          //  FB.ActivateApp();
            //FB.LogAppEvent(AppEventName.ActivatedApp);
        //}
        //else
          //  Debug.Log("OnInitComplete Failed");
    //}
}

