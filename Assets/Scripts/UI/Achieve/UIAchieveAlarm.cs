using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class UIAchieveAlarm : MonoBehaviour
{
    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_awardImage;

    [SerializeField]
    Text m_awardText;

    Coroutine m_coroutine;
    float m_timer = 0f;

    const float maxTime = 3f;

    RectTransform m_rectTransform;

    //Vector2

    //Vector2 closeMaxVector2;
    //Vector2 closeMinVector2;

    //Vector2 openMaxVector2;
    //Vector2 openMinVector2;

    //const float closeMax = ;
    //const float closeMin = ;

    //const float openMax = ;
    //const float openMin = ;

    const float openPos = 0f;
    const float closePos = 170f;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();

        m_rectTransform.anchoredPosition = new Vector3(0f, closePos);

    }

    //업적 검색하기
    //업적을 1개 이상 달성했으면 알람 보이기
    public void achieveUpdate()
    {
        //아직 사용하지 않음
        return;
        //달성한 업적 검색하기
        //조건 타임을 기준으로 검색
        //알람에서 떴으면 알람 리스트에 삽입

        for (int i = 0; i > Enum.GetValues(typeof(Achieve.TYPE_ACHIEVE_CATEGORY)).Length; i++)
        {
            IEnumerator enumerator = AchieveManager.GetInstance.getAchieveList((Achieve.TYPE_ACHIEVE_CATEGORY)i);

            while (enumerator.MoveNext())
            {

                Achieve achieve = enumerator.Current as Achieve;
                //            Debug.Log("achieve");

                //업적을 획득하지 않았으면
                    //                Debug.Log("achieve1");
                //업적 값에 도달했고 업적을 획득하지 않았으면
                if (achieve.valueRate() >= 1f && !achieve.isSuccess())
                {
                    Debug.Log("업적 달성 : " + achieve.key);
                    //갱신
//                    Account.GetInstance.accAchieve.setAchieve(achieve.key);

                    setAlarm(achieve);
                }
            }
        }
    }

    //public void achieveUpdate(Achieve.TYPE_ACHIEVE typeAchieve)
    //{

    //    //카테고리에 따라 업적 알람 알려주기

    //    IEnumerator enumerator = AchieveManager.GetInstance.getAchieveList(typeAchieve);

    //    while (enumerator.MoveNext())
    //    {
    //        Achieve achieve = enumerator.Current as Achieve;

    //        if (!achieve.isSuccess() && achieve.valueRate() >= 1f)
    //        {
    //            //알람 띄우기
    //            setAlarm(achieve);
    //            break;
    //        }
    //    }        
    //}


    public void setAlarm(Achieve achieve)
    {
        m_image.sprite = achieve.icon;
        m_nameText.text = achieve.name;
        m_awardImage.sprite = null;
        m_awardText.text = achieve.awardValue.ToString();

        if (m_coroutine == null)
            m_coroutine = StartCoroutine(awardCoroutine());

        //달성 파티클 및 효과음 삽입 필요
    }

    IEnumerator awardCoroutine()
    {
        m_timer = 0f;
        float setPos = closePos;
        float increasePos = closePos * Prep.frameTime * maxTime;
//        Debug.Log("increasePos : " + increasePos);
        while (m_timer <= maxTime)
        {
//            Debug.Log("set0");
            setPos -= increasePos;
            if (setPos <= 0f)
                setPos = 0f;
            m_rectTransform.anchoredPosition = new Vector3(0f, setPos);
            m_timer += Prep.frameTime;
            yield return new WaitForSeconds(Prep.frameTime);
        }
        //아래로
        
        m_timer = 0f;
        while (m_timer <= maxTime)
        {
//            Debug.Log("set1");
            m_timer += Prep.frameTime;
            yield return new WaitForSeconds(Prep.frameTime);
        }

        //위로
        m_timer = 0f;
        while (m_timer < maxTime)
        {
//            Debug.Log("set2");
            setPos += increasePos;
            if (setPos >= closePos)
                setPos = closePos;
            m_rectTransform.anchoredPosition = new Vector3(0f, setPos);

            m_timer += Prep.frameTime;
            yield return new WaitForSeconds(Prep.frameTime);
        }

        m_coroutine = null;
    }

}

