using System;
using UnityEngine;

public delegate void MsgPanelDelegate(string msg);

public class UIPlayButton : MonoBehaviour
{
    public delegate bool IsNotUsedDelegate(string key);
    public delegate float RateCardTimeDelegate(ICard iCard);

//    public event IsNotUsedDelegate isNotUsedEvent;
//    public event RateCardTimeDelegate rateCardTimeEvent;
//    public event MsgPanelDelegate msgPanelEvent;

    UIDataBoxManager m_uiDataBox;

    protected float m_infoTime = 0f;
    protected bool m_isClicked = false;
    
    int m_index;

    protected int index { get { return m_index; } set { m_index = value; } }

    protected UIDataBoxManager uiDataBox
    {
        get
        {
            if (m_uiDataBox == null)
                m_uiDataBox = UIPanelManager.GetInstance.root.uiCommon.uiDataBox;
            return m_uiDataBox;
        }
    }
}

