using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AccountSinario
{

    //스테이지 진행도
    //진행한 마지막 스테이지를 기억해야 함
//    string m_sinarioKey;


    //완료된 시나리오 리스트
    //시나리오 키, 별 획득 개수
    //선형으로 되어있음
    //마지막 키가 현재 완료된 메인키
    List<string> m_stageClearList = new List<string>();

    //현재 진행하는 등록 - 등록됨
    public string stageKey { 
        get { 
            //스테이지를 1번 이상 클리어 했으면 
            //클리어한 키의 조건키중 메인키가 스테이지 키
            if (m_stageClearList.Count > 0)
            {
                string clearStageKey = m_stageClearList.LastOrDefault();
                string nowStageKey = WorldManager.GetInstance.getNextStageKey(clearStageKey);
                return nowStageKey;
            }
            //처음 시작이면 첫번째 키
            return WorldManager.GetInstance.getFirstStageKey();
        } 
    }

    /// <summary>
    /// 현재 플레이하는 스테이지
    /// </summary>
    public Stage nowStage { get; set; }

    public AccountSinario(){
//        m_stageClearList.Add("Stage018");
    }

    /// <summary>
    /// 직렬화된 데이터 변환하기
    /// </summary>
    /// <param name="accSerial"></param>
    public AccountSinario(AccountSinarioSerial accSerial)
    {
        accSerial.stageClearList.AddRange(m_stageClearList);
    }

    /// <summary>
    /// 클리어한 스테이지 유무
    /// </summary>
    /// <param name="stageKey"></param>
    /// <returns></returns>
    public bool isStage(string stageKey)
    {
        return m_stageClearList.Contains(stageKey);
    }

    /// <summary>
    /// 시나리오 삽입하기
    /// </summary>
    /// <param name="stageKey"></param>
    /// <param name="count"></param>
    public void setStageClear(string stageKey)
    {
        if (!m_stageClearList.Contains(stageKey))
        {
            m_stageClearList.Add(stageKey);
        }
    }

    /// <summary>
    /// 직렬화 데이터 출력
    /// </summary>
    /// <returns></returns>
    public AccountSinarioSerial getSerial()
    {
        AccountSinarioSerial accSerial = new AccountSinarioSerial();
        accSerial.stageClearList.AddRange(m_stageClearList);
        return accSerial;
    }

    /// <summary>
    /// 메인 총 클리어수
    /// </summary>
    /// <returns></returns>
    public int getMainClearCount()
    {
        return m_stageClearList.Count;
    }
}
