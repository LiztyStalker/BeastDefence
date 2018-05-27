using UnityEngine;
using UnityEngine.UI;

public class UIShopButton : MonoBehaviour
{
    [SerializeField]
    Image m_icon;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_costText;

    [SerializeField]
    Image m_costIcon;

    Shop m_shop;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }

    public void setShop(Shop shop)
    {
        m_shop = shop;

        m_icon.sprite = shop.icon;
        m_nameText.text = shop.name;
        m_costText.text = shop.cost.ToString();
        m_costIcon.sprite = ShopManager.GetInstance.getShopCostIcon(shop.typeCostCategory);
    }

    void OnClicked()
    {
        Debug.Log("typeCost : " + m_shop.typeCostCategory);
        switch (m_shop.typeCostCategory)
        {
            case Shop.TYPE_SHOP_CATEGORY.Pay:
                UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("결제하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, paymentEvent);
                //결제 창으로 이동
                break;
            case Shop.TYPE_SHOP_CATEGORY.Fruit:
                if (Account.GetInstance.accData.isValue(m_shop.cost, m_shop.typeCostCategory))
                {
                    UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("구입하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, shopEvent);
                }
                else
                {
                    UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("열매가 부족합니다.", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
                }
                break;
        }
    }


    void shopEvent()
    {
        Account.GetInstance.accData.useValue(m_shop.cost, m_shop.typeCostCategory);
        Account.GetInstance.accData.addValue(m_shop.value, m_shop.typeShopCategory);
        UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("구입완료!", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK);
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.BUY);
    }

    void paymentEvent()
    {
        Prep.LogWarning("결제창 띄우기", "", GetType());
    }


}

