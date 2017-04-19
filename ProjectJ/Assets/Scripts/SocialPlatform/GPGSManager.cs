using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using GooglePlayGames.BasicApi;
using System.Text;

public class GPGSManager : MonoBehaviour {

    public static GPGSManager Instance = null;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Object manager has two instance");
    }

    void Start()
    {
        InitializeGPGS();
    }

/// <summary>
    /// 현재 로그인 중인지 체크
    /// </summary>
    public bool bLogin { get; set; }
 
    /// <summary>
    /// GPGS를 초기화 합니다.
    /// </summary>
    public void InitializeGPGS()
    {
        bLogin = false;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;
    }
 
    /// <summary>
    /// GPGS를 로그인 합니다.
    /// </summary>
    public void LoginGPGS()
    {
        // 로그인이 안되어 있으면
        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate(LoginCallBackGPGS);
    }
 
    /// <summary>
    /// GPGS Login Callback
    /// </summary>
    /// <param name="result"> 결과 </param>
    public void LoginCallBackGPGS(bool result)
    {
        bLogin = result;
    }
 
    /// <summary>
    /// GPGS를 로그아웃 합니다.
    /// </summary>
    public void LogoutGPGS()
    {
        // 로그인이 되어 있으면
        if (Social.localUser.authenticated)
        {
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            bLogin = false;
        }
    }
 
    /// <summary>
    /// GPGS에서 자신의 프로필 이미지를 가져옵니다.
    /// </summary>
    /// <returns> Texture 2D 이미지 </returns>
    public Texture2D GetImageGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.image;
        else
            return null;
    }
 
    /// <summary>
    /// GPGS 에서 사용자 이름을 가져옵니다.
    /// </summary>
    /// <returns> 이름 </returns>
    public string GetNameGPGS()
    {
        if (Social.localUser.authenticated)
            return Social.localUser.userName;
        else
            return null;
    }

    public void SaveGame(string _fileName)
    {
        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
        if (savedGame == null)
        {
            Log.PrintError(eLogFilter.GPGS, "SaveGame >> SavedGameClient is null");
            return;
        }

        savedGame.OpenWithAutomaticConflictResolution(_fileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    OpenGameToSaveCallBack);
    }

    private void OpenGameToSaveCallBack(SavedGameRequestStatus _status, ISavedGameMetadata _game)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            SaveGame(_game, Encoding.UTF8.GetBytes("Test Data!!!"), DateTime.Now.TimeOfDay);
        }
        else
        {
            PrintErrorMessage("Load Failed!!", _status);
        }
    }

    private void SaveGame(ISavedGameMetadata _game, byte[] _data, TimeSpan _totalPlaytime)
    {
        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder.WithUpdatedPlayedTime(_totalPlaytime).WithUpdatedDescription("Saved game at " + DateTime.Now);
        /*
        if (savedImage != null)
        {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below
            byte[] pngData = savedImage.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);

        }*/

        SavedGameMetadataUpdate updatedData = builder.Build();
        savedGame.CommitUpdate(_game, updatedData, _data, OnCommitUpdateCallBack);
    }

    private void OnCommitUpdateCallBack(SavedGameRequestStatus _status, ISavedGameMetadata _game)
    {
        if (_status == SavedGameRequestStatus.Success)
        {
            //데이터 저장이 완료되었습니다.
            Debug.Log("저장 완료");
        }
        else
        {
            //데이터 저장에 실패 했습니다.
            Debug.Log("저장 실패");
        }
    }

    public void LoadGame(string _fileName)
    {
        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
        savedGame.OpenWithAutomaticConflictResolution(_fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            OpenGameToLoadCallBack);
    }

    private void OpenGameToLoadCallBack(SavedGameRequestStatus _status, ISavedGameMetadata _game)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            LoadGame(_game);    
        }
    }

    private void LoadGame(ISavedGameMetadata _game)
    {
        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
        savedGame.ReadBinaryData(_game, delegate(SavedGameRequestStatus _status, byte[] _data){
            if(_status == SavedGameRequestStatus.Success)
            {
                string str = Encoding.Default.GetString(_data);
            }
            else
            {
                PrintErrorMessage("Load Failed!!", _status);
            }
        });
    }

    private void PrintErrorMessage(string _msg, SavedGameRequestStatus _errorCode)
    {
        string reason = "Unkown reason";
        switch(_errorCode)
        {
            case SavedGameRequestStatus.AuthenticationError: reason = "Authentication error";
                break;
            case SavedGameRequestStatus.BadInputError: reason = "Bad input error";
                break;
            case SavedGameRequestStatus.InternalError: reason = "Internal error";
                break;
            case SavedGameRequestStatus.TimeoutError: reason = "Timeout error";
                break;
        }

        Debug.Log(string.Format("{0}[{1}]", _msg, reason));
    }
}
