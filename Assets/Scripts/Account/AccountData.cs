using System;


public class AccountData
{
    //닉네임
    string m_nickName;
    //레벨
    int m_level;

    //경험치
    int m_nowExperiance;
//    int m_maxExperiance;

//    int m_nowCommanderExperiance;
//    int m_maxCommanderExperiance;

    //버전
    string m_version;

    //어플코드


    //골드
    int m_gold; //현재골드
    int m_totalGold; //총획득골드
    int m_usedGold; //총사용골드

    //열매
    int m_fruit; //현재열매
    int m_totalFruit; //총획득열매
    int m_usedFruit; //총사용열매

    //식량
    int m_nowFood; //현재식량
    int m_maxFood; //최대식량
    int m_totalFood; //총획득식량
    int m_usedFood; //총사용식량
    

    //전투 카운트
    int m_gamePlayCnt;

    //승리횟수
//    int m_victoryCnt;

    //패배횟수
//    int m_defeatCnt;
    
    //총 접속일
    int m_totalDays = 0;


    //마지막 접속 시간
    DateTime m_lastTime = new DateTime();

    //획득시간
    TimeSpan m_getTime = new TimeSpan(0, 5, 0);

    //플레이타임
    TimeSpan m_playTime = new TimeSpan();
    //
    //현재 시간
    //DateTime m_nowTime;

    //시나리오

    public string version { get { return m_version; } }
    public string name { get { return m_nickName; } }
    public int level { get { return m_level; } }
    public int nowExp { get { return m_nowExperiance; } }
    public int maxExp { get { return getMaxExpCalculator(); } }
    public float expRate { get { return (float)nowExp / (float)maxExp; } }

    public int gold { get { return m_gold; } }
    public int totalGold { get { return m_totalGold; } }
    public int usedGold { get { return m_usedGold; } }

    public int fruit { get { return m_fruit; } }
    public int totalFruit { get { return m_totalFruit; } }
    public int usedFruit { get { return m_usedFruit; } }

    public int nowFood { get { return m_nowFood; } }
    public int maxFood { get { return m_maxFood; } }
    public int totalFood { get { return m_totalFood; } }
    public int usedFood { get { return m_usedFood; } }


    public int gameplayCnt { get { return m_gamePlayCnt; } }
//    public int victoryCnt { get { return m_victoryCnt; } }
//    public int defeatCnt { get { return m_defeatCnt; } }

//    public TimeSpan foodTimer { get { return m_foodTimer; } }
    public DateTime lastTime { get { return m_lastTime; } }

    public AccountData()
    {
        m_level = 1;
        m_nowExperiance = 0;

        m_gold = 100000;
        m_fruit = 100000;
        m_maxFood = 5000;
        m_nowFood = m_maxFood;
        m_lastTime = DateTime.UtcNow;
//        m_maxExperiance = getE

        m_totalDays = 1;
    }



