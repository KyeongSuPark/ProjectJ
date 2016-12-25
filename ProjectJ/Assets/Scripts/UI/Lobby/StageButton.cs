using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageButton : MonoBehaviour {

    public Text m_IndexLabel;       ///< 인덱스 라벨
    public Image m_Image;

    private Stage m_Data;       ///< 스테이지 데이터

	// Use this for initialization
	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClicked_StageButton);
	}

    public void SetData(Stage _data)
    {
        m_Data = _data;
        Refresh();
    }

    public Stage GetData()
    {
        return m_Data;
    }

    /// <summary>
    ///  Data(Model)을 토대로 View(UI)갱신
    /// </summary>
    private void Refresh()
    {
        m_IndexLabel.text = m_Data.Id.ToString();
        m_Image.color = m_Data.Success ? Color.white : Color.cyan;
    }

    public void OnClicked_StageButton()
    {
        LobbyManager.Instance.OnClicked_StageButton(m_Data);
    }
}
