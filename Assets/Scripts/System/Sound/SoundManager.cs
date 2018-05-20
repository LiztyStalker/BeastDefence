using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : SingletonClass<SoundManager>
{

    Dictionary<TYPE_SOUND, Dictionary<string, AudioClip>> m_soundDic
        = new Dictionary<TYPE_SOUND, Dictionary<string, AudioClip>>();

//	Dictionary <string, AudioClip> m_bgmDic = new Dictionary<string, AudioClip>();
//	Dictionary <string, AudioClip> m_effectDic = new Dictionary<string, AudioClip>();
//    Dictionary<string, AudioClip> m_voiceDic = new Dictionary<string, AudioClip>();

	List<SoundPlay> m_soundList = new List<SoundPlay>();


//	AudioSource m_MyselfAudioSource;
//	AudioSource m_RangeAudioSource;
//	AudioSource m_WorldAuidoSource;

	readonly string[] m_btnKeys = 
	{
		"BtnNone",
		"BtnInfor",
		"BtnOK",
		"BtnCancel",
		"BtnWarning",
		"BtnError",
		"BtnSell",
		"BtnBuy"
	};


    public SoundManager()
    {
        initDictionary();
    }


	public void initDictionary(){

        for (int i = 0; i < Enum.GetValues(typeof(TYPE_SOUND)).Length; i++)
        {
            m_soundDic.Add((TYPE_SOUND)i, new Dictionary<string, AudioClip>());
        }

		AudioClip[] bgmList = Resources.LoadAll<AudioClip> ("Sound/BGM");

//		m_bgmDic.Clear ();
		foreach (AudioClip bgm in bgmList) {
            string bgmName = bgm.name.Trim();
//            Debug.Log("bgm : " + bgmName + " " + bgm);
            m_soundDic[TYPE_SOUND.BGM].Add(bgmName, bgm);
		}


		AudioClip[] effectList = Resources.LoadAll<AudioClip>("Sound/Effect");

//		m_effectDic.Clear ();
		foreach (AudioClip effect in effectList) {
            string effName = effect.name.Trim();
//            Debug.Log("effect : " + effName + " " + effect);
            m_soundDic[TYPE_SOUND.EFFECT].Add(effName, effect);
            //m_effectDic.Add(effName, effect);
		}


        AudioClip[] voiceList = Resources.LoadAll<AudioClip>("Sound/Voice");

//        m_voiceDic.Clear();
        foreach (AudioClip voice in voiceList)
        {
            string vicName = voice.name.Trim();
//            Debug.Log("effect : " + vicName + " " + voice);
            m_soundDic[TYPE_SOUND.VOICE].Add(vicName, voice);
//            m_voiceDic.Add(vicName, voice);
        }
	}


    //public void setMute(TYPE_SOUND typeSound){
    //    SoundPlay[] soundList = m_soundList.Where (sound => sound.typeSound == typeSound).ToArray<SoundPlay> ();
    //    foreach (SoundPlay sound in soundList) {
    //        if (typeSound == TYPE_SOUND.EFFECT)
    //            sound.setMute (!Prep.isEffect);
    //        else
    //            sound.setMute (!Prep.isBGM);
    //    }
    //}

    /// <summary>
    /// 볼륨 설정하기
    /// </summary>
    /// <param name="typeSound"></param>
    /// <param name="volume"></param>
    public void setVolume(TYPE_SOUND typeSound, float volume)
    {
        SoundPlay[] soundList = m_soundList.Where(sound => sound.typeSound == typeSound).ToArray<SoundPlay>();
        foreach (SoundPlay sound in soundList) {
            sound.setVolume(volume);
        }
    }



//    public void bgmPlay(SoundPlay soundPlayer, string key, bool is3DSound = false, bool isLoop = false, float runTime = 1f)
//    {
//    }

	/// <summary>
	/// 사운드 플레이
	/// </summary>
	/// <param name="soundCilp">Sound cilp.</param>
	/// <param name="key">Key.</param>
	public void audioPlay(SoundPlay soundPlayer, string key, bool is3DSound = false, bool isLoop = false, float runTime = 1f){

		if (string.IsNullOrEmpty (key))
			return;
        

        if (m_soundDic.ContainsKey(soundPlayer.typeSound))
        {
            if (m_soundDic[soundPlayer.typeSound].ContainsKey(key))
            {
                soundPlayer.audioPlay(m_soundDic[soundPlayer.typeSound][key], soundPlayer.typeSound, is3DSound, isLoop, runTime);
                m_soundList.Add(soundPlayer);
            }
            else
            {
                Debug.LogWarning("사운드 없음 : " + key);
            }
        }
//		soundCilp.audioPlay ();
	}

	/// <summary>
	/// 효과음 플레이
	/// </summary>
	/// <param name="soundPlayer">Sound cilp.</param>
	/// <param name="key">Key.</param>
    //public void effectPlay(SoundPlay soundPlayer, string key, bool is3DSound = true, bool isLoop = false, float runTime = 1f)
    //{
    //    if (string.IsNullOrEmpty (key))
    //        return;
		
    //    if (m_effectDic.ContainsKey (key)) {
    //        soundPlayer.audioPlay(m_effectDic[key], TYPE_SOUND.EFFECT, is3DSound, isLoop, runTime);
    //        m_soundList.Add (soundPlayer);
    //    } else {
    //        Debug.LogWarning ("사운드 없음 : " + key);
    //    }
    //}

	/// <summary>
	/// 버튼 효과음 플레이
	/// </summary>
	/// <param name="soundPlayer">Sound player.</param>
	/// <param name="typeBtnSound">Type button sound.</param>
    public void btnEffectPlay(SoundPlay soundPlayer, TYPE_BTN_SOUND typeBtnSound, float runTime = 1f)
    {
        audioPlay(soundPlayer, getBtnSoundKey(typeBtnSound), false, false, runTime);
	}


	string getBtnSoundKey(TYPE_BTN_SOUND typeBtnSound){
        return "BTN_" + typeBtnSound.ToString();
        //if (m_btnKeys.Length > 0 && m_btnKeys.Length > (int)typeBtnSound)
        //    return m_btnKeys [(int)typeBtnSound];
        //return "";
	}
	/// <summary>
	/// 사운드 종료
	/// </summary>
	/// <param name="soundCilp">Sound cilp.</param>

	public void soundEnd(SoundPlay soundCilp){
		if(m_soundList.Contains(soundCilp)) m_soundList.Remove (soundCilp);
	}

    /// <summary>
    /// 사운드 플레이 가져오기
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public SoundPlay getSoundPlay(GameObject gameObject)
    {
        SoundPlay soundPlay = gameObject.GetComponent<SoundPlay>();
        if (soundPlay == null)
            soundPlay = gameObject.AddComponent<SoundPlay>();
        return soundPlay;
    }



}


