using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAreaMap : MonoBehaviour
{
//    [SerializeField]
//    UIStageMap m_uiStageMap;

    [SerializeField]
    Transform m_mapTransform;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Transform m_contentTransform;

    [SerializeField]
    UIMissionView m_uiMissionView;

    [SerializeField]
    UIMissionAreaButton m_uiAreaBtn;

    List<UIMissionAreaButton> uiBtnList = new List<UIMissionAreaButton>();


    void Awake()
    {
        m_uiMissionView.prevAreaEvent += getPrevArea;
        m_uiMissionView.nextAreaEvent += getNextArea;
    }


    public void setArea(World world)
    {
        gameObject.SetActive(true);
        m_image.sprite = world.image;
        areaUpdate(world);
    }

    //월드맵에 따라 포인트 생성
    void areaUpdate(World world){


        Debug.Log("Pos : " + m_mapTransform.position);

        //맵 중심으로 벗어났을 때 그만큼 버튼도 벗어나게 제작해야 함
        //

        //현재 메인 임무 가져오기
        Stage nowStage = WorldManager.GetInstance.getStage(Account.GetInstance.accSinario.stageKey);

//        IEnumerator enumarator = WorldManager.GetInstance.areaValues;
        IEnumerator enumarator = WorldManager.GetInstance.getAreaEnumerator(world.key);
        
        while (enumarator.MoveNext())
        {

            

            Area area = enumarator.Current as Area;

            //현재 클리어한 미션까지 만들기
            //스테이지가 1 이상 있으면 지역 생성
            if (Prep.isAllStage || WorldManager.GetInstance.getAreaInStageCount(area.key) > 0)
            {
                UIMissionAreaButton uiBtn = Instantiate(m_uiAreaBtn);
                uiBtn.setBtn(area, m_mapTransform.position - transform.position);
                uiBtn.transform.SetParent(m_mapTransform);
                uiBtn.transform.localScale = Vector2.one;
                uiBtn.areaInfoEvent += m_uiMissionView.setArea;
                uiBtnList.Add(uiBtn);
           
                        
                //현재 클리어된 미션까지 가져오기
                if (!Prep.isAllStage)
                {
                    if (nowStage != null)
                    {
                        //메인미션 있는 곳으로 이동
                        if (nowStage.areaKey == area.key)
                        {
                            //하이라이트 보이기 - 메인, 긴급
                            uiBtn.setAlarm(UIMissionButton.TYPE_ALARM.Main);

                            //해당 미션으로 위치 이동
                            m_contentTransform.GetComponent<RectTransform>().anchoredPosition = 
                                new Vector2(
                                    -uiBtn.GetComponent<RectTransform>().anchoredPosition.x, 
                                    -uiBtn.GetComponent<RectTransform>().anchoredPosition.y
                                    );
                            break;
                        }
                    }
                }
            }
        }

        //긴급미션 알람 보이기

    }

    /// <summary>
    /// 전 지역 가져오기
    /// </summary>
    /// <param name="nowArea"></param>
    /// <returns></returns>
    Area getPrevArea(Area nowArea)
    {
        int index = getAreaIndex(nowArea);

        if(index - 1 < 0)
            return null;

        return uiBtnList[index - 1].area;
    }

    /// <summary>
    /// 다음 지역 가져오기
    /// </summary>
    /// <param name="nowArea"></param>
    /// <returns></returns>
    Area getNextArea(Area nowArea)
    {
        int index = getAreaIndex(nowArea);

        if (index + 1 >= uiBtnList.Count)
            return null;

        return uiBtnList[index + 1].area;

    }

    int getAreaIndex(Area nowArea)
    {
        int index = 0;
        while (uiBtnList.Count > index)
        {
            if (uiBtnList[index].isCompare(nowArea))
                break;
            index++;
        }
        return index;
    }


    void OnDisable()
    {
        for (int i = uiBtnList.Count - 1; i >= 0; i--)
        {
            Destroy(uiBtnList[i].gameObject);
        }
        uiBtnList.Clear();
    }


    
}

