using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Defence.CommanderPackage;

public class UIMap : MonoBehaviour
{

    readonly Vector2 imageSize = new Vector2(15f, 15f);
    readonly Vector2 imagePos = new Vector2(0f, 8f);

    [SerializeField]
    Scrollbar m_mapScrollBar;

    [SerializeField]
    Image m_image;
    
    [SerializeField]
    RectTransform m_playerCastleRectTransform;

    [SerializeField]
    RectTransform m_cpuCastleRectTransform;

    CameraController cameraController;

    TYPE_MAP_SIZE typeMapSize;


    Queue<Image> imageQueue = new Queue<Image>();
    Dictionary<int, Image> useImageDic = new Dictionary<int, Image>();


    float m_scrollLength;


    bool m_isDrag = false;

    //맵 길이
    public float mapLength { get { return (((float)typeMapSize + 1f) * Prep.castlePos); } }


    void Awake()
    {
        
        m_mapScrollBar.onValueChanged.AddListener((value) => OnValueChanged(value));
        m_mapScrollBar.value = 0.5f;

        //Image 큐 쌓기
        for (int i = 0; i < Prep.unitActorObjFoolCnt; i++)
        {
            createImage();
        }

        //스크롤바 거리 구하기
        m_scrollLength = Vector2.Distance(m_cpuCastleRectTransform.localPosition, m_playerCastleRectTransform.localPosition);

    }

    /// <summary>
    /// 이미지 새로 생성하기
    /// </summary>
    /// <returns></returns>
    Image createImage()
    {
        Image image = Instantiate(m_image);
        image.transform.SetParent(m_mapScrollBar.transform);
        image.GetComponent<RectTransform>().sizeDelta = imageSize;
        image.transform.localScale = Vector2.one;
        image.gameObject.SetActive(false);
        imageQueue.Enqueue(image);
        return image;
    }


    public void setSize(TYPE_MAP_SIZE typeMapSize)
    {
        this.typeMapSize = typeMapSize;

        if(cameraController == null)
            cameraController = Camera.main.GetComponent<CameraController>();

        cameraController.setSize(typeMapSize);

        //화면크기 = 맵크기 + 카메라 최대 이동력
        m_mapScrollBar.size = Prep.castlePos / ((((float)typeMapSize + 1f) * Prep.castlePos) + Prep.cameraLimitPos);

        //센터

    }
    
    //유닛 위치에 맞춰서 이미지 그리고 이동하기
    //유닛과 동기화가 필요
    /// <summary>
    /// 유닛 연결하기
    /// </summary>
    /// <param name="unitActor"></param>
    public void linkUnit(UnitActor unitActor, UIController uiController)
    {
        //빈 이미지가 없으면 - 새로 생성
        if (imageQueue.Count <= 0)
            createImage();

        //이미지 가져오기
        Image image = imageQueue.Dequeue();       

        //스테이지 가져오기
        Stage stage = Account.GetInstance.accSinario.nowStage;

        //아이콘 색상 넣기

        //플레이어 또는 적 처음 포지션
        if (uiController is UIPlayer)
        {
            image.GetComponent<RectTransform>().localPosition = m_playerCastleRectTransform.localPosition;
            image.GetComponent<Image>().color = Prep.getForceColor(stage.typeForce);

        }
        else if (uiController is UICPU)
        {
            image.GetComponent<RectTransform>().localPosition = m_cpuCastleRectTransform.localPosition;

            Commander commander = CommanderManager.GetInstance.getCommander(stage.deck.commanderKey);
            if (commander != null)
                image.GetComponent<Image>().color = Prep.getForceColor(commander.typeForce);

        }
        else
            Prep.LogWarning(uiController.ToString(), "해당사항 없음", GetType());

//        Debug.Log("line : " + LayerMask.LayerToName(unitActor.layer));

        //라인위치
        if (LayerMask.NameToLayer("TopGndLine") == unitActor.layer || LayerMask.NameToLayer("TopAirLine") == unitActor.layer)
            image.GetComponent<RectTransform>().anchoredPosition += imagePos;
        else if (LayerMask.NameToLayer("BotGndLine") == unitActor.layer  || LayerMask.NameToLayer("BotAirLine") == unitActor.layer)
            image.GetComponent<RectTransform>().anchoredPosition -= imagePos;


        image.gameObject.SetActive(true);

        useImageDic.Add(unitActor.GetInstanceID(), image);
    }

