using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class NoneUnitSearch : IUnitSearch
{




    public IActor searchUnitActor(IActor iActor, IActor targetActor, IUnitState iUnitState, TYPE_TEAM typeAlly)
    {
        return null;
    }

    public IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly)
    {
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

