using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitActorManager : MonoBehaviour, IActorManager
{


    enum TYPE_POS_X { TOP, MID, BOT }

    [SerializeField]
    UnitActor m_unitActor;

    [SerializeField]
    UnitActor m_buildingActor;

    //[SerializeField]
    //Transform[] m_playerPos;

    //[SerializeField]
    //Transform[] m_cpuPos;

    //[SerializeField]
    //Transform m_playerCastlePos;

    //[SerializeField]
    //Transform m_cpuCastlePos;

    MapController m_mapController;

    //사이즈만큼 벌리기
    Vector2 defencePos = new Vector2(-14f, 3.5f);

    //오브젝트 풀링 필요
    //사용된 유닛액터는 반납됨.
    //미사용된 유닛액터는 관리 후 요청이 있을 시 해당 유닛으로 초기화

    //사용 액터
    List<UnitActor> m_useActorList = new List<UnitActor>();

    //미사용 액터
    Queue<UnitActor> m_idleActorQueue = new Queue<UnitActor>();
    //

    //유닛 상태 리스트


    private List<UnitActor> useActorList { get { return m_useActorList; } }
    private Queue<UnitActor> idleActorQueue { get { return m_idleActorQueue; } }

//    int m_actorCount = 100;


//    public float topX { get { return m_playerPos[(int)TYPE_POS_X.TOP].position.x; } }
//    public float midX { get { return m_playerPos[(int)TYPE_POS_X.MID].position.x; } }
//    public float botX { get { return m_playerPos[(int)TYPE_POS_X.BOT].position.x; } }


    /// <summary>
    /// 행동
    /// </summary>
    public void uiUpdate(bool isRun)
    {
        foreach (UnitActor unitActor in useActorList)
        {
            unitActor.uiUpdate();
        }
    }

    public void stopUnits()
    {
        foreach (UnitActor unitActor in useActorList)
        {
            unitActor.stopUnit();
        }

    }


    public void initField(MapController mapController, UIController uiPlayer, UIController uiCPU, Stage stage)
    {

        idleActorQueue.Clear();
        useActorList.Clear();

        m_mapController = mapController;

        if (stage == null) stage = new Stage();

        //플레이어, 적군 성 제작
        Unit unit = UnitManager.GetInstance.getUnit("Castle");
        if(unit == null) Debug.LogError("유닛 못찾음");

        //Player 성벽 레벨
        UnitCard playerUnitCard = new UnitCard(unit, (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.CastleLv) + 1);
        
        //CPU 레벨 넣기
        UnitCard cpuUnitCard = new UnitCard(unit, stage.deck.castleLv);


        //성 레벨 정하기

        UnitActor playerCastleActor = createBuilding(playerUnitCard, uiPlayer, MapController.TYPE_MAP_LINE.Total);
        UnitActor cpuCastleActor = createBuilding(cpuUnitCard, uiCPU, MapController.TYPE_MAP_LINE.Total);


        //액터 초기화
        for (int i = 0; i < Prep.unitActorObjFoolCnt; i++)
        {
            idleActorQueue.Enqueue(createUnitActor());
        }
    




        ///주둔군 설정
        float mapSize = (float)stage.typeMapSize;
        int defenceCnt = (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.DefenceTechLv) + 1;
        int defenceLv = (int)Account.GetInstance.accDevelop.getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP.DefenceLv) + 1;

        //아군 주둔군 편제
        for (int x = 2; x >= 0; x--)
        {
            for (int y = 0; y < 3; y++)
            {
                if (defenceCnt > 0)
                {
                    UnitActor unitActor = idleActorQueue.Dequeue();
                    unitActor.setDefenceUnit(UnitManager.GetInstance.getUnit("CrossbowSoldier"), uiPlayer, defenceLv);
                    unitActor.transform.position = new Vector2(defencePos.x + (x * 1.5f) - (y * 0.75f) + mapSize, defencePos.y - (y * 0.75f));
                    //                unitActor.transform.SetParent(playerCastleActor.transform);
                    useActorList.Add(unitActor);
                    defenceCnt--;
                }
            }
        }
        
        defenceCnt = stage.deck.defenceCnt;

        //적 주둔군 편제
        for (int x = 2; x >= 0; x--)
        {
            for (int y = 0; y < 3; y++)
            {
                if (defenceCnt > 0)
                {
                    UnitActor unitActor = idleActorQueue.Dequeue();
                    unitActor.setDefenceUnit(UnitManager.GetInstance.getUnit("CrossbowSoldier"), uiCPU, stage.deck.defenceLv);
                    unitActor.transform.position = new Vector2(-defencePos.x - (x * 1.5f) + (y * 0.75f) + mapSize, defencePos.y - (y * 0.75f));
                    //                unitActor.transform.SetParent(cpuCastleActor.transform);
                    useActorList.Add(unitActor);
                    defenceCnt--;
                }
            }
        }







        //컨트롤러와 연동
        uiPlayer.setCastle(playerCastleActor, Account.GetInstance.accDevelop.getDevelopLevel("MunitionsUp"));
        uiCPU.setCastle(cpuCastleActor, stage.deck.munitionsLv);
    }


    UnitActor createBuilding(UnitCard unitCard, UIController uiController, MapController.TYPE_MAP_LINE typeMapLine)
    {
        Vector3 linePos = m_mapController.getCastlePos(uiController);// +(Vector3.forward * 1.5f);
        

        //큐가 비어있으면 새로 생성
        if (idleActorQueue.Count <= 0)
        {
            idleActorQueue.Enqueue(createUnitActor());
        }


        //위치 및 유닛 생성
        UnitActor buildingActor = (UnitActor)Instantiate(m_buildingActor);
        buildingActor.setBuildingUnit(unitCard, uiController, m_mapController);
        buildingActor.gameObject.layer = Prep.getLayer(typeMapLine, unitCard.typeMovement);
        buildingActor.transform.position = linePos;
        buildingActor.transform.localScale *= 1.75f;


        useActorList.Add(buildingActor);
        return buildingActor;
    }


    /// <summary>
    /// 유닛 행동자 삽입
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="uiController"></param>
    /// <param name="pos"></param>
    public void createActor(UnitCard unitCard, UIController uiController, MapController.TYPE_MAP_LINE typeMapLine)
    {


        Vector3 linePos = m_mapController.getLine(uiController, typeMapLine);


        createActor(unitCard, uiController, typeMapLine, linePos);

        //큐가 비어있으면 새로 생성
        //if (idleActorQueue.Count <= 0)
        //{
        //    idleActorQueue.Enqueue(createUnitActor());
        //}

        
        ////위치 및 유닛 생성
        //UnitActor unitActor = idleActorQueue.Dequeue();
        //unitActor.setUnit(unit, uiController, pos);
        //unitActor.gameObject.layer = getLayer(pos, unit.typeMovement);

        //Vector3 linePos = line.position;
        //if (unit.typeMovement == Unit.TYPE_MOVEMENT.AIR)
        //    linePos.y += 2f;
        //unitActor.transform.position = linePos;
        //useActorList.Add(unitActor);

    }


    /// <summary>
    /// 유닛 행동자 삽입
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="uiController"></param>
    /// <param name="pos"></param>
    public void createActor(UnitCard unitCard, UIController uiController, MapController.TYPE_MAP_LINE typeMapLine, Vector3 startPos)
    {
        
        //큐가 비어있으면 새로 생성
        if (idleActorQueue.Count <= 0)
        {
            idleActorQueue.Enqueue(createUnitActor());
        }

        if (unitCard.typeMovement == Unit.TYPE_MOVEMENT.Air)
            startPos.y += 2f;
        //else if (unit.typeMovement == Unit.TYPE_MOVEMENT.GROUND)
        //    startPos.y += 1f;

        //위치 및 유닛 생성
        UnitActor unitActor = idleActorQueue.Dequeue();
        unitActor.setUnit(unitCard, uiController, typeMapLine, startPos);
        unitActor.gameObject.layer = Prep.getLayer(typeMapLine, unitCard.typeMovement);


        //미니맵 동기화 및 생성하기
        m_mapController.linkUnit(unitActor, uiController);

        //유닛 시작 위치 정하기
        unitActor.transform.position = startPos;
        useActorList.Add(unitActor);
    }


    /// <summary>
    /// 유닛 행동자 반납
    /// </summary>
    /// <param name="unitActor"></param>
    /// <returns></returns>
    public bool removeActor(UnitActor unitActor)
    {
        if (useActorList.Contains(unitActor))
        {
            Debug.LogError("해제 : " + unitActor.name);
            unitActor.clear();
            useActorList.Remove(unitActor);
            idleActorQueue.Enqueue(unitActor);
            return true;
        }
        return false;
    }

    //public bool clearActor()
    //{
    //    for (int i = useActorList.Count - 1; i >= 0; i--)
    //    {
    //        useActorList[i].removeActor();
    //    }

    //    while(idleActorQueue.Count > 0)
    //    {
    //        UnitActor unitActor = idleActorQueue.Dequeue();
    //        Destroy(unitActor.gameObject);
    //    }

    //    idleActorQueue.Clear();
    //    return true;
    //}

    /// <summary>
    /// 유닛 행동자 생성
    /// </summary>
    /// <returns></returns>
    UnitActor createUnitActor()
    {
        UnitActor unitActor = Instantiate<UnitActor>(m_unitActor);
        //유닛 액터와 유닛 반환 연결하기
        unitActor.unitManagerRemoveUnitActorEvent += removeActor;

        //유닛 액터와 미니맵 연결하기
        unitActor.minimapRemoveActorEvent += m_mapController.unlinkUnit;
        unitActor.mapUnitActorUpdateEvent += m_mapController.updateImage;

        unitActor.clear();
        return unitActor;
    }

    /// <summary>
    /// 유닛을 사용중인지 여부
    /// </summary>
    /// <param name="unitKey"></param>
    /// <param name="uiController"></param>
    /// <returns></returns>
    public bool isUsedUnit(string unitKey, UIController uiController)
    {

        try
        {
            UnitActor unitActor = m_useActorList.Where(actor => actor.key == unitKey && actor.uiController == uiController).SingleOrDefault();
            return (unitActor != null);
        }
        catch (InvalidOperationException)
        {
            return false;
        }
        //유닛을 찾았으면 true
        //유닛을 찾지 못했으면 false
    }



    /// <summary>
    /// 버프 삽입하기
    /// 사용자가 같은 모든 유닛
    /// </summary>
    /// <param name="buffActor"></param>
    /// <param name="typeController"></param>
    /// <returns></returns>
    public bool addBuff(BuffActor buffActor, UnitActor.TYPE_CONTROLLER typeController)
    {
        foreach(UnitActor unitActor in m_useActorList){
            if(unitActor.typeController == typeController && unitActor.typeUnit != Unit.TYPE_UNIT.Building)
                unitActor.addBuff(buffActor, unitActor);
        }
        return true;
    }

    /// <summary>
    /// 버프 삽입하기
    /// 모든 유닛
    /// </summary>
    /// <param name="buffActor"></param>
    /// <returns></returns>
    public bool allAddBuff(BuffActor buffActor)
    {
        foreach (UnitActor unitActor in m_useActorList)
        {
            unitActor.addBuff(buffActor, unitActor);
        }
        return true;
    }

    /// <summary>
    /// 모든 버프 삭제하기
    /// </summary>
    /// <param name="buffActor"></param>
    /// <returns></returns>
    public bool allRemoveBuff(BuffActor buffActor)
    {
        foreach (UnitActor unitActor in m_useActorList)
        {
            BuffActor useBuffActor = unitActor.getBuff (buffActor);
            if (useBuffActor != null)
            {
                useBuffActor.endBuff();
            }
        }
        return true;
    }

    /// <summary>
    /// 군집된 병력 위치 가져오기 - 임시
    /// </summary>
    /// <param name="uiCtrler"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public Nullable<Vector2> searchManyUnitAssociation(UIController uiCtrler, TYPE_TEAM typeAlly)
    {

        foreach (UnitActor unitActor in useActorList)
        {
            if (typeAlly == TYPE_TEAM.Ally)
            {
                //아군일때 아군플레이어이고 유닛이면
                if (unitActor.uiController == uiCtrler && (unitActor.typeUnit == Unit.TYPE_UNIT.Soldier || unitActor.typeUnit == Unit.TYPE_UNIT.Hero))
                {
                    Debug.Log("unitActor : " + unitActor.key);
                    return unitActor.getPosition(unitActor.layer);
                }
            }
            else if(typeAlly == TYPE_TEAM.Enemy)
            {
                //적군일때 적군플레이어이고 유닛이면
                if (unitActor.uiController != uiCtrler && (unitActor.typeUnit == Unit.TYPE_UNIT.Soldier || unitActor.typeUnit == Unit.TYPE_UNIT.Hero))
                {
                    Debug.Log("unitActor : " + unitActor.key);
                    return unitActor.getPosition(unitActor.layer);
                }
            }
        }
        return null;
    }

    //void OnDisable()
    //{
    //    clearActor();
    //}

}

