using UnityEngine;

public abstract class CreateSkillActor : SkillActor
{
    //유닛 키
//    [SerializeField]
//    string m_unitKey;

    public string unitKey { get { return name; } }


    protected MapController.TYPE_MAP_LINE layerToPos(int layer)
    {
        if (layer >= 20)
            layer -= 3;
        return (MapController.TYPE_MAP_LINE)(layer - 17);
    }
}

