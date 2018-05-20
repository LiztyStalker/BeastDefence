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

    [SerializeField]
    UITutorial m_uiTutorial;

    UICommon m_uiCommon;

    SoundPlay m_soundPlay;


    public UIDataBox uiDataBox { get { return uiCommon.uiDataBox; } }
   
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

    public UITutorial uiTutorial
    {
        get
        {
            if (m_uiTutorial == null)
                m_uiTutorial = GameObject.Find("Game@Tutorial").GetComponent<UITutorial>();
            return m_uiTutorial;
        }
    }

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
        ActorManager.GetInstance.initActorManager();
        UIPanelManager.GetInstance.setRoot(this);
        soundPlay.audioPlay("BGMLobby", TYPE_SOUND.BGM);

        if (string.IsNullOrEmpty(Account.GetInstance.name))
            uiTutorial.uiName.viewPanel();

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if (UIPanelManager.GetInstance.nowPanel() == this)
                OnExitClicked();
            else
                UIPanelManager.GetInstance.backPanel();
        }
    }


    protected void OnEnable()
    {
        base.OnEnable();

        Time.timeScale = 1f;
        Account.GetInstance.saveData();
        //업적 재갱신

        //        IEnumerator enumerator = AchieveManager.GetInstance.values;

        //        while(enumerator.MoveNext())
        //        {

        //            Achieve achieve = enumerator.Current as Achieve;
        ////            Debug.Log("achieve");
        //            if (!Account.GetInstance.accAchieve.isAchieve(achieve.key))
        //            {
        ////                Debug.Log("achieve1");
        //                if (achieve.value <= Account.GetInstance.accAchieve.getAchieveValue(achieve.typeAchieve))
        //                {
        ////                    Debug.Log("achieve2");
        //                    //갱신
        //                    Account.GetInstance.accAchieve.setAchieve(achieve.key);
        //                    m_uiAchieveAlarm.setAlarm(achieve);
        //                }
        //            }
        //        }

        m_uiCommon = uiCommon;
        m_uiCommon.setCamera();

        //uiTutorial.uiIndicator.setTexture(m_lobbyBtns[0].GetComponent<RectTransform>());
    }

    public void OnMissionClicked()
    {
        m_uiMission.openPanel(this);
    }

    public void OnBarracksClicked()
    {
        m_uiBarracks.openPanel(this);
        m_uiBarracks.setSinario(null);
    }

    public void OnEmployeeClicked()
    {
        m_uiEmployee.openPanel(this);
    }

    public void OnDevelopClicked()
    {
        m_uiDevelop.openPanel(this);
    }

    public void OnShopClicked()
    {
        m_uiShop.openPanel(this);
    }

    public void OnShopTypeClicked(int typeShopCategory)
    {
        m_uiShop.openPanel(this);
        m_uiShop.viewShop((Shop.TYPE_SHOP_CATEGORY)typeShopCategory);
    }

    public void OnAchieveClicked()
    {
        m_uiAchieve.openPanel(this);
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
