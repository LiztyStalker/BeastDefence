using System;
using System.Collections.Generic;
using UnityEngine;

public class MapActor : MonoBehaviour
{

    Map m_map;

    MeshRenderer backgroundMesh;
    float delta = 0f;

    public void setActor(Map map, TYPE_MAP_SIZE typeMapSize)
    {

        //생성 및 관리


        GameObject obj = Instantiate(map.prefebs);
        obj.transform.SetParent(transform);
        
        //도로 메쉬 가져오기
        Transform road = obj.transform.Find("Road");
        MeshRenderer roadMesh = road.GetComponent<MeshRenderer>();

        if (roadMesh != null)
        {
            //도로 메쉬 사이즈 수정
            roadMesh.transform.localScale =
                new Vector3(
                    roadMesh.transform.localScale.x * ((float)typeMapSize + 1f),
                    roadMesh.transform.localScale.y,
                    roadMesh.transform.localScale.z
                    );
        }

        //펜스도 늘려야 함
        //맵 크기에 따라 tile 수 판정

        //울타리 메쉬 가져오기
        Transform fence = obj.transform.Find("Fence");

        MeshRenderer[] fenceMeshRenderer = fence.GetComponentsInChildren<MeshRenderer>();

        
        if (fenceMeshRenderer.Length > 0)
        {
            //울타리 메쉬 사이즈 수정
            foreach (MeshRenderer mesh in fenceMeshRenderer)
            {
                mesh.transform.localScale = new Vector3(
                    mesh.transform.localScale.x + ((float)typeMapSize * Prep.castlePos * 2f),
                    mesh.transform.localScale.y,
                    mesh.transform.localScale.z
                );
                mesh.material.mainTextureScale = new Vector2((float)typeMapSize + 1f, 1f);
            }

            
        }

        //구름 메쉬 가져오기
        Transform cloud = obj.transform.Find("BackGround").Find("BG_Cloud");
        backgroundMesh = cloud.GetComponent<MeshRenderer>();
        
        //m_roadMesh.transform.localScale =
        //    new Vector3(
        //        m_roadMesh.transform.localScale.x * ((float)typeMapSize + 1f),
        //        m_roadMesh.transform.localScale.y,
        //        m_roadMesh.transform.localScale.z
        //        );

    }


    void Update()
    {
        if (backgroundMesh != null)
        {
            delta += Time.deltaTime * 0.01f;
            backgroundMesh.material.mainTextureOffset = new Vector2(delta, 0f);
        }
    }
}

