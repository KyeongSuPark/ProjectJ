using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class TestItem
{
    public string m_ItemName;
    public Sprite m_Icon;
    public float m_Price = 1.0f;
}

public class ShopScrollList : MonoBehaviour {
    public List<TestItem> m_ItemList;
    public Transform m_ContentPanel;
    public ShopScrollList m_OtherShop;
    public Text m_GoldDisplay;
    public SimpleObjectPool m_ButtonObjectPool;
    public float m_Gold = 20.0f;

	// Use this for initialization
	void Start () {
        RefreshDisplay();
	}
    
    public void RefreshDisplay()
    {
        m_GoldDisplay.text = "Gold : " + m_Gold.ToString();
        RemoveButtons();
        AddButtons();
    }
	
    private void AddButtons()
    {
        foreach (TestItem item in m_ItemList)
        {
            GameObject newButton = m_ButtonObjectPool.GetObject();
            newButton.transform.SetParent(m_ContentPanel);
            newButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            SampleButton sampleButton = newButton.GetComponent<SampleButton>();
            sampleButton.Setup(item, this);
        }
    }

    private void RemoveButtons()
    {
        while(m_ContentPanel.childCount > 0)
        {
            GameObject toRemoveButton = transform.GetChild(0).gameObject;
            m_ButtonObjectPool.ReturnObject(toRemoveButton);
        }
    }

    public void TryTransferToOtherShop(TestItem _item)
    {
        if(m_OtherShop.m_Gold > _item.m_Price)
        {
            m_Gold += _item.m_Price;
            m_OtherShop.m_Gold -= _item.m_Price;
            AddItem(_item, m_OtherShop);
            RemoveItem(_item, this);

            RefreshDisplay();
            m_OtherShop.RefreshDisplay();
        }
    }

    private void AddItem(TestItem _item, ShopScrollList _shop)
    {
        _shop.m_ItemList.Add(_item);
    }

    private void RemoveItem(TestItem _item, ShopScrollList _shop)
    {
        for(int i = _shop.m_ItemList.Count - 1; i >= 0; --i)
        {
            if(_shop.m_ItemList[i] == _item)
            {
                _shop.m_ItemList.RemoveAt(i);
            }
        }
    }
}
