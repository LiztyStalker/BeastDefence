using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccountSerial
{
    public string nowVersion; //저장된 버전
    
    public AccountUnitSerial accUnitSerial;
    public AccountDevelopSerial accDevelopSerial;
    public AccountSinarioSerial accSinarioSerial;
    public AccountDataSerial accDataSerial;
    public AccountAchieveSerial accAchieveSerial;

    public float version()
    {
        try
        {
            float version = float.Parse(Application.version);
            float.TryParse(nowVersion, out version);
            return version;
        }
        catch
        {
            return -1f;
        }
    }


    public AccountSerial() { }

    /// <summary>
    /// 시리얼화
    /// </summary>
    /// <param name="account"></param>
    public AccountSerial(Account account)
    {
        accUnitSerial = account.accUnit.getSerial();
        accDevelopSerial = account.accDevelop.getSerial();
        accSinarioSerial = account.accSinario.getSerial();
        accDataSerial = account.accData.getSerial();
        accAchieveSerial = account.accAchieve.getSerial();
    }



}