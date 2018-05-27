using UnityEngine;
using UnityEngine.UI;

//메시지 UI
//메시지 활용

public enum TYPE_MSG_BTN {OK, OK_CANCEL}
public enum TYPE_MSG_PANEL { INFO, WARNING, ERROR }

public class UIMsg : UIPanel
{
    public delegate void OnOkDelegate();
    public delegate void OnCancelDelegate();

    OnOkDelegate m_okDel;
    OnCancelDelegate m_cancelDel;
    
    [SerializeField]
    Text m_titleText;

    [SerializeField]
    Text m_contentsText;

    [SerializeField]
    Transform[] m_msgBtns;

//    [SerializeField]
//    Transform m_wantedBtnPanel;

//    [SerializeField]
//    UIMsgFriendWanted m_msgFriendPanel;


    public void setMsg(string msg, TYPE_MSG_PANEL typeMsgPanel, TYPE_MSG_BTN typeBtn)
    {

        openPanel(null);
        
//        m_msgFriendPanel.gameObject.SetActive(false);
//        m_wantedBtnPanel.gameObject.SetActive(false);
        m_contentsText.text = msg;
        setTypeButton(typeBtn);
        setMsgText(typeMsgPanel);
            
    }

    public void setMsg(string msg, TYPE_MSG_PANEL typeMsgPanel, TYPE_MSG_BTN typeBtn, OnOkDelegate okDel)
    {
        m_okDel = okDel;
        setMsg(msg, typeMsgPanel, typeBtn);
    }

    public void setMsg(string msg, TYPE_MSG_PANEL typeMsgPanel, TYPE_MSG_BTN typeBtn, OnOkDelegate okDel, OnCancelDelegate cancelDel)
    {
        m_cancelDel = cancelDel;
        setMsg(msg, typeMsgPanel, typeBtn, okDel);
    }

    //public void setMsg(Friend friend, TYPE_MSG_PANEL typeMsgPanel, TYPE_MSG_BTN typeBtn, OnOkDelegate okDel)
    //{
    //    openPanel(null);
    //    m_msgFriendPanel.gameObject.SetActive(true);
    //    m_wantedBtnPanel.gameObject.SetActive(true);
    //    m_msgFriendPanel.setFriendView(friend);
    //    m_okDel = okDel;
    //}

    //public void setMsg(Friend friend, TYPE_MSG_PANEL typeMsgPanel, TYPE_MSG_BTN typeBtn, OnOkDelegate okDel, OnCancelDelegate cancelDel)
    //{
    //    m_cancelDel = cancelDel;
    //    setMsg(friend, typeMsgPanel, typeBtn, okDel);
    //}


    void setMsgText(TYPE_MSG_PANEL typeMsgPanel)
    {
        switch (typeMsgPanel)
        {
            case TYPE_MSG_PANEL.INFO:
                m_titleText.text = "정보";
                UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.INFOR);
                break;
            case TYPE_MSG_PANEL.WARNING:
                m_titleText.text = "경고!";
                UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.WARNING);
                break;
            case TYPE_MSG_PANEL.ERROR:
                m_titleText.text = "불가!";
                UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.ERROR);
                break;
            default:
                UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
                break;

        }
    }

    void setTypeButton(TYPE_MSG_BTN typeMsgButton)
    {
        resetBtn();
        if(m_msgBtns.Length > (int)typeMsgButton)
            m_msgBtns[(int)typeMsgButton].gameObject.SetActive(true);
    }

    void resetBtn()
    {
        for (int i = 0; i < m_msgBtns.Length; i++)
        {
            m_msgBtns[i].gameObject.SetActive(false);
        }
    }

    public void OnOKClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OK);
        if(m_okDel != null) m_okDel();
        closePanel();
    }

    public void OnCancelClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CANCEL);
        if (m_cancelDel != null) m_cancelDel();
        closePanel();
    }

    /// <summary>
    /// 닫기
    /// </summary>
    protected override void OnDisable()
    {
        m_okDel = null;
        m_cancelDel = null;
        base.OnDisable();
    }
}
