using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 프로그램 실행 되는 동안 살아있는 객체
/// 최소한의 필요한 정보만 담을 것
/// </summary>
public class Oracle : MonoBehaviour {

    private static Oracle m_Instance = null;

    private AccountData m_AccountData;                  ///< 계정 데이터
    private Dictionary<ulong, UserData> m_UserDatas;    ///< 친구들 데이터       (key:userId)
    private UserData m_PlayerData;                      ///< 플레이어 데이터

    private Dictionary<uint, ulong> m_RankCache;         ///< 순위 데이터         (key:순위, value:userId)

    ISocialPlatform m_SocialPlatform;                   ///< 소셜 플랫폼

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}

    public void Init()
    {
        m_RankCache = new Dictionary<uint, ulong>();
        m_UserDatas = new Dictionary<ulong, UserData>();
        m_AccountData = new AccountData();
        m_PlayerData = new UserData();
        m_AccountData.CreateDummyStageData();
    }

    public static Oracle Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject("Oracle").AddComponent<Oracle>();
                m_Instance.Init();
            }

            return m_Instance;
        }
    }

    public void SetSocialPlatform(ISocialPlatform _socialPlatform) { m_SocialPlatform = _socialPlatform; }
    public ISocialPlatform GetSocialPlatform() { return m_SocialPlatform; }

    public UserData GetPlayerData() { return m_PlayerData; }
    public AccountData GetAccountData() { return m_AccountData; }

    public UserData FindUserData(ulong _id)
    {
        if (m_UserDatas.ContainsKey(_id))
            return m_UserDatas[_id];
        return null;
    }

    public void SetUserDatas(List<UserData> _users)
    {
        m_UserDatas.Clear();
        foreach(var user in _users)
        {
            m_UserDatas.Add(user.Id, user);
        }
    }

    /// <summary>
    ///   저장된 Score를 가지고 순위를 매긴다
    /// </summary>
    public void RankUser()
    {
        //. 스코어 높은 순으로 정렬
        List<UserData> rankedUsers = new List<UserData>(m_UserDatas.Values);
        rankedUsers.Add(m_PlayerData);
        rankedUsers.Sort(delegate(UserData _a, UserData _b)
        {
            if (_a.Score > _b.Score) return -1;
            else if (_b.Score > _a.Score) return 1;
            return 0;
        });

        //. 순위를 매겨 준다.
        uint rank = 1;
        foreach (var rankData in rankedUsers)
        {
            m_RankCache.Add(rank, rankData.Id);

            //. 플레이어 순위 계산
            if(rankData.Id == m_PlayerData.Id)
            {
                m_PlayerData.Rank = rank;
                rank++;
                continue;
            }

            //. 다른 유저 순위 계산
            if(m_UserDatas.ContainsKey(rankData.Id))
            {
                m_UserDatas[rankData.Id].Rank = rank;
                rank++;
            }
        }
    }

    /// <summary>
    ///   순위에 맞는 유저의 데이터 반환
    /// </summary>
    /// <param name="_rank">순위</param>
    public UserData FindRankUser(uint _rank)
    {
        if(m_RankCache.ContainsKey(_rank))
            return FindUserData(m_RankCache[_rank]);
        return null;
    }
}
