using System;
using UnityEngine;
using System.Collections.Generic;

public class Prep
{

    #region #################### 사운드 ##########################

    /// <summary>
    /// 볼륨크기
    /// </summary>
    public static float volumeBGM = 1f;
    public static float volumeEffect = 1f;

    #endregion

    #region #################### 캐릭터 ##########################

    public static readonly string characterDataPath = "Data/Character";
    public static readonly string characterIconPath = "Image/Character";
//    public static readonly string characterFacePath = "Image/Character/Face";

    #endregion

    #region #################### 지휘관 ##########################


    public static readonly string commanderDataPath = "Data/Commander";
    public static readonly string commanderImagePath = "Image/Commander/Image";
    public static readonly string commanderIconPath = "Image/Commander/Icon";

    public static int commanderMaxLevel = 50;

    #endregion

    #region #################### 팁 ##########################

    public static readonly string tipDataPath = "Data/Tip";
    public static readonly string tipImagePath = "Image/Tip";

    #endregion

    #region #################### 자막 ##########################

    public static readonly string contentsDataPath = "Data/Contents";
    public static readonly string contentsImagePath = "Image/Contents";
//    public static readonly string characterFacePath = "Image/Character/Face";

    #endregion

    #region #################### 맵 ##########################

    public static readonly string mapDataPath = "Data/Map";
    public static readonly string mapBackgroundPath = "Image/Map/Background";
    public static readonly string mapImagePath = "Image/Map/Image";
    public static readonly string mapIconPath = "Image/Map/Icon";
    public static readonly string mapRoadPath = "Image/Map/Road";
    public static readonly string mapPrefebsPath = "Prefebs/Map";

    #endregion

    #region #################### 업적 ##########################
    
    public static readonly string achieveDataPath = "Data/Achieve";
    public static readonly string achieveImagePath = "Image/Achieve";

    #endregion

    #region #################### 상점 ##########################

    public static readonly string shopDataPath = "Data/Shop";
    public static readonly string shopImagePath = "Image/Shop";
    public static readonly string shopResourcePath = "Image/Shop/Resources";

    #endregion

    #region #################### 시나리오 ##########################

    public static readonly string deckDataPath = "Data/Deck";
    public static readonly string awardCategoryImagePath = "Image/Sinario/SinarioAward";

    public static readonly string stageAwardDataPath = "Data/SinarioAward";
    public static readonly string stageImagePath = "Image/Sinario/Sinario";
    public static readonly string stageDataPath = "Data/Stage";

    public static readonly string areaImagePath = "Image/Sinario/Area";
    public static readonly string areaDataPath = "Data/Area";

    public static readonly string worldImagePath = "Image/Sinario/World/Image";
    public static readonly string worldIconPath = "Image/Sinario/World/Icon";
    public static readonly string worldDataPath = "Data/World";

    #endregion

    #region #################### 징집 ##########################

    public static readonly string conscriptImagePath = "Image/Conscript";

    //1인, 5인, 10인, 영웅
    public static readonly int[] conscriptPay = { 100, 500, 1000, 500 };
    public static readonly string[] conscriptText = { "1명 징집", "5명 징집", "10명 징집", "영웅 1명 징집" };

    #endregion

    #region #################### 개발 및 연구 ##########################

    public static readonly string developImagePath = "Image/Develop";
    public static readonly string developDataPath = "Data/Develop";

    public const int techCount = 10;
    public static readonly string[] techLevelText = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    public const int techParentCount = 3;
    #endregion


    #region #################### 효과 ##########################

    public static readonly string effectDataPath = "Prefebs/Effect";

    #endregion

    #region #################### 리소스 ########################

    public static readonly string uiCommonPrefebsPath = "Prefebs/UI/Game@Common";

    #endregion

    //    public static readonly string IconPath = "Image/Research";
    //    public static readonly string researchDataPath = "Data/Research";

    #region #################### 추가조건 ##########################

    public static readonly string addConstraintDataPath = "Data/AddConstraint";

    #endregion

    #region #################### 버프 ##########################

    public static readonly string buffDataPath = "Data/Buff";

    #endregion

    #region #################### 스킬 ##########################

