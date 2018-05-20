using System;
using UnityEngine;

public class DirectUnitSearch : IUnitSearch
{

    //float range = 0f;

    /// <summary>
    /// 상태에 따른 유닛 1기 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="iUnitState"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public IActor searchUnitActor(IActor iActor, IActor targetActor, IUnitState iUnitState, TYPE_TEAM typeAlly)
    {
        
        if (iUnitState is MoveUnitState)
            return searchUnitActor(iActor, targetActor, typeAlly, iActor.sight);
        
        return searchUnitActor(iActor, targetActor, typeAlly);
    }
    

    /// <summary>
    /// 유닛 가져오기
    /// </summary>
    /// <returns></returns>
    public IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly)
    {
        return searchUnitActor(iActor, targetActor, typeAlly, iActor.range);

    }

    IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly, float range)
    {


        if (targetActor != null && !targetActor.isDead())
            return targetActor;

        //방어병사는 사거리의 모든 적 판단- 사각레이

        //공격 사정거리, 시야 사정거리



        //사정거리 내에 적 판단 - 레이
        RaycastHit2D[] rays;

        int layerMask = Prep.getLayerMask(iActor.layer, iActor.typeLine, iActor.typeMovement);

        if (iActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
        {
            rays = Physics2D.RaycastAll(iActor.getPosition(iActor.layer), Vector2.right, range, layerMask);
        }
        else
        {
            rays = Physics2D.RaycastAll(iActor.getPosition(iActor.layer), Vector2.left, range, layerMask);
        }

        //레이 적 발견
        foreach (RaycastHit2D ray in rays)
        {
            //유닛 태그이면
            if (ray.collider.tag == Prep.unitTag)
            {
                UnitActor tmpUnitActor = ray.collider.GetComponent<UnitActor>();
                //유닛이 있으면 
                if (tmpUnitActor != null)
                {
                    //적이면
                    if (iActor.typeController != tmpUnitActor.typeController)
                    {
                        return tmpUnitActor;
                    }
                }
            }
        }

        return null;
    }

    public IActor[] searchUnitActors(IActor iActor, TYPE_TEAM typeAlly)
    {
        return searchUnitActors(iActor, typeAlly, iActor.range);
    }

    public IActor[] searchUnitActors(IActor iActor, TYPE_TEAM typeAlly, float radius)
    {
        return null;
    }


}

