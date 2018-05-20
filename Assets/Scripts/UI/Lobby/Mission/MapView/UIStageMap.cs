using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용 안함
public class UIStageMap : MonoBehaviour
{
    [SerializeField]
    UIMissionInfo m_uiMissionInfo;

    [SerializeField]
    Transform m_mapTransform;

    [SerializeField]
    UIMissionStageButton m_uiStageButton;


    List<UIMissionStageButton> m_uiBtnList = new List<UIMissionStageButton>();


    //스테이지맵에 따라 버튼 생성

    public void setStage(Area area)
    {
        gameObject.SetActive(true);
        stageUpdate(area);
    }

    void stageUpdate(Area area)
    {


//        List<Stage> stages = new List<Stage>();


//        stages.Add(new Stage("a", "a", null, Vector2.zero, 1, 1, 1, "", "", null, ""));
//        stages.Add(new Stage("b", "b", null, Vector2.zero, 1, 1, 1, "", "", null, ""));
//        stages.Add(new Stage("c", "c", null, Vector2.zero, 1, 1, 1, "", "", null, ""));
//        stages.Add(new Stage("d", "d", null, Vector2.zero, 1, 1, 1, "", "", null, ""));
//        stages.Add(new Stage("e", "e", null, Vector2.zero, 1, 1, 1, "", "", null, ""));



//        IEnumerator enumarator = stages.GetEnumerator(); //WorldManager.GetInstance.missionValues;
        IEnumerator enumarator = WorldManager.GetInstance.getStageEnumerator(area.key);

        if (enumarator != null)
        {
            while (enumarator.MoveNext())
            {
                UIMissionStageButton uiBtn = Instantiate(m_uiStageButton);
                Stage stage = enumarator.Current as Stage;

                uiBtn.setBtn(stage);
                uiBtn.transform.SetParent(m_mapTransform);
                uiBtn.transform.localScale = Vector2.one;
                uiBtn.stageInfoEvent += m_uiMissionInfo.setStage;
                m_uiBtnList.Add(uiBtn);

                //현재 미션까지 만들기
                if (stage.key == Account.GetInstance.accSinario.stageKey)
                    break;
            }
        }
    }

    void OnDisable()
    {
        for (int i = m_uiBtnList.Count - 1; i >= 0; i--)
        {
            Destroy(m_uiBtnList[i].gameObject);
        }
        m_uiBtnList.Clear();
    }
    
    

}
