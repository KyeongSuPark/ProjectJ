using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;
using System;
public class FacebookPlatform : ISocialPlatform {
    private InitResult m_InitResult = null;
    private LoginResult m_LoginCallback = null;
    private QueryFriendsResult m_QueryFriendsCallback = null;
    private QueryScoresResult m_QueryScoreResult = null;
    private Dictionary<ulong, QueryPictureResult> m_QueryPictureResults = new Dictionary<ulong, QueryPictureResult>();

    public FacebookPlatform()
    {
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

            if (m_InitResult != null)
                m_InitResult(true);
        }
        else
        {
            if (m_InitResult != null)
                m_InitResult(false);

            Debug.LogError("Failed to Initialize the Facebook SDK");
        }

    }

    private void OnAuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var bToken = result.AccessToken;

            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            //. 결과 callback
            if (m_LoginCallback != null)
                m_LoginCallback(true);
        }
        else
        {
            Debug.Log("User cancelled login");

            //. 결과 callback
            if (m_LoginCallback != null)
                m_LoginCallback(false);
        }
    }

    private void OnFriendsCallback(IGraphResult _result)
    {
        List<UserData> users = new List<UserData>();

        //. json parsing
        var datas = _result.ResultDictionary["data"] as List<object>;
        foreach (var data in datas)
        {
            IDictionary<string, object> friendData = data as IDictionary<string, object>;
            if (friendData.ContainsKey("name") == false || friendData.ContainsKey("id") == false)
                continue;

            ulong id;
            if (ulong.TryParse(friendData["id"] as string, out id) == false)
                continue;

            UserData user = new UserData();
            user.Id = id;
            user.Name = friendData["name"] as string;
            users.Add(user);
        }

        if (m_QueryFriendsCallback != null)
            m_QueryFriendsCallback(users);
    }

    private void OnScoresCallback(IGraphResult _result)
    {
        //. json parsing
        Dictionary<ulong, int> friendScores = new Dictionary<ulong, int>(); //. friendId, score

        var datas = _result.ResultDictionary["data"] as List<object>;
        foreach (var data in datas)
        {
            IDictionary<string, object> scoreData = data as IDictionary<string, object>;
            if (scoreData.ContainsKey("user") == false)
                continue;

            IDictionary<string, object> userData = scoreData["user"] as IDictionary<string, object>;
            if(userData.ContainsKey("name") == false || userData.ContainsKey("id") == false)
                continue;

            int score = 0;
            string strScore = scoreData["score"].ToString();
            if (int.TryParse(strScore, out score) == false)
                continue;

            ulong id = 0;
            if (ulong.TryParse(userData["id"] as string, out id) == false)
                continue;

            friendScores.Add(id, score);
        }

        if (m_QueryScoreResult != null)
            m_QueryScoreResult(friendScores);
    }
    
    private void OnHideUnity(bool _isGameShown)
    {
        if (_isGameShown)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    /// <summary>
    ///  Sns 플랫폼 초기화
    /// </summary>
    public void Init(InitResult _callBack)
    {
        if (FB.IsInitialized == false)
        {
            m_InitResult = _callBack;
            FB.Init(SetInit, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    /// <summary>
    ///   로그인 
    /// </summary>
    public void Login(LoginResult _callBack)
    {
        m_LoginCallback = _callBack;
        List<string> permissions = new List<string>() { "public_profile", "user_friends" };
        FB.LogInWithReadPermissions(permissions, OnAuthCallBack);

        //. Todo 권한을 얻어야 한다.
        //List<string> permissions = new List<string>() { "publish_actions" };
        //FB.LogInWithPublishPermissions(permissions, OnAuthCallBack);
    }

    /// <summary>
    ///   친구 리스트 Query
    /// </summary>
    public void QueryFriends(QueryFriendsResult _callBack)
    {
        m_QueryFriendsCallback = _callBack;
        FB.API("me/friends", HttpMethod.GET, OnFriendsCallback);
    }

    /// <summary>
    ///   점수 리스트 Query
    /// </summary>
    public void QueryScores(QueryScoresResult _callBack)
    {
        m_QueryScoreResult = _callBack;
        FB.API("app/scores", HttpMethod.GET, OnScoresCallback);
    }

    /// <summary>
    ///   프로필 사진 Query
    /// </summary>
    public void QueryPicture(ulong _id, QueryPictureResult _callback)
    {
        m_QueryPictureResults.Add(_id, _callback);
        FB.API(_id.ToString() + "/picture?type=square&height=64&width=64", HttpMethod.GET, delegate(IGraphResult _result){
            if (_result.Texture == null)
                return;

            if (m_QueryPictureResults.ContainsKey(_id) == false)
                return;

            m_QueryPictureResults[_id](_result.Texture);
        });
    }
}
