using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitDataBox : UIDataBox
{
    //간단한 유닛 정보
    //유닛 이름
    //공격타입
    //체력
    //공격력
    //설명

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_typeLine;

    [SerializeField]
    Text m_healthText;

    [SerializeField]
    Text m_attackText;

    [SerializeField]
    Text m_contentsText;



    public void setData(UnitCard unitCard, Vector3 pos)
    {
        if (unitCard != null)
        {
            m_nameText.text = unitCard.name;
            m_typeLine.text = "공격라인 : " + unitCard.typeLine.ToString();
            m_healthText.text = "체력 : " + unitCard.health.ToString();
            m_attackText.text = "공격력 : " + unitCard.attack.ToString();
            m_contentsText.text = unitCard.contents;
        }
        else
        {
            m_nameText.text = "-";
            m_typeLine.text = "-";
            m_healthText.text = "-";
            m_attackText.text = "-";
            m_contentsText.text = "-";
        }

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(pos);

        gameObject.SetActive(true);

        //if (m_coroutine != null)
        //    StopCoroutine(m_coroutine);
        //m_coroutine = StartCoroutine(dataViewCoroutine());

    }
}

