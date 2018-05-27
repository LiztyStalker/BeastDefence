using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitView : MonoBehaviour
{
    public delegate void StageBtnDelegate(bool isUsed);
    delegate UnitCard SearchUnitCardDelegate(string unitKey);
    public delegate void FoodValueDelegate();

    public FoodValueDelegate foodValueEvent;
    public StageBtnDelegate stageBtnEvent;



    [SerializeField]
    UIUnitCard m_unitCard;

    [SerializeField]
    UIUnitInfo m_uiUnitInfo;

    [SerializeField]
    Transform m_unitWaitTransform;

    [SerializeField]
    Transform m_unitListTransform;

    [SerializeField]
    Text m_populationText;

    [SerializeField]
    Text m_barracksText;

    [SerializeField]
    Button m_upButton;

    [SerializeField]
    Button m_downButton;

    List<UIUnitCard> m_unitWaitList = new List<UIUnitCard>();
    List<UIUnitCard> m_unitCardList = new List<UIUnitCard>();

    UIDataBoxManager m_uiDataBox;

    ToggleGroup m_toggleGroup;

    Stage m_stage;
    CommanderCard m_commanderCard;

    int maxPopulation = 10;

    UIBarracks.TYPE_BARRACKS m_typeBarracks;

    void Awake()
    {
        m_uiDataBox = UIPanelManager.GetInstance.root.uiCommon.uiDataBox;

        m_upButton.onClick.AddListener(() => OnUpButtonClicked());
        m_downButton.onClick.AddListener(() => OnDownButtonClicked());
        //        m_changeButton.onClick.AddListener(() => OnUnitChangedClicked());

        m_uiUnitInfo.unitRefleshEvent += refleshCard;
        //for (int i = 0; i < m_barracksToggles.Length; i++)
        //{
        //    m_barracksToggles[i].isOn = false;
        //    m_barracksToggles[i].onValueChanged.AddListener((isOn) => OnUnitChangeClicked(isOn));
        //}

        //m_barracksToggles[0].isOn = true;

        //m_uiDataBox = ((UILobby)UIPanelManager.GetInstance.root).uiDataBox;
    }

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    setUnitCard();
    //    //전투 대기중인 병사 정보 보여주기
    //    //foreach(string unitKey in Account.GetInstance.accountUnit.unitWaitList){
    //    //    UIUnitCard uiUnitCard = createUIUnitCard();
    //    //    uiUnitCard.setUnitCard(Account.GetInstance.accountUnit.getUnitCard(unitKey));
    //    //    uiUnitCard.transform.SetParent(m_unitWaitTransform);
    //    //    uiUnitCard.transform.localScale = Vector2.one;
    //    //    m_unitWaitList.Add(uiUnitCard);
    //    //}


    //    ////인구 가져오기
    //    //m_populationText.text = string.Format("{0}/10", getUsePopulation());

    //    ////계정이 가지고 있는 병사 정보 다 보여주기
    //    //foreach (UnitCard unitCard in Account.GetInstance.accountUnit.unitCardList)
    //    //{
    //    //    //대기 병사에 이름이 없으면
    //    //    if (!Account.GetInstance.accountUnit.unitWaitList.Contains(unitCard.unit.key))
    //    //    {
    //    //        UIUnitCard uiUnitCard = createUIUnitCard();
    //    //        uiUnitCard.setUnitCard(unitCard);
    //    //        uiUnitCard.transform.SetParent(m_unitListTransform);
    //    //        uiUnitCard.transform.localScale = Vector2.one;
    //    //        m_unitCardList.Add(uiUnitCard);
    //    //    }
    //    //}

    //    //m_unitCardList[0].GetComponent<Toggle>().isOn = true;
    //    setSinario(false);

    //}

    //protected override void OnDisable()
    //{
    //    removeUnitCard();
    //    base.OnDisable();
    //}


    void setCommander(Stage stage)
    {
        //지휘관 가져오기 
        m_commanderCard = Account.GetInstance.accUnit.getCommanderCard(Account.GetInstance.accUnit.nowCommander);

        if (stage != null)
        {
            //지휘관이 해당 세력에 맞지 않으면 해당 세력에 맞는 지휘관으로 교체
            if ((stage.typeForce & m_commanderCard.typeForce) != stage.typeForce)
            {
                //해당하는 세력의 첫번째 지휘관 가져오기
                m_commanderCard = Account.GetInstance.accUnit.getFirstCommanderCard(stage.typeForce);
                Account.GetInstance.accUnit.nowCommander = m_commanderCard.key;
            }
        }
    }

    TYPE_FORCE getTypeForce()
    {
        if (m_stage == null)
            return TYPE_FORCE.All;
        return m_stage.typeForce;
    }


    public void initUnit(Stage stage, UIBarracks.TYPE_BARRACKS typeBarracks)
    {
        m_typeBarracks = typeBarracks;

        m_stage = stage;

        gameObject.SetActive(true);

        //기록된 모든 카드UI 지우기
        removeUnitCard();

        //그룹 재설정
        m_toggleGroup = m_unitListTransform.GetComponent<ToggleGroup>();

        //사령관 가져오기
        setCommander(stage);

        //타입에 따라 유닛 보여주기
        switch (typeBarracks)
        {
            case UIBarracks.TYPE_BARRACKS.Soldier:
                maxPopulation = (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.SoldierWait) + Prep.defaultSoldierPopulation;

                m_barracksText.text = string.Format("막사 인구 {0}/{1}", Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Soldier), Account.GetInstance.accUnit.getUnitMaxCount(Unit.TYPE_UNIT.Soldier));


                setUnitCard(
                    Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, getTypeForce()),
                    Account.GetInstance.accUnit.getUnitCards(Unit.TYPE_UNIT.Soldier, getTypeForce()),
                    Account.GetInstance.accUnit.getUnitCard
                    );

                StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.BarracksSoldier));


                break;
            case UIBarracks.TYPE_BARRACKS.Hero:
                maxPopulation = (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.HeroWait) + Prep.defaultHeroPopulation;

                m_barracksText.text = string.Format("막사 인구 {0}/{1}", Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Hero), Account.GetInstance.accUnit.getUnitMaxCount(Unit.TYPE_UNIT.Hero));

                setUnitCard(
                    Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Hero, getTypeForce()),
                    Account.GetInstance.accUnit.getUnitCards(Unit.TYPE_UNIT.Hero, getTypeForce()),
                    Account.GetInstance.accUnit.getUnitCard
                    );

                StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.BarracksHero));


                break;
            default:
                Prep.LogWarning(typeBarracks.ToString(), "해당하는 데이터가 없습니다.", GetType());
                break;
        }


        viewCardInfo(null);
    }

    /// <summary>
    /// 모든 카드 제거하기 - Clear
    /// </summary>
    void removeUnitCard()
    {
        for (int i = m_unitWaitList.Count - 1; i >= 0; i--)
        {
            Destroy(m_unitWaitList[i].gameObject);
        }

        m_unitWaitList.Clear();
        
        //        Debug.Log("m_unitCardList : " + m_unitCardList.Count);
        for (int i = m_unitCardList.Count - 1; i >= 0; i--)
        {
            //            Debug.Log("m_unitCardList : " + i);
            Destroy(m_unitCardList[i].gameObject);
        }

        m_unitCardList.Clear();

    }

    //카드 펼치기
    void setUnitCard(List<string> unitWaitList, IEnumerator unitCardEnumerator, SearchUnitCardDelegate searchUnitCardDel)
    {




        //전투 대기중인 병사 정보 보여주기
        //세력이 다르면 리스트에서 빠져야 함

        foreach (string unitKey in unitWaitList)
        {
//            Debug.LogWarning("wait : " + unitKey);
            UIUnitCard uiUnitCard = createUIUnitCard();
            uiUnitCard.setUnitCard(searchUnitCardDel(unitKey));
            uiUnitCard.transform.SetParent(m_unitWaitTransform);
            uiUnitCard.transform.localScale = Vector2.one;
            uiUnitCard.unitInfoEvent += viewCardInfo;
            m_unitWaitList.Add(uiUnitCard);
        }

        

        //인구 가져오기
        viewPopulation();

        while (unitCardEnumerator.MoveNext())
        {
            UnitCard unitCard = unitCardEnumerator.Current as UnitCard;

            if (!unitWaitList.Contains(unitCard.unit.key))
            {
                UIUnitCard uiUnitCard = createUIUnitCard();
                uiUnitCard.setUnitCard(unitCard);
                uiUnitCard.transform.SetParent(m_unitListTransform);
                uiUnitCard.transform.localScale = Vector2.one;
                uiUnitCard.unitInfoEvent += viewCardInfo;
                m_unitCardList.Add(uiUnitCard);
            }
        }

    }

    UIUnitCard createUIUnitCard()
    {
        UIUnitCard uiUnitCard = Instantiate(m_unitCard);
        uiUnitCard.setUnitCard(null);
//        uiUnitCard.setParent(this);
        uiUnitCard.GetComponent<Toggle>().group = m_toggleGroup;
        return uiUnitCard;
    }


    //해당 유닛카드 새로고침
    public void refleshCard(UnitCard unitCard)
    {
        //해당 유닛카드 새로고침
        foreach (UIUnitCard card in m_unitWaitList)
        {
            if (card.iCard == unitCard)
            {
                card.setUnitCard(unitCard);
                return;
            }
        }

        foreach (UIUnitCard card in m_unitCardList)
        {
            if (card.iCard == unitCard)
            {
                card.setUnitCard(unitCard);
                return;
            }
        }

        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

    }

    //void FixedUpdate()
    //{
    //    if (m_isMove)
    //        dragUnitCard();
    //}

    /// <summary>
    /// 대기인구
    /// </summary>
    void viewPopulation()
    {
        m_populationText.text = string.Format("대기 인구 {0}/{1}", getUsePopulation(), maxPopulation);
        //인구 삽입
        foodValueEvent();

    }

    /// <summary>
    /// 사용 인구
    /// </summary>
    /// <returns></returns>
    int getUsePopulation()
    {
        int population = 0;
        foreach (UIUnitCard uiUnitCard in m_unitWaitList)
        {
            population += uiUnitCard.getPopulation();
        }
        return population;
    }

    /// <summary>
    /// 카드 정보
    /// </summary>
    /// <param name="iCard"></param>
    public void viewCardInfo(ICard iCard)
    {
        m_uiUnitInfo.setUnitCard((UnitCard)iCard, getTypeForce());
        viewButton();
    }

    /// <summary>
    /// 버튼 보이기 (업, 다운)
    /// </summary>
    public void viewButton()
    {

        if (m_uiDataBox != null) m_uiDataBox.close();

        if (m_stage != null)
        {
            if (m_typeBarracks == UIBarracks.TYPE_BARRACKS.Hero)
            {
                //다음 스테이지가 1-3일때 영웅이 없으면 - 버튼 안나옴

                Debug.Log("nowStage : " + (m_unitWaitList.Count > 0));

                if (Account.GetInstance.accSinario.stageKey == "Stage013")
                {
                    stageBtnEvent(m_unitWaitList.Count > 0);
                }
            }
        }

        foreach (UIUnitCard uiUnitCard in m_unitWaitList)
        {
            if (uiUnitCard.toggle.isOn)
            {
                m_upButton.interactable = false;
                m_downButton.interactable = true;
                return;
            }
        }

        //계정이 가지고 있는 병사 정보 다 보여주기
        foreach (UIUnitCard uiUnitCard in m_unitCardList)
        {
            if (uiUnitCard.toggle.isOn)
            {
                m_upButton.interactable = true;
                m_downButton.interactable = false;
                return;
            }
        }
        m_upButton.interactable = false;
        m_downButton.interactable = false;



    }

    public void OnUpButtonClicked()
    {
        //대기 유닛 수량 조절하기
        if (m_unitWaitList.Count < 6)
        {
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.UP);

            upUnitCard();
            viewButton();
            viewPopulation();

            StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.BarracksUp));

        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("대기 칸이 가득 찼습니다.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
        }
    }

    public void OnDownButtonClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.DN);
        downUnitCard();
        viewButton();
        viewPopulation();
        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.BarracksDn));
    }

    //카드 올리기
    void upUnitCard()
    {
        foreach (UIUnitCard uiUnitCard in m_unitCardList)
        {
            if (uiUnitCard.toggle.isOn)
            {
                //인구 계산 필요
                int population = getUsePopulation() + uiUnitCard.getPopulation();
                if (population <= maxPopulation)
                {
                    m_unitWaitList.Add(uiUnitCard);
                    uiUnitCard.transform.SetParent(m_unitWaitTransform);
                    m_unitCardList.Remove(uiUnitCard);

                    Account.GetInstance.accUnit.addWaitList(getTypeForce(), (UnitCard)uiUnitCard.iCard);

                }
                else
                {
                    UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("대기 인구가 가득 찼습니다.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
                }
                break;
            }
        }
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

    }

    //카드 내리기
    void downUnitCard()
    {
        foreach (UIUnitCard uiUnitCard in m_unitWaitList)
        {
            if (uiUnitCard.toggle.isOn)
            {
                m_unitCardList.Add(uiUnitCard);
                uiUnitCard.transform.SetParent(m_unitListTransform);
                m_unitWaitList.Remove(uiUnitCard);

                Account.GetInstance.accUnit.removeWaitList(getTypeForce(), (UnitCard)uiUnitCard.iCard);

                break;
            }
        }
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

    }
    


    //public void beginUnitCard()
    //{
    //    if (!m_isMove)
    //    {
    //        m_isMove = true;
    //    }

    //}


    //public void dragUnitCard()
    //{
    //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    //카드 마우스 위치
    //}

    //public void endUnitCard()
    //{
    //    m_isMove = false;
    //    //등록 - 전 데이터는 없애기
    //    //카드 위치 확인 후 삽입
    //}




    //유닛 드래그로 리스트에 빼거나 넣기 또는 교체하기
    //인구가 넘으면 안된다는 메시지 출력하기




}

