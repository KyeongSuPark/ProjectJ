using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FBManager : MonoBehaviour {

    public static FBManager Instance = null;



	void Awake () {
        if (FB.IsInitialized == false)
            FB.Init(SetInit, OnHideUnity);
        else
            FB.ActivateApp();

        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("FBManager has two instances");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SetInit()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Face book initialized");

            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.LogError("Failed to Initialize the Facebook SDK");
        }

    }

    public void FBLogin()
    {
        List<string> permissions = new List<string>() { "public_profile", "user_friends" };
        FB.LogInWithReadPermissions(permissions, OnAuthCallBack);
    }

    private void OnAuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void InitFriedList()
    {

    }

    private void OnHideUnity(bool _isGameShown)
    {
        if (_isGameShown)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

}
