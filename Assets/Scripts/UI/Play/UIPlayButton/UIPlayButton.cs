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

    int m_index;

    protected int index { get { return m_index; } set { m_index = value; } }
}

