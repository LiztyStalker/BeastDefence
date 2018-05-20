using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MyselfBuffSkillActor : BuffSkillActor
{    



    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        BuffActor buffActor = getBuffActor();

        if (buffActor == null)
        {
            Prep.LogError(buffKey, "버프 못 찾음", GetType());
            return null;
        }

        if (iActor is UnitActor)
        {
            Prep.Log(buffKey, "버프 사용", GetType());
            ((UnitActor)iActor).addBuff(buffActor, ((UnitActor)iActor));
        }
        return null;
    }
}

