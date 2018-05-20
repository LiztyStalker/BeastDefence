using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICardSet : UIPanel
{
    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    bool isOpen = true;

    public void setUnit(Unit unit)
    {

//        GetComponent<Animator>()
        isOpen = true;

        if (unit != null)
        {
            m_nameText.text = unit.name;
            m_image.sprite = unit.icon;
        }
    }

    public IEnumerator waitCoroutine()
    {
        while(isOpen){
            yield return null;
        }
        closePanel();
        yield return null;
    }

    public void OnClicked()
    {
        isOpen = false;
    }
}

