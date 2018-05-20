using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField]
    UIMap m_uiMap;

//    CameraController m_cameraController;

    bool isDrag = false;
    bool isLeft = false;
    bool isRight = false;
    Vector2 defaultPos;
    float dirX;
    float devideWidth;

    TYPE_MAP_SIZE typeMapSize;


    const float devideScreen = 12f;

    public void setSize(TYPE_MAP_SIZE typeMapSize)
    {
        this.typeMapSize = typeMapSize;
        devideWidth = (Screen.width / devideScreen);
//        limitPos = ((float)m_typeMapSize * Prep.castlePos) + Prep.cameraLimitPos;

//        m_cameraController = Camera.main.GetComponent<CameraController>();
//        m_cameraController.setSize(typeMapSize);
    }

    //손을 누르고 이동하면 화면 이동
    //화면을 기준으로 슬라이더가 이동해야 함

    //슬라이더 크기 = 아군과 적군의 성 거리와 화면에 비추는 거리
    //슬라이더 위치 = 아군과 적군의 성 거리의 위치



    void FixedUpdate()
    {
        if (isLeft)
        {
            setLeft();
        }
        else if (isRight)
        {
            setRight();
        }
        else if (isDrag)
        {
            setDrag();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnBeginDrag(eventData);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        //화면의 거의 끝부분을 눌렀을 때 자동 이동
        Vector2 mousePos = Input.mousePosition;

        //(총 화면 / 12) 안쪽 또는 바깥쪽 총화면 - (총화면 / 12)




        if (mousePos.x > 0 && mousePos.x < devideWidth)
        {
            //자동 왼쪽으로 이동
            isLeft = true;
        }
        else if (mousePos.x > Screen.width - devideWidth && mousePos.x < Screen.width)
        {
            //자동 오른쪽 이동
            isRight = true;
        }
        else
        {
            Debug.Log("mousePos : " + mousePos);


            //처음 위치 기억
            defaultPos = Input.mousePosition;
            isDrag = true;
            m_uiMap.setDrag();
            //        Debug.Log("begin");

        }
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        //처음 위치를 기준으로 이동한 거리만큼 x축으로 이동
        //드래그시 드래그한 거리만큼 값 삽입
        if (isDrag)
        {
            dirX = Input.mousePosition.x - defaultPos.x;
            dirX = Mathf.Clamp(dirX, -1f, 1f);

            Debug.Log("drag : " + dirX);
        }
    }

    void setDrag()
    {
        //미니맵에 드래그 거리 계속 알려줌
        m_uiMap.moveCamera(dirX);
    }


    void setLeft()
    {
        dirX = -1f;
        setDrag();
        //왼쪽 이동 이미지 나오기
    }

    void setRight()
    {
        dirX = 1f;
        setDrag();
        //오른쪽 이동 이미지 나오기
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnPointerUp(eventData);
        ////드래그 종료
        //isDrag = false;
        //isLeft = false;
        //isRight = false;
        //dirX = 0f;
        //m_uiMap.stopCamera();
        //Debug.Log("end");
        ////이동 끄기
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
        isLeft = false;
        isRight = false;
        dirX = 0f;
        m_uiMap.stopCamera();
        Debug.Log("end");
    }


    public void OnLeftClicked()
    {
        m_uiMap.moveCamera(-1f);
    }

    public void OnRightClicked()
    {
        m_uiMap.moveCamera(1f);
    }

}