    public static readonly string skillActorDataPath = "Prefebs/SkillActor";
    public static readonly string skillDataPath = "Data/Skill";
    public static readonly string skillIconPath = "Image/Unit/Skill/Icon";

    #endregion

    #region #################### 유닛 ##########################

    public static readonly string unitDataPath = "Data/Unit";
    public static readonly string unitSkeletonPath = "Prefebs/SkeletonData";
    public static readonly string unitIconPath = "Image/Unit/Icon";

    public const int defaultSoldierPopulation = 10;
    public const int defaultSoldierCount = 10;
    public const int defaultSoldierLevel = 10;

    public const int defaultHeroPopulation = 5;
    public const int defaultHeroCount = 3;
    public const int defaultHeroLevel = 10;

    public static readonly string unitTag = "Unit";
    public static readonly string defenceTag = "Defence";

    #endregion

    #region #################### 탄환 ##########################

    public static readonly string bulletDataPath = "Data/Bullet";
    public static readonly string bulletIconPath = "Image/Unit/Bullet";

    #endregion

    #region #################### 상태이상 ##########################

    public static readonly string stateControlDataPath = "Data/StateControl";

    #endregion

    #region #################### 카메라 ##########################

    //카메라 이동 오프셋
    public static float cameraVelocityOffset = 10f;
    public const float cameraLimitPos = 2f;
    public const float castlePos = 8f;

    #endregion

    #region #################### 씬 ##########################
    
    public static readonly string scenePlay = "Game@Play";
    public static readonly string sceneLobby = "Game@Lobby";
    public static readonly string sceneMain = "Game@Main";
    public static readonly string sceneLoad = "Game@Load";

    #endregion

    #region #################### 변경 ############################

    public static string getForceToText(TYPE_FORCE typeForce)
    {
        switch (typeForce)
        {
            case TYPE_FORCE.Rebel:
                return "반란군";
            case TYPE_FORCE.FreeCompany:
                return "용병단";
            case TYPE_FORCE.Order:
                return "기사단";
            case TYPE_FORCE.Union_Order_Free:
                return "기사용병연합";
            case TYPE_FORCE.Union_Rebel_Free:
                return "반란용병연합";
            case TYPE_FORCE.All:
                return "연합";
            case TYPE_FORCE.None:
                return "야만군";
        }

        return "";
    }

    public static string getTypeStageToText(Stage.TYPE_STAGE typeStage)
    {
        switch (typeStage)
        {
            case Stage.TYPE_STAGE.Main:
                return "메인";
            case Stage.TYPE_STAGE.Infinite:
                return "무한";
            case Stage.TYPE_STAGE.Normal:
                return "일반";
            case Stage.TYPE_STAGE.Warning:
                return "긴급";
               
        }
        return "";
    }

    public static string getTypeSinarioAwardCategory(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeAward)
    {
        switch (typeAward)
        {
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Card:
                return "랜덤병사카드";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.CCard:
                return "지휘관카드";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Exp:
                return "경험치";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Food:
                return "식량";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Fruit:
                return "열매";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Gold:
                return "골드";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.HCard:
                return "영웅카드";
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.NCard:
                return "병사카드";
        }
        return "";
    }

    #endregion

    #region #################### 시스템 ##########################

    public static readonly string[] sortingLayerNames = { "TopLine", "MidLine", "BotLine" };

//    public static Transform[] playerPos;
//    public static Transform[] cpuPos;

    //1 프레임타임
    public const float frameTime = 0.04f;

    //카드 슬롯 수
    public const int maxUnitSlot = 6;
    public const int maxHeroSlot = 3;
    public const int maxSkillSlot = 2;

    //풀링 최대수
    public const int unitActorObjFoolCnt = 100;
    public const int bulletActorObjFoolCnt = 100;
    public const int effectActorObjFoolCnt = 100;

    //이동속도 오프셋
    public const float movementOffset = 0.5f;


    /// <summary>
    /// 성 체력 없음 경고 위치
    /// </summary>
    public const float castleWarningRate = 0.1f;

    /// <summary>
    /// 최대 게임 진행 시간
    /// </summary>
    public readonly static TimeSpan maxGameTime = new TimeSpan(0, 10, 0);


    #endregion

