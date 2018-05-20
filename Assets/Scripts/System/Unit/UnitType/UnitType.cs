using System;
using Spine.Unity;

public abstract class UnitType
{




    IUnitSearch m_iUnitSearch;

    IUnitState m_iUnitState;
    //대기, 이동, 공격, 사망, 스킬123
    //대기 - 경기 시작 전,
    //이동 - 사정거리 내에 적을 발견하지 않음
    //공격 - 사정거리 내 적 발견
    //사망 - 체력이 0 이하로 떨어짐
    //스킬123 - 일정확률로 스킬 사용. 끝나면 대기 상태로 이동

    //FSM 사용



    public IUnitState iUnitState { get { return m_iUnitState; } protected set { m_iUnitState = value; /*UnityEngine.Debug.LogWarning("UnitState : " + m_iUnitState.GetType());*/ } }
    public IUnitSearch iUnitSearch { get { return m_iUnitSearch; } protected set { m_iUnitSearch = value; } }
    /// <summary>
    /// 공격 액션
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="targetUnitActor"></param>
    /// <returns></returns>
    public abstract IActor attackAction(IActor iActor, IActor targetActor);
//    public abstract bool skeletonAnimation(UnitActor unitActor, SkeletonAnimation skeletonAnimation);

    /// <summary>
    /// 스킬 액션
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="targetUnitActor"></param>
    /// <returns></returns>
    public abstract IActor skillAction(IActor iActor, IActor targetActor);

    /// <summary>
    /// 초기화
    /// </summary>
    public UnitType()
    {
        clear();
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void clear()
    {
        m_iUnitState = new IdleUnitState();
    }


    /// <summary>
    /// 상태 업데이트하기
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="targetUnitActor"></param>
    public abstract IActor updateState(UnitActor unitActor, IActor targetActor);

    /// <summary>
    /// 사망 상태로 변경
    /// </summary>
    public void setDeadState()
    {
        m_iUnitState = new DeadUnitState();
    }

    /// <summary>
    /// 기본 상태로 변경
    /// </summary>
    public void setDefaultState()
    {
        m_iUnitState = new IdleUnitState();
        UnityEngine.Debug.Log("state : " + m_iUnitState.GetType());
    }

    /// <summary>
    /// 애니메이션 종료 이벤트
    /// </summary>
    public virtual void completeAction(UnitActor unitActor)
    {
        m_iUnitState.completeAction(unitActor);
    }

    /// <summary>
    /// 애니메이션 시작 이벤트
    /// </summary>
    /// <param name="unitActor"></param>
    public virtual void startAction(UnitActor unitActor)
    {
        m_iUnitState.startAction(unitActor);
    }
}

