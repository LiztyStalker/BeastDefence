using System;
using UnityEngine;
using Spine.Unity;


public class SoldierUnitType : UnitType, IUnitType
{
    //UnitActor m_unitActor;

    //public SoliderUnitType(UnitActor unitActor)
    //{
    //    m_unitActor = unitActor;
    //}

    public SoldierUnitType()
        : base()
    {
        iUnitSearch = new RangeUnitSearch();
    }

    /// <summary>
    /// 스킬 사용
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="bulletActorManager"></param>
    /// <param name="typeTargeting"></param>
    /// <returns></returns>
    public override IActor skillAction(IActor iActor, IActor targetActor)
    {
        //스킬 시작하기
        if (iUnitState is SkillUnitState)
            return ((SkillUnitState)iUnitState).skillAction(iActor, targetActor);
        return null;
    }


    /// <summary>
    /// 공격 행동
    /// </summary>
    /// <param name="iActor">사용자</param>
    /// <param name="targetActor">피격자</param>
    /// <param name="bulletActorManager">탄환행동자매니저</param>
    /// <returns></returns>
    public override IActor attackAction(IActor iActor, IActor targetActor)
    {
        if(iUnitState is AttackUnitState)
            return ((AttackUnitState)iUnitState).attackAction(iActor, targetActor);
        return null;
    }


    public override IActor updateState(UnitActor unitActor, IActor targetActor)
    {
        //적 탐색하기

        iUnitState = iUnitState.updateState(unitActor, targetActor);

        return targetActor;
    }


    public override void completeAction(UnitActor unitActor)
    {
        //스킬 상태이면 idle로 변환
        if (iUnitState is SkillUnitState)
            setDefaultState();
    }
}

