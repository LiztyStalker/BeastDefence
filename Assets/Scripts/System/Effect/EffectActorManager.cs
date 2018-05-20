using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EffectActorManager : MonoBehaviour, IActorManager
{
    [SerializeField]
    EffectActor m_effectActor;

    //사용 액터
    List<EffectActor> m_useActorList = new List<EffectActor>();

    //미사용 액터
    Queue<EffectActor> m_idleActorQueue = new Queue<EffectActor>();
    //

    private List<EffectActor> useActorList { get { return m_useActorList; } }
    private Queue<EffectActor> idleActorQueue { get { return m_idleActorQueue; } }


    void Awake()
    {
        for (int i = 0; i < Prep.effectActorObjFoolCnt; i++)
        {
            idleActorQueue.Enqueue(createActor());
        }
    }

    /// <summary>
    /// 이펙트 생성
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="uiController"></param>
    /// <param name="pos"></param>
    public EffectActor createActor(string key, Vector2 pos, UnitActor.TYPE_CONTROLLER typeCtrler, bool isFlip)
    {


        //큐가 비어있으면 새로 생성
        if (idleActorQueue.Count <= 0)
        {
            idleActorQueue.Enqueue(createActor());
        }

        //위치 및 효과 생성

        Effect effect = EffectManager.GetInstance.getEffect(key);

        //효과 복사
        if (effect != null)
        {
            EffectActor effectActor = idleActorQueue.Dequeue();
            effectActor.setEffect(effect, pos, isFlip);
            effectActor.transform.position = pos;
            useActorList.Add(effectActor);
            return effectActor;
        }

        return null;

        
    }

    /// <summary>
    /// 효과 행동자 반납
    /// </summary>
    /// <param name="actor"></param>
    /// <returns></returns>
    public bool removeActor(EffectActor actor)
    {
        if (useActorList.Contains(actor))
        {
            Debug.LogError("해제 : " + actor.name);
            useActorList.Remove(actor);
            idleActorQueue.Enqueue(actor);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 행동자 생성하기
    /// </summary>
    /// <returns></returns>
    EffectActor createActor()
    {
        EffectActor effectActor = Instantiate<EffectActor>(m_effectActor);
        effectActor.removeActorEvent += removeActor;
        effectActor.gameObject.SetActive(false);
        return effectActor;
    }

    //효과 삽입
    //효과 반납
}

