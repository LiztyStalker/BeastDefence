using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIContents : UIPanel
{
    //자막 관리

    public delegate void ContentsDelegate();
    public event ContentsDelegate contentsEvent;

    [SerializeField]
    GameObject m_contentsImageObj;

    [SerializeField]
    Image m_contentsImage;

    [SerializeField]
    UIContentsView[] m_uiContentsView;

    //자막 리스트

    List<Contents> contentsList;

    int index;

    bool isContents = false;

//    Coroutine m_coroutine = null;

    //void Start()
    //{
    //    if (m_coroutine != null)
    //        StopCoroutine(m_coroutine);
    //    m_coroutine = StartCoroutine(contentsCoroutine());
    //}

    public IEnumerator contentsCoroutine(Contents.TYPE_CONTENTS_EVENT typeConEvent)
    {
//        Debug.Log("typeConEvent: " + typeConEvent);
        //현재 자막 시작
        
        isContents = setContents(typeConEvent);

        //콜백 받아야 함
        //
        Time.timeScale = 0f;
        
        while (isContents)
        {
            //            Debug.Log("contents");
            yield return null;
        }

        Time.timeScale = 1f;
        yield return null;
    }


    bool setContents(Contents.TYPE_CONTENTS_EVENT typeConEvent)
    {
//        if (Account.GetInstance.accSinario.nowStage != null)
//        {
            if (setContents(Account.GetInstance.accSinario.stageKey, typeConEvent))
            {
                Debug.Log("자막 찾음");
                openPanel(null);
                return true;
            }
//        }
        return false;
    }

    public void contentsCallBack()
    {
        isContents = false;
    }

    public bool setContents(string stageKey, Contents.TYPE_CONTENTS_EVENT typeConEvent)// Contents.TYPE_CONTENTS_EVENT typeEvent)
    {
        
        contentsList = ContentsManager.GetInstance.getContents(stageKey, typeConEvent);

        index = -1;

        
        if (contentsList != null)
        {
            if (contentsList.Count > 0)
            {

                //이미 사용한 콘텐츠가 있으면 스킵
                foreach (Contents contents in contentsList)
                {
                    if (Account.GetInstance.accSinario.isContents(contents.key))
                    {
                        return false;
                    }
                }

                //사용하지 않은 콘텐츠이면
                //콘텐츠 삽입하기
                Account.GetInstance.accSinario.addContents(contentsList);

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
            //
            int viewIndex = (int)contentsList[index].typeContentsPos;
            if (viewIndex < m_uiContentsView.Length)
            {

                foreach (UIContentsView uiConView in m_uiContentsView)
                    uiConView.gameObject.SetActive(false);


                m_uiContentsView[viewIndex].setContents(contentsList[index]);
                m_contentsImage.sprite = contentsList[index].image;
                if (m_contentsImage.sprite == null)
                {
                    m_contentsImageObj.SetActive(false);
                }
                else
                {
                    m_contentsImageObj.SetActive(true);
                }

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
//        contentsEvent();
        contentsCallBack();
        gameObject.SetActive(false);
    }
}

