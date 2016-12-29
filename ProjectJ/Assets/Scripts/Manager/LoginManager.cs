using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickLoginButton_Facebook()
    {
        FBManager.Instance.FBLogin();   
    }

    public void OnClickLoginButton_KaKao()
    {

    }

    public void OnClickLoginButton_Naver()
    {

    }
}
