using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Defence.CommanderPackage
{


    public class CommanderManager : SingletonClass<CommanderManager>
    {
        enum TYPE_COMMANDER_DATA { Key, Name, TypeForce, Health, LeaderShip, Munitions, Skill0, Skill1, Skill2, Contents}

        Dictionary<string, Commander> m_commanderDic = new Dictionary<string, Commander>();

        public CommanderManager()
        {
            initParse();
        }

        void initParse()
        {

            Sprite[] icons = Resources.LoadAll<Sprite>(Prep.commanderIconPath);
            Sprite[] images = Resources.LoadAll<Sprite>(Prep.commanderImagePath);

            TextAsset textAsset = Resources.Load<TextAsset>(Prep.commanderDataPath);

            if (textAsset != null)
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(textAsset.text);

                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

                foreach (XmlNode xmlNode in xmlNodeList)
                {

                    try
                    {
                        string key = xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.Key.ToString()).InnerText;

                        if (key == "-")
                            continue;

                        string name = xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.Name.ToString()).InnerText;
                        
                        TYPE_FORCE typeForce = (TYPE_FORCE)Enum.Parse(typeof(TYPE_FORCE), xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.TypeForce.ToString()).InnerText);


                        Sprite icon = icons.Where(ic => ic.name == key).SingleOrDefault();
                        if(icon == null) Prep.LogError(key, "아이콘을 찾을 수 없음", GetType());

                        Sprite image = images.Where(im => im.name == key).SingleOrDefault();
                        if(image == null) Prep.LogError(key, "이미지를 찾을 수 없음", GetType());


                        int health = 0;
                        if(!int.TryParse(xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.Health.ToString()).InnerText, out health)){
                            health = 100;
                        }

                        int leadership = 0;
                        if(!int.TryParse(xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.LeaderShip.ToString()).InnerText, out leadership)){
                            leadership = 100;
                        }
                        int munitions = 0;
                        if(!int.TryParse(xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.Munitions.ToString()).InnerText, out munitions)){
                            munitions = 100;
                        }

                        string[] skills = new string[3];

                        for(int i = 0; i < 3; i++){
                            skills[i] = xmlNode.SelectSingleNode((TYPE_COMMANDER_DATA.Skill0 + i).ToString()).InnerText;
                        }

                        string contents = xmlNode.SelectSingleNode(TYPE_COMMANDER_DATA.Contents.ToString()).InnerText;


                        Commander commander = new Commander(
                                                key,
                                                name,
                                                icon,
                                                image,
                                                typeForce,
                                                health,
                                                leadership,
                                                munitions,
                                                skills,
                                                contents);

                        m_commanderDic.Add(key, commander);
                    }
                    catch (ArgumentException e)
                    {
                        Prep.LogError(e.Message, "에 대한 오류가 발생하였습니다.", GetType());
                    }
                    catch (NullReferenceException e)
                    {
                        Prep.LogError(e.Message, "을 찾을 수 없습니다.", GetType());
                    }
                }
            }
            else
            {
                Prep.LogError(Prep.commanderDataPath, "를 찾을 수 없음", GetType());
            }
        }

        //지휘관 가져오기
        public Commander getCommander(string key)
        {
            if (m_commanderDic.ContainsKey(key))
            {
                return m_commanderDic[key];
            }

            return null;
        }

        /// <summary>
        /// 지휘관 카드 가져오기
        /// </summary>
        /// <param name="key"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public CommanderCard getCommanderCard(string key, int level)
        {
            Commander commander = getCommander(key);
            if(commander != null){
                return new CommanderCard(commander, level);
            }
            return null;
        }

        /// <summary>
        /// 직렬화된 지휘관 카드를 변환하여 가져오기
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public CommanderCard getCommanderCard(AccountCommanderCardSerial serial)
        {
            if (getCommander(serial.key) != null)
            {
                return new CommanderCard(serial);
            }
            return null;
        }

    }
}

