using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecruit : UIPanel
{

    const int m_cost = 100;
    const int m_count = 5;

    [SerializeField]
    UICardSummary m_unitCardSummary;

    [SerializeField]
    Transform m_unitViewPanel;
    
    [SerializeField]
    Text m_timeText;

    //[SerializeField]
    //Button m_timeRefleshButton;

    [SerializeField]
    Button m_refleshButton;

    [SerializeField]
    Text m_costText;


//    [SerializeField]
    UIUnitInfomation m_uiUnitEmployee;
  

    List<UICardSummary> m_unitCardList = new List<UICardSummary>();

    void Awake()
    {
        m_uiUnitEmployee = UIPanelManager.GetInstance.root.uiCommon.uiUnitInfomation;

        //문제점이 발생할 수 있음 (이벤트 중복)
        m_uiUnitEmployee.recruitDataUpdateEvent += dataUpdate;

        m_refleshButton.onClick.AddListener(() => OnRefleshClicked());
//        m_timeRefleshButton.onClick.AddListener(() => OnTimeRefleshClicked());
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        clear();
        createCard();
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

    }

    void FixedUpdate()
    {


        if (Account.GetInstance.accUnit.nowTimeRate() >= 1f)
        {
            //새로고침 무료
            m_timeText.text = "새로고침 무료 가능";
            m_costText.text = "무료";
        }
        else
        {
            m_timeText.text = "새로고침 무료 대기" + Account.GetInstance.accUnit.nowTime();
            m_costText.text = string.Format("{0}", -m_cost);
        }
    }

    void dataUpdate(ISummary iSummary)
    {
        m_unitCardList.Remove((UICardSummary)iSummary);
        Destroy(iSummary.gameObject);
    }


    void clear()
    {
        for (int i = m_unitCardList.Count - 1; i >= 0; i--)
        {
            Destroy(m_unitCardList[i].gameObject);
        }

        m_unitCardList.Clear();
    }

    void createCard()
    {
        for (int i = 0; i < m_count; i++)
        {
            UICardSummary uiCard = Instantiate(m_unitCardSummary) as UICardSummary;
            uiCard.setUnit(UnitManager.GetInstance.getRandomUnit(0));
            uiCard.unitInformationEvent += m_uiUnitEmployee.setSummary;
            uiCard.transform.SetParent(m_unitViewPanel);
            uiCard.transform.localScale = Vector2.one;
            m_unitCardList.Add(uiCard);
        }
    }


    void OnTimeRefleshClicked()
    {

        clear();
        createCard();
        Account.GetInstance.accUnit.setRefleshTime();
    }



    void OnRefleshClicked()
    {

        //무료이면
        if (Account.GetInstance.accUnit.nowTimeRate() >= 1f)
        {
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
            OnTimeRefleshClicked();
        }
        //무료가 아니면
        else
        {
            if (Account.GetInstance.accData.isValue(100, Shop.TYPE_SHOP_CATEGORY.Fruit))
            {
                //새로고침 하시겠습니까?
                UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("비용을 소모하여 새로고침 하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, refleshEvent);
            }
            else
            {
                UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("비용이 부족합니다.", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK);
            }
        }
    }

    void refleshEvent()
    {
        Account.GetInstance.accData.useValue(100, Shop.TYPE_SHOP_CATEGORY.Fruit);
        clear();
        createCard();
    }

}

