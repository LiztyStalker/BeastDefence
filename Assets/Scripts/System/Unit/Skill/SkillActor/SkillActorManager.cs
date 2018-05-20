using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillActorManager : SingletonClass<SkillActorManager>
{

    Dictionary<string, SkillActor> m_skillActorDic = new Dictionary<string, SkillActor>();

    public SkillActorManager()
    {
        initParse();
    }

    void initParse()
    {
        //클래스 명으로 클래스 생성 후 데이터 삽입한 다음 리스트에 넣기
        SkillActor[] skillActorArray = Resources.LoadAll<SkillActor>(Prep.skillActorDataPath);

        if (skillActorArray.Length > 0)
        {
            foreach (SkillActor skillActor in skillActorArray)
            {
                m_skillActorDic.Add(skillActor.name, skillActor);
            }
        }
        else
        {
            Prep.LogError(Prep.skillActorDataPath, "를 찾을 수 없음", GetType());
        }
        
    }

    public SkillActor getSkillActor(string key)
    {
        if(m_skillActorDic.ContainsKey(key)){
            return m_skillActorDic[key];
        }

        Prep.LogWarning(key, "를 찾을 수 없음", GetType());
        return null;
    }


}
