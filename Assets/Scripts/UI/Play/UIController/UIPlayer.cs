using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defence.CommanderPackage;

public class UIPlayer : UIController, IRootPanel {


    [SerializeField]
    GameObject m_interfaceObject;

    [SerializeField]
    GameObject m_midPanel;

//    [SerializeField]
//    UINovel m_uiNovel;

    [SerializeField]
    UIResult m_uiResult;

    [SerializeField]
    UIDefeat m_uiDefeat;

    [SerializeField]
    UIGame m_uiGame;

    [SerializeField]
    UIGameMsg m_uiGameMsg;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Slider m_munitionsSlider;

    [SerializeField]
    Text m_munitionsText;

    [SerializeField]
    Button m_upgradeButton;
    
    [SerializeField]
    Slider m_playerHealthSlider;

    [SerializeField]
    Text m_playerHealthText;

    [SerializeField]
    Image m_playerHealthImage;

    [SerializeField]
    Slider m_cpuHealthSlider;

    [SerializeField]
    Text m_cpuHealthText;

    [SerializeField]
    Image m_cpuHealthImage;
   
        //유닛 생산 버튼
    [SerializeField]
    UIUnitButton[] m_uiUnitButtons;

    //영웅 생산 버튼
    [SerializeField]
    UIUnitButton m_uiHeroButton;

    [SerializeField]
    UISkillButton[] m_uiSkillButtons;

    //라인
//    [SerializeField]
//    Toggle[] m_lineToggles;

    [SerializeField]
    UIProjector m_uiProjector;

    //대사
    //[SerializeField]
    //UIContents m_uiContents;

    //진행시간
    [SerializeField]
    Text m_timeText;

    
    [SerializeField]
    GameObject m_itemObject;

    [SerializeField]
    Text m_goldText;

    [SerializeField]
    Text m_cardText;

    [SerializeField]
    Text m_fruitText;

    [SerializeField]
    UIPause m_uiPause;

    [SerializeField]
    
    UICommon m_uiCommon;

    UITutorial m_uiTutorial;
    
