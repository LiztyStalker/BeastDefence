using System;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField]
    UIName m_uiName;

    [SerializeField]
    UIIndicator m_uiIndicator;

    

    public UIName uiName { get { return m_uiName; } }
    public UIIndicator uiIndicator { get { return m_uiIndicator; } }

    void Awake()
    {
        //튜토리얼 값 가져오기
    }

    
    void Update()
    {
        //처음 시작 임무만 가능
        //스테이지1-1 클리어 - 
        //스테이지1-2 클리어 - 
        //스테이지1-3 클리어 - 영웅사용
        //모두 사용 가능
        //병사 관리, 뽑기, 개발
    }


    public void nextTutorial()
    {
        //다음 단계 진행
    }

    //튜토리얼 진행
}

