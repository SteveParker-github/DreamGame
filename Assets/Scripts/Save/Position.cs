using UnityEngine;

[System.Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;

    public Position(Vector3 position)
    {
        x = position.x;
        y = position.y + 0.1f;
        z = position.z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }
}
