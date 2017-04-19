using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void InitResult(bool _bSuccess);
public delegate void LoginResult(bool _bLogin);
public delegate void QueryFriendsResult(List<UserData> _friendList);
public delegate void QueryScoresResult(Dictionary<ulong, int> _scores);
public delegate void QueryPictureResult(Texture2D _picture);

/// <summary>
///   소셜 플랫폼 인터페이스
/// </summary>
public interface ISocialPlatform {

    /// <summary>
    ///  Sns 플랫폼 초기화
    /// </summary>
    void Init(InitResult _callBack);

    /// <summary>
    ///   로그인 
    /// </summary>
    void Login(LoginResult _callBack);

    /// <summary>
    ///   친구 리스트 Query
    /// </summary>
    void QueryFriends(QueryFriendsResult _callBack);

    /// <summary>
    ///   점수 리스트 Query
    /// </summary>  
    void QueryScores(QueryScoresResult _callBack);    

    /// <summary>
    ///   프로필 사진 Query
    /// </summary>
    void QueryPicture(ulong _id, QueryPictureResult _callback);
}