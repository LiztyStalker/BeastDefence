using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMain : MonoBehaviour
{


    [SerializeField]
    Text m_versionText;

    SoundPlay m_soundPlay;

    public SoundPlay soundPlay
    {
        get
        {
            if (m_soundPlay == null)
            {
                m_soundPlay = GetComponent<SoundPlay>();
                if (m_soundPlay == null)
                    m_soundPlay = gameObject.AddComponent<SoundPlay>();
            }

            return m_soundPlay;
        }
    }

    void Awake()
    {
        m_versionText.text = Application.version;

        Prep.volumeBGM = PlayerPrefs.GetFloat("VolumeBGM", 1f);
        Prep.volumeEffect = PlayerPrefs.GetFloat("VolumeEffect", 1f);

        soundPlay.audioPlay("BGMMain", TYPE_SOUND.BGM);


    }


    public void OnEnterClicked()
    {
        //UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        Account.GetInstance.nextScene = Prep.sceneLobby;
        SceneManager.LoadScene(Prep.sceneLoad);
    }
}

