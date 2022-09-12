using UnityEngine;

[System.Serializable]
public class Rotation
{
    public float w;
    public float x;
    public float y;
    public float z;

    public Rotation(Quaternion rotation)
    {
        w = rotation.w;
        x = rotation.x;
        y = rotation.y;
        z = rotation.z;
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(x, y, z, w);
    }

    public void ToFloat(Quaternion rotation)
    {
        w = rotation.w;
        x = rotation.x;
        y = rotation.y;
        z = rotation.z;
    }
}
