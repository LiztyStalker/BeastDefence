using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class DeckManager : SingletonClass<DeckManager>
{

    enum TYPE_DECK_LIST{
        Key, 
        CommanderKey, 
        CommanderLevel, 
        CastleLv, 
        MunitionsLv, 
        DefenceLv, 
        DefenceCnt,
        SoldierLv, 
        HeroLv, 
        Soldier, 
        Hero, 
        Skill
    }
    
//    readonly string xPath = "Deck/Data";

    Dictionary<string, Deck> m_deckDic = new Dictionary<string, Deck>();
        
    //public IEnumerator missionValues { get { return m_missionDic.Values.GetEnumerator(); } }


    public DeckManager()
    {
        initParse();
    }

    void initParse()
    {

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.deckDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {

                string key = xmlNode.SelectSingleNode(TYPE_DECK_LIST.Key.ToString()).InnerText;


                string commanderKey = xmlNode.SelectSingleNode(TYPE_DECK_LIST.CommanderKey.ToString()).InnerText;
                int commanderLevel = 0;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.CommanderLevel.ToString()).InnerText, out commanderLevel))
                    commanderLevel = 1;



                ICard[] cardArray = new ICard[Prep.maxUnitSlot + Prep.maxHeroSlot];// + Prep.maxSkillSlot];
                
                int castleLv = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.CastleLv.ToString()).InnerText, out castleLv))
                    castleLv = 1;

                int munitionsLv = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.MunitionsLv.ToString()).InnerText, out munitionsLv))
                    munitionsLv = 1;

                int defenceLv = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.DefenceLv.ToString()).InnerText, out defenceLv))
                    defenceLv = 1;

                int defenceCnt = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.DefenceCnt.ToString()).InnerText, out defenceCnt))
                    defenceCnt = 1;

                int soldierLv = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.HeroLv.ToString()).InnerText, out soldierLv))
                    soldierLv = 1;
                
                int heroLv = 1;
                if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DECK_LIST.HeroLv.ToString()).InnerText, out heroLv))
                    heroLv = 1;

                //유닛 카드 가져오기
                for (int i = 0; i < Prep.maxUnitSlot + Prep.maxHeroSlot; i++)
                {
                    string unitKey = "";


                    if (i < Prep.maxUnitSlot)
                        unitKey = xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_DECK_LIST.Soldier, i + 1)).InnerText;
                    else
                        unitKey = xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_DECK_LIST.Hero, (i - Prep.maxUnitSlot + 1))).InnerText;

                    Unit unit = UnitManager.GetInstance.getUnit(unitKey);

                    if (unit != null)
                    {
                        int level = soldierLv;
                        if (unit.typeUnit == Unit.TYPE_UNIT.Hero)
                            level = heroLv;

                        UnitCard unitCard = new UnitCard(unit, level);
                        cardArray[i] = unitCard;
                    }
                    else
                    {
                        cardArray[i] = null;
                    }
//                    Debug.LogWarning("unitCard : " + cardArray[i] + i + " " + key);
                }


                //스킬 카드 가져오기
                //지휘관으로 대체
                //for (int i = 0; i < Prep.maxSkillSlot; i++)
                //{

                //    string skillKey = xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_DECK_LIST.Skill, i + 1)).InnerText;

                //    Skill skill = SkillManager.GetInstance.getSkill(skillKey);

                //    if (skill != null)
                //    {
                //        SkillCard skillCard = new SkillCard(skill);
                //        cardArray[i + Prep.maxUnitSlot + Prep.maxHeroSlot] = skillCard;
                //    }
                //    else
                //    {
                //        cardArray[i + Prep.maxUnitSlot + Prep.maxHeroSlot] = null;
                //    }
                //}

                Deck deck = new Deck(
                                commanderKey,
                                commanderLevel,
                                castleLv, 
                                munitionsLv, 
                                defenceLv, 
                                defenceCnt,
                                soldierLv, 
                                heroLv, 
                                cardArray
                                );

                m_deckDic.Add(key, deck);
            }


        }
        else
        {
            Prep.LogError(Prep.deckDataPath, "를 찾을 수 없음", GetType());
        }
        
    }

    /// <summary>
    /// 덱 찾기
    /// </summary>
    /// <param name="stageKey"></param>
    /// <returns></returns>
    public Deck getDeck(string stageKey)
    {
        if (m_deckDic.ContainsKey(stageKey))
        {
            return m_deckDic[stageKey];
        }

        Prep.LogError(stageKey, "를 찾을 수 없음", GetType());

        return null;
    }
}

