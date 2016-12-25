using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 친구 데이터
/// </summary>
public class Friend
{
    public uint Id { get; set; }                     ///< 고유 아이디 (Sns Type마다 다를수도 있다)
    public string Name { get; set; }                 ///< 이름 또는 계정명
    public int Rank { get; set; }                    ///< 순위
    public int Point { get; set; }                   ///< 전체 점수

    public List<Stage> Stages { get; set; }
    public Friend()
    {
        Stages = new List<Stage>();
    }
}

/// <summary>
/// 스테이지 데이터
/// </summary>
public class Stage
{
    public uint Id { get; set; }            ///< Stage Id
    public int TryCount { get; set; }       ///< 시도 횟수 (성공할때 까지)
    public bool Success { get; set; }       ///< 성공 했냐?
    public string CheerMsg { get; set; }    ///< 응원 메시지
}

/// <summary>
///  친구 리스트 인터페이스
/// </summary>
public interface IFriendList
{
    /// <summary>
    ///  sns 로 데이터 읽어 와서 친구 리스트 구성
    /// </summary>
    void Parse(string _json);

    /// <summary>
    ///  친구 리스트 반환
    /// </summary>
    List<Friend> GetList();
}

/// <summary>
///  더미 데이터
/// </summary>
public class DummyFriedList : IFriendList
{
    private List<Friend> m_Friends = new List<Friend>();

    public DummyFriedList()
    {
        //. 더미 데이터 
        Stage stage1 = new Stage();
        stage1.Id = 1;
        stage1.TryCount = 15;
        stage1.CheerMsg = "경수야 힘을 내야지";

        Stage stage2 = new Stage();
        stage2.Id = 2;
        stage2.TryCount = 10;
        stage2.CheerMsg = "경수야 힘을 내야지2";

        Stage stage3 = new Stage();
        stage3.Id = 3;
        stage3.TryCount = 2;
        stage3.CheerMsg = "2번만에 깼다 ㅋㅋ";

        Friend friend1 = new Friend();
        friend1.Id = 1;
        friend1.Name = "우하나";
        friend1.Stages.Add(stage1);
        friend1.Stages.Add(stage2);
        friend1.Stages.Add(stage3);

        Friend friend2 = new Friend();
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

    public List<Friend> GetList()
    {
        return m_Friends;
    }
}

