using System;
using System.Linq;

//버프 행동자
//버프를 행동합니다.

public class BuffActor
{

    public delegate bool RemoveBuffActorDelegate(BuffActor buffActor);

    public event RemoveBuffActorDelegate removeBuffActorEvent;

    //현재 버프
    Buff m_buff;

    //현재 지속시간
    float m_time;

    //중첩 수
    int m_nowOverlapCnt = 1;


    public string key { get { return m_buff.key; } }
    public IStateControl[] stateControlArray { get { return m_buff.stateControlArray; } }

    /// <summary>
    /// 발동확률
    /// </summary>
    public float rate { get { return m_buff.rate; } }

    /// <summary>
    /// 발동확률 증가값
    /// </summary>
    public float increaseRate { get { return m_buff.increaseRate; } }


    /// <summary>
    /// 현재 중첩 카운트
    /// </summary>
    public int nowOverlapCnt { get { return m_nowOverlapCnt; } }

    /// <summary>
    /// 버프 사용처
    /// </summary>
    public Buff.TYPE_USED typeUsed { get { return m_buff.typeUsed; } }

    /// <summary>
    /// 버프 등록하기
    /// </summary>
    /// <param name="buff"></param>
    public void setBuff(Buff buff)
    {
        m_buff = buff;
               
    }
    
    //public void clear()
    //{
    //    m_buff = null;
    //    m_time = 0f;
    //}


    //public IStateControl[] getStateControls(IStateControl stateControl)
    //{
    //    return m_buff.stateControlArray.Where(stateCtrl => stateCtrl.GetType() == stateControl.GetType()).ToArray<IStateControl>();
    //}

    /// <summary>
    /// 버프 갱신
    /// 실행시 1중첩
    /// </summary>
    /// <param name="unitActor">사용자</param>
    /// <returns></returns>
    public bool replaceBuff(UnitActor unitActor)
    {
        //중첩시 중첩카운트 최대치만큼 1 증가
        if (m_buff.overlapCount > m_nowOverlapCnt)
            m_nowOverlapCnt++;

        //지속시간 초기화
        m_time = 0f;
        return true;
    }

    /// <summary>
    /// 버프 종료하기
    /// </summary>
    /// <returns></returns>
    public bool endBuff()
    {
        removeBuffActorEvent(this);
        return true;
    }

    /// <summary>
    /// 버프 프레임 당 행동
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="frameTime"></param>
    public void frameUpdate(UnitActor unitActor, float frameTime)
    {
        //상태 갱신
        foreach (IStateControl stateControl in stateControlArray)
        {
            //실시간 상태값 확인
            stateControl.frameUpdate(unitActor, m_nowOverlapCnt);
        }


        //시간이 0 초과이면 타이머 증가
        if (m_buff.time > 0f)
        {

            m_time += frameTime;

            if (m_time >= m_buff.time + unitActor.level * m_buff.increaseTime)
            {

                //시간이 다되면 사라지기
                endBuff();
            }
        }
        //else
        //{
            //Prep.Log(m_buff.key, " 지속 버프 ", GetType());
        //}
    }

    //특수조건 여부에 부합해야 함
    //특수조건
    // - 조건 여부에 문제가 있음
    // - 발동 조건, 버프 지속 조건


    /// <summary>
    /// 버프 특수조건 여부 확인
    /// </summary>
    /// <param name="iActor">행동자 : 유닛, 탄환</param>
    /// <param name="typeBuffState">특수조건 : Always, Attack, Hit</param>
    /// <returns>true : 특수조건 발동</returns>
    public bool isBuffAction(IActor iActor, AddConstraint.TYPE_BUFF_CONSTRAINT typeBuffState)
    {


        //추가조건 판단
        //해당 조건이 맞으면
        //
        //항상 - 언제나 사용가능
        //
        //if (typeBuffState == m_buff.typeBuffState)
        //{
    //            UnityEngine.Debug.LogError("addConstraint : " + m_buff.addConstraint);

        //추가조건이 있으면
        //
        if (m_buff.addConstraint != null)
        {
//            UnityEngine.Debug.LogError("typeBuffState : " + m_buff.addConstraint.GetType());

            //추가조건 판단
            return m_buff.addConstraint.isConstraint(iActor, typeBuffState);
        }

        //추가조건이 없으면 무조건 사용
//        }
        return true;
    }

    /// <summary>
    /// 버프 삭제 델리게이트
    /// </summary>
    /// <param name="removeBuffActorDel"></param>
    //public void setDelegate(RemoveBuffActorDelegate removeBuffActorDel){
    //    removeBuffActorEvent = removeBuffActorDel;
    //}

   
}

