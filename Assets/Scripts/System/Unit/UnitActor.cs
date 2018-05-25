using System;
using System.Collections;
using UnityEngine;
using Spine;
using Spine.Unity;
using Defence.CommanderPackage;

//직접 행동하는 클래스
public class UnitActor : MonoBehaviour, IActor
{

    public delegate void MapUnitActorUpdateDelegate(UnitActor unitActor);
    public delegate bool RemoveUnitActorDelegate(UnitActor unitActor);

    public event MapUnitActorUpdateDelegate mapUnitActorUpdateEvent;
    public event RemoveUnitActorDelegate unitManagerRemoveUnitActorEvent;
    public event RemoveUnitActorDelegate minimapRemoveActorEvent;


//    public enum TYPE_UNITACTOR { Idle, Move, Attack, Dead, Skill0, Skill1, Skill2 }

    public enum TYPE_CONTROLLER { PLAYER, CPU }

    const float rangeOffset = 1f;

    [SerializeField]
    Transform m_hpBar;

    MapController m_mapCtrler;
    
    BulletActorManager m_bulletActorManager;

    UIController m_uiController;

    Vector2 m_defaultBar;

    //### 스트레티지 패턴
    //유닛 타입
    IUnitType m_iUnitType;

    //유닛 상태
    //IUnitState m_iUnitState;

    //유닛 찾기

    //유닛 버프 제어
    UnitBuffControl m_unitBuffControl;

    //###

    UnitCard m_unitCard;

    //적 유닛
    IActor m_enemyActor;

    //사용자
    TYPE_CONTROLLER m_typeController;

    //스켈레톤 애니메이션
    SkeletonAnimation m_skeletonAnimation;


    //사운드
    SoundPlay m_soundPlay;

//    int m_maxHealth;
    int m_nowHealth;

    //레벨 - 계정 데이터에서 가져와야 함
    int m_level;

    //행동 타이머 - 공격속도
//    float m_actionTimer = 0f;

    //스킬 쿨타임
    float[] m_skillTimer = { 0f, 0f, 0f };
    
    //이동 상태
    bool isMove = false;

    //게임 진행 상태
    bool isRun = true;
    //장비 - 계정 데이터에서 가져와야 함

    float m_effectTimer = 0f;

    //라인 위치
    Vector3 m_linePos;

    private Unit unit { get { return m_unitCard.unit; } }

    public TYPE_CONTROLLER typeController { get { return m_typeController; } }

    public IUnitType unitType { get { return m_iUnitType; } }

    public Unit.TYPE_LINE typeLine { get { return m_unitCard.typeLine; } }

    public Unit.TYPE_MOVEMENT typeMovement { get { return m_unitCard.typeMovement; } }

    public int typeRange { get { return m_unitCard.typeRange; } }

    public Unit.TYPE_UNIT typeUnit { 
        get {
            if (this.tag == Prep.defenceTag)
                return Unit.TYPE_UNIT.Defence;
            return m_unitCard.typeUnit; 
        } 
    }

    public Unit.TYPE_TARGETING typeTarget { get { return m_unitCard.typeTargeting; } }
    
    public TYPE_FORCE typeForce { get { return m_unitCard.typeForce; } }

    public string key { get { return m_unitCard.key; } }

    public string effectKey { get { return m_unitCard.effectKey; } }

//    public int maxHealth { get { return m_maxHealth; } }
    public int nowHealth { get { return m_nowHealth; } private set { m_nowHealth = value; } }

//    public BulletActorManager bulletActorManager { get { return m_bulletActorManager; } }

    public UIController uiController { get { return m_uiController; } }

    public IUnitState iUnitState { get { return m_iUnitType.iUnitState; } }
    
    /// <summary>
    /// 현재 체력을 백분율로 바꾸기
    /// </summary>
    public float nowHealthRate { get { return (float)nowHealth / (float)maxHealth; } }

