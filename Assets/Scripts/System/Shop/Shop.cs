using UnityEngine;

public class Shop
{
    public enum TYPE_SHOP_CATEGORY { Gold, Fruit, Food, Pay }
//    public enum TYPE_SHOP_COST_CATEGORY { Gold, Fruit, Pay }

    string m_key;
    string m_name;
    TYPE_SHOP_CATEGORY m_typeShopCategory;
    Sprite m_icon;
    int m_value;
    TYPE_SHOP_CATEGORY m_typeCostCategory;
    int m_cost;


    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public TYPE_SHOP_CATEGORY typeShopCategory { get { return m_typeShopCategory; } }
    public TYPE_SHOP_CATEGORY typeCostCategory { get { return m_typeCostCategory; } }
    public Sprite icon { get { return m_icon; } }
    public int value { get { return m_value; } }
    public int cost { get { return m_cost; } }


    public Shop(
        string key,
        string name,
        TYPE_SHOP_CATEGORY typeShopCategory,
        TYPE_SHOP_CATEGORY typeCostCategory,
        Sprite icon,
        int value,
        int cost)
    {
        m_key = key;
        m_name = name;
        m_typeShopCategory = typeShopCategory;
        m_icon = icon;
        m_value = value;
        m_typeCostCategory = typeCostCategory;
        m_cost = cost;
    }
}

