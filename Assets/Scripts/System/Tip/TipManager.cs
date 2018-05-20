using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;

namespace Defence.TipManager
{
    public class TipManager : SingletonClass<TipManager>
    {

        enum TYPE_TIP_DATA { Key, Contents }

        readonly string xPath = "Tip/Data";

        List<Tip> m_tipList = new List<Tip>();

        public TipManager()
        {
            initParse();
        }

        void initParse()
        {
            Sprite[] tipSprites = Resources.LoadAll<Sprite>(Prep.tipImagePath);

            TextAsset textAsset = Resources.Load<TextAsset>(Prep.tipDataPath);

            if (textAsset != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(textAsset.text);

                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xPath);


                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    string key = xmlNode.SelectSingleNode(TYPE_TIP_DATA.Key.ToString()).InnerText;
                    string contents = xmlNode.SelectSingleNode(TYPE_TIP_DATA.Contents.ToString()).InnerText;
                    Sprite sprite = tipSprites.Where(spr => spr.name == key).SingleOrDefault();

                    if (sprite == null)
                    {
                        Prep.LogWarning(key, "아이콘이 없습니다", GetType());
                        continue;
                    }

                    Tip tip = new Tip(key, sprite, contents);

                    m_tipList.Add(tip);
                    //                Debug.LogWarning("childCnt : " + xmlNode.ChildNodes.Count);

                }
            }
            else
            {
                Prep.LogError(Prep.tipDataPath, "를 찾을 수 없습니다.", GetType());
            }
        }

        public Tip getRandomTip()
        {
            return m_tipList[Random.Range(0, m_tipList.Count)];
        }
    }
}

