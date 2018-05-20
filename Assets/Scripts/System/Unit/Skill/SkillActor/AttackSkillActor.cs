using UnityEngine;

public abstract class AttackSkillActor : SkillActor
{
    //찾기
    IUnitSearch m_iUnitSearch;

    //공격력
    [SerializeField]
    int m_attack;

//    [SerializeField]
//    bool m_isRange;

    public int attack { get { return m_attack; }}
//    protected bool isRange { get { return m_isRange; } }
    protected IUnitSearch unitSearch { get { return m_iUnitSearch; }}

}

