using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletActorManager : MonoBehaviour, IActorManager {

    GameController m_gameController;

    [SerializeField]
    BulletActor m_bulletActor;

    List<IActor> m_useActorList = new List<IActor>();

    //미사용 액터
    Queue<IActor> m_idleActorQueue = new Queue<IActor>();
    //

    //사용 액터
    private List<IActor> useActorList { get { return m_useActorList; } }

    //미사용 액터
    private Queue<IActor> idleActorQueue { get { return m_idleActorQueue; } }

    //    int m_actorCount = 100;


    public void setController(GameController gameController)
    {
        idleActorQueue.Clear();
        useActorList.Clear();

        Debug.Log("Clear");

        m_gameController = gameController;
        for (int i = 0; i < Prep.bulletActorObjFoolCnt; i++)
        {
            idleActorQueue.Enqueue(createActor());
        }
    }


    /// <summary>
    /// 행동
    /// </summary>
    public void uiUpdate()
    {

        for(int i = 0; i < useActorList.Count; i++){
            useActorList[i].uiUpdate();
        }
    }



    /// <summary>
    /// 탄환 랜덤 생성
    /// </summary>
    /// <param name="bulletKey"></param>
    /// <param name="iActor"></param>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="attack"></param>
    /// <param name="count"></param>
    /// <param name="gapTime"></param>
    public void createActor(string bulletKey, IActor iActor, Vector2 center, float radius, int attack, int count = 1, float gapTime = 0.1f)
    {
        StartCoroutine(createBulletCoroutine(bulletKey, iActor, center, radius, attack, count, gapTime));
    }

    IEnumerator createBulletCoroutine(string bulletKey, IActor iActor, Vector2 targetPos, float radius, int attack, int count = 1, float gapTime = 0.1f)
    {
        while (count-- > 0)
        {
            MapController.TYPE_MAP_LINE typeMapLine = m_gameController.mapCtrler.getRandomLineIndex(iActor.uiController);
            Vector2 randPos = m_gameController.mapCtrler.getRandomLinePosition(iActor.uiController, targetPos, radius, (int)typeMapLine);
            int layer = Prep.getLayer(typeMapLine, Unit.TYPE_MOVEMENT.Gnd);

            createActor(bulletKey, iActor, randPos, layer, attack);

            yield return new WaitForSeconds(gapTime);
        }
    }


    /// <summary>
    /// 탄환 생성

    /// </summary>
    /// <param name="bulletKey"></param>
    /// <param name="iActor"></param>
    /// <param name="attack"></param>
    /// <param name="target"></param>
    /// <param name="targetLayerMask"></param>
    /// <param name="count"></param>
    /// <param name="gapTime"></param>
    public void createActor(string bulletKey, IActor iActor, IActor targetActor, int attack, int count = 1, float gapTime = 0.1f)
    {
//        StartCoroutine(createBulletCoroutine(bulletKey, iActor, targetActor.transform.position, targetActor.layer, attack, count, gapTime));
        StartCoroutine(createBulletCoroutine(bulletKey, iActor, targetActor.getPosition(iActor.layer), targetActor.layer, attack, count, gapTime));
    }

    IEnumerator createBulletCoroutine(string bulletKey, IActor iActor, Vector2 targetPos, int targetLayer, int attack, int count = 1, float gapTime = 0.1f)
    {
        while (count-- > 0)
        {


            createActor(bulletKey, iActor, targetPos, targetLayer, attack);
            ////큐가 비어있으면 새로 생성
            //if (idleActorQueue.Count <= 0)
            //{
            //    idleActorQueue.Enqueue(createActor());
            //}


            ////위치 및 탄환 생성
            //IActor bulletActor = idleActorQueue.Dequeue();
            //Bullet bullet = BulletManager.GetInstance.getBullet(bulletKey);

            //((BulletActor)bulletActor).setBullet(bullet, iActor, targetActor.transform.position, attack);
            //((BulletActor)bulletActor).GetComponent<Collider2D>().enabled = true;
            //useActorList.Add(bulletActor);

            yield return new WaitForSeconds(gapTime);
        }
    }

    /// <summary>
    /// 탄환 생성 - 탄환키, 다른 공격력
    /// </summary>
    /// <param name="bulletKey"></param>
    /// <param name="iActor"></param>
    /// <param name="attack"></param>
    /// <param name="target"></param>
    /// <param name="targetLayerMask"></param>
    //public void createActor(string bulletKey, IActor iActor, IActor targetActor, int attack)
    //{

    //    createActor(bulletKey, iActor, targetActor, attack, iActor.transform.position);

    //    //큐가 비어있으면 새로 생성
    //    //if (idleActorQueue.Count <= 0)
    //    //{
    //    //    idleActorQueue.Enqueue(createBulletActor());
    //    //}


    //    ////위치 및 탄환 생성
    //    //BulletActor bulletActor = idleActorQueue.Dequeue();
    //    //Bullet bullet = BulletManager.GetInstance.getBullet(bulletKey);

    //    //bulletActor.setBullet(bullet, unitActor, attack, target, targetLayerMask, unitActor.transform.position);
    //    //bulletActor.GetComponent<Collider2D>().enabled = true;
    //    //useActorList.Add(bulletActor);
    //}

    /// <summary>
    /// 탄환 생성 - 탄환키
    /// </summary>
    /// <param name="bulletKey"></param>
    /// <param name="iActor"></param>
    /// <param name="target"></param>
    /// <param name="targetLayerMask"></param>
    public void createActor(string bulletKey, IActor iActor, IActor targetActor)
    {
        createActor(bulletKey, iActor, targetActor, iActor.attack);

        //큐가 비어있으면 새로 생성
        //if (idleActorQueue.Count <= 0)
        //{
        //    idleActorQueue.Enqueue(createBulletActor());
        //}


        ////위치 및 탄환 생성
        //BulletActor bulletActor = idleActorQueue.Dequeue();
        //Bullet bullet = BulletManager.GetInstance.getBullet(bulletKey);

        //bulletActor.setBullet(bullet, unitActor, target, targetLayerMask, unitActor.transform.position);
        //bulletActor.GetComponent<Collider2D>().enabled = true;
        //useActorList.Add(bulletActor);
    }

    /// <summary>
    /// 탄환 생성
    /// </summary>
    /// <param name="bulletKey"></param>
    /// <param name="iActor"></param>
    /// <param name="attack"></param>
    /// <param name="target"></param>
    /// <param name="targetLayerMask"></param>
    /// <param name="targetPos"></param>
    void createActor(
        string bulletKey, 
        IActor iActor, 
        Vector2 targetPos, 
        int targetLayer, 
        int attack
        )
    {
        //큐가 비어있으면 새로 생성
        if (idleActorQueue.Count <= 0)
        {
            idleActorQueue.Enqueue(createActor());
        }


        //위치 및 탄환 생성
        IActor bulletActor = idleActorQueue.Dequeue();
        Bullet bullet = BulletManager.GetInstance.getBullet(bulletKey);


        //성은 2 위로 공격
        //
        if (targetLayer == UnityEngine.LayerMask.NameToLayer("TotalLine"))
            targetPos.y += 2f;

        ((BulletActor)bulletActor).setBullet(bullet, iActor, targetPos, attack, targetLayer);

        
        ((BulletActor)bulletActor).GetComponent<Collider2D>().enabled = true;
        useActorList.Add(bulletActor);
    }


    /// <summary>
    /// 기본 탄환 생성
    /// </summary>
    /// <param name="iActor"></param>
    /// <param name="uiController"></param>
    /// <param name="pos"></param>
    public void createActor(IActor iActor, IActor targetActor)//, UIController uiController)
    {


        createActor(iActor.key, iActor, targetActor, iActor.attack);

        //큐가 비어있으면 새로 생성
        //if (idleActorQueue.Count <= 0)
        //{
        //    idleActorQueue.Enqueue(createBulletActor());
        //}


        ////위치 및 탄환 생성
        //BulletActor bulletActor = idleActorQueue.Dequeue();
        //Bullet bullet = BulletManager.GetInstance.getBullet(unitActor.key);

        //bulletActor.setBullet(bullet, unitActor, target, targetLayerMask);
        //bulletActor.transform.position = unitActor.transform.position;
        //bulletActor.GetComponent<Collider2D>().enabled = true;
        //useActorList.Add(bulletActor);
    }


    /// <summary>
    /// 탄환 행동자 반납
    /// </summary>
    /// <param name="iActor"></param>
    /// <returns></returns>
    public bool removeActor(IActor iActor)
    {
        if (useActorList.Contains(iActor))
        {
            ((BulletActor)iActor).clear();
            useActorList.Remove(iActor);
            ((BulletActor)iActor).gameObject.layer = 0;
            idleActorQueue.Enqueue(iActor);
            return true;
        }
        return false;
    }


    /// <summary>
    /// 탄환 행동자 생성
    /// </summary>
    /// <returns></returns>
    IActor createActor()
    {
        BulletActor bulletActor = Instantiate<BulletActor>(m_bulletActor);
        bulletActor.removeBulletActorEvent += removeActor;
        bulletActor.lineEvent += m_gameController.mapCtrler.getLine;
        bulletActor.clear();
        return bulletActor;
    }

    //public bool clearActor()
    //{
    //    for (int i = useActorList.Count - 1; i >= 0; i--)
    //    {
    //        ((BulletActor)useActorList[i]).removeActor();
    //    }

    //    while (idleActorQueue.Count > 0)
    //    {
    //        BulletActor actor = idleActorQueue.Dequeue() as BulletActor;
    //        Destroy(actor.gameObject);
    //    }

    //    idleActorQueue.Clear();
    //    return true;
    //}



    //void OnDisable()
    //{
    //    clearActor();
    //}

}
