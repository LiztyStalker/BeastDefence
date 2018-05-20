using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ParabolaTest : MonoBehaviour
{
    [SerializeField]
    Transform m_target;

    float g;

    void Start()
    {
        
        g = -Physics2D.gravity.y;
        paravolaTest3();

    }

    //수평
    void paravolaTest1()
    {
       



        //발사속도
        float moveSpeed = 20f;

        //거리
        float distance = Vector2.Distance(transform.position, m_target.position);

        Debug.Log("distance : " + distance);

        //발사 각도
        //수평일때만 사용 가능
        //
        float angle = 0.5f * Mathf.Asin((g * distance) / (moveSpeed * moveSpeed));

        Debug.Log("angle : " + angle * Mathf.Rad2Deg);


        //거리의 반값이 등록
        float r = moveSpeed * moveSpeed * 2f * Mathf.Sin(angle) * Mathf.Cos(angle) / g;

        Debug.Log("target : " + r);



        float vX = Mathf.Cos(angle) * moveSpeed;
        float vY = Mathf.Sin(angle) * moveSpeed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(vX, vY);
        GetComponent<Rigidbody2D>().gravityScale = 1f;

    }

    //고각
    void paravolaTest2()
    {
        //발사속도
        float moveSpeed = 20f;

        //거리 v0
        float distance = Vector2.Distance(m_target.position, transform.position);


        float angle = Mathf.Atan((moveSpeed * moveSpeed) / (0.5f * g * distance));


        Debug.Log("angle : " + angle * Mathf.Rad2Deg);


        float vX = Mathf.Cos(angle) * moveSpeed;
        float vY = Mathf.Sin(angle) * moveSpeed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(vX, vY);
        GetComponent<Rigidbody2D>().gravityScale = 1f;



    }

    /// <summary>
    /// 오차가 발생함
    /// 
    /// </summary>
    void paravolaTest3()
    {

        //발사속도
        float moveSpeed = 20f;

        //거리 v0
        float distanceX = m_target.position.x - transform.position.x;
        float distanceY = m_target.position.y - transform.position.y;


        float angle = 0.5f * Mathf.Asin((g * distanceX) / (moveSpeed * moveSpeed));
        float offset = Mathf.Atan2(distanceY, distanceX);

        angle += offset;

        Debug.Log("angle : " + angle * Mathf.Rad2Deg);


        float vX = Mathf.Cos(angle) * moveSpeed;
        float vY = Mathf.Sin(angle) * moveSpeed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(vX, vY);
        GetComponent<Rigidbody2D>().gravityScale = 1f;


    }
}

