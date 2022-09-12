using UnityEngine;

[System.Serializable]
public class Enemies
{
    public SceneEnemies[] sceneEnemies;

    public Enemies(SceneEnemies[] sceneEnemies)
    {
        this.sceneEnemies = sceneEnemies;
    }
}
