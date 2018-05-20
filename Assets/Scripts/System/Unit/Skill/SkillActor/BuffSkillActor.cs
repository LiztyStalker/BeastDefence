using UnityEngine;

public enum TYPE_TEAM { All, Ally, Enemy }

public abstract class BuffSkillActor : SkillActor
{


    //버프
    //[SerializeField]
    //string m_buffKey;

    //아군 적군 여부
//    [SerializeField]
//    TYPE_TEAM m_typeTeam;
    
    public string buffKey { get { return name; }}
//    public override TYPE_TEAM typeTeam { get { return m_typeTeam; } }

    protected BuffActor getBuffActor()
    {
        BuffActor buffActor = new BuffActor();
        Buff buff = BuffManager.GetInstance.getBuff(buffKey);
        if (buff == null)
        {
            Prep.LogError(buffKey, "찾을 수 없음", GetType());
            return null;
        }
        
        buffActor.setBuff(buff);
        
        return buffActor;
    }

   

}
