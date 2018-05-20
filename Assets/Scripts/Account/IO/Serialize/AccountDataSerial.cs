using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class AccountDataSerial
{
    //닉네임
    public string nickName;
    //레벨
    public int level;

    //경험치
    public int nowExp;
    public int maxExp;

    //버전
//    string m_version;

    //어플코드


    //골드
    public int gold; //현재골드
    public int totalGold; //총획득골드
    public int usedGold; //총사용골드

    //열매
    public int fruit; //현재열매
    public int totalFruit; //총획득열매
    public int usedFruit; //총사용열매

    //식량
    public int nowFood; //현재식량
    public int maxFood; //최대식량
    public int totalFood; //총획득식량
    public int usedFood; //총사용식량


    //전투 카운트
    public int gamePlayCnt;

    //승리횟수
//    public int victoryCnt;

    //패배횟수
//    public int defeatCnt;
    
    //마지막 접속 시간
    public string lastTime;

    //총 플레이시간
    public string playTime;

    //총 접속일
    public int totalDays;

    //
    //현재 식량 타임 5분
    //public string foodTimer;
}

