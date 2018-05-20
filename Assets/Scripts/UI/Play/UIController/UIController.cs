using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIController : UIPanel
{

    GameController m_gameController;

    //성 레벨
    UnitActor m_castle;

    //지휘관
    CommanderCard m_commanderCard;

    //버프 컨트롤
    CommanderBuffControl m_buffCtrl;

    //현재 보급 레벨
    int m_level = 1;

    //성 레벨
    //int m_castleLevel;

    //보급 레벨
//    int m_maxMunitionsLevel;

    //보급 충전레벨
//    int m_munitionsLevel;

    //성 최대 체력
//    int m_maxCastleHealth;

    //성 체력
//    int m_nowCastleHealth;

    //최대 보급 증가량 - 1레벨당 최대 보급량 증가량
    int m_increaseMunitions = 10;

    //최대 보급량 - 레벨에 따라 다름
    int m_maxMunitions = 50;

    //현재 보급현황 - 사용가능 보급
    int m_nowMunitions = 0;

    //보급량 - 1초당 보급량
    int m_addMunitions = 5;
    
    //보급량 증가량 - 레벨업시 보급량 증가량
    int m_increaseAddMunitions = 1;



    //유닛 현황 - 0~5 병사 / 영웅 현황 - 6~8 영웅
    //    Unit[] m_unitArray = new Unit[Prep.maxUnitSlot + Prep.maxHeroSlot];
    //    ICard[] m_iCardArray = new ICard[Prep.maxUnitSlot + Prep.maxHeroSlot];


    List<ICard> m_iCardList = new List<ICard>();
    //무한모드 인덱스 필요

    //사용불가 인덱스
    List<string> m_notUsedCardList = new List<string>();

    //쿨타임
    //float[] m_unitDelay = new float[Prep.maxUnitSlot + Prep.maxHeroSlot + Prep.maxSkillSlot];


    Dictionary<string, float> m_cardTimeDic = new Dictionary<string, float>();


    protected int level { get { return m_level - 1; } }

    public int maxCastleHealth { get { return m_castle.maxHealth; } }
    public int nowCastleHealth { get { return m_castle.nowHealth; } }

//    public abstract IActor iActor { get; }

    protected int maxMunitions { get { return m_maxMunitions + level * m_increaseMunitions; } }
    protected int nowMunitions { get { return m_nowMunitions; } set { m_nowMunitions = value; } }

    protected int addMunitions { get { return m_addMunitions + level * m_increaseAddMunitions; } }
//    protected int increaseAddMunitions { get { return m_increaseAddMunitions; } }

//    protected Unit[] unitArray { get { return m_unitArray; }}
//    protected ICard[] iCardArray { get { return m_iCardArray; } }
    protected List<ICard> iCardList { get { return m_iCardList; } }

    protected CommanderCard commanderCard { get { return m_commanderCard; } set { m_commanderCard = value; } }

//    protected float[] unitDelay { get { return m_unitDelay; } }

    /// <summary>
    /// 카드 추가하기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected bool addCardTime(string key)
    {
        if (!m_cardTimeDic.ContainsKey(key))
        {
            m_cardTimeDic.Add(key, 0f);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 카드시간 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected float getCardTime(string key)
    {
        if (m_cardTimeDic.ContainsKey(key))
        {
            return m_cardTimeDic[key];
        }
        return -1f;
    }

    /// <summary>
    /// 카드시간 초기화하기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected bool resetCardTime(string key)
    {
        if (m_cardTimeDic.ContainsKey(key))
        {
            m_cardTimeDic[key] = 0f;
            return true;
        }
        return false;
    }


    protected GameController gameController { get { return m_gameController; } }

    //성 레벨
    //protected int castleLevel { get { return m_castleLevel; } set{m_castleLevel = value;}}

    //보급 레벨
//    protected int maxMunitionsLevel { get { return m_maxMunitionsLevel; } set { m_maxMunitionsLevel = value; } }

    //보급 충전레벨
//    protected int munitionsLevel { get { return m_munitionsLevel; } set { m_munitionsLevel = value; } }

    //초기화
    //
    
    public void setController(GameController gameController)
    {
        m_gameController = gameController;
    }

    public void setCastle(UnitActor castle, int munitionLv)
    {
        m_castle = castle;
        m_level = munitionLv;
    }

    //스킬 현황

    //명령을 내림
    //생성, 스킬, 
    public void createUnit(UnitCard unitCard, int index, MapController.TYPE_MAP_LINE typeMapLine)
    {
        //영웅이면 인덱스를 등록
        if (unitCard.unit.typeUnit == Unit.TYPE_UNIT.Hero)
            m_notUsedCardList.Add(unitCard.key);

        gameController.createUnit(unitCard, this, typeMapLine);
        nowMunitions -= unitCard.munitions;
        resetCardTime(unitCard.key);
//        unitDelay[index] = 0f;
    }


    public void skillAction(SkillCard skillCard, int index)
    {
    }


    //뷰어 보임
    //군수품, 체력, 업그레이드량, 병력 버튼, 스킬, 영웅
    public virtual void uiUpdate(float frameTime, UnitActorManager unitActorManager)
    {

//        m_buffCtrl.uiUpdate(m_castle, frameTime);

        //등록된 영웅 인덱스에서 키로 찾기
        for (int index = m_notUsedCardList.Count - 1; index >= 0; index--)
        {
            //Debug.LogWarning("index : " + unitArray[m_heroIndexList[index]].key);
            //찾지 못했으면
            if (!unitActorManager.isUsedUnit(m_notUsedCardList[index], this))
            {
                //키 제거
                m_notUsedCardList.Remove(m_notUsedCardList[index]);
            }
        }



        foreach (ICard card in iCardList)
        {
            //쿨타임 사용
            if (!Prep.isNotUnitCoolTime)
            {

                if (card is UnitCard)
                {
                    if (((UnitCard)card).typeUnit == Unit.TYPE_UNIT.Hero)
                    {
                        //사용불가 키가 있는지 확인 - 없으면 쿨타임 증가
                        if (!isNotUsedCard(card.key))
                        {
//                            Debug.LogWarning("key : " + card.key);
                            //유닛 쿨타임 사용 여부
                            m_cardTimeDic[card.key] += frameTime;
                        }
                    }
                    else
                    {
                        m_cardTimeDic[card.key] += frameTime;
                    }

                }
                //스킬
                else
                {
                    m_cardTimeDic[card.key] += frameTime;
                }

            }
            //쿨타임 미사용 - 개발자용
            else
            {
                m_cardTimeDic[card.key] = float.MaxValue;
            }
        }






        ////쿨타임 연산
        //for (int i = 0; i < Prep.maxUnitSlot + Prep.maxHeroSlot + Prep.maxSkillSlot; i++)
        //{
        //    //쿨타임 사용
        //    if (!Prep.isNotUnitCoolTime)
        //    {
                

        //        //영웅 슬롯이면
        //        if (i >= Prep.maxUnitSlot)
        //        {
        //            //사용불가 키가 있는지 확인 - 없으면 쿨타임 증가
        //            if (!isNotUsedCard(i))
        //            {
        //                //유닛 쿨타임 사용 여부
        //                unitDelay[i] += frameTime;
                        
        //            }
        //        }
        //        //유닛 슬롯
        //        else
        //        {
        //            unitDelay[i] += frameTime;
        //        }
        //    }
        //    //쿨타임 미사용
        //    else
        //    {
        //        unitDelay[i] = float.MaxValue;
        //    }
        //}

        //성 체력이 낮으면 경고창 활성화
        setCastleWarning(m_castle.nowHealthRate);
        
    }


    //protected bool heroIndexContain(int index)
    //{
    //    //Debug.Log("heroCnt : " + m_heroIndexList.Count);
    //    return m_notUsedIndexList.Contains(index);
    //}

    /// <summary>
    /// 보급하기
    /// </summary>
    public void supplyMunitions()
    {
        //항상 군수품 채우기 여부
        if(Prep.isMunitionFull) nowMunitions = maxMunitions;

        if (nowMunitions + addMunitions >= maxMunitions)
        {
            nowMunitions = maxMunitions;
        }
        else
        {
            nowMunitions += addMunitions;
        }
    }

    /// <summary>
    /// 보급 업그레이드
    /// </summary>
    protected void upgradeMunitions()
    {
        m_level++;
        m_nowMunitions = 0;
    }

    /// <summary>
    /// 보급 업그레이드
    /// </summary>
    /// <param name="level"></param>
    protected void upgradeMunitions(int level)
    {
        m_level = level;
    }


    /// <summary>
    /// 유닛에 대한 보급 여부
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    protected bool isMunitions(UnitCard unitCard)
    {
        return (nowMunitions >= unitCard.munitions);
    }

    //게임 준비
    public virtual void gameReady(){}

    //시작
    public virtual void gameStart() { }

    //게임 끝
    public virtual void gameFinish() { }

    //패배 및 승리
    public virtual void gameEnd(bool isVictory) { }

    //결과창
    public virtual void gameResult(GameController gameCtrler, bool isVictory) { }
    
    //메시지
    public virtual void setMsg(string msg){}

    //성 체력 경고
    public virtual void setCastleWarning(float rate){}
    
    //영웅 등장
    public virtual void setHeroAppear(UIController uiCtrler){}

    //컨트롤러 버프
    
    //성위치 가져오기
    public Vector2 getCastlePos()
    {
        return m_castle.transform.position;
    }

    /// <summary>
    /// 버프 삽입하기
    /// </summary>
    /// <param name="buffActor"></param>
    /// <returns></returns>
    public bool addBuff(BuffActor buffActor)
    {
        //버프 삽입하기
//        return m_buffCtrl.addBuff(buffActor);
        return false;
    }

    

    /// <summary>
    /// 컨트롤러 가져오기
    /// </summary>
    /// <param name="uiCtrler"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public UIController getController(UIController uiCtrler, TYPE_TEAM typeAlly)
    {
        if (uiCtrler is UIPlayer)
            if(typeAlly == TYPE_TEAM.Ally)
                return gameController.playerController;
            else
                return gameController.cpuController;
        else
            if (typeAlly == TYPE_TEAM.Ally)
                return gameController.cpuController;
            else
                return gameController.playerController;
    }

    /// <summary>
    /// 카드 사용 중인지 여부
    /// true 카드 사용중
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected bool isNotUsedCard(string key)
    {
        return m_notUsedCardList.Contains(key);
    }

    /// <summary>
    /// 현재 카드 타임 비율 가져오기
    /// </summary>
    /// <param name="nowTime"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    protected float rateCardTime(ICard iCard)
    {
        if(m_cardTimeDic.ContainsKey(iCard.key))
            return Mathf.Clamp(m_cardTimeDic[iCard.key] / iCard.waitTime, 0f, 1f);
        return -1f;
    }
}

