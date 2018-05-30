using UnityEngine;
using Defence.CharacterPackage;

public class Contents
{
    public enum TYPE_CONTENTS_EVENT{
        Lobby, //로비 입장시
        World, //지역 입장시
        Option, //옵션 열면
        StageList, //미션 리스트 열면
        StageInfo, //미션 정보 열리면
        UnitInfo, //유닛 정보 열리면
        Account, //계정 열면
        Barracks, //막사 열리면
        BarracksSoldier, //병사창 열리면
        BarracksCommander, //지휘관창 열리면
        BarracksHero, //영웅창 열리면
        BarracksUp, //유닛 올림 이벤트
        BarracksDn, //유닛 내림 이벤트
        StageReady, //임무 준비창 열리면
        Shop, //상점창 열리면
        Develop, //개발창 열리면
        Employee, //고용창 열리면
        Achieve, //업적창 열리면
        Ready, //준비
        Start, //시작
        AppearAllyUnit, //유닛이 등장하면
        AppearEnemyUnit, //유닛이 등장하면
        AppearAllyHero, //영웅이 등장하면
        AppearEnemyHero, //영웅이 등장하면
        DownAllyHero, //아군 영웅 사망
        DownEnemyHero, //적군 영웅 사망
        Victory, //전투 승리시
        Defeat, //패배시
        Result //결과창 나타날시
    }
    public enum TYPE_CONTENTS_POS { Left, Right }

    string m_key;
    string m_parentKey;
    string m_stageKey;
    string m_character;
    Character.TYPE_FACE m_typeFace;
    Sprite m_image;
//    string m_eventClass;
    TYPE_CONTENTS_EVENT m_typeContentsEvent;
    TYPE_CONTENTS_POS m_typeContentsPos;

    string m_contents;

    public string key { get { return m_key; } }
    public string parentKey { get { return m_parentKey; } }
    public Sprite image { get { return m_image; } }
    public TYPE_CONTENTS_EVENT typeContentsEvent { get { return m_typeContentsEvent; } }
//    public string eventClass { get { return m_eventClass; } }
    public TYPE_CONTENTS_POS typeContentsPos { get { return m_typeContentsPos; } }
    public string stageKey { get { return m_stageKey; } }
    public string character { get { return m_character; } }
    public Character.TYPE_FACE typeFace { get { return m_typeFace; } }
    public string contents { get { return m_contents; } }


    public Contents(string key,
        string parentKey,
        string stageKey,
        string character,
        string contents,
        Character.TYPE_FACE typeFace,
        Sprite image,
        TYPE_CONTENTS_EVENT typeEvent,
        TYPE_CONTENTS_POS typePos

        )
    {
        m_key = key;
        m_parentKey = parentKey;
        m_stageKey = stageKey;
        m_character = character;
        m_typeFace = typeFace;
        m_contents = contents;
        m_image = image;
//        m_eventClass = eventClass;
        m_typeContentsEvent = typeEvent;
        m_typeContentsPos = typePos;
    }
}

