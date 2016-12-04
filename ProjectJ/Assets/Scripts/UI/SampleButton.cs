using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour {

    public Button m_Button;
    public Text m_NameLabel;
    public Text m_PriceLabel;
    public Image m_IconImage;

    private TestItem m_Item;
    private ShopScrollList m_ShopScrollList;

	// Use this for initialization
	void Start () {
        m_Button.onClick.AddListener(HandleClick);
	}

    public void Setup(TestItem _item, ShopScrollList _list)
    {
        m_Item = _item;
        m_NameLabel.text = _item.m_ItemName;
        m_PriceLabel.text = _item.m_Price.ToString();
        m_IconImage.sprite = _item.m_Icon;

        m_ShopScrollList = _list;
    }

    public void HandleClick()
    {
        m_ShopScrollList.TryTransferToOtherShop(m_Item);
    }
}
