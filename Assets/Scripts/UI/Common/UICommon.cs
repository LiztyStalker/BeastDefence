using System;
using UnityEngine;

public class UICommon : MonoBehaviour
{
    [SerializeField]
    UIMsg m_uiMsg;

    [SerializeField]
    UIOption m_uiOption;

    [SerializeField]
    UIDataBox m_uiDataBox;

    [SerializeField]
    UINormalBox m_uiNormalBox;

    [SerializeField]
    UIUnitInfomation m_uiUnitInfomation;

    [SerializeField]
    UIAchieveAlarm m_uiAchieveAlarm;

    [SerializeField]
    UIContents m_uiContents;

    SoundPlay m_btnSoundPlay;

    public UIMsg uiMsg { get { return m_uiMsg; } }
    public UIOption uiOption { get { return m_uiOption; } }
    public UIDataBox uiDataBox { get { return m_uiDataBox; } }
    public UINormalBox uiNormalBox { get { return m_uiNormalBox; } }
    public UIUnitInfomation uiUnitInfomation { get { return m_uiUnitInfomation; } }
    public UIAchieveAlarm uiAchieveAlarm { get { return m_uiAchieveAlarm; } }
    public UIContents uiContents { get { return m_uiContents; } }

    public SoundPlay btnSoundPlay { 
        get 
        {
            if (m_btnSoundPlay == null)
                m_btnSoundPlay = SoundManager.GetInstance.getSoundPlay(gameObject);
            return m_btnSoundPlay;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void setCamera()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    

}

