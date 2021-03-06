﻿using UnityEngine;

public class UIEmployee : UIPanel
{
    [SerializeField]
    UIRecruit m_uiRecruit;

    [SerializeField]
    UIConscript m_uiConscript;

    protected override void OnEnable()
    {
        base.OnEnable();
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.Employee));

    }

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

