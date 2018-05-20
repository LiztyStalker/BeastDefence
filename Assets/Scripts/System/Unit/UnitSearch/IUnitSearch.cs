using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IUnitSearch
{
    IActor searchUnitActor(IActor iActor, IActor targetActor, TYPE_TEAM typeAlly);
    IActor searchUnitActor(IActor iActor, IActor targetActor, IUnitState iUnitState, TYPE_TEAM typeAlly);
    IActor[] searchUnitActors(IActor iActor, TYPE_TEAM typeAlly);
    IActor[] searchUnitActors(IActor iActor, TYPE_TEAM typeAlly, float radius);
}

