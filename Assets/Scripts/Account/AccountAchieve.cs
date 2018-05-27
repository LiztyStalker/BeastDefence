using System;
using System.Collections;
using System.Collections.Generic;


public class AccountAchieve
{
    /// <summary>
    /// 획득한 업적 리스트
    /// </summary>
    List<string> m_achieveList = new List<string>();

   

    public IEnumerator achieveEnumerator { get { return m_achieveList.GetEnumerator(); } }

    public AccountAchieve(){}



    /// <summary>
    /// 업적 획득하기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool setAchieve(string key)
    {
        if (!isAchieve(key))
        {
            m_achieveList.Add(key);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 획득한 업적이 있는지 유무
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool isAchieve(string key)
    {
        return m_achieveList.Contains(key);
    }

    /// <summary>
    /// 달성한 업적 개수 가져오기
    /// </summary>
    /// <returns></returns>
    public int getAchieveSuccessCount()
    {
        return m_achieveList.Count;
    }


    /// <summary>
    /// 업적 데이터 가져오기 
    /// </summary>
    /// <param name="typeAchieve"></param>
    /// <returns></returns>
    public int getAchieveValue(Achieve.TYPE_ACHIEVE typeAchieve)
    {
        switch (typeAchieve)
        {
            case Achieve.TYPE_ACHIEVE.GamePlay: //게임 플레이 횟수 - 
                return Account.GetInstance.accData.gameplayCnt;
            case Achieve.TYPE_ACHIEVE.AccLevel: //계정 레벨 - 
                return Account.GetInstance.accData.level;
            case Achieve.TYPE_ACHIEVE.CmdLevel: //지휘관 최고 레벨 - 
                return Account.GetInstance.accUnit.getHighCommanderLevel();
            case Achieve.TYPE_ACHIEVE.DevCount: //개발 횟수 - 
                return Account.GetInstance.accDevelop.getTotalDevelopCount();
            case Achieve.TYPE_ACHIEVE.HeroLevel: //영웅 최고 레벨 - 
                return Account.GetInstance.accUnit.getHighUnit(Unit.TYPE_UNIT.Hero);
            case Achieve.TYPE_ACHIEVE.SoldierLevel: //병사 최고 레벨 - 
                return Account.GetInstance.accUnit.getHighUnit(Unit.TYPE_UNIT.Soldier);
            case Achieve.TYPE_ACHIEVE.LoginDay: //총 접속일 수 - 
                return Account.GetInstance.accData.getTotalDays();
            case Achieve.TYPE_ACHIEVE.MainStageCnt: //메인 클리어 횟수 - 
                return Account.GetInstance.accSinario.getMainClearCount();
            case Achieve.TYPE_ACHIEVE.PlayTime: //게임 진행 시간 - 
                return Account.GetInstance.accData.getPlayTime();
            case Achieve.TYPE_ACHIEVE.TotalFood: //총 획득 식량 -
                return Account.GetInstance.accData.totalFood;
            case Achieve.TYPE_ACHIEVE.TotalFruit: //총 획득 열매 -
                return Account.GetInstance.accData.totalFruit;
            case Achieve.TYPE_ACHIEVE.TotalGold: //총 획득 골드 -
                return Account.GetInstance.accData.totalGold;
            case Achieve.TYPE_ACHIEVE.TotalUsedFood: //총 사용 식량 -
                return Account.GetInstance.accData.usedFood;
            case Achieve.TYPE_ACHIEVE.TotalUsedFruit: //총 사용 열매 -
                return Account.GetInstance.accData.usedFruit;
            case Achieve.TYPE_ACHIEVE.TotalUsedGold: //총 사용 골드 -
                return Account.GetInstance.accData.usedGold;
            case Achieve.TYPE_ACHIEVE.TotalUsedHero: //총 출전 영웅 수 -
                return Account.GetInstance.accUnit.totalHeroEngage;
            case Achieve.TYPE_ACHIEVE.TotalUsedSoldier: //총 출전 병사 수 -
                return Account.GetInstance.accUnit.totalSoldierEngage;
            case Achieve.TYPE_ACHIEVE.TotalUsedUnit: //총 출전 유닛 수 -
                return Account.GetInstance.accUnit.totalUnitEngage;
            case Achieve.TYPE_ACHIEVE.GetUnit: //총 획득 유닛 수 -
                return Account.GetInstance.accUnit.getTotalUnitCount();
            case Achieve.TYPE_ACHIEVE.GetHero: //총 획득 영웅 수 -
                return Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Hero);
            case Achieve.TYPE_ACHIEVE.GetSoldier: //총 획득 병사 수 -
                return Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Soldier);
            default:
                Prep.LogError(typeAchieve.ToString(), "를 선언하지 않았습니다.", GetType());
                break;
        }
        return 0;
    }



    /// <summary>
    /// 역직렬화
    /// </summary>
    /// <param name="accAchieveSerial"></param>
    public AccountAchieve(AccountAchieveSerial accSerial)
    {
        //역직렬화
        m_achieveList.AddRange(accSerial.achieveList);
    }

    /// <summary>
    /// 직렬화
    /// </summary>
    /// <returns></returns>
    public AccountAchieveSerial getSerial()
    {
        AccountAchieveSerial accSerial = new AccountAchieveSerial();
        accSerial.achieveList.AddRange(m_achieveList);
        return accSerial;
    }
}

