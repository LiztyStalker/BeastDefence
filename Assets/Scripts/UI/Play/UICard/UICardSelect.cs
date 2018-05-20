using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardSelect : MonoBehaviour
{
    [SerializeField]
    Text m_countText;

    [SerializeField]
    UICardSelectButton[] m_cardBtns;

    [SerializeField]
    GameObject[] m_cardHighLights;

    [SerializeField]
    Button m_closeBtn;

    string[] unitkeys;
    int count;

    Animator animator;


    Dictionary<string, int> unitDic = new Dictionary<string, int>();
    

    void Awake()
    {

        unitDic.Clear();

        animator = GetComponent<Animator>();
        for (int i = 0; i < m_cardBtns.Length; i++)
        {
            m_cardBtns[i].GetComponent<Button>().onClick.AddListener(() => OnClicked());
            m_cardBtns[i].GetComponent<Button>().enabled = true;
            m_cardBtns[i].selectEvent += selectEvent;

        }

        foreach (GameObject obj in m_cardHighLights)
        {
            obj.SetActive(false);
        }

//        setCardSelect(10);
    }


    public void setCardSelect(string[] unitkeys, int count)
    {
        this.unitkeys = unitkeys;
        gameObject.SetActive(true);
        setCardSelect(count);


        for (int i = 0; i < m_cardBtns.Length; i++)
        {
            m_cardBtns[i].setUnit(unitkeys, i);
        }

    }

    void setCardSelect(int count)
    {
        this.count = count;
        animator.SetInteger("Count", count);
        m_countText.text = string.Format("{0}회 남음", count);
    }


    void OnClicked()
    {
        foreach (UICardSelectButton btn in m_cardBtns)
        {
            btn.GetComponent<Button>().enabled = false;
        }
    }

    void selectEvent(Unit unit, int index)
    {
        //유닛 추가하기

        if(unitDic.ContainsKey(unit.key))
            unitDic[unit.key]++;
        else
            unitDic.Add(unit.key, 1);

        m_cardHighLights[index].SetActive(true);
        animator.SetBool("isSelect", true);
        animator.SetInteger("Card", index);
        count--;
        setCardSelect(count);
    }


    void initSelect()
    {
        animator.SetBool("isSelect", false);
        for(int i = 0; i < m_cardBtns.Length; i++){
            m_cardBtns[i].GetComponent<Button>().enabled = true;
            m_cardBtns[i].setUnit(unitkeys, i);

        }

        foreach (GameObject obj in m_cardHighLights)
        {
            obj.SetActive(false);
        }


        //카드가 0이면 종료
    }

    public IEnumerator cardSelectCoroutine() 
    {
        while (gameObject.activeSelf)
        {
            yield return null;
        }
    }

    public Dictionary<string, int> getUnitDic()
    {
        return unitDic;
    }

    void closePanel()
    {
        gameObject.SetActive(false);
    }
}

