using UnityEngine;
using UnityEngine.UI;


public class UIOption : UIPanel
{

//    SoundPlay m_soundPlayer;

    [SerializeField]
    Slider m_bgmSlider;

    [SerializeField]
    Slider m_effectSlider;

    [SerializeField]
    Text m_bgmText;

    [SerializeField]
    Text m_effectText;


//        [SerializeField]
//        Toggle m_voiceToggle;

    void Awake()
    {

        //m_soundPlayer = GetComponent<SoundPlay>();
        //if (m_soundPlayer == null)
        //    m_soundPlayer = gameObject.AddComponent<SoundPlay>();

        m_bgmSlider.onValueChanged.AddListener((value) => OnBGMChanged(value));
        m_effectSlider.onValueChanged.AddListener((value) => OnEffectChanged(value));
    }


    void Start()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        m_bgmSlider.value = Prep.volumeBGM;
        m_effectSlider.value = Prep.volumeEffect;

        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.Option));


    }


    public void OnBGMChanged(float value)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        m_bgmText.text = string.Format("{0:f0}", value * 100f);
        m_bgmSlider.value = value;
        Prep.volumeBGM = value;
        PlayerPrefs.SetFloat("VolumeBGM", value);
        SoundManager.GetInstance.setVolume(TYPE_SOUND.BGM, value);
//        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
    }


    public void OnEffectChanged(float value)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        m_effectText.text = string.Format("{0:f0}", value * 100f);
        m_effectSlider.value = value;
        Prep.volumeEffect = value;
        PlayerPrefs.SetFloat("VolumeEffect", value);
        SoundManager.GetInstance.setVolume(TYPE_SOUND.EFFECT, value);
//        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
    }

    protected override void OnDisable()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
        base.OnDisable();
    }

    public void OnQuestionnaireClicked()
    {
        Application.OpenURL("https://goo.gl/forms/X8mabjXaMrryool43");
    }

    public void OnEmailClicked()
    {
        string mailto = "liztystk@gmail.com";
        string subject = EscapeURL("버그 / 문의사항");
        string body = EscapeURL
            (
                "이 곳에 내용을 작성해주세요.\n\n\n\n" +
                "________" +
                "Device Model : " + SystemInfo.deviceModel + "\n\n" +
                "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
                "________"
            );
 
        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }
 
    private string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
    
    //출처: http://minhyeokism.tistory.com/46 [programmer-dominic.kim]
}


