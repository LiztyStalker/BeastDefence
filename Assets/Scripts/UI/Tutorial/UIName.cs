﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class UIName : MonoBehaviour
{
    [SerializeField]
    Button m_okButton;

    [SerializeField]
    InputField m_inputField;

    void Awake()
    {
        m_okButton.onClick.AddListener(() => OnClicked());
    }


    public void viewPanel()
    {
        gameObject.SetActive(true);
    }

    void OnClicked()
    {

        if(m_inputField.text.Length < 2)
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("계정이름이 짧습니다. 2칸 이상으로 적어주세요.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
        else
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg(string.Format("계정이름을 '{0}' 로 사용하시겠습니까?", m_inputField.text), TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, clicked);
    }

    void clicked()
    {
        Account.GetInstance.accData.setName(m_inputField.text);
        //튜토리얼 시작
        gameObject.SetActive(false);
    }
}
