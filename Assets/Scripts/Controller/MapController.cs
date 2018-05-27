using System;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public enum TYPE_MAP_LINE { Total = -1, Top, Mid, Bot }

    [SerializeField]
    SpriteRenderer m_backgroundSprite;

//    [SerializeField]
//    MeshRenderer m_roadMesh;
    [SerializeField]
    MapActor m_mapActor;

    [SerializeField]
    DragController m_dragController;

    [SerializeField]
    UIMap m_uiMap;

    CastleController[] m_castleController;

    Map m_map;
    TYPE_MAP_SIZE m_typeMapSize;

    
    //라인 가져오기


    public CastleController playerCastleController { get { return m_castleController[0]; } }
    public CastleController cpuCastleController { get { return m_castleController[1]; } }

    /// <summary>
    /// 라인 가져오기
    /// 
    /// </summary>
    /// <param name="uiCtrler"></param>
    /// <param name="typeMapLine"></param>
    /// <returns></returns>
    public Vector3 getLine(UIController uiCtrler, TYPE_MAP_LINE typeMapLine)
    {
        return getController(uiCtrler).getLine(typeMapLine);
    }

    /// <summary>
    /// 성 위치 가져오기
    /// </summary>
    /// <param name="uiCtrler"></param>
    /// <returns></returns>
    public Vector3 getCastlePos(UIController uiCtrler)
    {
        return getController(uiCtrler).getCastlePos();
    }

    void Awake()
    {
        m_castleController = GetComponentsInChildren<CastleController>();
    }

    public void setMap(Map map, TYPE_MAP_SIZE typeMapSize)
    {

        m_map = map;

        m_typeMapSize = typeMapSize;

        m_dragController.setSize(typeMapSize);
        m_uiMap.setSize(typeMapSize);

        m_mapActor.setActor(map, typeMapSize);

        //m_roadMesh.transform.localScale =
        //    new Vector3(
        //        m_roadMesh.transform.localScale.x * ((float)typeMapSize + 1f),
        //        m_roadMesh.transform.localScale.y,
        //        m_roadMesh.transform.localScale.z
        //        );

        m_castleController = GetComponentsInChildren<CastleController>();
        m_castleController[0].initCastle(typeMapSize, true);
        m_castleController[1].initCastle(typeMapSize, false);

        

    }

    public void linkUnit(UnitActor unitActor, UIController uiController)
    {
        m_uiMap.linkUnit(unitActor, uiController);
    }

    public bool unlinkUnit(UnitActor unitActor)
    {
        return m_uiMap.unlinkUnit(unitActor);
    }

    public void updateImage(UnitActor unitActor)
    {
        m_uiMap.updateImage(unitActor);
    }

    /// <summary>
    /// 라인 랜덤 인덱스 가져오기
    /// </summary>
    /// <param name="uiCtrler"></param>
    /// <returns></returns>
    public TYPE_MAP_LINE getRandomLineIndex(UIController uiCtrler)
    {
        return getController(uiCtrler).getRandomLineIndex();
    }

    /// <summary>
    /// 라인 랜덤 위치 가져오기
    /// </summary>
    /// <param name="uiCtrler"></param>
    /// <param name="startPos"></param>
    /// <param name="radius"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 getRandomLinePosition(UIController uiCtrler, Vector2 startPos, float radius, int pos)
    {



        Vector2 linePos = getController(uiCtrler).getLine(getRandomLineIndex(uiCtrler));
        //랜덤으로 라인 가져오기


        //라인 시작지점부터 radius 범위까지 x축 가져오기
        //0 - 75% 1 - 100% 2 - 125%
        //        float randX = UnityEngine.Random.Range(startPos.x - (radius * 0.5f * (1f + (float)(pos - 1) * 0.25f)), startPos.x + (radius * 0.5f * (1f + (float)(pos - 1) * 0.25f)));
        float randX = UnityEngine.Random.Range(startPos.x - radius, startPos.x + radius);
        return new Vector2(randX, linePos.y);
    }

    CastleController getController(UIController uiCtrler)
    {
        if (uiCtrler is UIPlayer)
            return playerCastleController;
        else
            return cpuCastleController;
    }


}

