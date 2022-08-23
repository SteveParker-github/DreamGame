using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    public Position position;
    public Rotation rotation;

    public PlayerSave(Vector3 position, Quaternion rotation)
    {
        this.position = new Position(position);
        this.rotation = new Rotation(rotation);
    }
}
