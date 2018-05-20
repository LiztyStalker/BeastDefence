using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Buff
{
    public enum TYPE_USED { Unit, Cmd }
    
    //버프키
    string m_key;

    //버프명
    string m_name;

    //버프 사용처
    TYPE_USED m_typeUsed;

    //지속시간
    float m_time;

    //지속시간 증가량
    float m_increaseTime;

    //발동확률
    float m_rate;

    //발동확률증가량
    float m_increaseRate;

    //중첩 개수
    int m_overlapCount;

    //활동조건
//    TYPE_BUFF_CONSTRAINT m_typeBuffConstraint;

    //추가 발동조건
    AddConstraint m_addConstraint;

    //상태이상 종류
    IStateControl[] m_stateControlArray;

    //

    string m_contents;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public float time { get { return m_time; } }
    public float increaseTime { get { return m_increaseTime; } }
    public float rate { get { return m_rate; } }
    public float increaseRate { get { return m_increaseRate; } }
    public int overlapCount { get { return m_overlapCount; } }
    public TYPE_USED typeUsed { get { return m_typeUsed; } }
    public IStateControl[] stateControlArray { get { return m_stateControlArray; } }
    public AddConstraint addConstraint { get { return m_addConstraint; } }
//    public TYPE_BUFF_CONSTRAINT typeBuffState { get { return m_typeBuffConstraint; } }
    public string contents { get { return m_contents; } }

    public Buff(
        string key,
        string name,
        float time,
        float incTime,
        float rate,
        float incRate,
        int overlapCount,
//        TYPE_BUFF_CONSTRAINT typeBuffConstraint,
        TYPE_USED typeUsed,
        AddConstraint addConstraint,
        IStateControl[] stateControlArray,
        string contents
        )
    {
        m_key = key;
        m_name = name;
        m_time = time;
        m_increaseTime = incTime;
        m_rate = rate;
        m_increaseRate = incRate;
        m_overlapCount = overlapCount;
        m_typeUsed = typeUsed;
//        m_typeBuffConstraint = typeBuffConstraint;
        m_addConstraint = addConstraint;
        m_stateControlArray = stateControlArray;
        m_contents = contents;
    }
}
