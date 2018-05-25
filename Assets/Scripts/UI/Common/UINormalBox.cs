using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UINormalBox : MonoBehaviour
{
    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_contentsText;

    Coroutine m_coroutine;

    void Awake()
    {
        close();
    }

    public void setData(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeAwardCategory, string value)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        m_nameText.text = Prep.getTypeSinarioAwardCategory(typeAwardCategory);



        if (typeAwardCategory == SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.HCard ||
            typeAwardCategory == SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.NCard)
        {
            Unit unit = UnitManager.GetInstance.getUnit(value);
            if (unit != null)
            {
                value = unit.name;
            }
        }

        m_contentsText.text = value;
        gameObject.SetActive(true);

        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(dataViewCoroutine());
        
    }

    IEnumerator dataViewCoroutine()
    {
        yield return new WaitForSeconds(3f);
        m_coroutine = null;
        gameObject.SetActive(false);
    }

    public void close()
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = null;
        gameObject.SetActive(false);
    }
}

