using UnityEngine;
using Defence.CommanderPackage;

public class UICPU : UIController//, IActor
{

    public enum TYPE_AI_LEVEL { VERY_EASY = 0, EASY, NORMAL, HARD, VERY_HARD };

    //유닛 및 스킬 자동 사용
    TYPE_AI_LEVEL m_typeAILevel;

//    Unit[] m_unitArray = new Unit[Prep.maxUnitSlot + Prep.maxHeroSlot];

    float m_timer = 0;

    SkillCard skillCard = null;

    [SerializeField]
    UICPUSkill m_uiCpuSkill;

//    public override IActor iActor{get { return this; }}

    void Start()
    {
        Stage stage = Account.GetInstance.accSinario.nowStage;
        if (stage != null)
        {
            foreach (ICard iCard in stage.deck.cardArray)
            {
                if (iCard != null)
                {
                    //카드 리스트에 삽입
                    iCardList.Add(iCard);
                    //카드 타이머 삽입
                    addCardTime(iCard.key);
                }
            }
        }


        CommanderCard cmdCard = CommanderManager.GetInstance.getCommanderCard(stage.deck.commanderKey, stage.deck.commanderLevel);

        //지휘관 스킬 등록하기
        if (cmdCard != null)
        {
            for (int i = 0; i < Prep.maxSkillSlot; i++)
            {
                //스킬 찾기 - 2번째 스킬부터
                Skill skill = SkillManager.GetInstance.getSkill(cmdCard.skills[i + 1]);

                if(skill != null){
                    //스킬 레벨 - 지휘관 레벨
                    SkillCard skillCard = new SkillCard(skill, 1);

                    iCardList.Add(skillCard);
                    addCardTime(skillCard.key);
                }
            }
        }
    }


    public override void uiUpdate(float frameTime, UnitActorManager unitActorManager)
    {
        base.uiUpdate(frameTime, unitActorManager);

        //유닛 쿨타임이 다 되면 보급 사용 후 생산
//        for (int i = 0; i < iCardArray.Length; i++)
//        {

        //행동 타이머
        //체력이 줄수록 빨라짐 - 최대 2배
        m_timer += frameTime + (frameTime * (1f - (nowCastleHealth / maxCastleHealth))); 
        
        //생성한 유닛이 어느정도 쌓이면 생성하지 않고 업그레이드

        //체력이 줄 수록 보급량 상승


        //좌우로 움직이고 있어야 함 - 적 또는 아군


        if (3f - ((float)(m_typeAILevel) * 0.5f) < m_timer)
        {
            //카드 랜덤 가져오기
            int index = Random.Range(0, iCardList.Count);

            //카드 선택하기
            ICard iCard = iCardList[index];
//            Debug.Log("index iCardLis " + index + " " + iCard);
            if (iCard != null)
            {
                //생성불가가 아니고 생산시간이 되었고 군수품이 충족되면 
                if (!Prep.isNotEnemyCreate &&
                    rateCardTime(iCard) >= 1f &&
                    nowMunitions >= iCard.munitions
                    )
                {
                    //유닛카드
                    if (iCard is UnitCard)
                    {
                        //사용 가능이면
                        if (!isNotUsedCard(iCard.key))
                        {
                            createUnitIndex((UnitCard)iCard, index);
                            m_timer = 0f;
                        }
                        //Debug.Log("nowMunitions : " + nowMunitions);
                    }
                    else if(iCard is SkillCard)
                    {
                        //스킬카드 - 스킬사용 클래스 제작 필요
                        //스킬 사용 위치를 알아야 함 - CPU 패널 위치
                        //스킬 사용시 적용이면 적에게, 아군이면 아군위치로
                        //적용인지 아군용인지 구분 필요

                        if (m_uiCpuSkill.setSkill(this, (SkillCard)iCard, gameController.unitActorManager.searchManyUnitAssociation))
                        {
                            resetCardTime(iCard.key);
                            m_timer = 0f;
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 유닛 생성 명령
    /// </summary>
    /// <param name="unitCard"></param>
    /// <param name="index"></param>
    void createUnitIndex(UnitCard unitCard, int index)
    {
        //병사, 영웅은 따로 인덱스 제작
        MapController.TYPE_MAP_LINE typeMapLine = (Prep.isOneLine) ? MapController.TYPE_MAP_LINE.Top : (MapController.TYPE_MAP_LINE)Random.Range((int)MapController.TYPE_MAP_LINE.Top, (int)MapController.TYPE_MAP_LINE.Bot + 1);
        createUnit(unitCard, index, typeMapLine);

    }


}

