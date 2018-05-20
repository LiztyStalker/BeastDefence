using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICardSelectButton : MonoBehaviour
{

    public delegate void UnitCardSelectDelegate(Unit unit, int index);
    public event UnitCardSelectDelegate selectEvent;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    Unit unit;
    int index;

    const float rotateTime = 0.5f;
    const float rotateLimit = 90f;
    bool isLoop = true;
    bool isFlip = false;

    Coroutine coroutine;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }


    public void setUnit(string[] unitkeys, int index)
    {
        this.index = index;
        unit = UnitManager.GetInstance.getRandomUnit(unitkeys);

//        Debug.Log("index : " + unit.name);

        if (unit != null)
        {
            m_nameText.text = unit.name;
            m_image.sprite = unit.icon;
        }
        else
        {
            m_nameText.text = "";
            m_image.sprite = null;
        }
    }

    public void OnClicked()
    {

        GetComponent<Button>().enabled = false;
        //카드 랜덤 가져오기

        //큐에 삽입하기
        selectEvent(unit, index);
    }


}

