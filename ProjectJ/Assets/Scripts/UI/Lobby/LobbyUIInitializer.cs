using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DB;

public class LobbyUIInitializer : MonoBehaviour {

    public RectTransform m_StageContent;     ///< 스테이지 리스트 컨텐츠
    public GameObject m_StageButtonPrefab;   ///< 스테이지 버튼 프리팹

	// Use this for initialization
	void Start () {
       
	}

    /// <summary>
    ///   스테이지 UI 초기화
    /// </summary>
    void InitStageList()
    {
        for (int i = 0; i < 10; ++i)
        {
            StageButtonData stageData = new StageButtonData();
            stageData.SetIndex(i);
            if (i < 6)
                stageData.SetSuccess(true);
            else
                stageData.SetSuccess(false);

            GameObject newObj = GameObject.Instantiate(m_StageButtonPrefab);
            newObj.transform.SetParent(m_StageContent);

            StageButton stageBtn = newObj.GetComponent<StageButton>();
            stageBtn.SetData(stageData);
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

        }
    }
}
