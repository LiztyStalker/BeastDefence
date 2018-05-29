using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMission : UIPanel
{
    //큰 맵
    //작은맵
    //

    //큰맵버튼
    //작은맵 버튼


    //큰맵 - 큰 시나리오로 보임
    [SerializeField]
    UIAreaMap m_uiAreaMap;


    protected override void OnEnable()
    {
        base.OnEnable();

        m_uiAreaMap.gameObject.SetActive(true);

        World world = WorldManager.GetInstance.getWorld("Zoogalopolis");
        if (world != null)
        {
            m_uiAreaMap.setArea(world);
        }

        //현재 클리어된 미션 새로고침

        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);

        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.World));

    }


    //뒤로
    public void OnBackButton()
    {
        closePanel();
    }

    public override void closePanel()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.closePanel();
    }

    


}

