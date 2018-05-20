using System;
using UnityEngine;

public abstract class SkillActor : MonoBehaviour
{

//    protected int typeRange { get; private set; }

    EffectActorManager m_effectActorManager;

//    public virtual TYPE_TEAM typeTeam { get { return TYPE_TEAM.ENEMY; } }
    public virtual Unit.TYPE_TARGETING typeTarget { get { return Unit.TYPE_TARGETING.Melee; } }


    /// <summary>
    /// 적
    /// </summary>
    /// <param name="iActor"></param>
    /// <returns></returns>
    public IActor[] getTargetActors(IActor iActor, TYPE_TEAM typeTeam, float size, IUnitState unitState = null)
    {
        RangeUnitSearch unitSearch = new RangeUnitSearch();

        IActor[] targetActors;
        if (unitState == null)
            targetActors = unitSearch.searchUnitActors(iActor, typeTeam, size);
        else
            targetActors = unitSearch.searchUnitActors(iActor, unitState, typeTeam);
        
        return targetActors;
    }

    public int typeRangeCalculator(IActor[] targetActors, int actCnt)
    {
        if (actCnt == 0 || actCnt > targetActors.Length)
            return targetActors.Length;
        return actCnt;
    }


    protected EffectActorManager effectActorManager
    {
        get
        {
            if (m_effectActorManager == null)
                m_effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
            return m_effectActorManager;
        }
    }

    protected bool effectAction(string key, Vector2 pos, UnitActor.TYPE_CONTROLLER typeCtrler, bool isFlip)
    {
        if (effectActorManager != null)
        {
            effectActorManager.createActor(key, pos, typeCtrler, isFlip);
            return true;
        }
        return false;
    }

    public abstract IActor skillAction(IActor iActor, IActor targetActor, Skill skill);

}

