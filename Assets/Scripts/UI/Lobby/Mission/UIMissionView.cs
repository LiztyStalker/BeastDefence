using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defence.CharacterPackage;

public class UIMissionView : UIPanel
{


    public delegate Area PrevAreaDelegate(Area nowArea);
    public delegate Area NextAreaDelegate(Area nowArea);

    public event PrevAreaDelegate prevAreaEvent;
    public event NextAreaDelegate nextAreaEvent;

    //이름
    //설명 아이콘
    //미션 설명
    //노멀이지하드
    //보상
    //적군 덱
    //왼쪽 오른쪽 이동
    //


    [SerializeField]
    Text m_nameText;

    [SerializeField]
    UIBarracks m_uiBarracks;
    
    [SerializeField]
    UIMissionList m_uiMissionList;

    [SerializeField]
    UIMissionInfo m_uiMissionInfo;
    
    [SerializeField]
    Button m_leftBtn;

    [SerializeField]
    Button m_rightBtn;

    [SerializeField]
    Button m_nextBtn;

    [SerializeField]
    GameObject m_emptyObject;

    Area m_area;

    int m_index = 0;

    void Awake()
    {
        m_leftBtn.onClick.AddListener(() => OnLeftClicked());
        m_rightBtn.onClick.AddListener(() => OnRightClicked());

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
        

    }
    
    public void setArea(Area area)
    {
        openPanel(null);

        viewPanel(area);

    }


    void viewPanel(Area area)
    {
        m_area = area;

        m_uiMissionList.setArea(area, stageClickedEvent);

        if (m_uiMissionInfo.gameObject.activeSelf)
        {
            m_uiMissionInfo.gameObject.SetActive(false);
        }

        viewPanel(area.name);

    }

    void OnLeftClicked()
    {


        //미션이면 전 미션
        if(m_uiMissionInfo.gameObject.activeSelf)
        {

            Stage stage = m_uiMissionList.getPrevStage(m_uiMissionInfo.getStage());

            if (stage != null)
                m_uiMissionInfo.setStage(stage);
        }
        //지역이면 다음 지역
        else if (m_uiMissionList.gameObject.activeSelf)
        {
            Area area = prevAreaEvent(m_area);

            //상위 지역의 전 지역
            //전 지역 변환 후 삽입
            if (area != null)
                viewPanel(area);
            //전지역 유무 판별
        }


        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        //clear();
        //전 미션
        //전 미션이 없으면 사라짐
    }

    void OnRightClicked()
    {
        //지역이면 다음 지역
        //미션이면 다음 미션
        if(m_uiMissionInfo.gameObject.activeSelf)
        {
            Stage stage = m_uiMissionList.getNextStage(m_uiMissionInfo.getStage());

            if (stage != null)
                m_uiMissionInfo.setStage(stage);
        }
        else if (m_uiMissionList.gameObject.activeSelf)
        {
            //다음 지역 변환 후 삽입
            Area area = nextAreaEvent(m_area);

            if(area != null)
                viewPanel(area);

//                m_uiMissionList.setArea(area);

            //다음지역 유무 판별
        }

        //clear();
        //다음 미션
        //다음 미션이 없으면 사라짐
        //진행할 수 없으면 못 누름.

        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

    }

    /// <summary>
    /// 다음 이동 - 정보
    /// </summary>
    public void stageClickedEvent(Stage stage)
    {
                //지역이면 정보 보이기
//        Stage stage = m_uiMissionList.getSelectStage();

        if (stage == null)
        {
            Debug.LogWarning("스테이지를 선택하지 않았음");
            return;
        }

        m_uiMissionInfo.setStage(stage);
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.FLIP);

        viewPanel(stage.name);

    }


    /// <summary>
    /// 다음 이동 - 배럭
    /// </summary>
    public void OnEnterClicked()
    {


        //정보이면 다음 보이기
        
        Stage stage = m_uiMissionInfo.getStage();

        if (stage == null)
        {
            Debug.LogWarning("스테이지 오류");
            return;
        }

        m_uiBarracks.setSinario(stage);
        m_uiBarracks.openPanel(this);
        Account.GetInstance.accSinario.nowStage = stage;
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);



//        m_uiBarracks.openPanel(this);
//        m_uiBarracks.setSinario(m_stage);

//        Account.GetInstance.accSinario.nowStage = m_stage;
        //병영 덱 구성창 나옴
    }


    void viewPanel(string name)
    {
        if (m_uiMissionInfo.gameObject.activeSelf)
        {
            m_nameText.text = name;
            m_emptyObject.SetActive(false);
            m_nextBtn.gameObject.SetActive(true);

        }
        //지역이면 정보 보이기
        else if (m_uiMissionList.gameObject.activeSelf)
        {
            m_nameText.text = name;
            m_emptyObject.SetActive(true);
            m_nextBtn.gameObject.SetActive(false);
        }
    }


    public void OnBackClicked()
    {

        //정보이면 정보 닫기
        if (m_uiMissionInfo.gameObject.activeSelf)
        {
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.FOLD);
            m_uiMissionInfo.gameObject.SetActive(false);
            viewPanel(m_area.name);
        }
        //리스트이면 닫기
        else
        {
            closePanel();
        }


    }

    public override void closePanel()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.closePanel();
    }



    //void clear()
    //{
    //    for (int i = m_cardSummaryList.Count - 1; i >= 0; i--)
    //    {
    //        Destroy(m_cardSummaryList[i].gameObject);
    //    }

    //    m_cardSummaryList.Clear();


    //    for (int i = m_missionAwardBtnList.Count - 1; i >= 0; i--)
    //    {
    //        Destroy(m_missionAwardBtnList[i].gameObject);
    //    }

    //    m_missionAwardBtnList.Clear();
    //}
}