    /// <summary>
    /// 유닛 해제하기
    /// </summary>
    /// <param name="unitActor"></param>
    public bool unlinkUnit(UnitActor unitActor)
    {
        int instanceID = unitActor.GetInstanceID();
        if (useImageDic.ContainsKey(instanceID))
        {
            Image image = useImageDic[instanceID];
            image.gameObject.SetActive(false);
            image.GetComponent<Image>().color = Prep.getForceColor(TYPE_FORCE.None);
            imageQueue.Enqueue(image);
            useImageDic.Remove(instanceID);
            return true;
        }
        else
        {
            Debug.LogWarning("이미지 반납 실패");
            //반납 실패
        }
        return false;
    }

    /// <summary>
    /// 유닛 위치 설정하기
    /// </summary>
    /// <param name="unitActor"></param>
    /// <param name="pos"></param>
    public void updateImage(UnitActor unitActor)
    {

        int instanceID = unitActor.GetInstanceID();
        if (useImageDic.ContainsKey(instanceID))
        {
            Image image = useImageDic[instanceID];
            Vector2 nowPos = Vector2.zero;
            float lengthAdd = 0f;
            float scrollLength = m_scrollLength;
            //현재 위치에 따른 거리를 미니맵에 표시
            //현재 위치 / 최대 거리 = rate
            //slider * rate = 슬라이더 위치

//            Debug.Log("a : " + mapLength);

            if (LayerMask.NameToLayer("TopGndLine") == unitActor.layer || LayerMask.NameToLayer("TopAirLine") == unitActor.layer)
            {
                //비율만큼 더하기
                //위쪽
                lengthAdd = -2f;
                nowPos += imagePos;
            }
            else if (LayerMask.NameToLayer("BotGndLine") == unitActor.layer || LayerMask.NameToLayer("BotAirLine") == unitActor.layer)
            {
                //비율만큼 빼기
                //아래쪽
                lengthAdd = 2f;
                nowPos -= imagePos;
            }


            scrollLength = scrollLength * ((mapLength * 2f + lengthAdd) / (mapLength * 2f));

            //맵 사이즈에 대한 거리 비율을 가져옴 
            float rate = (unitActor.transform.position.x + mapLength + lengthAdd) / (mapLength * 2f + lengthAdd);
//            Debug.Log("rate : " + rate);

            //비율에 따른 거리 가져오기
            //거리 보정 필요 - 상, 중, 하
            //상 - 가까운 거리 - 이동속도가 줄어야 함 - 맵 거리의 비율만큼 느림
            //중 - 일반 거리 - 이동속도 그대로 - 8 100
            //하 - 먼 거리 - 이동속도 빠름 -10 - 맵 거리의 비율만큼 빠름
            float value = scrollLength * rate;

            nowPos.x = value - (scrollLength * 0.5f);

            image.gameObject.GetComponent<RectTransform>().localPosition = nowPos;
        }
    }

    void OnValueChanged(float value)
    {
        //드래그중이 아니면
        //카메라에게 알려야 함
//        if(!m_isDrag)
            cameraController.setPosition(value);
    }

    public void stopCamera()
    {
        m_isDrag = false;
        cameraController.stopCamera();
    }

    //public void setPosition(float ratePos)
    //{
    //    m_cameraController.setPosition(ratePos);
    //}

    public void moveCamera(float dirX)
    {
        //위치 가져오기
//        m_isDrag = true;
        cameraController.moveCamera(dirX);

//        float limitPos = (((float)m_typeMapSize + 1f) * Prep.castlePos) + Prep.cameraLimitPos;

        ////카메라 위치를 기준으로 비율 가져오기
        float ratePos = (cameraController.transform.position.x + cameraController.cameraLimitPos) / (cameraController.cameraLimitPos * 2f);

        m_mapScrollBar.value = ratePos;
    }

    public void setDrag(){
        m_isDrag = true;
    }
}

