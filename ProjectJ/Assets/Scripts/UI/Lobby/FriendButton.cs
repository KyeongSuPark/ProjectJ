﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FriendButton : MonoBehaviour {
    public Text m_NameLabel;    ///< 인덱스 라벨
    public Image m_Image;       ///< 프로필 이미지
    public Text m_RankLabel;    ///< 순위 라벨
    public Text m_PointLabel;   ///< 점수 라벨

    private UserData m_Data;      ///< 친구 데이터

    // Use this for initialization
    void Start()
    {
        Button btn = GetComponent<Button>();
        //btn.onClick.AddListener(OnClicked_StageButton);
    }

    public void SetData(UserData _data)
    {
        m_Data = _data;
        Refresh();
    }

    public UserData GetData()
    {
        return m_Data;
    }

    /// <summary>
    ///  Data(Model)을 토대로 View(UI)갱신
    /// </summary>
    private void Refresh()
    {
        m_NameLabel.text = m_Data.Name;
        m_PointLabel.text = m_Data.Score.ToString();
        m_RankLabel.text = m_Data.Rank.ToString();

        if(m_Image.sprite == null)
        {
            ISocialPlatform sf = Oracle.Instance.GetSocialPlatform();
            if (sf != null)
            {
                sf.QueryPicture(m_Data.Id, delegate(Texture2D _pic)
                {
                    if (_pic)
                    {
                        m_Image.sprite = Sprite.Create(_pic, new Rect(0, 0, 64, 64), new Vector2(0, 0));
                    }
                });
            }
        }
    }

    //public void OnClicked_StageButton()
    //{
    //    LobbyManager.Instance.OnClicked_StageButton(m_Data);
    //}
}
