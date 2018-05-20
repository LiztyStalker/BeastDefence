using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIContents : UIPanel
{
    //자막 관리

    public delegate void ContentsDelegate();
    public event ContentsDelegate contentsEvent;

    [SerializeField]
    UIContentsView[] m_uiContentsView;

    //자막 리스트

    List<Contents> contentsList;

    int index;

    public bool setContents(string stageKey, Contents.TYPE_CONTENTS_EVENT typeEvent)
    {

        contentsList = ContentsManager.GetInstance.getContents(stageKey, typeEvent);

        index = -1;

        if (contentsList != null)
        {
            if (contentsList.Count > 0)
            {
                //모든 대사 닫기
                foreach (UIContentsView uiContentsView in m_uiContentsView)
                {
                    uiContentsView.gameObject.SetActive(false);
                }
//                Debug.Log("contentView");
                //대사 실행하기
                OnNextClicked();
                //            m_uiContentsView[0].setContents(m_contentsList[m_index]);
                //            m_uiContentsView[0].gameObject.SetActive(true);
                return true;
            }
        }

//        contentsEvent();
        return false;
        //자막 초기화하기
        //순차대로 자막 보여주기
    }


    public void OnSkipClicked()
    {
        //바로 종료
        closePanel();
    }
    public void OnNextClicked()
    {
        index++;
        //다음 컨텐츠 가져오기
        //없으면 종료
        if (contentsList.Count > index)
        {
            int viewIndex = (int)contentsList[index].typeContentsPos;
            if (viewIndex < m_uiContentsView.Length)
            {

                foreach (UIContentsView uiConView in m_uiContentsView)
                    uiConView.gameObject.SetActive(false);

                m_uiContentsView[viewIndex].setContents(contentsList[index]);
                m_uiContentsView[viewIndex].gameObject.SetActive(true);
            }
            else
            {
                Prep.LogWarning(viewIndex.ToString(), "enum 인덱스가 벗어났습니다", GetType());
                closePanel();
            }
        }
        else
        {
            closePanel();
        }

    }


    public void closePanel()
    {
        contentsEvent();
        gameObject.SetActive(false);
    }
}

