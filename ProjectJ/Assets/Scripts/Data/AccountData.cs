using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 계정 데이터
/// </summary>
public class AccountData {
    private ulong m_AccountId;           ///< 계정 아이디
    private eSocialPlatform SocialType; ///< Sns Type
    private List<Stage> m_Stages;       ///< 내 스테이지 정보
    
    public AccountData()
    {
        m_Stages = new List<Stage>();
    }

    public List<Stage> GetStageList()
    {
        return m_Stages;
    }

    public ulong GetAccountId()
    {
        return m_AccountId;
    }

    public void CreateDummyStageData()
    {
        Stage stage1 = new Stage();
        stage1.Index = 1;
        stage1.Success = true;
        stage1.TryCount = 3;
        stage1.CheerMsg = "좀 잘해봐라";

        Stage stage2 = new Stage();
        stage2.Index = 2;
        stage2.Success = true;
        stage2.TryCount = 5;
        stage2.CheerMsg = "뻐큐머겅";

        Stage stage3 = new Stage();
        stage3.Index = 3;
        stage3.Success = false;
        stage3.TryCount = 50;
        stage3.CheerMsg = "두번 머겅";

        m_Stages.Add(stage1);
        m_Stages.Add(stage2);
        m_Stages.Add(stage3);
    }

}
