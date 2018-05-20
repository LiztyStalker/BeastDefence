using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorManager : SingletonClass<ActorManager>
{
    List<IActorManager> m_actorManagerList = new List<IActorManager>();


    public void initActorManager()
    {
        clearActorManager();


        addActorManager<BulletActorManager>("Manager@BulletActor");
        addActorManager<EffectActorManager>("Manager@EffectActor");
        addActorManager<UnitActorManager>("Manager@UnitActor");

        //GameObject obj = GameObject.Find("Manager@BulletActor");

        //if (obj != null)
        //{
        //    IActorManager actorManager = obj.GetComponent<BulletActorManager>();
        //    if(actorManager != null)
        //        m_actorManagerList.Add(actorManager);
        //}

        //m_actorManagerList.Add(GameObject.Find("Manager@BulletActor").GetComponent<BulletActorManager>());
        //m_actorManagerList.Add(GameObject.Find("Manager@EffectActor").GetComponent<EffectActorManager>());
        //m_actorManagerList.Add(GameObject.Find("Manager@UnitActor").GetComponent<UnitActorManager>());
    }


    void addActorManager<T>(string name) where T : IActorManager
    {
        GameObject obj = GameObject.Find(name);

        if (obj != null)
        {
            T actorManager = obj.GetComponent<T>();
            if (actorManager != null)
                m_actorManagerList.Add((IActorManager)actorManager);
            else
            {
                Prep.LogWarning(name, "클래스를 찾을 수 없음", GetType());
            }
        }
        else
        {
            Prep.LogWarning(name, "매니저를 찾을 수 없음", GetType());
        }
    }



    void clearActorManager()
    {
        m_actorManagerList.Clear();
    }

    public IActorManager getActorManager(System.Type type)
    {
        if (m_actorManagerList.Count < 0)
            initActorManager();
        return m_actorManagerList.Where(actorManager => actorManager.GetType() == type).SingleOrDefault();
    }

}
