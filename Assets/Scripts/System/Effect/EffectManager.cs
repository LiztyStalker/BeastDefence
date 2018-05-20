using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EffectManager : SingletonClass<EffectManager>
{


    Dictionary<string, Effect> m_effectDic = new Dictionary<string, Effect>();

    Dictionary<string, Effect> effectDic { get { return m_effectDic; } }


    public EffectManager()
    {
        initParse();
    }

    void initParse()
    {
        ParticleSystem[] particleArray = Resources.LoadAll<ParticleSystem>(Prep.effectDataPath);

        if (particleArray.Length > 0)
        {

            foreach (ParticleSystem particle in particleArray)
            {
                string key = particle.name;

                //파티클 이름에 맞는 오브젝트 가져오기

                ParticleSystem particleSys = particleArray.Where(part => part.name == key).SingleOrDefault();
                if (particleSys == null)
                    Prep.LogWarning(key, " 파티클을 찾을 수 없음", GetType());

                //찾을 수 없음


                Effect effect = new Effect(key, particleSys);

                effectDic.Add(key, effect);

            }
        }
        else
        {
            Prep.LogError(Prep.effectDataPath, " 을 찾을 수 없음", GetType());
        }
    }

    /// <summary>
    /// 효과 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Effect getEffect(string key)
    {
        if (effectDic.ContainsKey(key))
        {
            return effectDic[key];
        }

//        Prep.LogError(key, "를 찾을 수 없음", GetType());
        return null;
    }
}