    public UICommon uiCommon { 
        get {
            if (m_uiCommon == null)
            {
                GameObject obj = GameObject.Find("Game@Common");

                if(obj != null)
                    m_uiCommon = obj.GetComponent<UICommon>();

                if (m_uiCommon == null)
                {
                    UICommon tmpCommon = Resources.Load<UICommon>(Prep.uiCommonPrefebsPath);
                    if (tmpCommon != null)
                    {
                        m_uiCommon = Instantiate(tmpCommon);
                    }
                    else
                    {
                        Prep.LogError(Prep.uiCommonPrefebsPath, " 경로를 찾을 수 없음", GetType());
                    }
                }
                else
                {
                    Prep.LogError("Game@Common", "를 찾을 수 없음", GetType());
                }
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

//    public override IActor iActor { get { return m_uiProjector; } }


    //[SerializeField]
    //스킬

    int m_heroStartIndex = 0; //영웅 인덱스 시작
    int m_heroIndex = 0; //현재 영웅 인덱스
//    int m_heroCheck = 0;
    int m_heroEndIndex = 0; // 영웅 인덱스 끝

    int m_line = 0;

    void Start()
    {
        //패널 루트에 삽입
        UIPanelManager.GetInstance.setRoot(this);

        m_itemObject.SetActive(false);
        m_interfaceObject.SetActive(false);
        m_midPanel.SetActive(false);

        //보급 레벨 업그레이드
        upgradeMunitions((int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.MunitionsLv) + 1);

        //계정에 등록된 모든 유닛키를 가져와서 등록

        //레벨은 따로 가져옴


        //스테이지별로 유닛 덱 구성
        Stage stage = Account.GetInstance.accSinario.nowStage;

        if (stage != null)
        {


            //유닛 대기 가져오기
            List<string> unitWaitList = Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, stage.typeForce);

            foreach(string key in unitWaitList)
            {
                UnitCard card = Account.GetInstance.accUnit.getUnitCard(key);
                if (card != null)
                {
                    iCardList.Add(card);
                }
            }

            //영웅 시작점 끝점, 현재 위치 초기화 - 병사 끝점
            m_heroStartIndex = m_heroEndIndex = m_heroIndex = unitWaitList.Count;


            //영웅 대기 가져오기
            List<string> heroWaitList = Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Hero, stage.typeForce);

            foreach (string key in heroWaitList)
            {
                UnitCard card = Account.GetInstance.accUnit.getUnitCard(key);
                if (card != null)
                {
                    iCardList.Add(card);
                }
            }

            //지휘관 스킬 가져오기
            commanderCard = Account.GetInstance.accUnit.getNowCommanderCard();
            for (int i = 1; i < commanderCard.skills.Length; i++)
            {
                SkillCard skillCard = SkillManager.GetInstance.getSkillCard(commanderCard.skills[i], commanderCard.level);
                if (skillCard != null)
                    iCardList.Add(skillCard);
            }

        }

        //unitCardArray[0] = new UnitCard(UnitManager.GetInstance.getUnit("Engineer"));
        //unitCardArray[1] = new UnitCard(UnitManager.GetInstance.getUnit("FrozenMagician"));
        //unitCardArray[2] = new UnitCard(UnitManager.GetInstance.getUnit("LightningMagician"));
        //unitCardArray[3] = new UnitCard(UnitManager.GetInstance.getUnit("Shielder"));
        //unitCardArray[4] = new UnitCard(UnitManager.GetInstance.getUnit("CrawMaster"));
        //unitCardArray[5] = new UnitCard(UnitManager.GetInstance.getUnit("Sniper"));
        
        //영웅은 6부터 순서대로 쌓아야함

        //영웅
        //unitCardArray[6] = new UnitCard(UnitManager.GetInstance.getUnit("SwordMan"));
        //unitCardArray[7] = null;//UnitManager.GetInstance.getUnit("Engineer");
        //unitCardArray[8] = null;//UnitManager.GetInstance.getUnit("Engineer");


        //카드버튼 동기화
        //지휘관 카드 가져오기
//        CommanderCard commanderCard = Account.GetInstance.accUnit.getNowCommanderCard();

        //영웅카드 비워두기
        m_uiHeroButton.setUnit(null, m_heroIndex);


        int unitIndex = 0;
        int skillIndex = 0;
            
        foreach (ICard iCard in iCardList)
        {
            //유닛카드이면
            if (iCard is UnitCard)
            {
                //영웅이면
                if (((UnitCard)iCard).typeUnit == Unit.TYPE_UNIT.Hero)
                {

                    //영웅 카드가 비워져 있으면 - 삽입
                    if (m_uiHeroButton.isEmpty())
                    {
                        m_uiHeroButton.setUnit((UnitCard)iCard, m_heroIndex);

                        m_uiHeroButton.msgPanelEvent += setMsg;
                        m_uiHeroButton.createUnitEvent += createUnitIndex;
                        m_uiHeroButton.checkMunitionEvent += isMunitions;
                        m_uiHeroButton.rateCardTimeEvent += rateCardTime;
                        m_uiHeroButton.isNotUsedEvent += isNotUsedCard;
                    }
                    //영웅 인덱스 증가
                    m_heroEndIndex++;
                    unitIndex++;
                    
                }
                //병사이면
                else if (((UnitCard)iCard).typeUnit == Unit.TYPE_UNIT.Soldier)
                {
                    m_uiUnitButtons[unitIndex].setUnit((UnitCard)iCard, unitIndex);
                    m_uiUnitButtons[unitIndex].msgPanelEvent += setMsg;
                    m_uiUnitButtons[unitIndex].createUnitEvent += createUnitIndex;
                    m_uiUnitButtons[unitIndex].checkMunitionEvent += isMunitions;
                    m_uiUnitButtons[unitIndex].rateCardTimeEvent += rateCardTime;
                    m_uiUnitButtons[unitIndex].isNotUsedEvent += isNotUsedCard;
                    unitIndex++;

                    

                }
                //기타유닛이면 - 나타나면 안됨
                else
                {
                    Prep.LogError(iCard.key + " " + ((UnitCard)iCard).typeUnit, " 의 유닛 타입이 맞지 않습니다", GetType());
                }
            }
            //스킬카드이면
            else if(iCard is SkillCard)
            {
                m_uiSkillButtons[skillIndex].setSkill((SkillCard)iCard, skillIndex);

                m_uiSkillButtons[skillIndex].setProjectorEvent += m_uiProjector.setProjector;
                m_uiSkillButtons[skillIndex].closeProjectorEvent += closeProjector;
                m_uiSkillButtons[skillIndex].msgPanelEvent += setMsg;
                m_uiSkillButtons[skillIndex].rateCardTimeEvent += rateCardTime;
                m_uiSkillButtons[skillIndex].isNotUsedEvent += isNotUsedCard;
                skillIndex++;
            }

        }

        //빈 유닛카드가 있으면 비어있게 하기
        for (int i = 0; i < m_uiUnitButtons.Length; i++)
        {
            if (m_uiUnitButtons[i].isEmpty())
                m_uiUnitButtons[i].setUnit(null, i);
        }

        //빈 스킬카드가 있으면 비어있게 하기
        for (int i = 0; i < m_uiSkillButtons.Length; i++)
        {
            if (m_uiSkillButtons[i].isEmpty())
                m_uiSkillButtons[i].setSkill(null, i);
        }

        //카드키 타이머 초기화
        foreach (ICard card in iCardList)
        {
            Debug.Log("Card : " + card.key);
            addCardTime(card.key);
        }



        ////영웅 카드버튼에 삽입하기
        //m_heroIndex = Prep.maxUnitSlot;

        //if(iCardArray[m_heroIndex] != null)
        //    m_uiHeroButton.setUnit((UnitCard)iCardArray[m_heroIndex], m_heroIndex);
        //else
        //    m_uiHeroButton.setUnit(null, m_heroIndex);

        //m_uiHeroButton.msgPanelEvent += setMsg;
        //m_uiHeroButton.createUnitEvent += createUnitIndex;
        //m_uiHeroButton.checkMunitionEvent += isMunitions;
        //m_uiHeroButton.rateCardTimeEvent += rateCardTime;
        //m_uiHeroButton.isNotUsedEvent += isNotUsedCard;



        //CommanderCard commanderCard = Account.GetInstance.accUnit.getNowCommanderCard();
        

        ////스킬 카드버튼에 삽입하기
        ////지휘관 스킬 카드여야 함
        //for (int i = 0; i < Prep.maxSkillSlot; i++)
        //{
        //    //지휘관에서 가져와야 함
        //    if (commanderCard != null)
        //    {
        //        m_uiSkillButtons[i].setSkill(SkillManager.GetInstance.getSkill(commanderCard.skills[i + 1]), i);
        //    }
        //    else
        //    {
        //        m_uiSkillButtons[i].setSkill(null, i);
        //    }
            
        //    m_uiSkillButtons[i].setProjectorEvent += m_uiProjector.setProjector;
        //    m_uiSkillButtons[i].closeProjectorEvent += closeProjector;
        //    m_uiSkillButtons[i].msgPanelEvent += setMsg;
        //    m_uiSkillButtons[i].rateCardTimeEvent += rateCardTime;
        //    m_uiSkillButtons[i].isNotUsedEvent += isNotUsedCard;

        //}

        //상대 지휘관 카드 가져오기
        if (stage != null)
        {
            CommanderCard cmdCard = CommanderManager.GetInstance.getCommanderCard(stage.deck.commanderKey, stage.deck.commanderLevel);

            //지휘관 세력에 따라서 체력바 변경하기
            //지휘관 아이콘 삽입하기
            if (cmdCard != null)
                m_cpuHealthImage.color = Prep.getForceColor(cmdCard.typeForce);
        }

        m_playerHealthImage.color = Prep.getForceColor(Account.GetInstance.accUnit.getNowCommanderCard().typeForce);



        //m_uiContents.contentsEvent += gameController.contentsCallBack;

        //공용 UI 초기화
        //m_uiCommon = uiCommon;
        uiCommon.setCamera();
        //대사창 띄우기
        uiCommon.uiContents.contentsEvent += gameController.contentsCallBack;

        //프로젝터 초기화
        m_uiProjector.setUIController(this);
    }



   


    //public bool viewHeroUnit()
    //{
    //    if (iCardArray[m_heroIndex] != null)
    //    {
    //        m_uiHeroButton.setUnit((UnitCard)iCardArray[m_heroIndex], m_heroIndex);
    //        return true;
    //    }
    //    return false;
    //}

    //자막

    //준비
    public override void gameReady()
    {
        m_uiGame.gameReady();
    }

    //시작
    public override void gameStart() 
    {
        m_uiGame.gameStart();
        m_interfaceObject.SetActive(true);
        m_midPanel.SetActive(true);
    }

    //게임 마침
    public override void gameFinish()
    {
        m_midPanel.SetActive(false);
        m_uiGameMsg.gameFinish();
        m_interfaceObject.GetComponent<Animator>().SetBool("isActive", false);        
    }

    //패배 및 승리
    public override void gameEnd(bool isVictory)
    {
        m_uiGame.gameEnd(isVictory);
    }

    //결과창
    public override void gameResult(GameController gameCtrler, bool isVictory)
    {
        if(isVictory)
            m_uiResult.setResult(gameCtrler);
        else
            m_uiDefeat.setDefeat();
    }



    //뷰어 보임
    //군수품, 체력, 업그레이드량, 병력 버튼, 스킬, 영웅
    public override void uiUpdate(float frameTime, UnitActorManager unitActorManager)
    {
        base.uiUpdate(frameTime, unitActorManager);


        m_levelText.text = string.Format("Lv : {0}", level + 1);

        m_playerHealthText.text = string.Format("{0}", nowCastleHealth);
        m_playerHealthSlider.value = (float)nowCastleHealth / (float)maxCastleHealth;

        m_cpuHealthText.text = string.Format("{0}", gameController.cpuController.nowCastleHealth );
        m_cpuHealthSlider.value = (float)gameController.cpuController.nowCastleHealth / (float)gameController.cpuController.maxCastleHealth;

        m_munitionsText.text = string.Format("{0}/{1}", nowMunitions, maxMunitions);
        m_munitionsSlider.value = (float)nowMunitions / (float)maxMunitions;

        m_timeText.text = string.Format("{0:d2}:{1:d2}", gameController.gameTime.Minutes, gameController.gameTime.Seconds);

        if (m_munitionsSlider.value >= 1f)
        {
            m_upgradeButton.enabled = true;
            m_upgradeButton.GetComponentInChildren<Text>().text = "확장";
        }
        else
        {
            m_upgradeButton.enabled = false;
            m_upgradeButton.GetComponentInChildren<Text>().text = string.Format("+{0}", addMunitions);
        }


        //쿨타임 보이기

        for (int i = 0; i < m_uiUnitButtons.Length; i++)
        {
            //비어있지 않으면
            if (!m_uiUnitButtons[i].isEmpty())
            {
                string key = m_uiUnitButtons[i].key;
                m_uiUnitButtons[i].uiUpdate(getCardTime(key), isNotUsedCard(key));
            }
        }

        if(!m_uiHeroButton.isEmpty())
            m_uiHeroButton.uiUpdate(getCardTime(m_uiHeroButton.key), isNotUsedCard(m_uiHeroButton.key));

        for (int i = 0; i < m_uiSkillButtons.Length; i++)
        {
            m_uiSkillButtons[i].uiUpdate(getCardTime(m_uiSkillButtons[i].key), isNotUsedCard(m_uiSkillButtons[i].key));
        }



    }

    void createUnitIndex(UnitCard unitCard, int index)
    {
        //병사, 영웅은 따로 인덱스 제작
        createUnit(unitCard, index, (MapController.TYPE_MAP_LINE)m_line);
        //nowMunitions -= unit.munitions;
        //unitDelay[index] = 0f;
    }



    

    //int selectLineToggle()
    //{


    //    for (int i = 0; i < m_lineToggles.Length; i++)
    //    {
    //        if (m_lineToggles[i].enabled == true)
    //        {
    //            //클릭시 해당 라인이 켜져야 함
    //            if (m_lineToggles[i].isOn)
    //                return i;
    //        }
    //    }
    //    return 0;
    //}


    public void OnSelectLineClicked(int line)
    {
        Debug.Log("line : " + line);
        m_line = line;
        gameController.mapCtrler.playerCastleController.setLine(line);
    }

    //왼쪽으로 감소
    public void OnLeftHeroClick()
    {
        //시작점과 끝점이 같으면 영웅이 없음
        if (m_heroStartIndex == m_heroEndIndex)
            return;

        //인덱스가 시작 밑으로 내려가면 끝점으로 이동
        if (m_heroIndex - 1 < m_heroStartIndex)
        {
            m_heroIndex = m_heroEndIndex;
        }

        m_heroIndex--;

        m_uiHeroButton.setUnit((UnitCard)iCardList[m_heroIndex], m_heroIndex);

    }

    //오른쪽으로 증가
    public void OnRightHeroClick()
    {
        //시작점과 끝점이 같으면 영웅이 없음
        if (m_heroStartIndex == m_heroEndIndex)
            return;

        //인덱스가 끝점과 같으면 시작점으로 이동
        if (m_heroIndex + 1 >= m_heroEndIndex)
        {
            m_heroIndex = m_heroStartIndex - 1;
        }

        m_heroIndex++;

        m_uiHeroButton.setUnit((UnitCard)iCardList[m_heroIndex], m_heroIndex);
    }

    public void OnMunitionsUpgradeClicked()
    {
        //보급이 다 차있으면
        //업그레이드
        if (m_munitionsSlider.value >= 1f)
        {
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.LVUP);
            upgradeMunitions();
            setMsg("보급 확장 완료!");
        }
    }

    public override void setMsg(string msg)
    {
        m_uiGameMsg.setMsg(msg);
    }

    public override void setCastleWarning(float rate)
    {
        m_uiGameMsg.setCastleWarning(rate);
    }
    
    public override void setHeroAppear(UIController uiCtrler)
    {
        m_uiGameMsg.setHeroAppear(uiCtrler);
    }

    public void OnPauseClicked()
    {
        m_uiPause.openPanel(this);
    }


    public void closeProjector(SkillCard skillCard, int index, bool isUsed)
    {
        //사용되었으면 쿨타임 초기화
        if (isUsed) resetCardTime(skillCard.key);

        //프로젝터 종료 및 실행 여부
        m_uiProjector.closeProjector(skillCard, isUsed);
    }

}
