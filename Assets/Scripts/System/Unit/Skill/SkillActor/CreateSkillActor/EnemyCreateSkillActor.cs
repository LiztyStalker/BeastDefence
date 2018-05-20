using System;

public class EnemyCreateSkillActor : CreateSkillActor
{

//    public override TYPE_TEAM typeTeam { get { return TYPE_TEAM.ENEMY; } }

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        UnitActorManager unitActorManger = ActorManager.GetInstance.getActorManager(typeof(UnitActorManager)) as UnitActorManager;
        
        Unit unit = UnitManager.GetInstance.getUnit(unitKey);
        int level = iActor.level;
        
        if (unit != null)
        {
            UnitCard unitCard = new UnitCard(unit, level / 10);

            if (iActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
            {
//                unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), targetActor.transform.position - UnityEngine.Vector3.right);
                unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), targetActor.getPosition(targetActor.layer) - UnityEngine.Vector2.right);
            }
            else
            {
//                unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), targetActor.transform.position + UnityEngine.Vector3.right);
                unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), targetActor.getPosition(targetActor.layer) + UnityEngine.Vector2.right);
            }
        }

        return null;
    }
}

