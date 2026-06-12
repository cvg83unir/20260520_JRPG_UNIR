using UnityEngine;

public interface IVisible
{
    public enum Side { Friend, Neutral, Enemy};

    public Side GetSide();
    public Transform GetTransform();

}
