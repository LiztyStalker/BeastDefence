using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletActor : MonoBehaviour, IActor {


    public delegate Vector3 LineDelegate(UIController uiCtrler, MapController.TYPE_MAP_LINE typeMapLine);
    public delegate bool RemoveBulletActorDelegate(BulletActor bulletActor);

    public event RemoveBulletActorDelegate removeBulletActorEvent;
    public event LineDelegate lineEvent;

    EffectActorManager m_effectActorManager;

    [SerializeField]
    SpriteRenderer m_sprite;

    //탄환 데이터
    Bullet m_bullet;
    int m_attack;

    //사용자 데이터
    IActor m_iActor;

    bool m_isUsed = false;

    float m_timer = 0f;

    const float m_removeTime = 1f;

    public int level { get { return m_iActor.level; } }

    public int attack { get { return (m_attack > 0) ? m_attack : m_iActor.attack; } }

    public UnitActor.TYPE_CONTROLLER typeController { get { return m_iActor.typeController; } }

    public Bullet.TYPE_SHOOT typeShoot { get { return m_bullet.typeShoot; } }

    public Bullet.TYPE_HIT typeHit { get { return m_bullet.typeHit; } }

    public float moveSpeed { get { return m_bullet.moveSpeed; } }

    public float range { get { return m_bullet.radius; } }

    public float sight { get { return range; }
    }

    //탄환 타입
    //곡사, 직사
    //곡사 - 포물선 운동
    //직사 - 등속운동

    /// <summary>
    /// 탄환 행동자에게 반납하는 델리게이트
    /// </summary>
    /// <param name="removeUnitActorDel"></param>
    //public void setDelegate(RemoveBulletActorDelegate removeBulletActorDel)
    //{
    //    removeBulletActorEvent = removeBulletActorDel;
    //}


	// Use this for initialization
	void Awake () {
        clear();
	}

    /// <summary>
    /// 초기화
    /// </summary>
    public void clear()
    {
        m_isUsed = true;
        m_bullet = null;
        m_sprite.sprite = null;
        gameObject.layer = 0;
        m_timer = 0f;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = new Vector3(0f, 10f, 0f);
        transform.eulerAngles = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 탄환 세팅
    /// </summary>
    /// <param name="bullet">탄환</param>
    /// <param name="iActor">사용자</param>
    /// <param name="attack">공격력</param>
    /// <param name="target">목표</param>
    /// <param name="targetLayerMask">라인 레이어마스크</param>
    /// <param name="startPos">시작위치</param>
    public void setBullet(Bullet bullet, IActor iActor, Vector2 target, int attack, int targetLayerMask)
    {
        m_attack = attack;
        setBullet(bullet, iActor, target, targetLayerMask);
    }

    /// <summary>
    /// 탄환 정의하기
    /// </summary>
    /// <param name="bullet">탄환</param>
    /// <param name="iActor">사용자</param>
    /// <param name="targetPos">목표위치</param>
    /// <param name="targetLayerMask">목표 레이어마스크</param>
    public void setBullet(Bullet bullet, IActor iActor, Vector2 targetPos, int targetLayerMask)
    {

        //스킬탄환 - bullet에 들어있음
        //사용자 - iActor

        gameObject.SetActive(true);

        m_isUsed = false;
        m_bullet = bullet;
        m_iActor = iActor;

       // Debug.Log("IActor : " + iActor);

        m_sprite.sprite = bullet.image;
        gameObject.layer = targetLayerMask;

//        transform.position = (Vector2)iActor.transform.position;

        //탄환 시작 위치
        switch (m_bullet.typeShootPos)
        {
            case Bullet.TYPE_START_POS.Myself:
                transform.position = iActor.getPosition(iActor.layer);
                break;
            case Bullet.TYPE_START_POS.Enemy:
                transform.position = targetPos;
                break;
            case Bullet.TYPE_START_POS.Castle:
                transform.position = iActor.uiController.getCastlePos();
                break;
        }

        //사용자가 유닛이면
        if (iActor is UnitActor)
        {
            //사용자키와 탄환키가 맞으면 - 기본공격
            if (iActor.key == m_bullet.key)
                m_bullet.effectKey = ((UnitActor)iActor).effectKey;
            //사용자키와 탄환키가 안 맞으면 - 스킬 공격
            else
                m_bullet.effectKey = m_bullet.key;
        }
        //사용자가 유닛이 아니면 - 지휘관
        else
            m_bullet.effectKey = iActor.key;

//        GetComponent<CircleCollider2D>().radius *= m_bullet.radius;

        float flip = (iActor.isFlip) ? 1f : -1f;


        switch (m_bullet.typeShoot)
        {
            case Bullet.TYPE_SHOOT.Direct:

                
//                Vector2 dir = target - (Vector2)iActor.transform.position;
                Vector2 dir = targetPos - iActor.getPosition(iActor.layer);

                float rad = Mathf.Atan2(dir.y, dir.x);

                float velX = Mathf.Cos(rad) * bullet.moveSpeed * flip;
                float velY = Mathf.Sin(rad) * bullet.moveSpeed;

                GetComponent<Rigidbody2D>().velocity = new Vector2(velX, velY);

                break;
            case Bullet.TYPE_SHOOT.Curved:
                float gravity = -Physics2D.gravity.y;

                float distanceX = targetPos.x - transform.position.x;
                float distanceY = targetPos.y - transform.position.y;

                float angle = 0.5f * Mathf.Asin((gravity * distanceX) / (bullet.moveSpeed * bullet.moveSpeed));

                //전위차 보정
                angle += Mathf.Atan2(distanceY, distanceX);

                float vX = Mathf.Cos(angle * flip) * bullet.moveSpeed;// *flip;
                float vY = Mathf.Sin(angle) * bullet.moveSpeed;

                GetComponent<Rigidbody2D>().velocity = new Vector2(vX, vY);
                GetComponent<Rigidbody2D>().gravityScale = 1f;
                break;
            case Bullet.TYPE_SHOOT.Drop:
                
                if (iActor.isFlip)
                {
                    transform.position += Vector3.right * 4f;
                }
                else
                {
                    transform.position += Vector3.left * 4f;
                }

                transform.position += Vector3.up * 8f;

                goto case Bullet.TYPE_SHOOT.Curved;
            case Bullet.TYPE_SHOOT.Set:
                //바닥에 심기
//                transform.position = m_iActor.transform.position;
                transform.position = m_iActor.getPosition(iActor.layer);
                GetComponent<Rigidbody2D>().gravityScale = 1f;
                if (m_bullet.typeHit == Bullet.TYPE_HIT.Splash)
                    GetComponent<Collider2D>().enabled = false;

                break;
        }

        

        //탄환 파티클 생성
        createEffect(m_bullet.effectKey + "ActiveParticle");

    }
	
	// Update is called once per frame
    public void uiUpdate () {

        if (m_bullet != null)
        {


            //맵 밖으로 벗어나면 반납


//            m_unitActor.gameObject.layer

            

            //관통탄은 1초 뒤에 사라짐
            if (m_bullet.typeHit == Bullet.TYPE_HIT.Penetrate)
            {
                m_timer += Prep.frameTime;
                if (m_timer >= 1f)
                {
                    removeBullet();
                    return;
                }
            }
            

            //적 레이어 가져오기
            int index = Prep.getIndexPosition(gameObject.layer);

//            Debug.Log(index);

            //성이면 바닥 판정을 하지 않음


            //성이 아니면
            if (index >= 0)
            {

                Vector2 linePos = lineEvent(m_iActor.uiController, (MapController.TYPE_MAP_LINE)index);

                //하강하고 있을 때 바닥에 닿으면 
                if (GetComponent<Rigidbody2D>().velocity.y <= 0f && linePos.y - 0.2f >= transform.position.y)
                {
                    //이동 사라짐
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GetComponent<Rigidbody2D>().gravityScale = 0f;


                    //설치형이 아니면 1초 후 사라짐
                    //폭발형은 주변에 데미지 입히기


                    //
                    //switch (m_bullet.typeHit)
                    //{
                    //    case Bullet.TYPE_HIT.NONE:
                    //        //콜라이더 없음
                    //        //1초 후 사라짐
                    //        break;
                    //    case Bullet.TYPE_HIT.BOMB:
                    //        //경과 시간 후 주변에 데미지 입히기
                    //        //또는 콜라이더 충돌시 데미지 주기
                    //        break;
                    //    case Bullet.TYPE_HIT.PENETRATE:
                    //        //콜라이더 데미지 주기
                    //        //
                    //        goto case Bullet.TYPE_HIT.NONE;
                    //    case Bullet.TYPE_HIT.SPLASH:
                    //        //콜라이더 없이 지속적으로 데미지 입히기
                    //        //bomb 반복
                    //        break;
                    //}

                    //설치형이 아니면
                    if (m_bullet.typeShoot != Bullet.TYPE_SHOOT.Set)
                    {
                        //바닥용 이미지가 있으면 전환

                        //폭발형은 즉시 폭발
                        if (m_bullet.typeHit == Bullet.TYPE_HIT.Bomb)
                            removeBullet();
                        //폭발형인데 즉시 폭발 아니면 false로 전환
                        else
                            GetComponent<Collider2D>().enabled = false;

                        //1초 후 사라짐
                        m_timer += Prep.frameTime;
                        if (m_timer >= 1f)
                        {
                            removeBullet(false);
                        }
                    }
                    else
                    {
                        //설치형이면
                        //바닥에 있기
                        //공격형에 따라서 적을 공격
                        //일반 - 접촉한 적에게 1회 공격후 사라짐
                        //폭발 - 접촉한 적에게 1회 폭발형 공격
                        //관통 - 접촉한 적에게 1회 공격후 사라지지 않음
                        //이동속도만큼 타이머 후 사라짐
                        //콜라이더 값 계속 가져오기

                        //지속 - 주변 적에게 지속적으로 공격
                        switch (m_bullet.typeHit)
                        {
                            case Bullet.TYPE_HIT.Splash:

                                //1초당 데미지
                                IUnitSearch unitSearch = new BoxUnitSearch();
                                IActor[] iActors = unitSearch.searchUnitActors(m_iActor, TYPE_TEAM.Enemy);

                                if (iActors != null)
                                {
                                    //                                Debug.LogError("unitActors : " + iActors.Length);
                                    foreach (IActor actor in iActors)
                                    {
                                        hitBullet(actor);
                                    }
                                }

                                goto default;
                            //설치형은 이동속도만큼 지속됨
                            default:
                                m_timer += Prep.frameTime;
                                if (m_timer >= m_bullet.moveSpeed)
                                    removeBullet(false);
                                break;
                        }
                    }

                }
            }

            float vX = GetComponent<Rigidbody2D>().velocity.x;
            float vY = GetComponent<Rigidbody2D>().velocity.y;

            float angle = Mathf.Atan2(vY, vX) * Mathf.Rad2Deg;

            //스프라이트 각도
            m_sprite.transform.eulerAngles = new Vector3(0f, 0f, angle);

            //바닥에 닿으면






            ////바닥에 닿으면 부딪힌 후 1초 뒤에 사라짐
            //if (Prep.playerPos[index].position.y >= transform.position.y)
            //{
            //    //충돌체 사용금지
            //    GetComponent<Collider2D>().enabled = false;
            //    transform.position = new Vector2(transform.position.x, Prep.playerPos[index].position.y - 0.5f);
            //    //1초뒤 사라짐
            //    m_timer += Prep.frameTime;
            //    if (m_timer >= 1f)
            //        m_removeBulletActorDel(this);

            //    //일반탄 - 콜라이더 사라짐
            //    //폭발탄 - 폭발
            //    //관통탄 - 없음

            //}
            //else
            //{
            //    //관통탄은 1초뒤에 사라짐
            //    if (m_bullet.typeHit == Bullet.TYPE_HIT.PENETRATE)
            //    {
            //        m_timer += Prep.frameTime;
            //        if (m_timer >= 1f)
            //            m_removeBulletActorDel(this);
            //    }
            //}
            
        }

    }

    void removeBullet(bool isHit = true)
    {
        //탄환 피격 파티클
        if(isHit)
            createEffect(m_bullet.effectKey + "HitParticle");

        removeActor();
//        removeBulletActorEvent(this);
    }

    public void removeActor()
    {
        removeBulletActorEvent(this);
    }

    bool createEffect(string key)
    {
        Debug.LogError("Particle : " + key);

        m_effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
        if (m_effectActorManager != null)
        {
            m_effectActorManager.createActor(key, transform.position, m_iActor.typeController, m_iActor.isFlip);
            return true;
        }
        return false;

    }


    void OnTriggerEnter2D(Collider2D col)
    {

        if (!m_isUsed)
        {
            if (col.tag == Prep.unitTag)
            {
                //Debug.Log("Hit");
                //피격자
                UnitActor unitActor = col.GetComponent<UnitActor>();

                
                if (unitActor != null)
                {

                    if (unitActor.typeController != typeController)
                    {

                        //Debug.Log("Hit1");
                        //원거리 발사시 라인 탄환에 등록
                        //해당 탄환은 라인에 있는 적에게만 피격
                        //지상
                        //공중

                        //목표 라인과 같으면

                        if (unitActor.gameObject.layer == gameObject.layer)
                        {
                            //Debug.Log("Hit2");
                            //피격자가 공중일 때
                            if (unitActor.typeMovement == Unit.TYPE_MOVEMENT.Air)
                            {
                                //공격자가 공중 공격이 없으면
                                if ((m_iActor.typeLine & Unit.TYPE_LINE.AIR_FORWARD) != Unit.TYPE_LINE.AIR_FORWARD &&
                                    (m_iActor.typeLine & Unit.TYPE_LINE.AIR_SIDE) != Unit.TYPE_LINE.AIR_SIDE)
                                {
                                    return;
                                }
                            }

                            //피격
                            //
                            hitBullet(unitActor);

                        }




                        //원거리 -
                        //지상 전방일 때 적이 같은 라인에 지상이면
                        //지상만 판정
                        //if ((m_unitActor.typeLine & Unit.TYPE_LINE.GND_FORWARD) == Unit.TYPE_LINE.GND_FORWARD)
                        //{
                        //    if (unitActor.gameObject.layer == m_unitActor.gameObject.layer && unitActor.typeMovement == Unit.TYPE_MOVEMENT.GROUND)
                        //    {
                        //        hitBullet(unitActor);
                        //        return;
                        //    }
                        //}

                        //지상 측방
                        //지상 측방일 때 적이 지상이면
                        //지상만 판정
                        //if ((m_unitActor.typeLine & Unit.TYPE_LINE.GND_SIDE) == Unit.TYPE_LINE.GND_SIDE)
                        //{
                        //    if (unitActor.typeMovement == Unit.TYPE_MOVEMENT.GROUND)
                        //    {
                        //        hitBullet(unitActor);
                        //        return;
                        //    }
                        //}


                        //공중 전방일 때
                        //공중및 지상 같은 라인만 판정
                        //if ((m_unitActor.typeLine & Unit.TYPE_LINE.AIR_FORWARD) == Unit.TYPE_LINE.AIR_FORWARD){
                        //    if(unitActor.gameObject.layer == m_unitActor.gameObject.layer)
                        //    {
                        //        hitBullet(unitActor);
                        //        return;
                        //    }
                        //}

                        //공중 측방
                        //전체 판정

                        //if ((m_unitActor.typeLine & Unit.TYPE_LINE.AIR_SIDE) == Unit.TYPE_LINE.AIR_SIDE)
                        //{
                        //    m_isUsed = true;
                        //    unitActor.hitUnit(this);
                        //    m_removeBulletActorDel(this);
                        //}
                    }
                }
            }
        }

        //적에게 피격시 반납
        //m_removeBulletActorDel(this);



    }

    //접촉시 데미지
    void hitBullet(IActor iActor)
    {
        //Debug.Log("bullet");
        //단일공격

        //현재 버프가 되어있으면 그것을 포함하여 가하기

        Debug.Log("bullet : " + m_bullet.key);


        EffectActorManager effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;

        //효과 주기
        //effectActorManager.createActor("BulletEffect", transform.position);

        switch (m_bullet.typeHit)
        {
            case Bullet.TYPE_HIT.Normal:
                m_isUsed = true;
                iActor.hitActor(this, attack);
                removeBullet();
                break;
            case Bullet.TYPE_HIT.Bomb:
                
                RaycastHit2D[] rays = Physics2D.CircleCastAll(transform.position, m_bullet.radius, Vector2.zero, gameObject.layer);

                foreach (RaycastHit2D ray in rays)
                {
                    if (ray.collider.tag == Prep.unitTag)
                    {

                        UnitActor rangeUnitActor = ray.collider.GetComponent<UnitActor>();

//                        Debug.LogWarning("지뢰 : " + rangeUnitActor.name);

                        if (rangeUnitActor != null)
                        {
                            if (rangeUnitActor.typeController != typeController)
                            {
                                rangeUnitActor.hitActor(this, attack);
                            }
                        }
                    }
                }

                removeBullet();

                break;
            default:
                iActor.hitActor(this, attack);
                createEffect(m_bullet.effectKey + "HitParticle");
                break;
            //case Bullet.TYPE_HIT.PENETRATE:
            //    unitActor.hitUnit(this, attack);
            //    break;
            //case Bullet.TYPE_HIT.SPLASH:
            //    unitActor.hitUnit(this, attack);
            //    break;
        }



        //적 공격
    }

    public string key
    {
        get { return m_bullet.key; }
    }
    
    public int layer
    {
        get { return gameObject.layer; }
    }

    public UIController uiController
    {
        get { return m_iActor.uiController; }
    }

    public bool hitActor(IActor iActor, int attack)
    {
        return false;
    }

    public int nowHealth
    {
        get { return 0; }
    }








    public Unit.TYPE_UNIT typeUnit
    {
        get { return m_iActor.typeUnit; }
    }

    public Unit.TYPE_LINE typeLine
    {
        get { return m_iActor.typeLine; }
    }

    public Unit.TYPE_MOVEMENT typeMovement
    {
        get { return m_iActor.typeMovement; }
    }
    public bool isDead()
    {
        return true;
    }
    
    public Vector2 getPosition(int layer)
    {
        return transform.position;
    }

    public void setPosition(Vector2 pos)
    {
        transform.position = pos;
    }


    public Unit.TYPE_TARGETING typeTarget
    {
        get { return m_iActor.typeTarget; }
    }


    public bool isFlip
    {
        get {
            return (transform.localScale.x > 0) ? false : true;
        }
    }
}
