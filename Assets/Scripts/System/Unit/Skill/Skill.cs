using System.Collections;
using UnityEngine;


public class Skill
{
    //패시브, 액티브
    public enum TYPE_SKILL_ACTIVE {
        Passive = 0x0, 
        Attack = 0x1,
        Hit = 0x2,
        Move = 0x4,
        Active = 0x8,
        Active_Range = 0x16,
        Active_Target = 0x32,

        Move_Attack = Attack | Move
    }
    
//    [SerializeField]
    string m_key;

//    [SerializeField]
    string m_name;
        
//    [SerializeField]
    Sprite m_icon;
        
//    [SerializeField]
    TYPE_SKILL_ACTIVE m_typeSkillActive;

    //팀
    TYPE_TEAM m_typeTeam;

    //카운트
    int m_actorCnt;

    //사이즈
    float m_size;

    //범위
    float m_range;

    float m_coolTime;

    float m_increaseCoolTime;

//    [SerializeField]
    float m_rate;

    float m_increaseRate;

    //스킬 행동
    SkillActor m_skillActor;

    string m_contents;



    public string key { get { return m_key; } set { m_key = value; } }
    public string name { get { return m_name; } set { m_name = value; } }
    public Sprite icon { get { return m_icon; } set { m_icon = value; } }
    public TYPE_SKILL_ACTIVE typeSkillPlay { get { return m_typeSkillActive; } set { m_typeSkillActive = value; } }
    public int actorCnt { get { return m_actorCnt; } }
    public float size { get { return m_size; } }
    public float coolTime { get { return m_coolTime; } set { m_coolTime = value; } }
    public float skillRate { get { return m_rate; } set { m_rate = value; } }
    public float range { get { return m_range; } }
    public SkillActor skillActor { get { return m_skillActor; } set { m_skillActor = value; } }
    public string contents { get { return m_contents; } set { m_contents = value; } }
    public TYPE_TEAM typeTeam { get { return m_typeTeam; } }
    public Unit.TYPE_TARGETING typeTarget { get { return m_skillActor.typeTarget; } }




    //[MenuItem("Create/Create Asset")]
    //static void CreateAsset()
    //{
    //    var createAsset = CreateInstance<Skill>();

    //    AssetDatabase.CreateAsset(createAsset, "Assets/Editor/Skill.asset");
    //    AssetDatabase.Refresh();
    //}


    public Skill() { }

    public Skill(
        string key,
        string name,
        Sprite icon,
        TYPE_TEAM typeTeam,
        int actorCnt,
        float size,
        TYPE_SKILL_ACTIVE typeActive,
        float coolTime,
        float increaseCoolTime,
        float rate,
        float increaseRate,
        SkillActor skillActor,
        string contents
        )
    {
        m_key = key;
        m_name = name;
        m_icon = icon;
        m_typeTeam = typeTeam;
        m_actorCnt = actorCnt;
        m_size = size;
        m_typeSkillActive = typeActive;
        m_coolTime = coolTime;
        m_increaseCoolTime = increaseCoolTime;
        m_rate = rate;
        m_range = range;
        m_increaseRate = increaseRate;
        m_skillActor = skillActor;
        m_contents = contents;
    }

    //스킬 실행하기
    //자신, 적
    public IActor skillAction(IActor iActor, IActor targetActor)
    { 
        //해당에 맞는 스킬 실행
        //행동 - 공격, 이동
        //버프 - 버프 부여
        //설치 - 오브젝트 설치

        if(m_skillActor != null)
            return m_skillActor.skillAction(iActor, targetActor, this);

        Prep.LogWarning(this.name, "이 등록되지 않았습니다", GetType());
        return null;


    }


    public bool isSkillAction(float skillTimer)
    {
        //액티브 형이고
        if (typeSkillPlay != Skill.TYPE_SKILL_ACTIVE.Passive)
        {
            //스킬 쿨타임에 도달했고
            if (coolTime <= skillTimer)
            {
                //스킬 발동 확률에 걸렸으면
                if (Prep.isInfiniteSkillRate || skillRate > UnityEngine.Random.Range(0f, 1f))
                {
                    Debug.LogWarning("스킬발동 : " + name);
                    return true;
                }
            }
        }
        return false;
    }


    //public IEnumerator skillCoroutine(UnitActor unitActor)
    //{
    //    foreach (ISkillAction skillAction in skillActionArray)
    //    {
    //        skillAction.skillAction(unitActor);
    //        yield return new WaitForSeconds(skillAction.nextDelay);
    //    }
    //}
   


}

