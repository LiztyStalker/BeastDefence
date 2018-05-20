using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AllyCreateSkillActor : CreateSkillActor
{

//    public override TYPE_TEAM typeTeam {get{return TYPE_TEAM.ALLY;}}

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {

        IUnitSearch unitSearch = new RangeUnitSearch();
        IActor getActor = unitSearch.searchUnitActor(iActor, targetActor, TYPE_TEAM.Ally);

        UnitActorManager unitActorManger = ActorManager.GetInstance.getActorManager(typeof(UnitActorManager)) as UnitActorManager;

        if (getActor != null)
        {

            Unit unit = UnitManager.GetInstance.getUnit(unitKey);
            int level = iActor.level;

            if (unit != null)
            {
                UnitCard unitCard = new UnitCard(unit, level / 10);

                //위치 등록
                if (iActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
                {
//                    unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), getActor.transform.position + UnityEngine.Vector3.right);
                    unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), getActor.getPosition(getActor.layer) + UnityEngine.Vector2.right);
                }
                else
                {
//                    unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), getActor.transform.position - UnityEngine.Vector3.right);
                    unitActorManger.createActor(unitCard, iActor.uiController, layerToPos(iActor.layer), getActor.getPosition(getActor.layer) - UnityEngine.Vector2.right);
                }
            }
        }
        
        return null;
    }
}

