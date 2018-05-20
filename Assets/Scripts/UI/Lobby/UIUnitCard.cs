using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIUnitCard : MonoBehaviour //, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public delegate void UnitInfoDelegate(ICard iCard);
    public event UnitInfoDelegate unitInfoEvent;

//    UIBarracks m_uiBarracks;

    [SerializeField]
    Image m_icon;

    [SerializeField]
    Transform m_infoTransform;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_populationText;

    [SerializeField]
    Text m_munitionsText;
    
    ICard m_iCard;

    Toggle m_toggle;

    public Toggle toggle { get { return m_toggle; } }
    public ICard iCard { get { return m_iCard; } }


    void Awake()
    {
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener((isOn) => OnUnitCardClicked(isOn));
    }

    /// <summary>
    /// 인구 가져오기
    /// </summary>
    /// <returns></returns>
    public int getPopulation()
    {
        if (iCard != null)
        {
            return iCard.population;
        }
        return 0;
    }

    public void setUnitCard(ICard iCard)
    {
        m_iCard = iCard;

        if (m_iCard == null)
        {
            m_icon.sprite = null;
            m_icon.gameObject.SetActive(false);
            m_infoTransform.gameObject.SetActive(false);
            GetComponent<Toggle>().enabled = false;
        }
        else
        {
            m_icon.sprite = iCard.icon;
            m_icon.gameObject.SetActive(true);
            m_infoTransform.gameObject.SetActive(true);

            m_nameText.text = iCard.name;
            m_nameText.color = Color.white;
            m_levelText.text = string.Format("Lv {0}", iCard.level);
            m_levelText.color = Color.white;
            m_populationText.text = string.Format("{0}", iCard.population);
            m_populationText.color = Color.white;
            m_munitionsText.text = string.Format("{0}", iCard.munitions);
            m_munitionsText.color = Color.white;
            GetComponent<Toggle>().enabled = true;
        }
    }

   

    //public void setParent(UIBarracks uiBarracks)
    //{
    //    m_uiBarracks = uiBarracks;
    //}


    public void OnUnitCardClicked(bool isOn)
    {
        if (isOn)
        {

            if (m_iCard != null)
            {
                UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
                unitInfoEvent(m_iCard);
            }
        }

    }

//    //시작
//    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
//    {
//        //현재 기억하고 있는 오브젝트 가리기
//        //자유 셀 보여주기

////        m_uiBarracks.beginUnitCard(m_unitCard);
//        GetComponent<CanvasGroup>().blocksRaycasts = false;
//    }


//    //끝
//    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
//    {

//        GetComponent<CanvasGroup>().blocksRaycasts = true;

////        m_uiBarracks.endUnitCard();
//        //자유 셀 가리기
//        //현재 위치 판정

//        //대기
//        //위치 변경 불가 - 현재 위치로 변경
//        //대기위치 - 현재 위치로
//        //막사위치 - 현재 위치로



//        //종료 - 현재 위치가 어디인지 확인
//        //대기 슬롯이면 삽입
//        //막사 슬롯이면 삽입
//    }


//    //움직임
//    void IDragHandler.OnDrag(PointerEventData eventData)
//    {
//        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

////        m_uiBarracks.dragUnitCard();
//        //자유 카드가 마우스를 따라 움직여야 함
//        //현재 위치 이동
//        //대기, 막사 이동
//    }


}

