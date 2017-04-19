using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour {

    ISocialPlatform m_SocialPlatform;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickLoginButton_Facebook()
    {
        m_SocialPlatform = new FacebookPlatform();
        m_SocialPlatform.Init(OnInit);
        Oracle.Instance.SetSocialPlatform(m_SocialPlatform);
    }

    public void OnClickLoginButton_KaKao()
    {

    }

    public void OnClickLoginButton_Naver()
    {

    }

    public void OnInit(bool _bSuccess)
    {
        if (_bSuccess)
            m_SocialPlatform.Login(OnLoginResult);
    }

    public void OnLoginResult(bool _bLogin)
    {
        if(_bLogin)
        {
            m_SocialPlatform.QueryFriends(OnQueryFriensResult);
        }
    }

    public void OnQueryFriensResult(List<UserData> _users)
    {
        Oracle.Instance.SetUserDatas(_users);
        List<ulong> userIds = new List<ulong>();
        foreach (UserData _friend in _users)
            userIds.Add(_friend.Id);

        //. 스테이지 정보 얻어 와야 하고

        //. 점수 정보 채워야 하고
        m_SocialPlatform.QueryScores(OnQueryScoreResult);
    }

    public void OnQueryScoreResult(Dictionary<ulong, int> _scores)
    {
        //. 쿼리 결과가 없으면
        if (_scores.Count == 0)
        {
            //. 로비로 고!
            string sceneName = string.Format(R.String.SCENE_LOBBY);
            SceneManager.LoadScene(sceneName);
        }

        UserData playerData = Oracle.Instance.GetPlayerData();

        //. 스코어 데이터 가져와서 셋팅
        foreach(var score in _scores)
        {
            //. 플레이어가 있으면 셋팅
            if (score.Key == playerData.Id)
            {
                playerData.Score = (uint)score.Value;
                continue;
            }

            //. 다른 유저 스코어 설정
            UserData user = Oracle.Instance.FindUserData(score.Key);
            if (user == null)
                continue;

            user.Score = (uint)score.Value;
        }

        //. 순위를 매기자
        Oracle.Instance.RankUser();

        //. 완료 되면 다음 단계 진행
        SceneManager.LoadScene(R.String.SCENE_LOBBY);
    }
}