    //개발자용
    #region #################### 개발자용 ##########################

    /// <summary>
    /// 적 생성 여부
    /// </summary>
    public static bool isNotEnemyCreate = false;

    /// <summary>
    /// 한 라인만 사용 여부
    /// </summary>
    public static bool isOneLine = false;

    /// <summary>
    /// 스킬 무한 사용 여부
    /// </summary>
    public static bool isInfiniteSkillRate = false;

    /// <summary>
    /// 
    /// </summary>
    public static bool isUnitControl = false;

    /// <summary>
    /// 상태이상 사용 여부
    /// </summary>
    public static bool isStateControl = false;

    /// <summary>
    /// 모든 미션 보이기
    /// </summary>
    public static bool isAllStage = false;

    /// <summary>
    /// 항상 군수품 차있기
    /// </summary>
    public static bool isMunitionFull = false;

    /// <summary>
    /// 유닛 쿨타임 사용
    /// </summary>
    public static bool isNotUnitCoolTime = false;
    //    static string[] layerMaskName = { "TopGndLine", "MidGndLine", "BotGndLine", "TopAirLine", "MidAirLine", "BotAirLine" };

    /// <summary>
    /// 모든 유닛 가지기
    /// </summary>
    public static bool isAllUnit = false;

    /// <summary>
    /// 모든 지휘관 가지기
    /// </summary>
    public static bool isAllCommander = false;

    #endregion





    /// <summary>
    /// 공격가능 레이어마스크 계산하기
    /// </summary>
    /// <param name="layerMaskIndex"></param>
    /// <param name="typeLine"></param>
    /// <param name="typeMovement"></param>
    /// <returns></returns>
    public static int getLayerMask(int layerMaskIndex, Unit.TYPE_LINE typeLine, Unit.TYPE_MOVEMENT typeMovement)
    {

        int layerMask = 1 << LayerMask.NameToLayer("TotalLine");



//        Debug.Log("enum " + (Unit.TYPE_LINE.GND_ALL | Unit.TYPE_LINE.AIR_ALL));

//        layerMask |= 1 << LayerMask.NameToLayer("TopGndLine");
//        layerMask = 1 << layerMaskIndex;
//        layerMask |= 1 << layerMaskIndex;

//        Debug.Log("layerMask : " + layerMask);

//        return layerMask;

        if (layerMaskIndex == LayerMask.NameToLayer("TotalLine"))
        {
            typeLine = Unit.TYPE_LINE.ALL;
        }
        //없음

        //지상전방
        if ((typeLine & Unit.TYPE_LINE.GND_FORWARD) == Unit.TYPE_LINE.GND_FORWARD)
            //지상형이면
            if(typeMovement == Unit.TYPE_MOVEMENT.Gnd)
                layerMask |= 1 << layerMaskIndex;
            //공중형이면
            else
                layerMask |= 1 << (layerMaskIndex - 3);

        //지상측방
        if ((typeLine & Unit.TYPE_LINE.GND_SIDE) == Unit.TYPE_LINE.GND_SIDE)
        {
            layerMask |= 1 << LayerMask.NameToLayer("TopGndLine");
            layerMask |= 1 << LayerMask.NameToLayer("MidGndLine");
            layerMask |= 1 << LayerMask.NameToLayer("BotGndLine");
        }
            

        //공중전방
        if ((typeLine & Unit.TYPE_LINE.AIR_FORWARD) == Unit.TYPE_LINE.AIR_FORWARD)
        {
            //지상형이면
            if (typeMovement == Unit.TYPE_MOVEMENT.Gnd)
                layerMask |= 1 << (layerMaskIndex + 3);
            //공중형이면
            else
                layerMask |= 1 << layerMaskIndex;
        }

        //공중측방
        if ((typeLine & Unit.TYPE_LINE.AIR_SIDE) == Unit.TYPE_LINE.AIR_SIDE)
        {
            layerMask |= 1 << LayerMask.NameToLayer("TopAirLine");
            layerMask |= 1 << LayerMask.NameToLayer("MidAirLine");
            layerMask |= 1 << LayerMask.NameToLayer("BotAirLine");
        }


//        Debug.Log("layerMask : " + layerMask);

        return layerMask;

    }


