using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UIDevelop : UIPanel
{

    [SerializeField]
    Toggle[] m_toggle;

    [SerializeField]
    UIDevelopButton m_uiDevelopButton;

    [SerializeField]
    Transform m_developPanel;

    [SerializeField]
    UIDevelopInfo m_uiDevelopInfo;

    [SerializeField]
    UIDevelopTech m_uiDevelopTech;

    //테크창 리스트
    List<UIDevelopTech> m_developTechList = new List<UIDevelopTech>();

    //테크 버튼 리스트
    List<UIDevelopButton> m_developBtnList = new List<UIDevelopButton>();

    //빈 오브젝트
    List<GameObject> m_emptyObject = new List<GameObject>();

    Develop.TYPE_DEVELOP_GROUP m_typeDevGroup;

    const float cellSize = 360f;
    
    bool m_isFirst = true;

    //테크 행렬
    Develop[][] developMatrix = new Develop[Prep.techCount][];


    void Awake()
    {
        m_uiDevelopInfo.developDataUpdateEvent += updateDevelop;
        

        //테크 최종 카운트 가져오기
        for (int i = 0; i < Prep.techCount; i++)
        {
            UIDevelopTech uiDevelopTech = Instantiate(m_uiDevelopTech);
            uiDevelopTech.setTech(i);
            uiDevelopTech.transform.SetParent(m_developPanel);
            uiDevelopTech.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, 0f);
            uiDevelopTech.GetComponent<RectTransform>().localPosition = Vector2.zero;
            uiDevelopTech.transform.localScale = Vector2.one;
//            uiDevelopTech.GetComponent<Image>();
            m_developTechList.Add(uiDevelopTech);
        }
        foreach (Toggle toggle in m_toggle)
        {
            toggle.onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
        }


        m_toggle[0].isOn = true;

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //초기화


        //개발 버튼 삽입
        //해당 타입에 따라서 개발 정보 보여주기
//        IEnumerator enumerator = DevelopManager.GetInstance.developValues;



//        while (enumerator.MoveNext())
//        {
//            UIDevelopButton devBtn = Instantiate(m_uiDevelopButton) as UIDevelopButton;
//            Develop dev = enumerator.Current as Develop;
//            //개발된 레벨 가져오기
//            int level = Account.GetInstance.accDevelop.getDevelopLevel(dev.key);

//            //셋팅
//            devBtn.setDevelop(dev, level);


//            //테크 레벨에 따라 삽입하기
//            devBtn.transform.SetParent(m_developTechList[dev.techLevel].transform);


//            //

////            devBtn.transform.SetParent(m_developPanel);
//            devBtn.transform.localScale = Vector2.one;
//            devBtn.developViewInfoEvent += m_uiDevelopInfo.setDevelop;

//            m_developBtnList.Add(devBtn);

//        }
    }

    public void setDevelop(Develop.TYPE_DEVELOP_GROUP typeDevGroup)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        //초기화
        clearDevelop();

        //타입에 따라 연구 보여주기
        IEnumerator enumerator = DevelopManager.GetInstance.getDevelopEnumerator(typeDevGroup);

        
        int maxLevel = DevelopManager.GetInstance.getDevelopMaxPosition(typeDevGroup);

        //매트릭스 생성
        for(int i = 0; i < developMatrix.Length; i++){
            developMatrix[i] = new Develop[maxLevel];
        }

        //매트릭스에 개발 데이터 삽입
        while (enumerator.MoveNext())
        {

            Develop dev = enumerator.Current as Develop;
            
            //매트릭스에 개발 데이터 삽입하기
            developMatrix[dev.techLevel][dev.position] = dev;

        }


        //빈 오브젝트 카운트
        int emptyCnt = 0;

        //매트릭스 기반 트리구조 제작
        for (int i = 0; i < developMatrix.Length; i++)
        {

            for (int j = 0; j < developMatrix[i].Length; j++)
            {

                if (developMatrix[i][j] != null)
                {
                    Develop dev = developMatrix[i][j];

                    UIDevelopButton devBtn = Instantiate(m_uiDevelopButton) as UIDevelopButton;
                    //개발된 레벨 가져오기
                    int level = Account.GetInstance.accDevelop.getDevelopLevel(dev.key);

                    //셋팅
                    devBtn.setDevelop(dev, level);


                    //테크 레벨에 따라 삽입하기
                    devBtn.transform.SetParent(m_developTechList[i].transform);


                    //

                    //            devBtn.transform.SetParent(m_developPanel);
                    devBtn.transform.localScale = Vector2.one;
                    devBtn.developViewInfoEvent += m_uiDevelopInfo.setDevelop;

                    m_developBtnList.Add(devBtn);
                }
                else
                {
                    //빈 오브젝트가 없으면 생성
                    if (emptyCnt >= m_emptyObject.Count)
                    {

                        GameObject gameObj = new GameObject();
                        gameObj.AddComponent<Image>();
                        gameObj.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                        m_emptyObject.Add(gameObj);
                    }

                    //배치
                    m_emptyObject[emptyCnt].SetActive(true);
                    m_emptyObject[emptyCnt].transform.SetParent(m_developTechList[i].transform);
                    m_emptyObject[emptyCnt].transform.SetAsLastSibling();
                    m_emptyObject[emptyCnt].transform.localScale = Vector2.one;
                    emptyCnt++;
                }
            }


            //

        }



        //빈 오브젝트가 남아돌면 정리
        for (int i = emptyCnt; i < m_emptyObject.Count; i++)
        {
            m_emptyObject[i].SetActive(false);
        }

            

    }


    void updateDevelop()
    {
        foreach (UIDevelopButton devBtn in m_developBtnList)
        {
            int level = Account.GetInstance.accDevelop.getDevelopLevel(devBtn.develop.key);

            //셋팅
            devBtn.setDevelop(devBtn.develop, level);
            //업데이트

        }
    }

    void clearDevelop()
    {
        //

        for (int i = m_developBtnList.Count - 1; i >= 0; i--)
        {
            Destroy(m_developBtnList[i].gameObject);
        }


        m_developBtnList.Clear();


    }


    void Update()
    {
        //개발 버튼 연결하기
        foreach (UIDevelopButton devBtn in m_developBtnList)
        {

            //부모 가져오기
            int index = 0;
            foreach (string parentKey in devBtn.develop.parentDic.Keys)
            {

                UIDevelopButton tmpBtn = m_developBtnList.Where(btn => btn.develop.key == parentKey).SingleOrDefault();

                if (tmpBtn != null)
                {
//                    Debug.Log("parentKey : " + tmpBtn.linkTransform.position);
                    devBtn.setLine(index, tmpBtn.linkTransform.position);
                }
                index++;
            }
        }
    }



    void dataUpdate(UIDevelopButton developBtn)
    {
        Develop dev = developBtn.develop;
        int level = Account.GetInstance.accDevelop.getDevelopLevel(dev.key);
        m_developBtnList.Find(devf => devf == developBtn).setDevelop(dev, level);
    }


    //protected override void OnDisable()
    //{
    //    for (int i = m_developBtnList.Count - 1; i >= 0; i--)
    //    {
    //        Destroy(m_developBtnList[i].gameObject);
    //    }

    //    m_developBtnList.Clear();

    //    base.OnDisable();
    //}

    //업데이트


    void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < m_toggle.Length; i++)
            {
                if (m_toggle[i].isOn)
                {
                    setDevelop((Develop.TYPE_DEVELOP_GROUP)i);
                    break;
                }
            }
        }
    }
}

