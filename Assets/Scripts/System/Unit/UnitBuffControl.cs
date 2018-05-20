using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UnitBuffControl
{

    public delegate int RateHealthCalculateDelegate(int preMaxHealth);

    public event RateHealthCalculateDelegate rateHealthCalculate;

    public enum TYPE_ABILITY{Attack, Defence, AttackSpeed, MoveSpeed, Range, RecoveryHealth, MaxHealth}



    EffectActorManager m_effectActorManager;
    
    //
//    List<EffectActor> m_buffEffectList = new List<EffectActor>();

    //현재 걸린 버프
//    List<BuffActor> m_buffActorList = new List<BuffActor>();

    Dictionary<BuffActor, EffectActor> m_buffActorDic = new Dictionary<BuffActor, EffectActor>();

    //제약조건 버프


    //발동된 버프
    //List<BuffActor> m_actionBuffActorList = new List<BuffActor>();

    //추가 데이터
    //List<IStateControl> m_abilityList = new List<IStateControl>();

    //버프전 체력
    int preMaxHealth;


    public BuffActor[] getBuffActors()
    {
        return m_buffActorDic.Keys.ToArray<BuffActor>();
    }


    EffectActorManager effectActorManager { 
        get 
        {
            if (m_effectActorManager == null)
                m_effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
            return m_effectActorManager; 
        } 
    }


    /// <summary>
    /// 값 계산하기
    /// </summary>
    /// <param name="value"></param>
    /// <param name="iStateControl"></param>
    /// <returns></returns>
    //public float valueCalculate(float value, IStateControl iStateControl)
    //{
    //    float staticValue = m_abilityList.Where(ability => ability is IStateControl && ability.typeStateValue == StateControl.TYPE_STATE_VALUE.STATIC).Sum(ability => ability.value);
    //    if (staticValue != 0)
    //        return staticValue;
    //    else
    //    {
    //        value *= m_abilityList.Where(ability => ability is IStateControl && ability.typeStateValue == StateControl.TYPE_STATE_VALUE.RATE).Sum(ability => ability.value);
    //        value += m_abilityList.Where(ability => ability is IStateControl && ability.typeStateValue == StateControl.TYPE_STATE_VALUE.PLUS).Sum(ability => ability.value);
    //    }
    //    return value;
    //}

    /// <summary>
    /// 조건이 있는 값 계산하기
    /// </summary>
    /// <param name="value"></param>
    /// <param name="iStateControl"></param>
    /// <param name="iActor"></param>
    /// <param name="typeBuffConstraint"></param>
    /// <returns></returns>
    //public float valueCalculate(float value, IStateControl iStateControl, IActor iActor, Buff.TYPE_BUFF_CONSTRAINT typeBuffConstraint)
    //{

    //    //조건있는 버프가 사용되었는지 확인




    //    return value;
    //}





    /// <summary>
    /// 조건에 부합했을 때 조건에 부합한 버프 가져오기
    /// 
    /// </summary>
    /// <param name="assaultUnitActor"></param>
    /// <param name="attack"></param>
    /// <returns></returns>
    List<BuffActor> useBuff(IActor iActor, AddConstraint.TYPE_BUFF_CONSTRAINT typeBuffConstraint)
    {
        List<BuffActor> activeBuffList = new List<BuffActor>();

//        foreach (BuffActor buffActor in m_buffActorList)
        foreach (BuffActor buffActor in m_buffActorDic.Keys)
        {
            if (buffActor.isBuffAction(iActor, typeBuffConstraint))
            {
//                UnityEngine.Debug.Log("buff : " + buffActor.GetType());                
                //발동한 모든 버프 가져오기
                if (Prep.isInfiniteSkillRate ||
                    buffActor.rate >= UnityEngine.Random.Range(0f, 1f))
                {
//                    UnityEngine.Debug.Log("active : " + buffActor.stateControlArray[0].value);
                    activeBuffList.Add(buffActor);
                }
            }
        }

        return activeBuffList;
    }


    /// <summary>
    /// 값 계산하기
    /// </summary>
    /// <param name="value">입력 값</param>
    /// <param name="stateControl">해당 클래스</param>
    /// <param name="iActor">행동자 : 유닛, 탄환</param>
    /// <param name="typeBuffConstraint">행동 조건</param>
    /// <returns></returns>
    public float valueCalculate(float value, IStateControl stateControl, IActor iActor, AddConstraint.TYPE_BUFF_CONSTRAINT typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.None)
    {

        //발동된 버프 가져오기
        List<BuffActor> activeBuffList = useBuff(iActor, typeBuffConstraint);


        //버프 발동
        foreach (BuffActor buffActor in activeBuffList)
        {
            effectActorManager.createActor(buffActor.key + "ConActiveParticle", iActor.getPosition(iActor.layer), iActor.typeController, iActor.isFlip);

//            if (effectActor != null)
//                effectActor.transform.SetParent(iActor.transform);

            //버프와 파티클 같이 넣기
//            m_buffActorDic.Add(buffActor, effectActor);

        }

        //해당하는 버프
        //        UnityEngine.Debug.Log("actionCnt : " + activeBuffList.Count);


        List<IStateControl> stateList = new List<IStateControl>();

        for (int i = activeBuffList.Count - 1; i >= 0; i--)
        {
            stateList.AddRange(activeBuffList[i].stateControlArray);
        }

        //        UnityEngine.Debug.Log("stateCnt : " + stateList.Count + " " + stateControl.GetType());

        if (stateList.Count > 0)
        {
            //고정값 가져오기
            List<IStateControl> staticList = stateList.Where(stateCtrl => stateCtrl.GetType() == stateControl.GetType() && stateCtrl.typeStateValue == StateControl.TYPE_STATE_VALUE.Static).ToList<IStateControl>();

            //최대값 반환
            //고정값이 2개 이상이면 높은 값 또는 낮은 값으로 출력 (해당 클래스가 알려주어야 함)
            if (staticList.Count > 0)
                return staticList.Max(stateCtrl => stateCtrl.value);


            //모든 비율에 대한 합 계산
            List<IStateControl> multiList = stateList.Where(stateCtrl => stateCtrl.GetType() == stateControl.GetType() && stateCtrl.typeStateValue == StateControl.TYPE_STATE_VALUE.Rate).ToList<IStateControl>();
            if (multiList.Count > 0)
            {
                float rate = 1f + multiList.Sum(stateCtrl => stateCtrl.value);
                //비율에 대한 데이터 곱하기
                value *= rate;
            }


            //            UnityEngine.Debug.Log("multi : " + value + " " + stateControl.GetType());

            //모든 변수에 대한 합 계산
            List<IStateControl> sumList = stateList.Where(stateCtrl => stateCtrl.GetType() == stateControl.GetType() && stateCtrl.typeStateValue == StateControl.TYPE_STATE_VALUE.Plus).ToList<IStateControl>();
            if (sumList.Count > 0)
                value += sumList.Sum(stateCtrl => stateCtrl.value);

            //UnityEngine.Debug.Log("value : " + value + " " + stateControl.GetType());
        }

        return value;
    }

    /// <summary>
    /// 제약조건 가져오기
    /// </summary>
    /// <param name="stateControl">제약조건 상태</param>
    /// <param name="iActor">행동자 : 유닛, 탄환</param>
    /// <param name="typeBuffConstraint">행동 조건</param>
    /// <returns>제약조건 유무</returns>
    public bool isConstraint(IStateControl stateControl, IActor iActor, AddConstraint.TYPE_BUFF_CONSTRAINT typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.None)
    {
        List<BuffActor> activeBuffList = useBuff(iActor, typeBuffConstraint);

        List<IStateControl> stateList = new List<IStateControl>();

        

        for (int i = activeBuffList.Count - 1; i >= 0; i--)
        {
            stateList.AddRange(activeBuffList[i].stateControlArray);
        }


        //foreach (IStateControl stateCtrl in stateList)
        //{
        //    UnityEngine.Debug.LogWarning("activeCnt : " + stateCtrl.GetType() + " " + stateCtrl.typeStateValue);
        //}

        List<IStateControl> boolList = stateList.Where(stateCtrl => stateCtrl.GetType() == stateControl.GetType() && stateCtrl.typeStateValue == StateControl.TYPE_STATE_VALUE.Bool).ToList<IStateControl>();


        //불리언값이 있으면 불리언 값으로 출력 false, true
        //이외에는 false
        if (boolList.Count > 0)
            if (boolList.Where(stateCtrl => stateCtrl.value == 1f).ToList<IStateControl>().Count > 0)
                return true;

        return false;
    }


    /// <summary>
    /// 현재 상태 가져오기
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="iActor"></param>
    /// <param name="typeBuffConstraint"></param>
    /// <returns></returns>
    //public IUnitState valueCalculate(UnitActor unitActor, IActor iActor, Buff.TYPE_BUFF_CONSTRAINT typeBuffConstraint)
    //{
    //    return unitActor.iUnitState;
    //}

    
    /// <summary>
    /// 데이터 업데이트
    /// </summary>
    public void uiUpdate(UnitActor unitActor, float frameTime)
    {
        BuffActor[] buffActorArray = m_buffActorDic.Keys.ToArray();
        for (int i = 0; i < buffActorArray.Length; i++)
        {
            buffActorArray[i].frameUpdate(unitActor, frameTime);
        }
    }

    /// <summary>
    /// 버프 삽입하기
    /// </summary>
    /// <param name="buffActor">버프</param>
    /// <param name="unitActor">버프 삽입자</param>
    /// <returns>중첩여부 - true 중첩</returns>
    public bool addBuff(BuffActor buffActor, UnitActor unitActor)
    {

        //같은 키 버프 찾기
        BuffActor useBuffActor = m_buffActorDic.Keys.Where(buffAct => buffAct.key == buffActor.key).SingleOrDefault();

        UnityEngine.Debug.LogError("삽입 버프 : " + buffActor.key);
        //같은 키 버프를 찾았으면
        preMaxHealth = unitActor.maxHealth;

        if (useBuffActor != null)
        {
           
            UnityEngine.Debug.LogWarning("중복버프 : " + buffActor.key);
            //버프 갱신하기
            useBuffActor.replaceBuff(unitActor);
            return true;
        }
        else
        {
            //버프 삽입하기
            buffActor.removeBuffActorEvent += removeBuff;

            //파티클 유닛 하위에 삽입하기
            EffectActor effectActor = effectActorManager.createActor(buffActor.key + "BuffParticle", unitActor.transform.position, unitActor.typeController, unitActor.isFlip);

            if(effectActor != null)
                effectActor.transform.SetParent(unitActor.transform);

            //버프와 파티클 같이 넣기
            m_buffActorDic.Add(buffActor, effectActor);
        }  
        
        preMaxHealth = rateHealthCalculate(preMaxHealth);
        //foreach (IStateControl iStateControl in buffActor.stateControlArray)
        //{
        //    m_abilityList.Add(iStateControl);
        //}
        return false;
    }

    /// <summary>
    /// 버프 가져오기
    /// </summary>
    /// <param name="buffActor"></param>
    /// <returns></returns>
    public BuffActor getBuff(BuffActor buffActor)
    {
        //같은 버프키가 있으면 가져오기
        return m_buffActorDic.Keys.Where(buffAct => buffAct.key == buffActor.key).SingleOrDefault();
    }

    /// <summary>
    /// 버프 삭제하기
    /// </summary>
    /// <param name="buffActor"></param>
    /// <returns></returns>
    bool removeBuff(BuffActor buffActor)
    {
        //등록되어있는 버프 삭제
        if (m_buffActorDic.ContainsKey(buffActor))
        {

            //버프에 걸려있는 모든 상태이상 제어자 삭제하기
            //foreach (IStateControl iStateControl in buffActor.stateControlArray)
            //{
            //    if (!m_abilityList.Contains(iStateControl))
            //    {
            //        m_abilityList.Remove(iStateControl);
            //    }
            //}

            //if (m_actionBuffActorList.Contains(buffActor))
            //{
            //    m_actionBuffActorList.Remove(buffActor);
            //}
            
            //파티클 제거
            if (m_buffActorDic[buffActor] != null)
            {
                m_buffActorDic[buffActor].removeActor();
                m_buffActorDic[buffActor].transform.SetParent(null);
            }
//            UnityEngine.Debug.LogError("삭제 버프 : " + buffActor.key);
            //속도 갱신


            //제거가 성공되면
            if (m_buffActorDic.Remove(buffActor))
            {
//                buffActor.clear();
                preMaxHealth = rateHealthCalculate(preMaxHealth);
                return true;
            }

        }
        return false;
    }


}

