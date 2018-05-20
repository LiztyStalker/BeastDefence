using System;
using IOPackage;

/// <summary>
/// 계정 데이터 카테고리
/// </summary>
public enum TYPE_ACCOUNT_CATEGORY { 
    Gold, 
    Fruit, 
    Food, 
    Level, 
    Exp, 
    Cmd_Exp, 
    Card 
}


public class Account : SingletonClass<Account>
{    
    
    //데이터
    AccountData m_accountData;


    //막사
    AccountUnit m_accountUnit;
        //유닛슬롯
        //영웅슬롯
        //지휘관슬롯

    //개발 및 연구
    AccountDevelop m_accountDevelop;
    
    //스킬
    //AccountSkill m_accountSkill;
    
    //시나리오
    AccountSinario m_accountSinario;


    //업적
    AccountAchieve m_accountAchieve;


    public string nextScene { get; set; }

    public string name { get { return accData.name; } }
    public int level { get { return accData.level; } }
    public int fruit { get { return accData.fruit; } }
    public int gold { get { return accData.gold; } }
    public int maxFood { get { return accData.maxFood; } }
    public int nowFood { get { return accData.nowFood; } }
    public int nowExp { get { return accData.nowExp; } }
    public int maxExp { get { return accData.maxExp; } }

    public AccountUnit accUnit { get { return m_accountUnit; } }
    //public AccountSkill accSkill { get { return m_accountSkill; } }
    public AccountDevelop accDevelop { get { return m_accountDevelop; } }
    public AccountSinario accSinario { get { return m_accountSinario; } }
    public AccountData accData { get { return m_accountData; } }
    public AccountAchieve accAchieve { get { return m_accountAchieve; } }

    public Account()
    {
        if (!loadData())
        {
            m_accountUnit = new AccountUnit();
            //m_accountSkill = new AccountSkill();
            m_accountDevelop = new AccountDevelop();
            m_accountSinario = new AccountSinario();
            m_accountData = new AccountData();
            m_accountAchieve = new AccountAchieve();
        }
        //저장된 데이터가 있으면 가져오기
        //저장된 데이터가 없으면 새로 만들기
    }

   
    public bool loadData()
    {
        AccountSerial accSerial = IOData.GetInstance.loadData("data");
        if (accSerial != null)
        {
            m_accountUnit = new AccountUnit(accSerial.accUnitSerial);
            m_accountDevelop = new AccountDevelop(accSerial.accDevelopSerial);
            m_accountSinario = new AccountSinario(accSerial.accSinarioSerial);
            m_accountData = new AccountData(accSerial.accDataSerial);
            m_accountAchieve = new AccountAchieve(accSerial.accAchieveSerial);

            Prep.Log("", "불러오기 성공!", GetType());

            return true;
        }
        Prep.LogWarning("", "불러오기 실패", GetType());
        return false;
    }

    public bool saveData()
    {

        AccountSerial accSerial = new AccountSerial(this);
        if (IOData.GetInstance.saveData(accSerial, "data"))
        {
            //저장 성공
            Prep.Log("", "저장 성공!", GetType());
            return true;
        }
        //저장 실패
        Prep.LogWarning("", "저장 실패", GetType());
        return false;
    }

    /// <summary>
    /// 출전
    /// </summary>
    /// <returns></returns>
    public bool setEngage(TYPE_FORCE typeForce)
    {
        m_accountData.addGamePlay();
        return m_accountUnit.setEngageReport(typeForce);
    }


}

