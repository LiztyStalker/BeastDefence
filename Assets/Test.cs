using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
    //void Start () {

    //    int day = 6;
    //    int k = 1;

    //  int[] answer = new int[12];
    //  int[] days = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    //  int preDay = 0;
      
      
      
    //  for(int i = 0; i < 12; i++){
    //      if (i > 0)
    //          preDay += days[i - 1] % 7;
    //      int week = (k + day + preDay - 1) % 7;
          
    //      if(week >= 5)
    //          answer[i] = 1;
    //      else
    //          answer[i] = 0;
    //      Debug.Log(i + " : " + answer[i]);

    //  }




    // }


    //int[][] mapCheck = new int[][]{
    //        new int[]{0,0,0}, 
    //        new int[]{0,0,0}, 
    //        new int[]{0,0,0}
    //    };

    int[][] mapCheck = new int[][]{
            new int[]{0,0,0,0,0}, 
            new int[]{0,0,0,0,0}, 
            new int[]{0,0,0,0,0}, 
            new int[]{0,0,0,0,0}
        };

    void Start()
    {

        int[][] maps = new int[][]{
            new int[]{0,0,1,0,0}, 
            new int[]{0,1,1,0,1}, 
            new int[]{0,0,1,0,1}, 
            new int[]{1,1,1,0,1}
        };

        //int[][] maps = new int[][]{
        //    new int[]{0,0,1}, 
        //    new int[]{0,1,1}, 
        //    new int[]{0,0,1}
        //};
       
        
        int answer = 0;
        int maxY = maps.Length;
        int maxX = maps[0].Length;
        
        
        
        
        //먼저 섬 찾기
        for(int y = 0; y < maxY; y++){
            for(int x = 0; x < maxX; x++){
                //섬이면
                if(maps[y][x] == 1){
                    //이미 체크되지 않았으면
                    if(mapCheck[y][x] == 0){
                        //섬 들어가기
                        int value = map(maps, x, y, maxX, maxY);
                        
                        //들어간 섬이 큰 섬이면
                        if(answer < value)
                            answer = value;
                    }
                }           
            }
        }
        
        //섬 구하기
        
        
        Debug.Log(answer);
    }
    
    
    
    int map(int[][] maps, int mX, int mY, int maxX, int maxY){
        int value = 0;

        //상륙하지 않은 섬 확인
        if (mapCheck[mY][mX] == 0)
        {
            mapCheck[mY][mX] = -1;

            if (mX > 0)
                value += mapChecker(maps, mX - 1, mY, maxX, maxY);
            else
                value += 1;

            if (mX < maxX - 1)
                value += mapChecker(maps, mX + 1, mY, maxX, maxY);
            else
                value += 1;

            if (mY > 0)
                value += mapChecker(maps, mX, mY - 1, maxX, maxY);
            else
                value += 1;

            if (mY < maxY - 1)
                value += mapChecker(maps, mX, mY + 1, maxX, maxY);
            else
                value += 1;
        }
        
        return value;
    }
    
    
    int mapChecker(int[][] maps, int mX, int mY, int maxX, int maxY){
        //또 다른 섬 확인 

        if(maps[mY][mX] == 1)
            return map(maps, mX, mY, maxX, maxY);

        //모서리이면 1 추가
        else
            return 1;
    }
    
 }


