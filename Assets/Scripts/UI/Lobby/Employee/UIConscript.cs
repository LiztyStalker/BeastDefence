using UnityEngine;
using UnityEngine.UI;

public class UIConscript : UIPanel
{

    [SerializeField]
    UIConscriptButton m_uiConscriptButton;

    [SerializeField]
    UIConscriptView m_uiConscriptView;

    [SerializeField]
    Transform m_conscriptButtonPanel;

    UIConscriptButton[] m_conscriptButtons;

    void Awake()
    {
        Sprite[] conscriptSprite = Resources.LoadAll<Sprite>(Prep.conscriptImagePath);
        m_conscriptButtons = new UIConscriptButton[conscriptSprite.Length];

        for (int i = 0; i < m_conscriptButtons.Length; i++)
        {
            m_conscriptButtons[i] = Instantiate(m_uiConscriptButton) as UIConscriptButton;
            m_conscriptButtons[i].transform.SetParent(m_conscriptButtonPanel);
            m_conscriptButtons[i].transform.localScale = Vector2.one;
            m_conscriptButtons[i].setConscript(conscriptSprite[i], i);

            if (i == 0)
                m_conscriptButtons[i].GetComponent<Button>().onClick.AddListener(() => OnOneConscriptClicked());
            if (i == 1)
                m_conscriptButtons[i].GetComponent<Button>().onClick.AddListener(() => OnFiveConscriptClicked());
            if (i == 2)
                m_conscriptButtons[i].GetComponent<Button>().onClick.AddListener(() => OnTenConscriptClicked());
            if (i == 3)
                m_conscriptButtons[i].GetComponent<Button>().onClick.AddListener(() => OnHeroConscriptClicked());

        }
    }

    public void OnOneConscriptClicked()
    {
        if (Account.GetInstance.accData.isValue(Prep.conscriptPay[0], TYPE_ACCOUNT_CATEGORY.Fruit))
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("징집하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, onOneConscriptEvent);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("열매가 부족합니다", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }
    }

    void onOneConscriptEvent()
    {
        Account.GetInstance.accData.useValue(Prep.conscriptPay[0], TYPE_ACCOUNT_CATEGORY.Fruit);
        m_uiConscriptView.setConscript(1, Unit.TYPE_UNIT.Soldier);
    }

    public void OnFiveConscriptClicked()
    {
        if (Account.GetInstance.accData.isValue(Prep.conscriptPay[1], TYPE_ACCOUNT_CATEGORY.Fruit))
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("징집하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, onFiveConscriptEvent);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("열매가 부족합니다", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }
    }

    void onFiveConscriptEvent()
    {
        Account.GetInstance.accData.useValue(Prep.conscriptPay[1], TYPE_ACCOUNT_CATEGORY.Fruit);
        m_uiConscriptView.setConscript(5, Unit.TYPE_UNIT.Soldier);
    }


    public void OnTenConscriptClicked()
    {
        if (Account.GetInstance.accData.isValue(Prep.conscriptPay[2], TYPE_ACCOUNT_CATEGORY.Fruit))
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("징집하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, onTenConscriptEvent);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("열매가 부족합니다", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }
    }

    void onTenConscriptEvent()
    {
        Account.GetInstance.accData.useValue(Prep.conscriptPay[2], TYPE_ACCOUNT_CATEGORY.Fruit);
        m_uiConscriptView.setConscript(10, Unit.TYPE_UNIT.Soldier);
    }

    public void OnHeroConscriptClicked()
    {

        if (Account.GetInstance.accData.isValue(Prep.conscriptPay[3], TYPE_ACCOUNT_CATEGORY.Fruit))
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("징집하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, onHeroConscriptEvent);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("열매가 부족합니다", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }
    }
    void onHeroConscriptEvent()
    {
        Account.GetInstance.accData.useValue(Prep.conscriptPay[3], TYPE_ACCOUNT_CATEGORY.Fruit);
        m_uiConscriptView.setConscript(1, Unit.TYPE_UNIT.Hero);
    }

}

