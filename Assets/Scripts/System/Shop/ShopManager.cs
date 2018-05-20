using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class ShopManager : SingletonClass<ShopManager>
{
    enum TYPE_SHOP_DATA { Key, Category, Name, Value, CostCategory, Cost}

//    readonly string xPath = "Shop/Data";

    Dictionary<string, Shop> m_shopDic = new Dictionary<string, Shop>();

    Dictionary<Shop.TYPE_SHOP_CATEGORY, Sprite> m_shopSpriteDic = new Dictionary<Shop.TYPE_SHOP_CATEGORY, Sprite>();
    


    public ShopManager()
    {
        initParse();
    }

    void initParse()
    {
        
        Sprite[] shopResourceSprites = Resources.LoadAll<Sprite>(Prep.shopResourcePath);

        //상점 재화 이미지 가져오기
        for (int i = 0; i < Enum.GetValues(typeof(Shop.TYPE_SHOP_CATEGORY)).Length; i++)
        {
            if (shopResourceSprites.Length > i)
                m_shopSpriteDic.Add((Shop.TYPE_SHOP_CATEGORY)i, shopResourceSprites[i]);
        }

        //상점 리소스 이미지 가져오기
        Sprite[] shopSprites = Resources.LoadAll<Sprite>(Prep.shopImagePath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.shopDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));


            foreach (XmlNode xmlNode in xmlNodeList)
            {

                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_SHOP_DATA.Key.ToString()).InnerText;
                    string name = xmlNode.SelectSingleNode(TYPE_SHOP_DATA.Name.ToString()).InnerText;

                    Sprite icon = shopSprites.Where(shopSpr => shopSpr.name == key).SingleOrDefault();
                    if (icon == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());



                    string typeCategoryStr = xmlNode.SelectSingleNode(TYPE_SHOP_DATA.Category.ToString()).InnerText;
                    Shop.TYPE_SHOP_CATEGORY typeShopCategory = (Shop.TYPE_SHOP_CATEGORY)Enum.Parse(typeof(Shop.TYPE_SHOP_CATEGORY), typeCategoryStr);


                    //string categoryKey = xmlNode.SelectSingleNode(TYPE_SHOP_DATA.Category.ToString()).InnerText;
                    //Shop.TYPE_SHOP_CATEGORY typeShopCategory = Shop.TYPE_SHOP_CATEGORY.Gold;
                    //switch (categoryKey)
                    //{
                    //    case "Fruit":
                    //        typeShopCategory = Shop.TYPE_SHOP_CATEGORY.Fruit;
                    //        break;
                    //    case "Food":
                    //        typeShopCategory = Shop.TYPE_SHOP_CATEGORY.Food;
                    //        break;
                    //    case "Gold":
                    //        typeShopCategory = Shop.TYPE_SHOP_CATEGORY.Gold;
                    //        break;
                    //    case "Package":
                    //        typeShopCategory = Shop.TYPE_SHOP_CATEGORY.Pay;
                    //        break;
                    //}


                    string costCategoryKey = xmlNode.SelectSingleNode(TYPE_SHOP_DATA.CostCategory.ToString()).InnerText;
                    Shop.TYPE_SHOP_CATEGORY typeCostCategory = (Shop.TYPE_SHOP_CATEGORY)Enum.Parse(typeof(Shop.TYPE_SHOP_CATEGORY), costCategoryKey);
                    //Shop.TYPE_SHOP_CATEGORY typeCostCategory = Shop.TYPE_SHOP_CATEGORY.Gold;
                    //switch (costCategoryKey)
                    //{
                    //    case "Fruit":
                    //        typeCostCategory = Shop.TYPE_SHOP_CATEGORY.Fruit;
                    //        break;
                    //    case "Gold":
                    //        typeCostCategory = Shop.TYPE_SHOP_CATEGORY.Gold;
                    //        break;
                    //    case "Pay":
                    //        typeCostCategory = Shop.TYPE_SHOP_CATEGORY.Pay;
                    //        break;
                    //}


                    int cost = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_SHOP_DATA.Cost.ToString()).InnerText, out cost))
                    {
                        cost = 0;
                    }

                    int value = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_SHOP_DATA.Value.ToString()).InnerText, out value))
                    {
                        value = 0;
                    }

                    Shop shop = new Shop(key, name, typeShopCategory, typeCostCategory, icon, value, cost);
                    m_shopDic.Add(key, shop);
                }
                catch (ArgumentException e) 
                {
                    Prep.LogError(e.Message, "에 대한 오류 발생", GetType());
                }

            }
        }
        else
        {
            Prep.LogError(Prep.shopDataPath, "를 찾을 수 없음", GetType());
        }
    }


    public Shop[] getShop(Shop.TYPE_SHOP_CATEGORY typeShopCategory)
    {
        return m_shopDic.Values.Where(shop => shop.typeShopCategory == typeShopCategory).ToArray();
    }

    public Shop getShop(string key)
    {
        if (m_shopDic.ContainsKey(key))
        {
            return m_shopDic[key];
        }
        return null;
    }

    public Sprite getShopCostIcon(Shop.TYPE_SHOP_CATEGORY typeCostCategory)
    {
        if (m_shopSpriteDic.ContainsKey(typeCostCategory))
        {
            return m_shopSpriteDic[typeCostCategory];
        }
        return null;
    }


}

