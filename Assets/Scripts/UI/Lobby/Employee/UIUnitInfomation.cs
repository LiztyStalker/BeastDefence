﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInfomation : UIPanel
{


    public delegate void RecruitDataUpdateDelegate(ISummary iSummary);
    public event RecruitDataUpdateDelegate recruitDataUpdateEvent;

    [SerializeField]
    UIUnitInfo m_uiUnitInfo;

    [SerializeField]
    GameObject m_employeeButton;

    ISummary m_iSummary;

    UIDataBox m_uiDataBox;
    
    void Awake()
    {
        m_uiDataBox = UIPanelManager.GetInstance.root.uiCommon.uiDataBox;
    }

    /// <summary>
    /// 요약카드 설정 - 구입 정보
    /// </summary>
    /// <param name="iSummary"></param>
    /// <param name="isCost"></param>
    public void setSummary(ISummary iSummary, bool isCost = true)
    {
        if (iSummary.unit != null)
        {
            openPanel(null);
            m_iSummary = iSummary;
            m_uiUnitInfo.setUnit(iSummary.unit);
            m_employeeButton.SetActive(isCost);
        }
    }

    /// <summary>
    /// 요약카드 설정 - 지휘관 있음
    /// </summary>
    /// <param name="iSummary"></param>
    /// <param name="typeForce"></param>
    /// <param name="level"></param>
    public void setSummary(ISummary iSummary, TYPE_FORCE typeForce, int level)
    {
        if (iSummary.unit != null)
        {
            openPanel(null);
            m_iSummary = iSummary;
            m_uiUnitInfo.setUnitCard(new UnitCard(iSummary.unit, level), typeForce);
            m_employeeButton.SetActive(false);
        }
    }

    /// <summary>
    /// 요약카드 설정 - 지휘관 없음
    /// </summary>
    /// <param name="iSummary"></param>
    /// <param name="level"></param>
    public void setSummary(ISummary iSummary, int level)
    {
        if (iSummary.unit != null)
        {
            openPanel(null);
            m_iSummary = iSummary;
            m_uiUnitInfo.setUnitCard(new UnitCard(iSummary.unit, level));
            m_employeeButton.SetActive(false);
        }
    }

    /// <summary>
    /// 고용하기
    /// </summary>
    public void OnEmployeeClicked()
    {
        //고용
        if (Account.GetInstance.accData.isValue(m_iSummary.unit.cost, TYPE_ACCOUNT_CATEGORY.Fruit))
        {
            m_uiDataBox.close();
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("모집하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, employeeEvent);
//            ((UILobby)UIPanelManager.GetInstance.root).uiMsg.setMsg("모집하시겠습니까?", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, employeeEvent);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("열매가 부족합니다", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
//            ((UILobby)UIPanelManager.GetInstance.root).uiMsg.setMsg("열매가 부족합니다", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }

    }

    void employeeEvent()
    {
        Account.GetInstance.accData.useValue(m_iSummary.unit.cost, TYPE_ACCOUNT_CATEGORY.Fruit);
        Account.GetInstance.accUnit.addUnit(m_iSummary.unit);
        recruitDataUpdateEvent(m_iSummary);
        closePanel();
        UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("모집 완료", TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK);
    }



    public override void closePanel()
    {
        m_uiDataBox.close();
        base.closePanel();
    }
    
}
