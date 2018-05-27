using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIShop : UIPanel
{
    [SerializeField]
    Toggle[] m_shopToggle;

    [SerializeField]
    UIShopButton m_uiShopBtn;

    [SerializeField]
    Transform m_shopPanel;

    Shop.TYPE_SHOP_CATEGORY m_typeShopCategory;


    List<UIShopButton> m_uiShopBtnList = new List<UIShopButton>();

    void Awake()
    {

        for (int i = 0; i < m_shopToggle.Length; i++)
        {
            m_shopToggle[i].onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
        }

        if(m_shopToggle.Length > 0)
            m_shopToggle[0].isOn = true;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);

        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.Shop));

        viewShop(m_typeShopCategory);

    }

    public void viewShop(Shop.TYPE_SHOP_CATEGORY typeShopCategory)
    {

        clear();

        m_typeShopCategory = typeShopCategory;
        Shop[] shopArray = ShopManager.GetInstance.getShop(typeShopCategory);

        for (int i = 0; i < shopArray.Length; i++)
        {
            UIShopButton uiShopBtn = Instantiate(m_uiShopBtn);
            uiShopBtn.setShop(shopArray[i]);
            uiShopBtn.transform.SetParent(m_shopPanel);
            uiShopBtn.transform.localScale = Vector2.one;
            m_uiShopBtnList.Add(uiShopBtn);
        }

        m_shopToggle[(int)m_typeShopCategory].isOn = true;

    }

    void clear()
    {
        for (int i = m_uiShopBtnList.Count - 1; i >= 0; i--)
        {
            Destroy(m_uiShopBtnList[i].gameObject);
        }

        m_uiShopBtnList.Clear();
    }


    void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < m_shopToggle.Length; i++)
            {
                if (m_shopToggle[i].isOn)
                {
                    m_typeShopCategory = (Shop.TYPE_SHOP_CATEGORY)i;
                    break;
                }
            }
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
            viewShop(m_typeShopCategory);
        }
    }

    protected override void OnDisable()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        clear();
        base.OnDisable();
    }



}

