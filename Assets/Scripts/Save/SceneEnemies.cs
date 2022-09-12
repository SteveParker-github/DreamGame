using UnityEngine;

[System.Serializable]
public class SceneEnemies
{
    public string sceneName;
    public bool hasSpawned;
    public Enemy[] enemies;

    public SceneEnemies(string sceneName, bool hasSpawned, Enemy[] enemies)
    {
        this.sceneName = sceneName;
        this.hasSpawned = hasSpawned;
        this.enemies = enemies;
    }
}
