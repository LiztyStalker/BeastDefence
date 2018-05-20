using UnityEngine;

public interface IActor
{

    int nowHealth { get; }

    string key { get; }
    bool isFlip { get; }
    Unit.TYPE_UNIT typeUnit { get; }
    Unit.TYPE_LINE typeLine { get; }
    Unit.TYPE_MOVEMENT typeMovement { get; }
    Unit.TYPE_TARGETING typeTarget { get; }
    int attack { get; }
    int layer { get; }
    int level { get; }
    float range { get; }
    float sight { get; }
    UnitActor.TYPE_CONTROLLER typeController { get; }
    //Transform transform { get; }
    void uiUpdate();
    Vector2 getPosition(int layer);
    void setPosition(Vector2 pos);

    UIController uiController { get; }
//    Rigidbody2D rigidBody2D { get; }

    bool hitActor(IActor iActor, int attack);
    bool isDead();
//    bool addBuff(BuffActor buffActor, IActor iActor);
}

