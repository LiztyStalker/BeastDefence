using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxUnitSearch : IUnitSearch
{

    /// <summary>
    /// 상태에 따라 적 1기 가져오기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="iUnitState"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public IActor searchUnitActor(IActor iActor, IActor targetActor, IUnitState iUnitState, TYPE_TEAM typeAlly)
    {
        //이동 상태이면
        if (iUnitState is MoveUnitState) return searchUnitActor(iActor, targetActor, typeAlly, iActor.sight);
        return searchUnitActor(iActor, targetActor, typeAlly);
    }


    /// <summary>
    /// 1기 적 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly)
    {
        return searchUnitActor(iActor, targetActor, typeAlly, iActor.range);
    }

    /// <summary>
    /// 1기 적 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="typeAlly"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly, float range)
    {
        
        //적 발견 및 체력 있음
        if (targetActor != null && !targetActor.isDead())
        {
            //사정거리 이내
            if (Vector2.Distance(iActor.getPosition(iActor.layer), targetActor.getPosition(targetActor.layer)) <= range)
            {
                return targetActor;
            }
        }
        
        RaycastHit2D[] rays;

        int layerMask = Prep.getLayerMask(iActor.layer, iActor.typeLine, iActor.typeMovement);
//        Vector2 pos = new Vector2(iActor.getPosition(iActor.layer).x, iActor.getPosition(iActor.layer).y);
        rays = Physics2D.BoxCastAll(iActor.getPosition(iActor.layer), Vector2.one * range * 2f, 0f, Vector2.zero, range, layerMask);

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
                    if (isAlly(iActor, tmpUnitActor, typeAlly))
                    {
                        return tmpUnitActor;
                    }
                }
            }
        }


        return null;
    }

    /// <summary>
    /// 다중 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public IActor[] searchUnitActors(IActor iActor, TYPE_TEAM typeAlly)
    {
        return searchUnitActors(iActor, typeAlly, iActor.range);
    }

    /// <summary>
    /// 다중 적 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public IActor[] searchUnitActors(IActor iActor, TYPE_TEAM typeAlly, float radius)
    {
        List<UnitActor> unitActorList = new List<UnitActor>();

        RaycastHit2D[] rays;

        int layerMask = Prep.getLayerMask(iActor.layer, iActor.typeLine, iActor.typeMovement);
        Debug.Log("IActor : " + layerMask);

        Vector2 pos = new Vector2(iActor.getPosition(iActor.layer).x, iActor.getPosition(iActor.layer).y);

        Debug.Log("pos : " + pos + " " + Input.mousePosition);
        rays = Physics2D.BoxCastAll(pos, new Vector2(radius, 10f), 0f, Vector2.zero, radius, layerMask);

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
                    if (isAlly(iActor, tmpUnitActor, typeAlly))
                    {
                        //킵
                        unitActorList.Add(tmpUnitActor);
                    }
                }
            }
        }

        return unitActorList.ToArray();
    }

    bool isAlly(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly)
    {
        //아군일 경우
        if (typeAlly == TYPE_TEAM.Ally)
        {
            if (iActor.typeController == targetActor.typeController)
            {
                return true;
            }
        }
        //적군일 경우
        else if (typeAlly == TYPE_TEAM.Enemy)
        {
            if (iActor.typeController != targetActor.typeController)
            {
                return true;
            }
        }
        //자신일 경우
        else
        {
            if (iActor == targetActor)
                return true;
        }
        return false;
    }




}