    /// <summary>
    /// 값 추가
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeCategory"></param>
    public void addValue(int value, SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeCategory)
    {
        switch (typeCategory)
        {
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Gold:
                m_gold += value;
                m_totalGold += value;
                break;
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Fruit:
                m_fruit += value;
                m_totalFruit += value;
                break;
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Food:
                m_nowFood += value;
                m_totalFood += value;
                break;
            case SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Exp:
                m_nowExperiance += value;
                while (m_nowExperiance >= maxExp)
                {
                    addValue(1, TYPE_ACCOUNT_CATEGORY.Level);
                    useValue(maxExp, TYPE_ACCOUNT_CATEGORY.Exp);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 값 추가
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeCategory"></param>
    public void addValue(int value, TYPE_ACCOUNT_CATEGORY typeCategory)
    {
        switch (typeCategory)
        {
            case TYPE_ACCOUNT_CATEGORY.Gold:
                m_gold += value;
                m_totalGold += value;
                break;
            case TYPE_ACCOUNT_CATEGORY.Food:
                m_nowFood += value;
                m_totalFood += value;
                break;
            case TYPE_ACCOUNT_CATEGORY.Fruit:
                m_fruit += value;
                m_totalFruit += value;
                break;
            case TYPE_ACCOUNT_CATEGORY.Level:
                m_level += value;
                break;
            case TYPE_ACCOUNT_CATEGORY.Exp:
                m_nowExperiance += value;
                //넘으면 레벨업
                while(m_nowExperiance >= maxExp)
                {
                    addValue(1, TYPE_ACCOUNT_CATEGORY.Level);
                    useValue(maxExp,  TYPE_ACCOUNT_CATEGORY.Exp);
                }
                break;
            //지휘관 경험치
            //case TYPE_ACCOUNT_CATEGORY.Cmd_Exp:
            //    if (m_nowCommanderExperiance + value >= m_maxCommanderExperiance)
            //        m_nowCommanderExperiance = m_maxCommanderExperiance;
            //    else
            //        m_nowCommanderExperiance += value;

            //    break;
        }
    }

    /// <summary>
    /// 진행시간 추가하기
    /// </summary>
    /// <param name="timeSpan"></param>
    public void addPlayTime(TimeSpan timeSpan)
    {
        m_playTime.Add(timeSpan);
    }

    /// <summary>
    /// 상점에서 추가
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeShopCategory"></param>
    public void addValue(int value, Shop.TYPE_SHOP_CATEGORY typeShopCategory)
    {
        addValue(value, shopCategoryToAccountCategory(typeShopCategory));
    }


    TYPE_ACCOUNT_CATEGORY shopCategoryToAccountCategory(Shop.TYPE_SHOP_CATEGORY typeShopCategory)
    {
        TYPE_ACCOUNT_CATEGORY typeCategory = TYPE_ACCOUNT_CATEGORY.Gold;

        switch (typeShopCategory)
        {
            case Shop.TYPE_SHOP_CATEGORY.Gold:
                typeCategory = TYPE_ACCOUNT_CATEGORY.Gold;
                break;
            case Shop.TYPE_SHOP_CATEGORY.Fruit:
                typeCategory = TYPE_ACCOUNT_CATEGORY.Fruit;
                break;
            case Shop.TYPE_SHOP_CATEGORY.Food:
                typeCategory = TYPE_ACCOUNT_CATEGORY.Food;
                break;
        }
        return typeCategory;
    }

    public void setName(string name)
    {
        m_nickName = name;
    }

    void foodCalculator()
    {

        //현재 시간 - 마지막시간 / 5분 = 식량 수급량
        int minutes = Convert.ToInt32(DateTime.UtcNow.Subtract(lastTime).TotalMinutes);

        if (m_nowFood + (minutes / m_getTime.Minutes) > m_maxFood)
            m_nowFood = m_maxFood;
        else
        {
            m_nowFood += (minutes / m_getTime.Minutes);
            m_totalFood += (minutes / m_getTime.Minutes);
        }
        
        m_lastTime = DateTime.UtcNow;

    }

    public string nowTime()
    {
//        foodCalculator();

        if (m_nowFood >= m_maxFood)
        {
            m_lastTime = DateTime.UtcNow;
            return "-:-";
        }

        TimeSpan nowTime = m_getTime.Subtract(DateTime.UtcNow.Subtract(lastTime));
        if (nowTime.TotalMinutes < 0)
            foodCalculator();
        return string.Format("{0:d2}:{1:d2}", nowTime.Minutes, nowTime.Seconds);
    }


    public float nowTimeRate()
    {
        if (m_nowFood >= m_maxFood)
            return 0f;

        return (float)(DateTime.UtcNow.Subtract(lastTime).TotalSeconds / m_getTime.TotalSeconds);
    }


    /// <summary>
    /// 사용 가능한지 확인
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeAccount"></param>
    /// <returns></returns>
    public bool isValue(int value, TYPE_ACCOUNT_CATEGORY typeAccount)
    {
        switch (typeAccount)
        {
            case TYPE_ACCOUNT_CATEGORY.Gold:
                return isValue(m_gold, value);
            case TYPE_ACCOUNT_CATEGORY.Food:
                return isValue(m_nowFood, value);
            case TYPE_ACCOUNT_CATEGORY.Fruit:
                return isValue(m_fruit, value);
//            case TYPE_ACCOUNT_CATEGORY.Cmd_Exp:
//                return isValue(m_nowCommanderExperiance, value);
        }
        return false;
    }

    /// <summary>
    /// 사용 가능한지 확인
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeShopCategory"></param>
    public bool isValue(int value, Shop.TYPE_SHOP_CATEGORY typeShopCategory)
    {
        return isValue(value, shopCategoryToAccountCategory(typeShopCategory));
    }

    /// <summary>
    /// 사용
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeAccount"></param>
    /// <returns></returns>
    public bool useValue(int value, TYPE_ACCOUNT_CATEGORY typeAccount)
    {
        switch (typeAccount)
        {
            case TYPE_ACCOUNT_CATEGORY.Gold:
                if (isValue(m_gold, value))
                {
                    m_gold -= value;
                    m_usedGold += value;
                    return true;
                }
                break;
            case TYPE_ACCOUNT_CATEGORY.Food:
                if (isValue(m_nowFood, value))
                {
                    m_nowFood -= value;
                    m_usedFood += value;
                    return true;
                }
                break;
            case TYPE_ACCOUNT_CATEGORY.Fruit:
                if (isValue(m_fruit, value))
                {
                    m_fruit -= value;
                    m_usedFruit += value;
                    return true;
                }
                break;
            case TYPE_ACCOUNT_CATEGORY.Exp:
                if (isValue(m_nowExperiance, value))
                {
                    m_nowExperiance -= value;
                    return true;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// 사용
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeShopCategory"></param>
    /// <returns></returns>
    public bool useValue(int value, Shop.TYPE_SHOP_CATEGORY typeShopCategory)
    {
        return useValue(value, shopCategoryToAccountCategory(typeShopCategory));
    }

    bool isValue(int value, int remove)
    {
        if (value - remove < 0)
            return false;
        return true;
    }

    /// <summary>
    /// 게임 플레이 추가
    /// </summary>
    public void addGamePlay()
    {
        m_gamePlayCnt++;
    }
    
    /// <summary>
    /// 최대 경험치 가져오기
    /// </summary>
    /// <returns></returns>
    int getMaxExpCalculator()
    {
        return m_level * 100;
    }

    /// <summary>
    /// 총 접속일 가져오기
    /// </summary>
    /// <returns></returns>
    public int getTotalDays()
    {
        return m_totalDays;
    }

    /// <summary>
    /// 총 진행 시간 가져오기
    /// </summary>
    /// <returns></returns>
    public int getPlayTime()
    {
        return m_playTime.Hours;
    }


    #region ################################ 직렬화 #####################################

    /// <summary>
    /// 직렬화하기
    /// </summary>
    /// <returns></returns>
    public AccountDataSerial getSerial()
    {
        AccountDataSerial accSerial = new AccountDataSerial();
        accSerial.nickName = m_nickName;

        accSerial.level = m_level;
        accSerial.nowExp = m_nowExperiance;

        accSerial.nowFood = nowFood;
        accSerial.maxFood = maxFood;
        accSerial.totalFood = m_totalFood;
        accSerial.usedFood = m_usedFood;

        accSerial.fruit = fruit;
        accSerial.totalFruit = m_totalFruit;
        accSerial.usedFruit = m_usedFruit;

        accSerial.totalGold = m_totalGold;
        accSerial.usedGold = m_usedGold;
        accSerial.gold = gold;

//        accSerial.victoryCnt = m_victoryCnt;
//        accSerial.defeatCnt = m_defeatCnt;

        accSerial.gamePlayCnt = m_gamePlayCnt;

        accSerial.totalDays = m_totalDays;

        accSerial.lastTime = m_lastTime.ToString();
        accSerial.playTime = m_playTime.ToString();

        return accSerial;

    }

    /// <summary>
    /// 역직렬화
    /// </summary>
    /// <param name="accSerial"></param>
    public AccountData(AccountDataSerial accSerial)
    {

        m_nickName = accSerial.nickName;

        m_level = accSerial.level;
        m_nowExperiance = accSerial.nowExp;

        m_nowFood = accSerial.nowFood;
        m_maxFood = accSerial.maxFood;
        m_totalFood = accSerial.totalFood;
        m_usedFood = accSerial.usedFood;

        m_fruit = accSerial.fruit;
        m_usedFruit = accSerial.usedFruit;
        m_totalFruit = accSerial.totalFruit;

        m_gold = accSerial.gold;
        m_usedGold = accSerial.usedGold;
        m_totalGold = accSerial.totalGold;

        //        m_victoryCnt = accSerial.victoryCnt;
        //        m_defeatCnt = accSerial.defeatCnt;

        m_gamePlayCnt = accSerial.gamePlayCnt;

        m_totalDays = accSerial.totalDays;

        DateTime.TryParse(accSerial.lastTime, out m_lastTime);
        TimeSpan.TryParse(accSerial.playTime, out m_playTime);

        //마지막 접속시간과 현재 접속시간의 날짜가 다르면 (년도, 월, 일) 1 추가
        if (m_lastTime.Day != DateTime.UtcNow.Day ||
           m_lastTime.Month != DateTime.UtcNow.Month ||
           m_lastTime.Year != DateTime.UtcNow.Year
            )
        {
            m_totalDays++;
        }

    }


    #endregion
}

