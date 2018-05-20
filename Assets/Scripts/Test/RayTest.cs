using System;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    [SerializeField]
    float m_range;

    void FixedUpdate()
    {

        RaycastHit2D[] rays;

        int layerMask = Prep.getLayerMask(17, Unit.TYPE_LINE.ALL, Unit.TYPE_MOVEMENT.Gnd);
        Debug.Log("IActor : " + layerMask);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        Debug.Log("pos : " + pos + " " + Input.mousePosition);
        rays = Physics2D.BoxCastAll(pos, Vector2.one * m_range, 0f, Vector2.zero, m_range, layerMask);

               

        foreach (RaycastHit2D ray in rays)
        {
            Debug.Log("ray : " + ray.collider.name);
        }

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + (m_range * 0.5f), transform.position.y - (m_range * 0.5f), 0), Vector2.one * m_range);
    }
}

