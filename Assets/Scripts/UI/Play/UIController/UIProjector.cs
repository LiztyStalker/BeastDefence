using UnityEngine;
using UnityEngine.EventSystems;

public class UIProjector : MonoBehaviour, IActor
{
    [SerializeField]
    GameObject m_uiTarget;

    [SerializeField]
    Projector m_projector;

    const float projectY = -1.5f;
    const float projectZ = -5f;
    const float defaultFieldOfView = 40f;


    UIController m_uiController;
    SkillCard m_skillCard;

    EffectActorManager m_effectActorManager;

    protected EffectActorManager effectActorManager
    {
        get
        {
            if (m_effectActorManager == null)
                m_effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
            return m_effectActorManager;
        }
    }

    void Awake()
    {
        
//        m_projector = GetComponent<Projector>();
        m_uiTarget.SetActive(false);
        m_projector.gameObject.SetActive(false);
    }


    void FixedUpdate()
    {
        if (m_projector.gameObject.activeSelf)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.y = projectY;
            mousePos.z = projectZ;
            //y축은 고정, x축은 움직임
            m_projector.transform.position = mousePos;
        }
        else if (m_uiTarget.activeSelf)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = projectZ;
            //x,y축은 움직임
            m_uiTarget.transform.position = mousePos;
        }
    }


    public void setUIController(UIController uiController)
    {
        m_uiController = uiController;
    }

    public void setProjector(SkillCard skillCard)
    {
        m_skillCard = skillCard;

        Awake();

        //타입에 따라서 스킬프로젝터 사용
        switch (skillCard.typeSkillPlay)
        {
            case Skill.TYPE_SKILL_ACTIVE.Active:
                break;
            case Skill.TYPE_SKILL_ACTIVE.Active_Range:
                //프로젝터 열고 범위 보여주기


                //사이즈
                gameObject.SetActive(true);
                m_projector.fieldOfView = defaultFieldOfView * skillCard.size;
                m_projector.gameObject.SetActive(true);
//                m_uiTarget.SetActive(false);

                break;
            case Skill.TYPE_SKILL_ACTIVE.Active_Target:
                //해당 위치 보여주기
                gameObject.SetActive(true);
                m_uiTarget.transform.localScale = Vector2.one * skillCard.size;
                m_uiTarget.SetActive(true);
//                m_projector.gameObject.SetActive(false);
                break;
        }

        

        //스킬 사이즈 보여주기
        //

        //40n = n


    }

    public void closeProjector(SkillCard skillCard, bool isUsed)
    {
        if (isUsed)
        {
            //스킬 사용
            runSkill(skillCard);
        }
            

        gameObject.SetActive(false);
    }

    void runSkill(SkillCard skillCard)
    {
        //
        //현재 위치에 스킬 실행
        //유닛을 거치지 않고 직접 실행
        //스킬 실행

        skillEffectAction(skillCard, "Active");

        skillCard.skillAction(this, null);


        //
        //범위스킬이면 현재 위치를 중심으로 범위만큼 가져오기
        //프로젝터 위치

        //쿨타임 초기화
        
    }

    bool skillEffectAction(SkillCard skillCard, string verb)
    {
        if (effectActorManager != null)
        {
            string skillName = skillCard.key + verb + "Particle";
            //해당 키에 따른 이펙트 생성
            Debug.Log("particleName : " + skillName);


            switch (skillCard.typeSkillPlay)
            {
                case Skill.TYPE_SKILL_ACTIVE.Active:
                    effectActorManager.createActor(skillName, transform.position, typeController, isFlip);
                    break;
                case Skill.TYPE_SKILL_ACTIVE.Active_Range:
                    effectActorManager.createActor(skillName, m_projector.transform.position, typeController, isFlip);
                    break;
                case Skill.TYPE_SKILL_ACTIVE.Active_Target:
                    effectActorManager.createActor(skillName, m_uiTarget.transform.position, typeController, isFlip);
                    break;
            }
            return true;
        }
        return false;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //현재 마우스포인터를 기준으로 계속 움직임
    //    Debug.Log("스킬프로젝터 이동");

    //}



    public string key
    {
        get { return m_skillCard.key; }
    }

    public Unit.TYPE_UNIT typeUnit
    {
        get { return Unit.TYPE_UNIT.Building; }
    }

    public int attack
    {
        get { return 0; }
    }

    public int layer
    {
        get { return LayerMask.NameToLayer("TotalLine"); }
    }

    public UnitActor.TYPE_CONTROLLER typeController
    {
        get { return UnitActor.TYPE_CONTROLLER.PLAYER; }
    }

    //스킬 레벨
    public int level { get { return 0; } }


    public UIController uiController
    {
        get { return m_uiController; }
    }

    public bool hitActor(IActor iActor, int attack)
    {
        return false;
    }
    
    public Unit.TYPE_LINE typeLine
    {
        get { return Unit.TYPE_LINE.ALL; }
    }

    public int nowHealth
    {
        get { return 0; }
    }


    public float range
    {
        get { return 0f; }
    }


    public Unit.TYPE_MOVEMENT typeMovement
    {
        get { return Unit.TYPE_MOVEMENT.Gnd; }
    }


    public bool isDead()
    {
        return true;
    }


    public float sight
    {
        get { return range; }
    }


    public Vector2 getPosition(int layer)
    {
        //현재 위치에서 랜덤으로 떨어지기

        if (m_projector.gameObject.activeSelf)
        {
            return m_projector.transform.position;
        }
        else if (m_uiTarget.activeSelf){
            return m_uiTarget.transform.position;
        }
        return transform.position;
    }

    public void setPosition(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }


    public void uiUpdate()
    {
    }
    
    public Unit.TYPE_TARGETING typeTarget
    {
        get { return m_skillCard.typeTarget; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_projector.transform.position, Vector2.one * m_projector.fieldOfView / 40f * 2f);
    }



    public bool isFlip{ get{ return false; }}

}

