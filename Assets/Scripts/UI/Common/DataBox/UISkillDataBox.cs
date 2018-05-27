using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkillDataBox : UIDataBox, IPointerClickHandler
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

//    Coroutine m_coroutine;



    //스킬설명
    //유닛설명
    //보상설명

    //void Awake()
    //{
    //    close();
    //}

    //void Start()
    //{
    //    if (m_coroutine != null)
    //        StopCoroutine(m_coroutine);
    //    m_coroutine = StartCoroutine(dataViewCoroutine());
    //}

    public void setSkillData(SkillCard skillCard)
    {
        setSkillData(skillCard.skill, skillCard.level);
    }

    public void setSkillData(SkillCard skillCard, Vector3 pos)
    {
        setSkillData(skillCard.skill, skillCard.level, pos);
    }

    public void setSkillData(Skill skill, int level)
    {
        setSkillData(skill, level, Input.mousePosition);
    }

    public void setSkillData(Skill skill, int level, Vector3 pos)
    {
        if (skill != null)
        {
            Vector2 pivot = Vector2.zero;

            //화면 오른쪽을 선택한 경우
            if (Screen.width / 2 > (int)Input.mousePosition.x)
                pivot.x = 0f;
            else
                pivot.x = 1f;

            if (Screen.height / 2 > (int)Input.mousePosition.y)
                pivot.y = 0f;
            else
                pivot.y = 1f;

            GetComponent<RectTransform>().pivot = pivot;

            //            Vector2 tmpPos = 

            GetComponent<RectTransform>().position = (Vector2)Camera.main.ScreenToWorldPoint(pos);


            //            transform.position = Camera.main.ScreenToWorldPoint(pos);

            //            GetComponent<RectTransform>().localPosition = 0f;


            m_nameText.text = skill.name;
            m_typeText.text = skill.typeSkillPlay.ToString();
            m_levelText.text = string.Format("{0}", (level / 10) + 1);
            m_cooltimeText.text = string.Format("{0}", skill.coolTime);
            m_rateText.text = string.Format("{0}", skill.skillRate);
            m_contentsText.text = skill.contents;

            if (gameObject.activeSelf)
                Start();
            else
                gameObject.SetActive(true);

        }
    }

    //IEnumerator dataViewCoroutine()
    //{
    //    yield return new WaitForSeconds(3f);
    //    m_coroutine = null;
    //    gameObject.SetActive(false);
    //}

    //public void close()
    //{
    //    if (m_coroutine != null)
    //        StopCoroutine(m_coroutine);
    //    m_coroutine = null;
    //    gameObject.SetActive(false);
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    close();
    //}
}

