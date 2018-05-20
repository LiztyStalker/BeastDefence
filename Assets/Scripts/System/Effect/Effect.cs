using UnityEngine;

public class Effect
{

    string m_key;
    ParticleSystem m_particleSystem;
    //키
    //파티클
    //

    public string key { get { return m_key; } }
    public ParticleSystem particleSystem { get { return m_particleSystem; } }

    public Effect(string key, ParticleSystem particleSystem)
    {
        m_key = key;
        m_particleSystem = particleSystem;
    }


}

