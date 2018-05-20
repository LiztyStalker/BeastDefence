using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectActor : MonoBehaviour
{
    public delegate bool RemoveEffectActorDelegate(EffectActor actor);

    public event RemoveEffectActorDelegate removeActorEvent;
    

    Effect m_effect;
    SoundPlay m_sound;
    
    ParticleSystem m_particleSystem;
    //효과 삽입
    //효과 초기화
    //


    Coroutine coroutine = null;
    
    
    public void setEffect(Effect effect, Vector2 pos, bool isFlip)
    {
        gameObject.SetActive(true);

//        Debug.Log("seteffect : " + effect.key + " " + isFlip);

        m_effect = effect;

        m_particleSystem = (ParticleSystem)Instantiate(effect.particleSystem);
        m_particleSystem.transform.SetParent(transform);
        m_particleSystem.transform.localPosition = Vector2.zero;

        m_particleSystem.transform.localScale = //new Vector3(m_particleSystem.transform.localScale.x * -1f, m_particleSystem.transform.localScale.y, 1f);
            (isFlip) ?
            new Vector3(-1f, 1f, 1f) :
            m_particleSystem.transform.localScale;


        transform.position = pos;
        
        m_sound = GetComponent<SoundPlay>();

        if(m_sound == null)
            m_sound = gameObject.AddComponent<SoundPlay>();

        string soundKey = effect.key.Replace("Particle", "Sound");

        SoundManager.GetInstance.audioPlay(m_sound, soundKey, false, false);

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(effectCoroutine());

        //System.Type type = effect.particleSystem.GetType();

        //Debug.Log("type : " + type);

        //m_particleSystem = gameObject.GetComponent<ParticleSystem>();

        
        //System.Reflection.PropertyInfo[] properties = type.GetProperties();

        //Debug.Log("fields : " + properties.Length);

        //foreach (System.Reflection.PropertyInfo property in properties)
        //{
        //    Debug.Log("copy : " + property.PropertyType);
        //    property.SetValue(m_particleSystem, property.GetValue(effect.particleSystem, null), null);
        //}

        //m_particleSystem.Play();

    }


    IEnumerator effectCoroutine(){

        while (true)
        {
            if(!m_particleSystem.main.loop){
                if (!m_particleSystem.IsAlive())
                {
                    //사운드 재생중이 아니면
                    if (m_sound != null && !m_sound.isPlaying())
                    {
                        break;
                    }
                }
            }
            yield return null;
        }

        removeActor();
    }
    

    public void removeActor()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = null;

        //Missing 에러 -
        m_particleSystem.Stop();
        Destroy(m_particleSystem.gameObject);
        removeActorEvent(this);
        gameObject.SetActive(false);
    }

}

