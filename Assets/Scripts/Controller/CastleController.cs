using System;
using System.Collections;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    [SerializeField]
    Transform[] m_lineTransforms;


    MeshRenderer[] lineRenderers;


    readonly float[] m_lengthOffset = { 0f, 1f, 2f }; //생산 위치 조정

//    readonly Vector2 m_defaultPos = new Vector2(-Prep.castlePos, -1f); //성 기본 위치

    //플레이어 여부
    bool m_isPlayer = false;

    //맵 사이즈만큼 벌리기
//    Transform[] m_linePos;

    public void initCastle(TYPE_MAP_SIZE typeMapSize, bool isPlayer)
    {

        Vector2 defaultPos = new Vector2(-Prep.castlePos * ((float)typeMapSize + 1f), -1f);

        m_isPlayer = isPlayer;
        if(!isPlayer)
            defaultPos.x *= -1f;

//        Debug.Log(defaultPos);
        transform.position = defaultPos;

        //기본 거리 8 - 소형
        //2배 중형
        //3배 대형

        //사이즈만큼 라인 렌더러 늘리기
        //

        lineRenderers = new MeshRenderer[m_lineTransforms.Length];

        for(int i = 0; i < m_lineTransforms.Length; i++)
        {
            lineRenderers[i] = m_lineTransforms[i].Find("LineMesh").GetComponent<MeshRenderer>();
        }

        StartCoroutine(meshCoroutine());
        setLine(0);
    }

    IEnumerator meshCoroutine()
    {
        float offset = 0f;
        while (gameObject.activeSelf)
        {
            offset -= Time.deltaTime * 0.5f;
            foreach(MeshRenderer mesh in lineRenderers){
                mesh.material.mainTextureOffset = new Vector2(offset, 0f);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 라인 선택하기
    /// </summary>
    /// <param name="line"></param>
    public void setLine(int line)
    {
        for (int i = 0; i < m_lineTransforms.Length; i++)
        {
            if(i == line)
                m_lineTransforms[i].gameObject.SetActive(true);
            else
                m_lineTransforms[i].gameObject.SetActive(false);
        }

    }


    /// <summary>
    /// 해당라인 처음 위치 가져오기
    /// 0~2 - 해당 라인
    /// 이외 - 자신
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 getLine(MapController.TYPE_MAP_LINE typeMapLine)
    {
//        Debug.Log("index : " + typeMapLine);
        if (typeMapLine == MapController.TYPE_MAP_LINE.Total)
            return transform.position;
//            return new Vector2(transform.position.x + ((m_isPlayer) ? -2f : 2f), transform.position.y + 2f);

        int index = (int)typeMapLine;
//        Debug.LogError("index : " + index);
        return new Vector3(transform.position.x + ((m_isPlayer) ? -m_lengthOffset[index] : m_lengthOffset[index]), m_lineTransforms[index].position.y, m_lineTransforms[index].position.z);
    }

    /// <summary>
    /// 성 원래 위치
    /// </summary>
    /// <returns></returns>
    public Vector3 getCastlePos()
    {
        return transform.position;
    }

    /// <summary>
    /// 라인 랜덤 선택
    /// </summary>
    /// <returns></returns>
    public MapController.TYPE_MAP_LINE getRandomLineIndex()
    {
        return (MapController.TYPE_MAP_LINE)UnityEngine.Random.Range(0, m_lineTransforms.Length);
    }

}

