using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class AccountDevelop
{

    const char splitChar = '/';

    Dictionary<string, int> m_developDic = new Dictionary<string, int>();

    Dictionary<string, int> developDic { get { return m_developDic; } }

    public int totalDevelop{get{return developDic.Values.Sum(dev => dev);}}

    //성레벨   
    //보급레벨
    //방어레벨
    //최대레벨
    //경험치획득량


    public AccountDevelop() { }

    public AccountDevelop(AccountDevelopSerial accSerial)
    {
        foreach (string str in accSerial.developList)
        {
            string[] data = str.Split(splitChar);
            if (data.Length == 2)
            {
                try
                {
                    int cnt = int.Parse(data[1]);
                    m_developDic.Add(data[0], cnt);
                }
                catch (ArgumentNullException)
                {
                    Prep.LogError(str, "를 분할하는데 실패했습니다.", GetType());
                }
            }
        }
    }

    //지휘관


    /// <summary>
    /// 개발 여부 가져오기
    /// 없으면 0
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int getDevelopLevel(string key)
    {
        if (developDic.ContainsKey(key))
        {
            return developDic[key];
        }
        return 0;
    }
    

    /// <summary>
    /// 개발 삽입
    /// 1레벨로 시작
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool addDevelop(string key)
    {
        //키 삽입
        if (!developDic.ContainsKey(key))
        {
            developDic.Add(key, 0);
        }
        //키 레벨 증가
        developDic[key]++;
        return true;
    }

    /// <summary>
    /// 개발 값 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public float getDevelopValue(string key){
        int level = getDevelopLevel(key);
        Develop develop = DevelopManager.GetInstance.getDevelop(key);
        return develop.getValue(level);
    }



    /// <summary>
    /// 계산된 개발 값 가져오기
    /// </summary>
    /// <param name="typeValueDevelop"></param>
    /// <returns></returns>
    public float getDevelopValue(Develop.TYPE_DEVELOP_VALUE_GROUP typeValueDevelop)
    {
        //종류에 따른 개발 값 가져오기
        IEnumerator enumerator = DevelopManager.GetInstance.getDevelopEnumerator(typeValueDevelop);
        float value = 0f;

        while (enumerator.MoveNext())
        {
            Develop dev = enumerator.Current as Develop;

            //현재 키가 연구되어 있으면
            if (m_developDic.ContainsKey(dev.key))
            {
                //현재 값 가져와서 합하기
                value += dev.getValue(m_developDic[dev.key]);
            }
        }

        return value;
    }

    /// <summary>
    /// 개발 종류에 대한 테크 레벨 가져오기
    /// </summary>
    /// <param name="typeDevGroup"></param>
    /// <returns></returns>
    public int getDevelopTechLevel(Develop.TYPE_DEVELOP_GROUP typeDevGroup)
    {
        string techStr = string.Format("{0}TechLv", typeDevGroup);
        try{
            Develop.TYPE_DEVELOP_VALUE_GROUP typeValueGroup = (Develop.TYPE_DEVELOP_VALUE_GROUP)Enum.Parse(typeof(Develop.TYPE_DEVELOP_VALUE_GROUP), techStr);
            return (int)getDevelopValue(typeValueGroup);
        }
        catch(ArgumentException){
            Prep.LogError(techStr, " Enum 타입을 찾지 못함", GetType());
            return 0;
        }
        
    }

    public AccountDevelopSerial getSerial()
    {
        AccountDevelopSerial accSerial = new AccountDevelopSerial();
        foreach (string key in m_developDic.Keys)
        {
            string data = string.Format("{0}{2}{1}", key, m_developDic[key], splitChar);
            accSerial.developList.Add(data);
        }
        return accSerial;
    }

    /// <summary>
    /// 총 개발 개수
    /// </summary>
    /// <returns></returns>
    public int getTotalDevelopCount()
    {
        return m_developDic.Values.Sum();
    }
}

