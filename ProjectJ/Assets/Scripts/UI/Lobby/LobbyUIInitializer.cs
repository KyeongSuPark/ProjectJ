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
        InitFriedList();
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
    void InitFriedList()
    {
        DummyFriedList dummy = new DummyFriedList();

        foreach (Friend _friend in dummy.GetList())
        {
            GameObject newObj = GameObject.Instantiate(m_FriendUIPrefab);
            newObj.transform.SetParent(m_FriendScrollContent);
            newObj.transform.localScale = new Vector3(1.0f, 1.0f);

            FriendButton friendButton = newObj.GetComponent<FriendButton>();
            friendButton.SetData(_friend);
        }
    }
}
