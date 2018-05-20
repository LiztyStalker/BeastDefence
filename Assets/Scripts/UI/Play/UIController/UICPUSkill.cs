using System;
using UnityEngine;

public class UICPUSkill : MonoBehaviour, IActor
{
    public delegate Nullable<Vector2> SearchUnitAssociationPosDelegate(UIController uiController, TYPE_TEAM typeAlly);

    UIController m_uiController;

    //CPU 스킬 사용
    SkillCard m_skillCard;

    EffectActorManager m_effectActorManager;

    protected EffectActorManager effectActorManager
    {
        get
        {
            if (m_effectActorManager == null)
                m_effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
            return m_effectActorManager;
        }
    }

    public bool setSkill(UIController uiCtrler, SkillCard skillCard, SearchUnitAssociationPosDelegate searchUnitAssociationDel)
    {
        m_uiController = uiCtrler;
        m_skillCard = skillCard;
        Nullable<Vector2> pos = searchUnitAssociationDel(uiCtrler, skillCard.typeAlly);

        if (pos != null)
        {
            transform.position = pos.Value;
            skillEffectAction(skillCard, "Active");
            skillCard.skillAction(this, null);
            return true;
        }

        transform.position = Vector2.zero;
        return false;
    }

    bool skillEffectAction(SkillCard skillCard, string verb)
    {
        if (effectActorManager != null)
        {
            string skillName = skillCard.key + verb + "Particle";
            //해당 키에 따른 이펙트 생성
            Debug.Log("particleName : " + skillName);


            switch (skillCard.typeSkillPlay)
            {
                case Skill.TYPE_SKILL_ACTIVE.Active:
                    effectActorManager.createActor(skillName, Vector2.zero, typeController, isFlip);
                    break;
                default:
                    effectActorManager.createActor(skillName, transform.position, typeController, isFlip);
                    break;
            }
            return true;
        }
        return false;
    }

    public int nowHealth
    {
        get { return 0; }
    }


    public float range
    {
        get { return 0f; }
    }

    public string key
    {
        get { return m_skillCard.key; }
    }


    public Unit.TYPE_MOVEMENT typeMovement
    {
        get { return Unit.TYPE_MOVEMENT.Gnd; }
    }


    public bool isDead()
    {
        return true;
    }


    public float sight
    {
        get { return range; }
    }


    public Vector2 getPosition(int layer)
    {
        //현재 위치에서 랜덤으로 떨어지기
        return transform.position;
    }

    public void setPosition(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }


    public void uiUpdate()
    {
        //
    }

    public UIController uiController
    {
        get { return m_uiController; }
    }


    public bool hitActor(IActor iActor, int attack)
    {
        return false;
    }

    public Unit.TYPE_LINE typeLine
    {
        get { return Unit.TYPE_LINE.ALL; }
    }

    public Unit.TYPE_UNIT typeUnit
    {
        get { throw new System.NotImplementedException(); }
    }

    public int attack
    {
        get { return 0; }
    }

    public int layer
    {
        get { return LayerMask.NameToLayer("TotalLine"); }
    }

    public int level { get { return 0; } }


    public UnitActor.TYPE_CONTROLLER typeController
    {
        get { return UnitActor.TYPE_CONTROLLER.CPU; }
    }

    public Unit.TYPE_TARGETING typeTarget
    {
        get { return m_skillCard.typeTarget; }
    }



    public bool isFlip {get { return true; } }

}

