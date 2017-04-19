using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyUIInitializer : MonoBehaviour {

    public RectTransform m_StageScrollContent;     ///< 스테이지 리스트
    public RectTransform m_FriendScrollContent;    ///< 친구 리스트 
    public GameObject m_StageButtonPrefab;         ///< 스테이지 버튼 프리팹
    public GameObject m_FriendUIPrefab;            ///< 친구 UI 프리팹

	// Use this for initialization
	void Start () {
        InitStageList();
        InitFriendList();
	}

    /// <summary>
    ///   스테이지 UI 초기화
    /// </summary>
    void InitStageList()
    {
        AccountData data = Oracle.Instance.GetAccountData();

        List<Stage> myStages = data.GetStageList();
        foreach (var _stage in myStages)
        {
            GameObject newObj = GameObject.Instantiate(m_StageButtonPrefab);
            newObj.transform.SetParent(m_StageScrollContent);
            newObj.transform.localScale = new Vector3(1.0f, 1.0f);

            StageButton stageBtn = newObj.GetComponent<StageButton>();
            stageBtn.SetData(_stage);
        }
    }

    /// <summary>
    ///  친구 UI 초기화
    /// </summary>
    void InitFriendList()
    {
        int toRank = 5; //. 기본 3등 까지
        UserData playerData = Oracle.Instance.GetPlayerData();

        //. 500위 밖에 있거나, 15위 안에 있는 경우 max rank list 만큼 보여준다.
        if(playerData.Rank >= R.Const.MAX_RANK_COUNT || playerData.Rank <= toRank)
        {
            toRank = R.Const.MAX_RANK_LIST;
        }

        List<UserData> rankedUsers = new List<UserData>();
        //. 1등부터 지정된 순위까지 
        for (uint i = 1; i <= toRank; ++i)
        {
            UserData user = Oracle.Instance.FindRankUser(i);
            if (user == null)
                continue;

            rankedUsers.Add(user);
        }
        
        //. 유저 앞/뒤로 2등 씩
        if(toRank != R.Const.MAX_RANK_LIST)
        {
            uint start = playerData.Rank - 2;
            uint end = playerData.Rank + 2;

            //. 유효 범위 체크
            start = (uint)Mathf.Clamp(start, 1, R.Const.MAX_RANK_COUNT);
            end = (uint)Mathf.Clamp(end, 1, R.Const.MAX_RANK_COUNT);
            if(start < end)
            {
                for(uint i = start; i <= end; ++i)
                {
                    UserData user = Oracle.Instance.FindRankUser(i);
                    if (user == null || rankedUsers.Contains(user))
                        continue;

                    rankedUsers.Add(user);
                }
            }
        }

        //. ui 구성
        foreach (UserData _rankedUser in rankedUsers)
        {
            GameObject newObj = GameObject.Instantiate(m_FriendUIPrefab);
            newObj.transform.SetParent(m_FriendScrollContent);
            newObj.transform.localScale = new Vector3(1.0f, 1.0f);

            FriendButton friendButton = newObj.GetComponent<FriendButton>();
            friendButton.SetData(_rankedUser);
        }
    }
}
