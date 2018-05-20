using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SingleBuffSkillActor  : BuffSkillActor
{
    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        BuffActor buffActor = getBuffActor();

        if (buffActor == null)
            return null;

//        IUnitSearch unitSearch = new RangeUnitSearch();
//        targetActor = unitSearch.searchUnitActor(iActor, null, typeAlly);

//        IActor[] targetActors = getTargetActors(iActor, typeAlly);
//        typeRange = typeRangeCalculator(targetActors, typeRange);
        
        if (targetActor != null)
        {
            if(targetActor is UnitActor)
                ((UnitActor)targetActor).addBuff(buffActor, ((UnitActor)targetActor));
        }
        
        return null;
    }

}

