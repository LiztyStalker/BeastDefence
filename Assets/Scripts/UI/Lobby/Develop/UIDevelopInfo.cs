using UnityEngine;
using UnityEngine.UI;

public class UIDevelopInfo : UIPanel
{

    public delegate void DevelopDataUpdateDelegate();// uiDevelopBtn);

    public event DevelopDataUpdateDelegate developDataUpdateEvent;



    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_contents;

    [SerializeField]
    Text m_nowValueText;

    [SerializeField]
    Text m_nextValueText;

    [SerializeField]
    Image m_icon;

    [SerializeField]
    Text m_costText;

    //이름
    //설명
    //증가량
    //비용
    //버튼


    //연구 비용
    UIDevelopButton m_developButton;
    int m_level;

    public void setDevelop(UIDevelopButton devBtn, int level)
    {
        openPanel(null);
        m_level = level;
        m_developButton = devBtn;
        m_nameText.text = m_developButton.develop.name;
        m_contents.text = string.Format(m_developButton.develop.contents, level + 1);

        if (level == 0)
            m_nowValueText.text = "0";
        else
            m_nowValueText.text = m_developButton.develop.getValue(level).ToString();

        m_nextValueText.text = m_developButton.develop.getValue(level + 1).ToString();
        m_icon.sprite = m_developButton.develop.icon;
        m_costText.text = m_developButton.develop.getCost(level).ToString();
    }

    public void OnDevelopClicked()
    {


        if (Account.GetInstance.accData.isValue(m_developButton.develop.getCost(m_level), TYPE_ACCOUNT_CATEGORY.Gold))
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("개발하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, developEvent);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("골드가 부족합니다.", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }

    }

    void developEvent()
    {
        //완료

        Account.GetInstance.accData.useValue(m_developButton.develop.getCost(m_level), TYPE_ACCOUNT_CATEGORY.Gold);
        Account.GetInstance.accDevelop.addDevelop(m_developButton.develop.key);
        developDataUpdateEvent();
//        developDataUpdateEvent(m_developButton.develop.typeDevelopGroup);
        //developDataUpdateEvent(m_developButton);
        closePanel();
    }


}

