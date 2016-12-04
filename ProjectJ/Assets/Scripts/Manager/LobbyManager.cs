using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {
    public static LobbyManager Instance = null;

    private AccountData m_AccountData;      ///< 계정 데이터

	// Use this for initialization
	void Start () {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Object manager has two instance");

        m_AccountData = new AccountData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClicked_StageButton(StageButtonData _stageData)
    {
        Debug.Log("버튼 눌렸다 " + _stageData.GetIndex());
    }
}
