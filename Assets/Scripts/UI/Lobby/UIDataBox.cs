using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDataBox : MonoBehaviour
{
    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;


    [SerializeField]
    Text m_typeText;

    [SerializeField]
    Text m_cooltimeText;

    [SerializeField]
    Text m_rateText;

    [SerializeField]
    Text m_contentsText;

    Coroutine m_coroutine;

    void Awake()
    {
        close();
    }

    public void setSkillData(Skill skill, int level)
    {


        if (skill != null)
        {

            //화면 오른쪽을 선택한 경우
            if(Screen.width / 2 > (int)Input.mousePosition.x)
                GetComponent<RectTransform>().pivot = Vector2.right;
            else
                GetComponent<RectTransform>().pivot = Vector2.one;

            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            m_nameText.text = skill.name;
            m_typeText.text = skill.typeSkillPlay.ToString();
            m_levelText.text = string.Format("{0}", (level / 10) + 1);
            m_cooltimeText.text = string.Format("{0}", skill.coolTime);
            m_rateText.text = string.Format("{0}", skill.skillRate);
            m_contentsText.text = skill.contents;
            gameObject.SetActive(true);

            if (m_coroutine != null)
                StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(dataViewCoroutine());
        }
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

