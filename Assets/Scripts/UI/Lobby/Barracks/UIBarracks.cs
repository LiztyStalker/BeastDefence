using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Defence.CommanderPackage;

public class UIBarracks : UIPanel
{
    public enum TYPE_BARRACKS {Commander, Soldier, Hero}


    [SerializeField]
    UIUnitView m_uiUnitView;

    [SerializeField]
    UICommanderView m_uiCommanderView;

    [SerializeField]
    Button m_sinarioButton;

    [SerializeField]
    Text m_foodText;

    [SerializeField]
    UIMissionReady m_uiMissionReady;

    [SerializeField]
    Toggle[] m_barracksToggles;


    //세력 설정하기

    List<UIUnitCard> m_unitWaitList = new List<UIUnitCard>();
    List<UIUnitCard> m_unitCardList = new List<UIUnitCard>();

    TYPE_BARRACKS m_typeBarracks;

    Stage m_stage;

    bool isInit = false;

    void Awake()
    {
        m_uiUnitView.stageBtnEvent += setNextButton;
    }

    void initBarracks()
    {
        if (!isInit)
        {
            for (int i = 0; i < m_barracksToggles.Length; i++)
            {
                m_barracksToggles[i].isOn = false;
                m_barracksToggles[i].onValueChanged.AddListener((isOn) => OnChangeClicked(isOn));
            }

            if (m_barracksToggles.Length > 0)
                m_barracksToggles[0].isOn = true;

            m_uiUnitView.foodValueEvent += setFoodText;
            isInit = true;
        }
    }
    

    protected override void OnEnable()
    {
        base.OnEnable();
        //타입에 따라 보여주기
        //setSinario(null);
        selectPanel();
        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.Barracks));

    }

    /// <summary>
    /// 시나리오 진행중 여부
    /// </summary>
    /// <param name="isSinario"></param>
    public void setSinario(Stage stage)
    {
        initBarracks();

        m_stage = stage;
        m_sinarioButton.gameObject.SetActive((stage != null));


        if (stage != null)
        {
            //지휘관이 하나만 있으면 - 병사로 이동
            if (Account.GetInstance.accUnit.getCommanderCount() == 1)
                m_typeBarracks = TYPE_BARRACKS.Soldier;
            else
                m_typeBarracks = TYPE_BARRACKS.Commander;
        }

        //스테이지
        for(int i = 0 ; i < m_barracksToggles.Length; i++)
        {
            if((int)m_typeBarracks == i)
                m_barracksToggles[i].isOn = true;
            else
                m_barracksToggles[i].isOn = false;
        }

        



        //m_typeBarracks = TYPE_BARRACKS.Commander;
        selectPanel();
        setFoodText();
    }

    void selectPanel()
    {
        if (m_typeBarracks == TYPE_BARRACKS.Commander)
        {
            m_uiUnitView.gameObject.SetActive(false);
            m_uiCommanderView.initCards(m_stage);

        }
        else
        {
            m_uiUnitView.initUnit(m_stage, m_typeBarracks);
            m_uiCommanderView.gameObject.SetActive(false);
        }
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

    }


    public void setFoodText()
    {
        if(m_stage != null)
            m_foodText.text = string.Format("-{0}", Account.GetInstance.accUnit.getUnitTotalPopulation(m_stage.typeForce));
    }


    public void OnChangeClicked(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < m_barracksToggles.Length; i++)
            {
                if (m_barracksToggles[i].isOn)
                {
                    m_typeBarracks = (TYPE_BARRACKS)i;
                    break;
                }
            }

        }

        selectPanel();
    }

    /// <summary>
    /// 다음 단계 진행 - 시나리오
    /// </summary>
    public void OnNextClicked()
    {

        if (m_stage != null)
        {
            //지휘관이면 - 병사로 - 병사이면 영웅으로 - 영웅이면 다음

            switch(m_typeBarracks){
                case TYPE_BARRACKS.Commander:
                    m_typeBarracks = TYPE_BARRACKS.Soldier;
//                    m_barracksToggles[(int)m_typeBarracks - 1].isOn = false;
                    m_barracksToggles[(int)m_typeBarracks].isOn = true;
                break;
                case TYPE_BARRACKS.Soldier:
                //영웅이 하나도 없으면

                    //대기중인 병사가 하나도 없으면


                    Debug.Log("Cnt : " + Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, m_stage.typeForce).Count);

                    if (Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, m_stage.typeForce).Count <= 0){
                        UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("병사는 적어도 1기 이상 대기해야 합니다.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
                        return;
                    }

                    if (Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Hero) <= 0)
                    {
                        //영웅칸으로 곧바로 넘어가기
                        goto case TYPE_BARRACKS.Hero;
                    }

                    m_typeBarracks = TYPE_BARRACKS.Hero;
//                    m_barracksToggles[(int)m_typeBarracks - 1].isOn = false;
                    m_barracksToggles[(int)m_typeBarracks].isOn = true;
                    break;
                case TYPE_BARRACKS.Hero:
                    //대기중인 유닛이 0명이면 
                    UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

                    

                    //최종 전투대기로 전환
                    m_uiMissionReady.setMissionReady(m_stage);
                    m_uiMissionReady.openPanel(this);
                    break;
            }
        }
    }

    /// <summary>
    /// 뒤로가기
    /// </summary>
    bool OnBackClicked()
    {
        if (m_stage != null)
        {

            //지휘관이면 - 병사로 - 병사이면 영웅으로 - 영웅이면 다음
            switch (m_typeBarracks)
            {
                case TYPE_BARRACKS.Commander:
                    return true;
                case TYPE_BARRACKS.Soldier:
                    //지휘관이 하나만 있으면 곧바로 뒤로
                    if (Account.GetInstance.accUnit.getCommanderCount() <= 1)
                    {
                        return true;
                    }
                    m_typeBarracks = TYPE_BARRACKS.Commander;
                    m_barracksToggles[(int)m_typeBarracks].isOn = true;
                    break;
                case TYPE_BARRACKS.Hero:
                    m_typeBarracks = TYPE_BARRACKS.Soldier;
                    m_barracksToggles[(int)m_typeBarracks].isOn = true;
                    break;
            }

        }
        return false;
    }

    void setNextButton(bool isUsed)
    {
        m_sinarioButton.interactable = isUsed;
    }

    public override void closePanel()
    {
        if (m_stage != null)
        {
            if (OnBackClicked())
                base.closePanel();
        }
        else
            base.closePanel();
    }

    protected override void OnDisable()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.OnDisable();
    }







}

