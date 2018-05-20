using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class StateControl
{
    /// <summary>
    /// 추가값
    /// Static - 고정값 
    /// Rate - 비율 * 곱연산 -1 ~ 1
    /// Plus - 추가 + 합연산
    /// Bool - 참, 거짓 - 0 > 참, 0 =< 거짓
    /// </summary>
    public enum TYPE_STATE_VALUE { Static, Rate, Plus, Bool }

    //값
    float m_value = 1f;

    //증가량
    float m_increaseValue = 0f;

    //추가값
    TYPE_STATE_VALUE m_typeStateValue;

    //중첩
    int m_overlapCnt = 1;


    public StateControl()
    {
        this.value = 1f;
        this.increaseValue = 0f;
        this.typeStateValue = TYPE_STATE_VALUE.Rate;
        this.overlapCnt = 1;
    }

    //값 가져오기
    public float value { get { return m_value * overlapCnt; } protected set { m_value = value; } }

    //증가값
    public float increaseValue { get { return m_increaseValue * overlapCnt; } protected set { m_increaseValue = value; } }

    //추가 연산 가져오기
    public TYPE_STATE_VALUE typeStateValue { get { return m_typeStateValue; } protected set { m_typeStateValue = value; } }

    //중첩
    public int overlapCnt { get { return m_overlapCnt; } protected set { m_overlapCnt = value; } }

    public virtual bool frameUpdate(UnitActor unitActor, int overlapCnt)
    {
        m_overlapCnt = overlapCnt;
        return false;
    }


}

