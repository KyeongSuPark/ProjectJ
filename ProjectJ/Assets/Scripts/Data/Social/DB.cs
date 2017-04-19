using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 유저 데이터
/// </summary>
public class UserData
{
    public ulong Id { get; set; }                    ///< 고유 아이디 
    public string Name { get; set; }                 ///< 이름 또는 계정명
    public uint Rank { get; set; }                   ///< 순위
    public uint Score { get; set; }                  ///< 전체 점수
    public eSocialPlatform eSnsType { get; set; }    ///< 소셜 플랫폼
    public ulong SnsId { get; set; }                 ///< 소셜 플랫폼 Id
    public Coin Coin { get; set; }                   ///< 코인
    public List<Stage> Stages { get; set; }
    public UserData()
    {
        Stages = new List<Stage>();
    }
}

/// <summary>
///  친구 관계
/// </summary>
public class FriendRelation
{
    public ulong FirendUserId;          ///< 친구 UserId
    public eFriendRelation Relation;    ///< 친구 요청 상태
}

/// <summary>
/// 코인
/// - 오락실에서 넣었던 그 코인
/// </summary>
public class Coin
{
    public uint Qnty;               ///< 현재 수량
    public Time RechargeRemainTime; ///< 다음 충전까지 남은 시간
}

/// <summary>
/// 스테이지 데이터
/// </summary>
public class Stage
{
    public uint Index { get; set; }         ///< Stage Index
    public uint TryCount { get; set; }      ///< 시도 횟수 (성공할때 까지)
    public bool Success { get; set; }       ///< 성공 했냐?
    public uint Score { get; set; }         ///< 스테이지 개별 Score
    public string CheerMsg { get; set; }    ///< 응원 메시지
}

/// <summary>
///  더미 데이터
/// </summary>
public class DummyFriendList
{
    private List<UserData> m_Friends = new List<UserData>();

    public DummyFriendList()
    {
        //. 더미 데이터 
        Stage stage1 = new Stage();
        stage1.Index = 1;
        stage1.TryCount = 15;
        stage1.CheerMsg = "경수야 힘을 내야지";

        Stage stage2 = new Stage();
        stage2.Index = 2;
        stage2.TryCount = 10;
        stage2.CheerMsg = "경수야 힘을 내야지2";

        Stage stage3 = new Stage();
        stage3.Index = 3;
        stage3.TryCount = 2;
        stage3.CheerMsg = "2번만에 깼다 ㅋㅋ";

        UserData friend1 = new UserData();
        friend1.Id = 1;
        friend1.Name = "우하나";
        friend1.Stages.Add(stage1);
        friend1.Stages.Add(stage2);
        friend1.Stages.Add(stage3);

        UserData friend2 = new UserData();
        friend2.Id = 2;
        friend2.Name = "이다솜";
        friend2.Stages.Add(stage1);
        friend2.Stages.Add(stage2);
        friend2.Stages.Add(stage3);

        m_Friends.Add(friend1);
        m_Friends.Add(friend2);
    }

    public void Parse(string _json)
    {
    }

    public List<UserData> GetList()
    {
        return m_Friends;
    }
}

