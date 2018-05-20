using System;


public class CommanderBuffSkilLActor : BuffSkillActor
{
    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        BuffActor buffActor = getBuffActor();

        if (buffActor.typeUsed == Buff.TYPE_USED.Cmd)
        {
            UIController uiCtrler = iActor.uiController.getController(iActor.uiController, skill.typeTeam);

            if (uiCtrler != null)
            {
                uiCtrler.addBuff(buffActor);
            }
        }

        return null;

    }
}

