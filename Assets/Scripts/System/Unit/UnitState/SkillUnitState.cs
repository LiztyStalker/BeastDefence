using UnityEngine;
using Spine;



public class SkillUnitState : UnitState, IUnitState
{
    readonly string particleText = "Particle";


    int m_index;

    public SkillUnitState(int index)
    {
        m_index = index;
    }



    public IActor skillAction(IActor iActor, IActor targetActor)
    {
        skillEffectAction( ((UnitActor)iActor), "Active");

        //스킬 발동 애니메이션 실행
        //effectActorManager.createActor(,);

        return ((UnitActor)iActor).skillAction(m_index, targetActor);
    }
    
    bool skillEffectAction(UnitActor unitActor, string verb)
    {
        if (effectActorManager != null)
        {
            string skillName = unitActor.getSkillKey(m_index);
            if (!string.IsNullOrEmpty(skillName))
            {
                skillName += verb + particleText;
                //해당 키에 따른 이펙트 생성
                Debug.Log("particleName : " + skillName);
                effectActorManager.createActor(skillName, unitActor.transform.position, unitActor.typeController, unitActor.isFlip);
            }
            return true;
        }
        return false;
    }

    public IUnitState updateState(UnitActor unitActor, IActor targetActor)
    {
//        Debug.Log("스킬");

        //스킬 불가이면
        if (unitActor.isConstraint(new NotSkillStateControl(), unitActor))
        {
            //대기로
            return new IdleUnitState();
        }

        unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        
        //스킬 시작
        return this;
    }
    
    //애니메이션 종료 이펙트 실행
    public void completeAction(UnitActor unitActor)
    {
        skillEffectAction(unitActor, "End"); 
        unitActor.transform.localScale = new Vector3(1f, 1f);
        string skillName = unitActor.getSkillKey(m_index);

    }

    public TrackEntry setAnimation(UnitActor unitActor, SetAnimationDelegate setAnimationDel)
    {
        //애니메이션 실행
        //스킬시작 이펙트 실행
        //EffectActorManager effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
        //effectActorManager.createActor(,);

        return setAnimationDel(string.Format("Skill{0}", m_index), false);
    }

    public void startAction(UnitActor unitActor)
    {
        skillEffectAction(unitActor, "Start");
    }

    public Skill.TYPE_SKILL_ACTIVE getTypeSkillActive()
    {
        return Skill.TYPE_SKILL_ACTIVE.Attack;
    }
}

