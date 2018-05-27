using System.Collections.Generic;
using UnityEngine;


public class UIPanelManager : SingletonClass<UIPanelManager>
{

    IRootPanel m_root;
    Stack<UIPanel> m_panelStack = new Stack<UIPanel>();
        
    public IRootPanel root { get { return m_root; } }

    public void setRoot(IRootPanel iRoot)
    {
        m_root = iRoot;
    }

    public int panelCount()
    {
        return m_panelStack.Count;
    }

    /// <summary>
    /// 현재 패널 가져오기
    /// </summary>
    /// <returns></returns>
    public UIPanel nowPanel()
    {
        if(m_panelStack.Count > 0)
            return m_panelStack.Peek();
        return null;
    }

    /// <summary>
    /// 패널 집어넣기
    /// </summary>
    /// <param name="panel"></param>
    public void pushPanel(UIPanel panel)
    {
        m_panelStack.Push(panel);
//            Debug.Log("push : " + panel + " " + m_panelStack.Peek());
    }

    /// <summary>
    /// 패널 빼기
    /// </summary>
    public void popPanel()
    {
        if(m_panelStack.Count > 0)
            m_panelStack.Pop();
    }

    /// <summary>
    /// 패널 뒤로가기
    /// </summary>
    public UIPanel backPanel()
    {
        nowPanel().closePanel();
        return nowPanel();
    }


    /// <summary>
    /// 해당 패널로 이동하기
    /// null은 처음으로
    /// </summary>
    /// <param name="uiPanel"></param>
    /// <returns></returns>
    public UIPanel moveIndex(UIPanel uiPanel)
    {

        if (uiPanel == null)
            uiPanel = (UIPanel)root;

        while(panelCount() > 0){
            if (uiPanel.GetType() == m_panelStack.Peek().GetType())
            {
                return m_panelStack.Peek();
            }
            m_panelStack.Peek().closePanel();
        }
        return null;

    }
}


