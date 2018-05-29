using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : UIPanel, IRootPanel
{
    //[SerializeField]
    //UIDataBox m_uiDataBox;

    //[SerializeField]
    //UIMsg m_uiMsg;

//    [SerializeField]
//    UIAchieveAlarm m_uiAchieveAlarm;

    //[SerializeField]
    //UIUnitInfomation m_uiUnitInformation;


    [SerializeField]
    Button[] m_lobbyBtns;

    [SerializeField]
    UIAccount m_uiAccount;

    [SerializeField]
    UIMission m_uiMission;

    [SerializeField]
    UIBarracks m_uiBarracks;

    [SerializeField]
    UIEmployee m_uiEmployee;

    [SerializeField]
    UIDevelop m_uiDevelop;

    [SerializeField]
    UIShop m_uiShop;

    [SerializeField]
    UIAchieve m_uiAchieve;

    //[SerializeField]
    //UIOption m_uiOption;

    [SerializeField]
    UICommon m_tmpUICommon;

//    [SerializeField]
//    UITutorial m_uiTutorial;

    [SerializeField]
    GameObject m_backgroundPanel;
    
    UICommon m_uiCommon;

    SoundPlay m_soundPlay;

    bool m_isContents = false;

    public UIDataBoxManager uiDataBox { get { return uiCommon.uiDataBox; } }
   
//    public UIAchieveAlarm uiAchieveAlarm { get { return m_uiAchieveAlarm; } }
    public UIUnitInfomation uiUnitInformation { get { return uiCommon.uiUnitInfomation; } }
    public UIMsg uiMsg { 
        get {
            return uiCommon.uiMsg; 
        } 
    }

    public UICommon uiCommon
    {
        get
        {
            if (m_uiCommon == null)
            {
                GameObject commonObj = GameObject.Find("Game@Common");

                if (commonObj == null)
                {
                    m_uiCommon = Instantiate(m_tmpUICommon, Vector2.zero, Quaternion.identity);
                    m_uiCommon.name = m_uiCommon.name.Replace("(Clone)", "");
                }
                else
                    m_uiCommon = commonObj.GetComponent<UICommon>();
            }


            return m_uiCommon;
        }
    }

    //public UITutorial uiTutorial
    //{
    //    get
    //    {
    //        if (m_uiTutorial == null)
    //            m_uiTutorial = GameObject.Find("Game@Tutorial").GetComponent<UITutorial>();
    //        return m_uiTutorial;
    //    }
    //}

    public SoundPlay soundPlay
    {
        get
        {
            if (m_soundPlay == null)
            {
                m_soundPlay = GetComponent<SoundPlay>();
                if (m_soundPlay == null)
                    m_soundPlay = gameObject.AddComponent<SoundPlay>();
            }

            return m_soundPlay;
        }
    }



    void Awake()
    {


        Camera.main.ResetAspect();
        Screen.SetResolution(Screen.width, Screen.width * 9 / 16, true);

        ActorManager.GetInstance.initActorManager();
        UIPanelManager.GetInstance.setRoot(this);
        soundPlay.audioPlay("BGMLobby", TYPE_SOUND.BGM);

    }

    

    IEnumerator lobbyCoroutine()
    {
        if (string.IsNullOrEmpty(Account.GetInstance.name))
            yield return uiCommon.uiName;

        //로비 자막 이벤트
        yield return StartCoroutine(uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.Lobby));

//            uiCommon.uiName.viewPanel();
    }

    void Update()
    {
//        uiUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (UIPanelManager.GetInstance.nowPanel() == this)
                OnExitClicked();
            else
            {
                UIPanelManager.GetInstance.backPanel();
            }
            
        }
    }

    public override void uiUpdate()
    {
        base.uiUpdate();

//        StartCoroutine(contentsCoroutine(UIPanelManager.GetInstance.nowPanel().GetType().Name));
//        setContents(UIPanelManager.GetInstance.nowPanel().GetType().Name);


        if (UIPanelManager.GetInstance.nowPanel() == this)
        {
            //패널 닫기
            m_backgroundPanel.SetActive(false);
        }
    }


    protected void OnEnable()
    {
        base.OnEnable();

        Time.timeScale = 1f;
        Account.GetInstance.saveData();
        //업적 재갱신

       


        //스테이지 1-4가 되면 임무를 제외한 모든 버튼 풀리기

        Debug.Log(" : " + Account.GetInstance.accSinario.stageKey);

        for (int i = 0; i < m_lobbyBtns.Length; i++)
        {
            if (i == 0)
                m_lobbyBtns[i].interactable = true;
            else
                m_lobbyBtns[i].interactable = Account.GetInstance.accSinario.isUsed();
        }

        m_uiCommon = uiCommon;
        m_uiCommon.setCamera();

        StartCoroutine(lobbyCoroutine());

//        uiCommon.uiAchieveAlarm.achieveUpdate();

        //uiTutorial.uiIndicator.setTexture(m_lobbyBtns[0].GetComponent<RectTransform>());
    }




    public void OnAccountClicked()
    {
        m_uiAccount.openPanel(this);
    }

    public void OnMissionClicked()
    {
        m_uiMission.openPanel(this);
        m_backgroundPanel.SetActive(true);
    }

    public void OnBarracksClicked()
    {
        m_uiBarracks.openPanel(this);
        m_uiBarracks.setSinario(null);
        m_backgroundPanel.SetActive(true);
    }

    public void OnEmployeeClicked()
    {
        m_uiEmployee.openPanel(this);
        m_backgroundPanel.SetActive(true);
    }

    public void OnDevelopClicked()
    {
        m_uiDevelop.openPanel(this);
        m_backgroundPanel.SetActive(true);
    }

    public void OnShopClicked()
    {
        m_uiShop.openPanel(this);
        m_backgroundPanel.SetActive(true);
    }

    public void OnShopTypeClicked(int typeShopCategory)
    {
        m_uiShop.openPanel(this);
        m_uiShop.viewShop((Shop.TYPE_SHOP_CATEGORY)typeShopCategory);
        m_backgroundPanel.SetActive(true);
    }

    public void OnAchieveClicked()
    {
        m_uiAchieve.openPanel(this);
        m_backgroundPanel.SetActive(true);
    }

    public void OnOptionClicked()
    {
        uiCommon.uiOption.openPanel(this);
    }

    public void OnExitClicked()
    {
        uiCommon.uiMsg.setMsg("게임을 종료하시겠습니까?", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK_CANCEL, exitEvent);
    }

    void exitEvent()
    {
        Application.Quit();
    }
}
