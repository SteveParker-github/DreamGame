using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    public Position position;
    public Rotation rotation;
    public float damageHealth;
    public int dialogueFails;

    public PlayerSave(Vector3 position, Quaternion rotation, float damageHealth, int dialogueFails)
    {
        this.position = new Position(position);
        this.rotation = new Rotation(rotation);
        this.damageHealth = damageHealth;
        this.dialogueFails = dialogueFails;
    }
}
