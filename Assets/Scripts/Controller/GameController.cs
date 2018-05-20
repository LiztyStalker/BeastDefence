using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    //사용자
    
    [SerializeField]
    UIController m_playerController;

    [SerializeField]
    UIController m_cpuController;

    [SerializeField]
    MapController m_mapController;
    //유닛 관리자
//    [SerializeField]
    UnitActorManager m_unitActorManager;

//    [SerializeField]
    BulletActorManager m_bulletActorManager;

    //플레이 시간
    TimeSpan m_gameTime = new TimeSpan();
    
    Stage m_stage;

    SoundPlay m_soundPlay;
       

    //위치
    bool m_isRun = false;
    bool m_isPause = false;
    bool m_isVictory = false;
    
    bool m_isContents = false;


    public MapController mapCtrler { get { return m_mapController; } }
    public UIController playerController { get { return m_playerController; } }
    public UIController cpuController { get { return m_cpuController; } }
    public UnitActorManager unitActorManager { get { return m_unitActorManager; } }
    public TimeSpan gameTime { get { return m_gameTime; } }

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

        m_unitActorManager = ActorManager.GetInstance.getActorManager(typeof(UnitActorManager)) as UnitActorManager;
        m_bulletActorManager = ActorManager.GetInstance.getActorManager(typeof(BulletActorManager)) as BulletActorManager;


        m_bulletActorManager.setController(this);
        playerController.setController(this);
        cpuController.setController(this);

        //맵 삽입
        m_stage = Account.GetInstance.accSinario.nowStage;

        if (m_stage != null)
        {
            Map map = MapManager.GetInstance.getMap(m_stage.mapKey);

            m_mapController.setMap(map, m_stage.typeMapSize);

            soundPlay.audioPlay("BGM" + map.key, TYPE_SOUND.BGM);
//            ((UIPlayer)playerController).eve
            
        }
        //테스트 모드
        else
        {
            m_stage = new Stage();
            m_mapController.setMap(null, m_stage.typeMapSize);
        }

        //성, 포탑, 방벽 배치
        //데이터를 가져와야함
        unitActorManager.initField(m_mapController, m_playerController, m_cpuController, m_stage);

        StartCoroutine(runCoroutine());


    }


    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 400), "Admin Menu");

        Prep.isInfiniteSkillRate = GUI.Toggle(new Rect(20, 40, 200, 20), Prep.isInfiniteSkillRate, "발동확률 무한");
        Prep.isNotEnemyCreate = GUI.Toggle(new Rect(20, 60, 200, 20), Prep.isNotEnemyCreate, "적 미생성");
        Prep.isOneLine = GUI.Toggle(new Rect(20, 80, 200, 20), Prep.isOneLine, "한 라인");
        Prep.isNotUnitCoolTime = GUI.Toggle(new Rect(20, 100, 200, 20), Prep.isNotUnitCoolTime, "유닛쿨타임");
        Prep.isMunitionFull = GUI.Toggle(new Rect(20, 120, 200, 20), Prep.isMunitionFull, "군수품 채우기");


        //캐릭터 행동
        //캐릭터 행동
        //Prep.isUnitControl = GUI.Toggle(new Rect(20, 80, 60, 20), Prep.isUnitControl, "유닛 직접 제어");



        //GUI.Button(new Rect(20, 80, 60, 20), "대기");
        //GUI.Button(new Rect(20, 80, 60, 20), "이동");
        //GUI.Button(new Rect(20, 80, 60, 20), "공격");
        //GUI.Button(new Rect(20, 80, 60, 20), "스킬0");
        //GUI.Button(new Rect(20, 80, 60, 20), "스킬1");
        //GUI.Button(new Rect(20, 80, 60, 20), "스킬2");
        //GUI.Button(new Rect(20, 80, 60, 20), "사망");



        //상태이상 삽입 및 취소
        //Prep.isStateControl = GUI.Toggle(new Rect(20, 120, 100, 20), Prep.isStateControl, "상태이상");

        if (GUI.Button(new Rect(20, 140, 100, 20), "공격력 10% 증가"))
            activeBuff("AttackUp");

        if (GUI.Button(new Rect(20, 160, 100, 20), "공속 10% 증가"))
            activeBuff("AttackSpeedUp");

        if (GUI.Button(new Rect(20, 180, 100, 20), "이속 10% 증가"))
            activeBuff("MoveSpeedUp");

        if (GUI.Button(new Rect(20, 200, 100, 20), "체력 10% 증가"))
            activeBuff("MaxHealthUp");

        if (GUI.Button(new Rect(20, 220, 100, 20), "체력 10% 회복"))
            activeBuff("RecoveryHealthUp");

        if (GUI.Button(new Rect(20, 240, 100, 20), "방어 10% 증가"))
            activeBuff("DefenceUp");



        if (GUI.Button(new Rect(140, 140, 100, 20), "공격력 10% 감소"))
            activeBuff("AttackDn");

        if (GUI.Button(new Rect(140, 160, 100, 20), "공속 10% 감소"))
            activeBuff("AttackSpeedDn");

        if (GUI.Button(new Rect(140, 180, 100, 20), "이속 10% 감소"))
            activeBuff("MoveSpeedDn");

        if (GUI.Button(new Rect(140, 200, 100, 20), "체력 10% 감소"))
            activeBuff("MaxHealthDn");

        if (GUI.Button(new Rect(140, 220, 100, 20), "체력 10% 하락"))
            activeBuff("RecoveryHealthDn");

        if (GUI.Button(new Rect(140, 240, 100, 20), "방어 10% 감소"))
            activeBuff("DefenceDn");



        if (GUI.Button(new Rect(20, 260, 100, 20), "이동만 삽입"))
            activeBuff("OnlyMove");

        if (GUI.Button(new Rect(20, 280, 100, 20), "공격불가 삽입"))
            activeBuff("NotAttack");

        if (GUI.Button(new Rect(20, 300, 100, 20), "무적 삽입"))
            activeBuff("InvisibleState");

        if (GUI.Button(new Rect(20, 320, 100, 20), "스킬불가 삽입"))
            activeBuff("NotSkill");

        if (GUI.Button(new Rect(20, 340, 100, 20), "이동불가 삽입"))
            activeBuff("NotMove");



        if (GUI.Button(new Rect(140, 260, 100, 20), "이동만 해제"))
            removeBuff("OnlyMove");

        if (GUI.Button(new Rect(140, 280, 100, 20), "공격불가 해제"))
            removeBuff("NotAttack");

        if (GUI.Button(new Rect(140, 300, 100, 20), "무적 해제"))
            removeBuff("InvisibleState");

        if (GUI.Button(new Rect(140, 320, 100, 20), "스킬불가 해제"))
            removeBuff("NotSkill");

        if (GUI.Button(new Rect(140, 340, 100, 20), "이동불가 해제"))
            removeBuff("NotMove");

    }

    void activeBuff(string key)
    {
        BuffActor buffActor = getBuffActor(key);
        if (buffActor != null)
            unitActorManager.addBuff(buffActor, UnitActor.TYPE_CONTROLLER.PLAYER);
        else
            Debug.LogError("버프 삽입 불가");
    }

    void removeBuff(string key)
    {
        BuffActor buffActor = getBuffActor(key);
        if (buffActor != null)
            unitActorManager.allRemoveBuff(buffActor);
        else
            Debug.LogError("버프 삭제 불가");
    }

    BuffActor getBuffActor(string key)
    {
        BuffActor buffActor = new BuffActor();
        Buff buff = BuffManager.GetInstance.getBuff(key);
        
        if (buff == null) return null;

        buffActor.setBuff(buff);
        return buffActor;
    }

    //각종 입력을 받음

    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitCard">생성할 유닛</param>
    /// <param name="uiController">생성한 컨트롤러</param>
    /// <param name="pos">위치</param>
    public void createUnit(UnitCard unitCard, UIController uiController, MapController.TYPE_MAP_LINE typeMapLine)
    {
        //영웅이면 메시지
        if (unitCard.typeUnit == Unit.TYPE_UNIT.Hero)
        {
            playerController.setHeroAppear(uiController);
            cpuController.setHeroAppear(uiController);
        }

        unitActorManager.createActor(unitCard, uiController, typeMapLine);
    }

    public void contentsCallBack()
    {
        m_isContents = false;
    }

    IEnumerator contentsCoroutine(Contents.TYPE_CONTENTS_EVENT typeEvent)
    {
//        Debug.Log("contents");
        //현재 자막 시작
        m_isContents = ((UIPlayer)playerController).setContents(typeEvent);

        //콜백 받아야 함
        //
        while (m_isContents)
        {
//            Debug.Log("contents");
            yield return null;
        }
        yield return null;
    }

    IEnumerator readyCoroutine()
    {
        Debug.Log("준비");
        m_playerController.gameReady();
        yield return new WaitForSeconds(1f);

        Debug.Log("시작");
        m_playerController.gameStart();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator runCoroutine()
    {
        //자막
//        uiUpdate(0f);


//        playerController.gameResult(this);

//        yield break;

        yield return StartCoroutine(contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.Start));
        //준비 및 시작
        yield return StartCoroutine(readyCoroutine());
        m_isRun = true;

        float timer = 0f;

        while (m_isRun)
        {
            //시간추가
            timer += Prep.frameTime;


            //보급품 추가
            if (timer >= 1f)
            {

                m_gameTime += TimeSpan.FromSeconds(1);
//                Debug.Log("add : " + m_gameTime);
                playerController.supplyMunitions();
                cpuController.supplyMunitions();
                timer = 0f;
            }

            //제한시간 9분이 되면 총공격 - 
            //제한시간 10분이 되면 게임 종료 - 패배로 간주
            if(Prep.maxGameTime < gameTime)
                m_isRun = false;


            //업데이트
            uiUpdate(Prep.frameTime);
            //playerController.uiUpdate(Prep.frameTime, unitActorManager);
            //cpuController.uiUpdate(Prep.frameTime, unitActorManager);
            //m_bulletActorManager.uiUpdate();
            //unitActorManager.uiUpdate(m_isRun);
                                    
            //조건
            if (isGameEnd())
            {
                m_isRun = false;
            }

            yield return new WaitForSeconds(Prep.frameTime);
        }


        //모든 유닛 멈추기
        unitActorManager.stopUnits();

        //게임 마침
        gameFinish();

        //파괴된 성으로 카메라 이동
        yield return StartCoroutine(endAnimationCoroutine());

        //승리시 종료 자막
        if (m_isVictory)
        {
            //종료 자막
            yield return StartCoroutine(contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.End));

        }

        //패배 또는 승리
        yield return StartCoroutine(endCoroutine());

        //결과창 보이기
        //
        
        playerController.gameResult(this, m_isVictory);

    }


    void gameFinish()
    {
        playerController.gameFinish();
        cpuController.gameFinish();
    }

    void uiUpdate(float frameTime)
    {
        playerController.uiUpdate(frameTime, unitActorManager);
        cpuController.uiUpdate(frameTime, unitActorManager);
        m_bulletActorManager.uiUpdate();
        unitActorManager.uiUpdate(m_isRun);
    }


    IEnumerator endAnimationCoroutine()
    {

        //성 파괴 또는 제한시간 버티기

        Vector3 lerpPos = Vector3.zero;

        if (m_cpuController.nowCastleHealth <= 0)
        {
            //cpu 성으로 이동
            lerpPos = m_cpuController.getCastlePos();
            lerpPos.x += 2f;
            //플레이어 승리
            m_isVictory = true;
        }
        else if (m_playerController.nowCastleHealth <= 0)
        {
            //player 성으로 이동
            lerpPos = m_playerController.getCastlePos();
            lerpPos.x -= 2f;
            //플레이어 패배
            m_isVictory = false;
        }
        //성 폭발 애니메이션

        lerpPos.z = Camera.main.transform.position.z;

        float time = 3f;

        while (time >= 0f)
        {
            Debug.Log("lerp");
            //카메라 러프로 이동
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, lerpPos, 0.1f);
            time -= Prep.frameTime;
            yield return new WaitForSeconds(Prep.frameTime);
        }

    }

    IEnumerator endCoroutine()
    {
        m_playerController.gameEnd(m_isVictory);
        if(m_isVictory)
            soundPlay.audioPlay("BGMVictory", TYPE_SOUND.BGM);
        else
            soundPlay.audioPlay("BGMDefeat", TYPE_SOUND.BGM);
        yield return new WaitForSeconds(3f);
    }

    bool isGameEnd()
    {
        //성 체력이 0 이하이면
        if (m_cpuController.nowCastleHealth <= 0 ||
            m_playerController.nowCastleHealth <= 0)
        {
            
            return true;
        }
        return false;
    }
}
