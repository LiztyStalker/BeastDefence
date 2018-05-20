using UnityEngine;

public class UIEmployee : UIPanel
{
    [SerializeField]
    UIRecruit m_uiRecruit;

    [SerializeField]
    UIConscript m_uiConscript;

    public void OnRecruitClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
        m_uiRecruit.openPanel(this);
    }

    public void OnConscriptClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
        m_uiConscript.openPanel(this);
    }

    protected override void OnDisable()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.OnDisable();
    }
}

