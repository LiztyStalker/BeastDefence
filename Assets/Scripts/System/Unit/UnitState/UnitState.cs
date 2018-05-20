using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UnitState
{
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

}

