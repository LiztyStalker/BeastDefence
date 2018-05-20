using UnityEngine;


public interface ICard
{
    string key { get; }
    int population { get; }
    Sprite icon { get; }
    string name { get; }
    int level { get; }
    int munitions { get; }
    string contents { get; }
    float waitTime { get; }
}

