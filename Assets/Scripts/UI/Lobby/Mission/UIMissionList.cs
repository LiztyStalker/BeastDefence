using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIMissionList : MonoBehaviour
{

    [SerializeField]
    UIMissionListButton m_uiMissionListButton;

    [SerializeField]
    Transform m_missionTransform;

    List<UIMissionListButton> dataList = new List<UIMissionListButton>();

//    ToggleGroup toggleGroup;

    UIUnitInfomation uiUnitInformation;

    public void setArea(Area area, UIMissionListButton.StageDelegate stageDel)
    {
        clear();

//        toggleGroup = m_missionTransform.GetComponent<ToggleGroup>();

//        uiUnitInformation = ((UILobby)UIPanelManager.GetInstance.root).uiUnitInformation;

        //스테이지 리스트 만들기
        //
        IEnumerator enumarator = WorldManager.GetInstance.getStageEnumerator(area.key);

        if (enumarator != null)
        {
            enumarator.Reset();
            while (enumarator.MoveNext())
            {
                Stage stage = enumarator.Current as Stage;

                //클리어한 메인퀘스트는 보이지 말기
                if (!Account.GetInstance.accSinario.isStage(stage.key))
                {
                    UIMissionListButton uiBtn = Instantiate(m_uiMissionListButton);

                    uiBtn.setStage(stage);
                    uiBtn.transform.SetParent(m_missionTransform);
                    uiBtn.transform.localScale = Vector2.one;
                    uiBtn.stageEvent += stageDel;
//                    uiBtn.GetComponent<Toggle>().group = toggleGroup;
                    //                uiBtn.unit  += uiUnitInformation.setSummary;
                    dataList.Add(uiBtn);

                    //메인 퀘스트와 긴급 퀘스트는 최상위로 올리기
                    if (stage.typeStage == Stage.TYPE_STAGE.Main ||
                        stage.typeStage == Stage.TYPE_STAGE.Warning
                        )
                        uiBtn.transform.SetAsFirstSibling();


                    //현재 미션까지 만들기
                    if (!Prep.isAllStage)
                    {
                        if (stage.key == Account.GetInstance.accSinario.stageKey)
                            break;
                    }
                }
            }
        }

        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.StageList));


    }

    /// <summary>
    /// 스테이지 선택
    /// </summary>
    /// <returns></returns>
    public Stage getSelectStage()
    {
        if (dataList.Count > 0)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].GetComponent<Toggle>().isOn)
                {
                    return dataList[i].getStage();
                }
            }
            //스테이지를 선택하세요
        }
        
        //리스트가 없음
        return null;
    }

    void clear()
    {
        for (int i = dataList.Count - 1; i >= 0; i--)
        {
            Destroy(dataList[i].gameObject);
        }

        dataList.Clear();
    }

    public Stage getPrevStage(Stage nowStage)
    {

        int index = getStageIndex(nowStage);

        if (index - 1 < 0)
            return null;
        
        return dataList[index - 1].getStage();
    }

    public Stage getNextStage(Stage nowStage)
    {
        int index = getStageIndex(nowStage);

        if (index + 1 >= dataList.Count)
            return null;

        return dataList[index + 1].getStage();
    }

    int getStageIndex(Stage nowStage)
    {
        int index = 0;
        while (dataList.Count > index)
        {
            if (dataList[index].getStage() == nowStage)
                break;
            index++;
        }
        return index;
    }


}