    /// <summary>
    /// 사운드 플레이어
    /// </summary>
    public SoundPlay soundPlay {
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

    //public Vector2 linePos { get { return m_linePos; } }
    /// <summary>
    /// 공격력
    /// </summary>
    public int attack
    {
        get
        {
            return (int)m_unitBuffControl.valueCalculate((float)(m_unitCard.attack), new AttackStateControl(), this, AddConstraint.TYPE_BUFF_CONSTRAINT.Attack); 
        }
    }

    /// <summary>
    /// 공격속도
    /// </summary>
    public float attackSpeed { 
        get 
        {
            return m_unitBuffControl.valueCalculate((float)(m_unitCard.attackSpeed), new AttackSpeedStateControl(), this, AddConstraint.TYPE_BUFF_CONSTRAINT.Attack);
        } 
    }

    /// <summary>
    /// 체력
    /// </summary>
    public int maxHealth { 
        get 
        {
            return (int)m_unitBuffControl.valueCalculate((float)(m_unitCard.health), new MaxHealthStateControl(), this);
        } 
    }

    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed { 
        get 
        {

            return m_unitBuffControl.valueCalculate((float)(m_unitCard.moveSpeed), new MoveSpeedStateControl(), this, AddConstraint.TYPE_BUFF_CONSTRAINT.Move) * Prep.movementOffset;
        } 
    }

    /// <summary>
    /// 사정거리
    /// </summary>
    public float range { 
        get 
        {
            return m_unitBuffControl.valueCalculate(m_unitCard.range, new RangeStateControl(), this, AddConstraint.TYPE_BUFF_CONSTRAINT.Attack) * rangeOffset;
        }
    }

    /// <summary>
    /// 시야
    /// </summary>
    public float sight
    {
        get {
            if (range > 5f) return range;
            return 5f; 
        }
    }


    /// <summary>
    /// 정해진 방어력
    /// </summary>
    public float defence
    {
        get
        {
            return m_unitBuffControl.valueCalculate(1f, new DefenceStateControl(), this, AddConstraint.TYPE_BUFF_CONSTRAINT.Hit);
        }
    }

    /// <summary>
    /// 공격시 방어력에 대하여 연산값 가져오기
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    //float getAttackValue(int attack){
    //    return m_unitBuffControl.valueCalculate(attack, new DefenceStateControl(), this, Buff.TYPE_BUFF_CONSTRAINT.HIT);
    //}


    /// <summary>
    /// 소모 군수품
    /// </summary>
    public int munitions { get { return m_unitCard.munitions; } }
        
    /// <summary>
    /// 레벨
    /// </summary>
    public int level { get { return m_unitCard.level; } }


    /// <summary>
    /// 방어력
    /// </summary>
    int getCalculateAttack(IActor iActor, int attack)
    {
        return (int)m_unitBuffControl.valueCalculate((float)attack, new DefenceStateControl(), iActor, AddConstraint.TYPE_BUFF_CONSTRAINT.Hit);
    }

    /// <summary>
    /// 레이어에 따른 위치 가져오기
    /// </summary>
    /// <param name="layer">공격자 레이어</param>
    /// <returns></returns>
    public Vector2 getPosition(int layer)
    {
        if(m_mapCtrler == null) return transform.position;

        if (layer >= 20)
            layer -= 3;

        layer -= 17;


        if (uiController is UIPlayer)
        {
            return m_mapCtrler.playerCastleController.getLine((MapController.TYPE_MAP_LINE)layer);
        }
        else
        {
            return m_mapCtrler.cpuCastleController.getLine((MapController.TYPE_MAP_LINE)layer);
        }

    }


    /// <summary>
    /// 유닛 행동자에게 반납하는 델리게이트
    /// </summary>
    /// <param name="removeUnitActorDel"></param>
    //public void setDelegate(RemoveUnitActorDelegate removeUnitActorDel)
    //{
    //    unitManagerRemoveUnitActorEvent = removeUnitActorDel;
    //}

    #region  ######################################### 유닛 셋 #########################################


    public void setBuildingUnit(UnitCard unitCard, UIController uiController, MapController mapCtrler)
    {
        m_unitCard = unitCard;
        //유닛타입
        m_iUnitType = new BuildingUnitType();

        //유닛 찾기
        //m_iUnitSearch = new UnderUnitSearch();

        m_uiController = uiController;

        m_mapCtrler = mapCtrler;

        Vector2 flip = transform.localScale;

        if (uiController is UIPlayer)
        {
            m_typeController = TYPE_CONTROLLER.PLAYER;
            flip.x *= -1f;
        }
        else
        {
            m_typeController = TYPE_CONTROLLER.CPU;
        }

        m_nowHealth = maxHealth;

        Transform spineObj = transform.Find("SpineObject");
        m_skeletonAnimation = (SkeletonAnimation)Instantiate(m_unitCard.unit.skeletonAnimation);
        m_skeletonAnimation.transform.SetParent(spineObj);
        m_skeletonAnimation.GetComponent<MeshRenderer>().sortingLayerName = "Castle";
        m_skeletonAnimation.transform.localPosition = Vector3.zero;

        transform.localScale = flip;

        //        m_skeletonAnimation.transform.localScale = flip;

        //이벤트 발생
        //        m_skeletonAnimation.state.Event += HandleEvent;




        //        GetComponent<Collider2D>().enabled = false;
        //대기 상태로 전환
        gameObject.SetActive(true);
        m_hpBar.gameObject.SetActive(false);
        initSkill();

        setSkeletonBuildingColor(uiController);


    }
    public void setDefenceUnit(UnitCard unitCard, UIController uiController)
    {
        m_unitCard = unitCard;

        //유닛타입
        m_iUnitType = new TowerUnitType();

        //유닛 찾기
        //m_iUnitSearch = new UnderUnitSearch();

        m_uiController = uiController;

        Vector2 flip = Vector2.one;

        if (uiController is UIPlayer)
        {
            m_typeController = TYPE_CONTROLLER.PLAYER;
        }
        else
        {
            m_typeController = TYPE_CONTROLLER.CPU;
            flip.x *= -1f;
        }

        m_nowHealth = maxHealth;

        m_skeletonAnimation = (SkeletonAnimation)Instantiate(m_unitCard.unit.skeletonAnimation);
        m_skeletonAnimation.transform.SetParent(transform);
        m_skeletonAnimation.GetComponent<MeshRenderer>().sortingLayerName = "Defencer";
        m_skeletonAnimation.transform.localPosition = Vector2.zero;
        m_skeletonAnimation.transform.localScale = flip;

        //이벤트 발생
        m_skeletonAnimation.state.Event += HandleEvent;




        GetComponent<Collider2D>().enabled = false;
        //대기 상태로 전환

        gameObject.SetActive(true);
        m_hpBar.gameObject.SetActive(false);

        initSkill();

        //색상 삽입하기
        setSkeletonColor(unitCard.typeUnit, uiController);

        gameObject.tag = Prep.defenceTag;

    }
    public void setUnit(UnitCard unitCard, UIController uiController, MapController.TYPE_MAP_LINE typeMapLine, Vector3 linePos)
    {
        m_unitCard = unitCard;
        m_linePos = linePos;

        //유닛타입
        m_iUnitType = new SoldierUnitType();

        //유닛 찾기
        //m_iUnitSearch = unit.getUnitSearch();

        m_uiController = uiController;


        Vector2 flip = Vector2.one;

        if (uiController is UIPlayer)
        {
            m_typeController = TYPE_CONTROLLER.PLAYER;
            //스테이지 색상
        }
        else
        {
            //커맨드 색상
            m_typeController = TYPE_CONTROLLER.CPU;
            flip.x *= -1f;
        }

        m_nowHealth = maxHealth;

        m_skeletonAnimation = (SkeletonAnimation)Instantiate(m_unitCard.unit.skeletonAnimation);
        m_skeletonAnimation.transform.SetParent(transform);
        m_skeletonAnimation.GetComponent<MeshRenderer>().sortingLayerName = getSortingLayer((int)typeMapLine);
        m_skeletonAnimation.transform.localPosition = Vector2.zero;
        m_skeletonAnimation.transform.localScale = flip;


        //시작 이벤트 발생
        m_skeletonAnimation.state.Start += StartEvent;

        //해당 이벤트 발생
        m_skeletonAnimation.state.Event += HandleEvent;

        //종료 이벤트 발생
        m_skeletonAnimation.state.Complete += CompleteEvent;

        if (typeUnit != Unit.TYPE_UNIT.Skill)
            GetComponent<Collider2D>().enabled = true;
        else
            GetComponent<Collider2D>().enabled = false;

        gameObject.SetActive(true);
        initSkill();

        //색상 삽입하기
        setSkeletonColor(unitCard.typeUnit, uiController);

        //대기 상태로 전환
        
        //지휘관 버프 삽입하기
        if (uiController is UIPlayer)
        {
            //아군 지휘관 버프
            CommanderCard commanderCard = Account.GetInstance.accUnit.getCommanderCard(Account.GetInstance.accUnit.nowCommander);

            if (commanderCard != null)
            {
                Skill skill = SkillManager.GetInstance.getSkill(commanderCard.skills[0]);

                if (skill != null)
                {
                    if (skill.typeTeam == TYPE_TEAM.All ||
                        skill.typeTeam == TYPE_TEAM.Ally)
                        skill.skillAction(this, null);
                }
            }

            //적군 지휘관 버프
            CommanderCard eCmdCard = CommanderManager.GetInstance.getCommanderCard(Account.GetInstance.accSinario.nowStage.deck.commanderKey, Account.GetInstance.accSinario.nowStage.deck.commanderLevel);

            if (eCmdCard != null)
            {
                Skill skill = SkillManager.GetInstance.getSkill(eCmdCard.skills[0]);
                Debug.Log("skill : " + skill);
                if (skill != null)
                {
                    if (skill.typeTeam == TYPE_TEAM.All ||
                        skill.typeTeam == TYPE_TEAM.Enemy)
                        skill.skillAction(this, null);
                }
            }

        }
        
        else
        {
            //적군 지휘관 버프
            CommanderCard commanderCard = CommanderManager.GetInstance.getCommanderCard(Account.GetInstance.accSinario.nowStage.deck.commanderKey, Account.GetInstance.accSinario.nowStage.deck.commanderLevel);

            if (commanderCard != null)
            {
                Skill skill = SkillManager.GetInstance.getSkill(commanderCard.skills[0]);

                if (skill != null)
                {
                    if (skill.typeTeam == TYPE_TEAM.All ||
                        skill.typeTeam == TYPE_TEAM.Ally)
                        skill.skillAction(this, null);
                }
            }

            //아군 지휘관 버프
            CommanderCard eCmdCard = Account.GetInstance.accUnit.getCommanderCard(Account.GetInstance.accUnit.nowCommander);

            if (eCmdCard != null)
            {
                Skill skill = SkillManager.GetInstance.getSkill(eCmdCard.skills[0]);

                if (skill != null)
                {
                    if (skill.typeTeam == TYPE_TEAM.All ||
                        skill.typeTeam == TYPE_TEAM.Enemy)
                        skill.skillAction(this, null);
                }
            }

        }
    }


    void setSkeletonColor(Unit.TYPE_UNIT typeUnit, UIController uiCtrler)
    {
        Spine.Skeleton skeleton = m_skeletonAnimation.skeleton;
        Color color = Prep.getForceColor(TYPE_FORCE.None);
        Stage stage = Account.GetInstance.accSinario.nowStage;

        if (uiCtrler is UIPlayer)
        {
            if (stage != null)
            {
                color = Prep.getForceColor(stage.typeForce);
            }
            else
            {
                Prep.LogWarning("", "스테이지를 찾을 수 없음", GetType());
            }
        }
        else if (uiCtrler is UICPU)
        {
            Commander commander = CommanderManager.GetInstance.getCommander(stage.deck.commanderKey);
            if (commander != null)
            {
                color = Prep.getForceColor(commander.typeForce);
            }
            else
            {
                Prep.LogWarning("", "지휘관을 찾을 수 없음", GetType());
            }
        }
        else
        {
            Prep.LogWarning("", "해당 컨트롤러를 찾을 수 없음", GetType());
        }
        


        //색상 삽입
        try
        {
            skeleton.FindSlot("LBand").SetColor(color);
            skeleton.FindSlot("RBand").SetColor(color);

            if(typeUnit == Unit.TYPE_UNIT.Hero)
                skeleton.FindSlot("Flag").SetColor(color);
        }
        catch (NullReferenceException e)
        {
            Prep.LogWarning(e.Message, "색상 키를 가져올 수 없음", GetType());
        }

    }


    void setSkeletonBuildingColor(UIController uiCtrler)
    {
        Spine.Skeleton skeleton = m_skeletonAnimation.skeleton;
        Color color = Prep.getForceColor(TYPE_FORCE.None);
        Stage stage = Account.GetInstance.accSinario.nowStage;

        if (uiCtrler is UIPlayer)
        {
            if (stage != null)
            {
                color = Prep.getForceColor(stage.typeForce);
            }
            else
            {
                Prep.LogWarning("", "스테이지를 찾을 수 없음", GetType());
            }
        }
        else if (uiCtrler is UICPU)
        {
            Commander commander = CommanderManager.GetInstance.getCommander(stage.deck.commanderKey);
            if (commander != null)
            {
                color = Prep.getForceColor(commander.typeForce);
            }
            else
            {
                Prep.LogWarning("", "지휘관을 찾을 수 없음", GetType());
            }
        }
        else
        {
            Prep.LogWarning("", "해당 컨트롤러를 찾을 수 없음", GetType());
        }



        //색상 삽입
        try
        {
            skeleton.FindSlot("Cap").SetColor(color);
            skeleton.FindSlot("Flag").SetColor(color);
        }
        catch (NullReferenceException e)
        {
            Prep.LogWarning(e.Message, "색상 키를 가져올 수 없음", GetType());
        }

    }

    public void setBuildingUnit(Unit unit, UIController uiController, MapController mapCtrler)
    {
        setBuildingUnit(new UnitCard(unit), uiController, mapCtrler);
    }

    /// <summary>
    /// 방어병력 설정하기
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="uiController"></param>
    public void setDefenceUnit(Unit unit, UIController uiController, int level)
    {
        setDefenceUnit(new UnitCard(unit, level), uiController);
    }
    
    /// <summary>
    /// 유닛 삽입하기
    /// </summary>
    /// <param name="unit"></param>
    public void setUnit(Unit unit, UIController uiController, MapController.TYPE_MAP_LINE typeMapLine, Vector2 linePos)
    {
        setUnit(new UnitCard(unit), uiController, typeMapLine, linePos);
    }

    #endregion ##################################################################################



    #region ####################################### 스파인 이벤트 ###########################################

    //시작할 때마다 공격속도 또는 이동속도 변경
    //시작 이벤트 발생
    void StartEvent(Spine.TrackEntry entry)
    {
        m_iUnitType.startAction(this);
        setAnimationSpeed(entry);
    }


    /// <summary>
    /// 이벤트 발생
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="e"></param>
    void HandleEvent(Spine.TrackEntry entry, Spine.Event e)
    {

        //공격하면
        if (e.String == "Attack")
        {
            //공격 타입
            //유닛타입에 대한 공격 실행 후 적 판별
            m_enemyActor = m_iUnitType.attackAction(this, m_enemyActor);
        }

        else if (e.String == "Skill")
        {
            //스킬 데이터 발동
            //=
            m_enemyActor = m_iUnitType.skillAction (this, m_enemyActor);
        }
    }

    void CompleteEvent(Spine.TrackEntry entry)
    {

        //Debug.Log("Entry : " + entry.ToString());

        //if (entry.ToString() == "Skill0" ||
        //    entry.ToString() == "Skill1" ||
        //    entry.ToString() == "Skill2"
        //    )
        //{
        //    //스킬 종료 이벤트 실행
        //}
        m_iUnitType.completeAction(this);
        setAnimationSpeed(entry);
    }


    #endregion ##################################### 스파인 이벤트 #############################################

    /// <summary>
    /// 렌더러 레이어 가져오기
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    string getSortingLayer(int pos)
    {
        if (pos < 0)
            return "Castle";
        return Prep.sortingLayerNames[pos];
    }


    void setAnimationSpeed(Spine.TrackEntry entry)
    {
        switch (entry.ToString())
        {
            case "Idle":
                goto case "Move";
            case "Move":
                entry.TimeScale = moveSpeed * 0.5f;
                break;
            case "Dead":
                entry.TimeScale = 1f;
                break;
            case "Attack":
                entry.TimeScale = attackSpeed;
                break;
            case "Skill0":
                goto case "Attack";
            case "Skill1":
                goto case "Attack";
            case "Skill2":
                goto case "Attack";
        }
    }


    /// <summary>
    /// 초기화하기
    /// </summary>
    public void clear()
    {
//        m_unit = null;

        m_nowHealth = 0;

        gameObject.layer = 0;

//        m_typeUnitActor = TYPE_UNITACTOR.Idle;
        if(m_iUnitType != null)
            m_iUnitType.clear();

        transform.position = new Vector3(0f, 10f, 0f);

        if(m_skeletonAnimation != null)
            Destroy(m_skeletonAnimation);


        gameObject.SetActive(false);
    }

    //초기화
    void Awake()
    {
        m_unitBuffControl = new UnitBuffControl();
        m_unitBuffControl.rateHealthCalculate += rateHealthCalculate;
        m_bulletActorManager = GameObject.Find("Manager@BulletActor").GetComponent<BulletActorManager>();
        m_defaultBar = new Vector2(m_hpBar.transform.localScale.x, m_hpBar.transform.localScale.y);
        m_soundPlay = GetComponent<SoundPlay>();
        if (m_soundPlay == null) m_soundPlay = gameObject.AddComponent<SoundPlay>();
        clear();
    }


    /// <summary>
    /// 행동
    /// </summary>
    public void uiUpdate()
    {

        if (isRun)
        {
            //중력이 있으면
            if (GetComponent<Rigidbody2D>().gravityScale > 0f)
            {
                //바닥까지 닿는지 확인
                if (transform.position.y < m_linePos.y)
                {
                    //바닥까지 닿으면

                    //중력 없애기
                    GetComponent<Rigidbody2D>().gravityScale = 0f;
                    transform.position = new Vector2(transform.position.x, m_linePos.y);
                }
            }

            //오른쪽 최대값
            //왼쪽 최대값
            //바닥 최대값

            //        UnityEngine.Debug.LogWarning("data : " + name);

            m_unitBuffControl.uiUpdate(this, Prep.frameTime);

            if (!isDead())
            {


                //적 가져오기 - 각 유닛마다 적 가져오는 알고리즘 필요
                if (!(m_iUnitType.iUnitState is SkillUnitState))
                {
                //    //적 가져오기 - 시야, 사정거리 판정 필요
                //    //사정거리 - 직접 공격할 수 있는 거리
                //    //시야 - 적을 발견한 거리 
                //m_enemyActor = m_iUnitType.iUnitSearch.searchUnitActor(this, m_enemyActor, TYPE_ALLY.ENEMY);

                    m_enemyActor = m_iUnitType.iUnitSearch.searchUnitActor(this, m_enemyActor, m_iUnitType.iUnitState, TYPE_TEAM.Enemy);

                //    //무조건 양수로 시작
                //    //적이 오른쪽
                //    //적이 왼쪽
                }


                for (int i = 0; i < m_skillTimer.Length; i++)
                {
                    m_skillTimer[i] += Prep.frameTime;
                }

                //현재 상태 가져오기
                //            m_iUnitType.updateState(this, m_enemyUnitActor);

                //미니맵으로 연동하기


                //애니메이션 실행
                //            skeletonAnimation();

                m_iUnitType.updateState(this, m_enemyActor);

                //유닛 위치 미니맵 업데이트 
                if (typeUnit == Unit.TYPE_UNIT.Hero || typeUnit == Unit.TYPE_UNIT.Soldier)
                    mapUnitActorUpdateEvent(this);

                skeletonAnimation();
            }
        }

    }

    /// <summary>
    /// 행동 멈추기
    /// </summary>
    public void stopUnit()
    {
        isRun = false;
        if (!isDead())
        {
            m_iUnitType.setDefaultState();
            //m_iUnitType.updateState(this, null);
            skeletonAnimation();
        }
    }


    #region ######################################## 애니메이션 ##########################################

    /// <summary>
    /// 애니메이션 실행
    /// </summary>
    void skeletonAnimation()
    {
        if (m_unitCard != null)
        {
            //현재 상태 이름 가져오기
            string unitStateName = m_iUnitType.iUnitState.ToString().Replace("UnitState", "");

            //현재 유닛 상태이름과 같지 않으면 새 애니메이션 실행
            if (nowRunAnimationName() != unitStateName)
                m_iUnitType.iUnitState.setAnimation(this, setAnimation);

        }

    }

    /// <summary>
    /// 애니메이션 유무 판단
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool isSkeletonAnimation(string name)
    {
        return (m_skeletonAnimation.skeleton.data.FindAnimation(name) != null ) ? true : false;
    }

    /// <summary>
    /// 현재 실행중인 애니메이션 이름 가져오기
    /// </summary>
    /// <returns></returns>
    string nowRunAnimationName()
    {



        if (m_skeletonAnimation.state.Tracks.Count > 0){
//            Debug.Log("FindAnime : " + m_skeletonAnimation.state.GetCurrent(0).animation.name);
            string name = m_skeletonAnimation.state.GetCurrent(0).animation.name;
            if (name == "Skill0" || name == "Skill1" || name == "Skill2")
                name = "Skill";
            return name;
        }
        return "";
    }

    /// <summary>
    /// 애니메이션 실행
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isLoop"></param>
    /// <returns></returns>
    TrackEntry setAnimation(string name, bool isLoop)
    {
        if (isSkeletonAnimation(name))
        {
//            Debug.Log("Animation : " + name);
            return m_skeletonAnimation.state.SetAnimation(0, name, isLoop);
        }
        return null;
    }

    /// <summary>
    /// 버프 삽입하기
    /// </summary>
    /// <param name="buffActor"></param>
    /// <param name="useUnitActor"></param>
    /// <returns></returns>
    public bool addBuff(BuffActor buffActor, UnitActor useUnitActor)
    {
        return m_unitBuffControl.addBuff(buffActor, useUnitActor);
    }


    /// <summary>
    /// 등록된 버프 가져오기
    /// </summary>
    /// <param name="buffActor"></param>
    /// <returns></returns>
    public BuffActor getBuff(BuffActor buffActor)
    {
        return m_unitBuffControl.getBuff(buffActor);
    }
    

    /// <summary>
    /// 비율에 따른 체력 계산
    /// </summary>
    /// <param name="preMaxHealth">전 최대 체력</param>
    /// <returns>현재 최대 체력</returns>
    int rateHealthCalculate(int preMaxHealth)
    {
        float rate = ((float)maxHealth) / ((float)preMaxHealth);
        nowHealth = (int)((float)nowHealth * rate);
        Debug.LogError("nowHealth : " + rate + " " + nowHealth + " " + maxHealth + " " + preMaxHealth);
        return maxHealth;
    }


//    void OnDrawGizmos()
//    {
//        if (m_unit != null)
//        {
//            Debug.Log("gizmos");
//            Gizmos.color = Color.blue;
////            Gizmos.DrawCube(transform.position, Vector3.one);
//            Gizmos.DrawRay(transform.position, Vector2.right);// * m_unit.range);
//        }

    //    }


    #endregion ######################################## 애니메이션 ##########################################




    public void addHealth(int health)
    {

        Debug.Log("nowHealth : " + nowHealth + " " + health);

        health = -health;

        if (nowHealth + health >= maxHealth)
        {
            m_nowHealth = maxHealth;
        }
        else
        {
            m_nowHealth += health;
        }

        viewHPBar();


    }


    /// <summary>
    /// 피격하기
    /// 근접공격
    /// </summary>
    /// <param name="iActor"></param>
    /// <returns></returns>
    //bool hitUnit(IActor iActor)
    //{
    //    return hitActor(iActor, iActor.attack);
    //}


    /// <summary>
    /// 피격하기
    /// 피격 조건 없음
    /// </summary>
    /// <param name="attack"></param>
    /// <returns>false = 피격됨, true = 피격되지 않음</returns>
    public bool hitUnit(int attack)
    {


        if (nowHealth - attack < 0)
            nowHealth = 0;
        else
            nowHealth -= attack;
//        nowHealth -= attack;



        viewHPBar();

        if (isDead())
        {

            //영웅이면 메시지
            if (m_unitCard.typeUnit == Unit.TYPE_UNIT.Hero)
            {
                //부상 메시지 출력
                Debug.Log(m_unitCard.name + " 영웅이 부상을 당했습니다.");
            }
            //사망 상태로 전환
            m_iUnitType.setDeadState();

            //사망시 미니맵 반납
            if (minimapRemoveActorEvent != null) 
                minimapRemoveActorEvent(this);

            //사망 코루틴 실행
            StartCoroutine(deadCoroutine());
        }


        return true;
    }

    /// <summary>
    /// 사망여부 판정
    /// </summary>
    /// <returns>true : 사망</returns>
    public bool isDead()
    {
        if (nowHealth <= 0)
            return true;
        return false;
    }

    
    /// <summary>
    /// 피격하기
    /// 스킬 공격
    /// </summary>
    /// <param name="iActor">행동자 : 유닛, 탄환</param>
    /// <param name="attack">공격력</param>
    /// <returns>피격여부 - true 피격 - false 미피격</returns>
    public bool hitActor(IActor iActor, int attack)
    {
        //반환을 Enum형으로 제작하여 피격이 어떻게 되었는지 필요

        if (m_unitBuffControl.isConstraint(new InvisibleStateControl(), iActor, AddConstraint.TYPE_BUFF_CONSTRAINT.Hit))
        {
            //제약조건 발동시 피격되지 않음
            Debug.LogWarning("무적");
            return false;
        }

        //회피

//        Debug.LogError("Defence : " + defence);

        attack = getCalculateAttack(iActor, attack);

        Debug.Log("피격 : " + attack + " " + m_unitCard.name);

        return hitUnit(attack);
    }


    public bool createEffect(string key, Vector2 pos)
    {
        EffectActorManager effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
        if (effectActorManager != null)
        {
            effectActorManager.createActor(key + "Particle", pos, typeController, isFlip);
            return true;
        }
        return false;
    }


    void viewHPBar()
    {
        if (m_hpBar != null)
        {
            float sizeX = m_defaultBar.x * ((float)nowHealth / (float)maxHealth);

            sizeX = Mathf.Clamp(sizeX, 0f, m_defaultBar.x);

            m_hpBar.transform.localScale = new Vector3(sizeX, m_defaultBar.y);
        }
    }


    //사망 처리
    //코루틴 실행 후 오브젝트 반납
    IEnumerator deadCoroutine()
    {
        //사망 애니메이션 실행
        //애니메이션 종료되면 사라짐

        GetComponent<Collider2D>().enabled = false;
        skeletonAnimation();
        yield return new WaitForSeconds(1f);
                
        //매니저한테 반납 - 내부에서 청소
        if (unitManagerRemoveUnitActorEvent != null)
        {
            removeActor();
        }


    }


    public void removeActor()
    {
        transform.localScale = Vector2.one;
        unitManagerRemoveUnitActorEvent(this);
    }


    /// <summary>
    /// 스킬 초기화 하기
    /// </summary>
    public void initSkill() 
    {
        for(int i = 0; i < m_unitCard.unit.skillArray.Length; i++){

//            Debug.LogError("Skill : " + m_unit.skillArray[i] + " " + m_unit.name); 
            if (m_unitCard.unit.skillArray[i] != null)
            {
                if (m_unitCard.unit.skillArray[i].typeSkillPlay == Skill.TYPE_SKILL_ACTIVE.Passive)
                {
                    Debug.LogError("SkillPassive : " + m_unitCard.unit.skillArray[i].name);
                    m_unitCard.unit.skillArray[i].skillAction(this, null);
                }
            }
        }
    }


    /// <summary>
    /// 스킬 활성화 여부
    /// </summary>
    /// <returns>스킬 인덱스 : -1은 미사용</returns>
    public int isSkillActive(IUnitState iUnitState)
    {
        //스킬 미사용 상태가 아니면

        if (!isConstraint(new NotSkillStateControl(), this, AddConstraint.TYPE_BUFF_CONSTRAINT.Attack))
        {
            for (int i = 0; i < m_unitCard.unit.skillArray.Length; i++)
            {
                //스킬이 있고
                if (m_unitCard.unit.skillArray[i] != null)
                {
                    //현재 상태에 맞는 스킬이고
                    if ((iUnitState.getTypeSkillActive() | m_unitCard.unit.skillArray[i].typeSkillPlay) == m_unitCard.unit.skillArray[i].typeSkillPlay)
                    {
                        //쿨타임에 도달했으면
                        if (m_unitCard.unit.skillArray[i].isSkillAction(m_skillTimer[i]))
                        {
                            //쿨타임 초기화 및 스킬 발동
                            m_skillTimer[i] = 0f;
                            return i;
                        }
                    }
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// 스킬 이름 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string getSkillKey(int index)
    {
        if (index >= 0 && index < m_unitCard.unit.skillArray.Length)
        {
            return m_unitCard.unit.skillArray[index].key;
        }
        Prep.LogWarning(index.ToString(), " 스킬 인덱스 범위를 넘어섬", GetType());
        return string.Empty;
    }


    public IActor skillAction(int index, IActor targetActor)
    {
        return m_unitCard.unit.skillArray[index].skillAction(this, targetActor);
    }


    /// <summary>
    /// 참, 거짓 값 가져오기
    /// </summary>
    /// <param name="iStateControl"></param>
    /// <param name="iActor"></param>
    /// <param name="typeBuffConstraint"></param>
    /// <returns></returns>
    public bool isConstraint(IStateControl iStateControl, IActor iActor, AddConstraint.TYPE_BUFF_CONSTRAINT typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.None)
    {
        return m_unitBuffControl.isConstraint(iStateControl, iActor, typeBuffConstraint);
    }


    public int layer
    {
        get { return gameObject.layer; }
    }
    
    public void setPosition(Vector2 pos)
    {
        transform.position = pos;
    }




    public bool isFlip
    {
        get
        {
            if(m_skeletonAnimation != null)
                return (transform.localScale.x * m_skeletonAnimation.transform.localScale.x > 0) ? false : true;
            return false;
        }
    }

}

