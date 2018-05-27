using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchieve : UIPanel
{
    //초기화
    //버튼 누를때 성공 및 실패 여부

    enum TYPE_FILTER { Total, Success, NonSuccess }


    [SerializeField]
    UIAchieveData m_uiAchieveData;

    [SerializeField]
    Transform m_achieveTransform;

    [SerializeField]
    Transform m_achieveBtnTransform;

    List<UIAchieveData> m_uiAchieveList = new List<UIAchieveData>();

    Toggle[] achieveToggle;

    void Awake()
    {
        achieveToggle = m_achieveBtnTransform.GetComponentsInChildren<Toggle>();
        if (achieveToggle != null)
        {
            foreach (Toggle toggle in achieveToggle)
            {
                toggle.onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
            }
        }

        if (achieveToggle.Length > 0)
            achieveToggle[0].isOn = true;
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
    }

    void initAchieveList(Achieve.TYPE_ACHIEVE_CATEGORY typeAchieveCategory)
    {
        clear();

        IEnumerator enumerator = AchieveManager.GetInstance.getAchieveList(typeAchieveCategory);

        while (enumerator.MoveNext())
        {
            Achieve achieve = enumerator.Current as Achieve;

            UIAchieveData uiAchieveData = Instantiate(m_uiAchieveData);
            uiAchieveData.setAchieve(achieve);
            uiAchieveData.transform.SetParent(m_achieveTransform);
            uiAchieveData.transform.localScale = Vector2.one;
            uiAchieveData.refleshAchieveDataEvent += refleshAchieveData;


            m_uiAchieveList.Add(uiAchieveData);
        }

        //달성 전 업적은 위로, 달성된 업적은 아래로
        foreach (UIAchieveData data in m_uiAchieveList)
        {
            if (data.isSuccess)
                data.transform.SetAsLastSibling();

            else if (data.valueRate >= 1f)
                data.transform.SetAsFirstSibling();
        }
    }




    //protected override void OnDisable()
    //{
    //    for (int i = m_uiAchieveList.Count - 1; i >= 0; i--)
    //    {
    //        Destroy(m_uiAchieveList[i].gameObject);
    //    }

    //    m_uiAchieveList.Clear();

    //    base.OnDisable();
    //}

    void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            if (achieveToggle != null)
            {
                for (int i = 0; i < achieveToggle.Length; i++)
                {
                    if (achieveToggle[i].isOn)
                    {
                        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

                        initAchieveList((Achieve.TYPE_ACHIEVE_CATEGORY)i);
//                        setFilter((TYPE_FILTER)i);
                    }
                }
            }
        }
    }

    void clear()
    {
        for (int i = m_uiAchieveList.Count - 1; i >= 0; i--)
        {
            Destroy(m_uiAchieveList[i].gameObject);
        }
        m_uiAchieveList.Clear();
    }

    /// <summary>
    /// 해당업적 새로고침
    /// </summary>
    /// <param name="achieve"></param>
    void refleshAchieveData(Achieve achieve)
    {

        OnValueChanged(true);
        //foreach (UIAchieveData data in m_uiAchieveList)
        //{
        //    if (data.key == achieve.key)
        //    {
        //        data.setAchieve(achieve);
        //        data.transform.SetAsLastSibling();
        //        break;
        //    }
        //}
    }

    public override void closePanel()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.closePanel();
    }

    //void setFilter(TYPE_FILTER typeFilter)
    //{
    //    switch (typeFilter)
    //    {
    //        case TYPE_FILTER.Total:
    //            foreach (UIAchieveData uiAchieve in m_uiAchieveList)
    //            {
    //                uiAchieve.gameObject.SetActive(true);
    //            }
    //            break;
    //        case TYPE_FILTER.Success:
    //            foreach (UIAchieveData uiAchieve in m_uiAchieveList)
    //            {
    //                if(uiAchieve.isSuccess)
    //                    uiAchieve.gameObject.SetActive(true);
    //                else
    //                    uiAchieve.gameObject.SetActive(false);
    //            }

    //            break;
    //        case TYPE_FILTER.NonSuccess:
    //            foreach (UIAchieveData uiAchieve in m_uiAchieveList)
    //            {
    //                if (uiAchieve.isSuccess)
    //                    uiAchieve.gameObject.SetActive(false);
    //                else
    //                    uiAchieve.gameObject.SetActive(true);
    //            }

    //            break;
    //        default:
    //            Prep.LogError(typeFilter.ToString(), "를 찾을 수 없습니다.", GetType());
    //            break;
    //    }
    //}



}

