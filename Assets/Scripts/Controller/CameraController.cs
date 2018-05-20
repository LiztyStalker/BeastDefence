using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{

    //카메라 컨트롤러
    //화면 이동
    //화면 흔들림
    //화면 줌인, 줌아웃

    //
    Rigidbody2D m_rigidbody;
    const float dirY = -1f;
    
    
//    float limitPos;

    TYPE_MAP_SIZE m_typeMapSize;

    //카메라 최대 이동거리
    public float cameraLimitPos { get { return ((float)m_typeMapSize * Prep.castlePos) + Prep.cameraLimitPos; } }

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }


    public void setSize(TYPE_MAP_SIZE typeMapSize)
    {
        m_typeMapSize = typeMapSize;
    }

    //손을 누르고 이동하면 화면 이동
    //화면을 기준으로 슬라이더가 이동해야 함

    //슬라이더 크기 = 아군과 적군의 성 거리와 화면에 비추는 거리
    //슬라이더 위치 = 아군과 적군의 성 거리의 위치


    public void moveCamera(float dirX)
    {

//        Debug.Log("cameralimitPos : " + cameralimitPos);
        if (transform.position.x + dirX > cameraLimitPos)
        {
            transform.position = new Vector3(cameraLimitPos, dirY, -10f);
            stopCamera();
        }

        //최소값을 넘으면 최소값으로
        else if (transform.position.x + dirX < -cameraLimitPos)
        {
            transform.position = new Vector3(-cameraLimitPos, dirY, -10f);
            stopCamera();
        }

        //아니면 가속도만큼
        else
        {
            m_rigidbody.velocity = new Vector2(dirX, 0f) * Prep.cameraVelocityOffset;
        }
    }

    public void stopCamera()
    {
        m_rigidbody.velocity = Vector2.zero;
    }

    public void setPosition(float ratePos)
    {
        //0~1 위치
        //0.5가 중간값
        //맵 크기 = typeSize * 8
        //카메라 최대값 cameraLimit
        //카메라 이동경로 limitPos * 2f (양수, 음수)
        //
        float valuePos = (cameraLimitPos * 2f) * ratePos;
        transform.position = new Vector3(valuePos - cameraLimitPos, dirY, -10f);
    }

 

}

