using System;
using UnityEngine;
using Spine.Unity;

public class BuildingUnitType : UnitType, IUnitType
{

    float m_effectTimer = 0f;

    public BuildingUnitType()
        : base()
    {
        iUnitSearch = new NoneUnitSearch();
    }

    public override IActor skillAction(IActor iActor, IActor targetActor)
    {
        return null;
    }

    public override IActor attackAction(IActor iActor, IActor targetActor)
    {
        return null;
    }

    public override IActor updateState(UnitActor unitActor, IActor targetActor)
    {
        iUnitState = iUnitState.updateState(unitActor, targetActor);

        //체력에 따라서 상태 변하기

        //성위치를 기준
        Vector2 pos = new Vector2(
            UnityEngine.Random.Range(
                unitActor.transform.position.x - 1f,
                unitActor.transform.position.x + 1f
            ),
            UnityEngine.Random.Range(
                unitActor.transform.position.y + 2f - 2f,
                unitActor.transform.position.y + 2f + 2f)
            );

        m_effectTimer += Prep.frameTime;

        
        if (unitActor.nowHealthRate < 0.25f)
        {
            if (m_effectTimer > 0.1f)
            {
                unitActor.createEffect("CastleSmoke", pos);
                m_effectTimer = 0f;
            }
            //애니메이션 변경
        }
        else if (unitActor.nowHealthRate < 0.5f)
        {

            if (m_effectTimer > 0.25f)
            {
                unitActor.createEffect("CastleSmoke", pos);
                m_effectTimer = 0f;
            }
            //애니메이션 변경
        }
        else if (unitActor.nowHealthRate < 0.75f)
        {

            if (m_effectTimer > 0.5f)
            {
                unitActor.createEffect("CastleSmoke", pos);
                m_effectTimer = 0f;
            }
            //애니메이션 변경
        }
        else
        {
            //애니메이션 변경 - idle
        }
        

        return targetActor;
    }
    
}

