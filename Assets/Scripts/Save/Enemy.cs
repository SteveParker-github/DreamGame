using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string enemyName;
    public Position position;
    public Position velocity;
    public Position targetLocation;
    public Rotation rotation;
    public bool isAlive;
    public float absorbHealth;
    public float stunHealth;
    public bool isStunned;
    public bool beenStaggered;
    public bool isStagger;

    public Enemy(
        string enemyName,
        Vector3 position,
        Vector3 velocity,
        Vector3 targetLocation,
        Quaternion rotation,
        bool isAlive,
        float absorbHealth,
        float stunHealth,
        bool isStunned,
        bool beenStaggered,
        bool isStagger)
    {
        this.enemyName = enemyName;
        this.position = new Position(position);
        this.velocity = new Position(velocity);
        this.targetLocation = new Position(targetLocation);
        this.rotation = new Rotation(rotation);
        this.isAlive = isAlive;
        this.absorbHealth = absorbHealth;
        this.stunHealth = stunHealth;
        this.isStunned = isStunned;
        this.beenStaggered = beenStaggered;
        this.isStagger = isStagger;
    }
}