    /// <summary>
    /// 세력 색상 가져오기
    /// </summary>
    /// <param name="typeForce"></param>
    /// <returns></returns>
    public static Color getForceColor(TYPE_FORCE typeForce)
    {
        switch(typeForce){
            case TYPE_FORCE.Rebel:
                return Color.blue;
            case TYPE_FORCE.FreeCompany:
                return Color.yellow;
            case TYPE_FORCE.Order:
                return Color.red;
            case TYPE_FORCE.Union_Rebel_Free:
                return Color.green;
            case TYPE_FORCE.Union_Order_Free:
                return new Color(1f, 0.42f, 0f); //오렌지
            case TYPE_FORCE.All:
                return Color.white;
        }
        return Color.gray;
    }


    public static void LogError(string key, string msg, Type type)
    {
        Debug.LogError(string.Format("{0} {1} {2}", key, msg, type));
    }

    public static void LogWarning(string key, string msg, Type type)
    {
        Debug.LogWarning(string.Format("{0} {1} {2}", key, msg, type));
    }

    public static void Log(string key, string msg, Type type)
    {
        Debug.Log(string.Format("{0} {1} {2}", key, msg, type));
    }

    /// <summary>
    /// 레이어마스크 가져오기
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="typeMovement"></param>
    /// <returns></returns>
    public static int getLayer(MapController.TYPE_MAP_LINE typeMapLine, Unit.TYPE_MOVEMENT typeMovement)
    {
        return 1 + (int)typeMapLine + LayerMask.NameToLayer("TotalLine") + ((typeMovement == Unit.TYPE_MOVEMENT.Gnd) ? 0 : 3);
    }

    //public static int getIndexPosition(IActor iActor, int layer, Unit.TYPE_MOVEMENT typeMovement)
    //{
    //    //if (layer == 0)
    //    //    return 0;

    //    int index = layer - 1 - LayerMask.NameToLayer("TotalLine") - ((typeMovement == Unit.TYPE_MOVEMENT.GROUND) ? 0 : 3);

    //    //성이 타겟이면 자신 위치로
    //    //if(index < 0)
    //    //    return getIndexPosition(unitActor, unitActor.gameObject.layer, typeMovement);
    //    return index;
    //}

    /// <summary>
    /// 생산 위치 가져오기
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="typeMovement"></param>
    /// <returns></returns>
    public static int getIndexPosition(int layer)
    {
        //if (layer == 0)
        //    return 0;

//        Debug.Log("layer : " + layer + " " + LayerMask.NameToLayer("TotalLine") + " " + ((typeMovement == Unit.TYPE_MOVEMENT.GROUND) ? 0 : 3));

        int index = layer - 1 - LayerMask.NameToLayer("TotalLine");

        if (index >= 3)
            index -= 3;

        //성이 타겟이면 자신 위치로
        //if(index < 0)
        //    return getIndexPosition(unitActor, unitActor.gameObject.layer, typeMovement);
        return index;
    }

    /// <summary>
    /// XmlDataPath 가져오기
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string getXmlDataPath(Type type)
    {
        return string.Format("{0}/Data", type.Name.ToString().Replace("Manager", ""));
    }

    /// <summary>
    /// 라인 랜덤 - 맵컨트롤러에서 가져오기
    /// </summary>
    /// <returns></returns>
    //public static int getRandomLineIndex()
    //{
    //    return UnityEngine.Random.Range(0, playerPos.Length);
    //}

//    public static Vector2 getRandomLinePosition(Vector2 startPos, float radius, int pos)
//    {
//        Transform linePos = playerPos[pos];
//        //랜덤으로 라인 가져오기


//        //라인 시작지점부터 radius 범위까지 x축 가져오기
//        //0 - 75% 1 - 100% 2 - 125%
////        float randX = UnityEngine.Random.Range(startPos.x - (radius * 0.5f * (1f + (float)(pos - 1) * 0.25f)), startPos.x + (radius * 0.5f * (1f + (float)(pos - 1) * 0.25f)));
//        float randX = UnityEngine.Random.Range(startPos.x - radius, startPos.x + radius);
//        return new Vector2(randX, linePos.position.y);
//    }
}
