using System;
using UnityEngine;

/// <summary>
/// 자신을 기준으로 아래 유닛 가져오기
/// </summary>
public class UnderUnitSearch : IUnitSearch
{

    float m_offsetRange = 2f;

    /// <summary>
    /// 상태에 따른 1기 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="iUnitState"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    IActor IUnitSearch.searchUnitActor(IActor iActor, IActor targetActor, IUnitState iUnitState, TYPE_TEAM typeAlly)
    {
        if (iUnitState is MoveUnitState)
            return searchUnitActor(iActor, targetActor, typeAlly, iActor.sight);

        return searchUnitActor(iActor, targetActor, typeAlly);
    }

    /// <summary>
    /// 1기 찾기
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
    /// 1기 찾기
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="typeAlly"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly, float range)
    {
        if (targetActor != null && !targetActor.isDead())
            return targetActor;


        RaycastHit2D[] rays;

        if (iActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
        {
            Vector2 pos = new Vector2(iActor.getPosition(iActor.layer).x + range, iActor.getPosition(iActor.layer).y - range);

            rays = Physics2D.BoxCastAll(pos, Vector2.one * range * m_offsetRange, 0f, Vector2.zero, range * m_offsetRange);

//            rays = Physics2D.BoxCastAll(pos, Vector2.one * unitActor.range * 0.01f, 0f, Vector2.right);
        }
        else
        {

            Vector2 pos = new Vector2(iActor.getPosition(iActor.layer).x - range, iActor.getPosition(iActor.layer).y - range);
            rays = Physics2D.BoxCastAll(pos, Vector2.one * range * m_offsetRange, 0f, Vector2.zero, range * m_offsetRange);
//            rays = Physics2D.BoxCastAll(pos, Vector2.one * unitActor.range * 0.01f, 0f, Vector2.left);
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

    public IActor[] searchUnitActors(IActor unitActor, TYPE_TEAM typeAlly, float radius)
    {
        return null;
    }

    /// <summary>
    /// IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_ALLY typeAlly)과 동일
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="targetActor"></param>
    /// <param name="iUnitState"></param>
    /// <param name="typeAlly"></param>
    /// <returns></returns>
    public IActor searchUnitActor(IActor iActor, IActor targetActor, IUnitState iUnitState, TYPE_TEAM typeAlly)
    {
        return searchUnitActor(iActor, targetActor, typeAlly);
    }

    

}

