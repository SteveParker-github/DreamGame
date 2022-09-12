using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour
{
    private Dictionary<string, SceneEnemies> sceneEnemies;

    // Start is called before the first frame update
    void Start()
    {
        sceneEnemies = new Dictionary<string, SceneEnemies>();
    }

    private bool KeyExists(string sceneName)
    {
        return sceneEnemies.ContainsKey(sceneName);
    }

    public bool HasSpawned(string sceneName)
    {
        if (!KeyExists(sceneName)) return false;

        return sceneEnemies[sceneName].hasSpawned;
    }

    public Enemy[] GetEnemies(string sceneName)
    {
        if (!KeyExists(sceneName)) return new Enemy[] { };

        return sceneEnemies[sceneName].enemies;
    }

    public void SetDead(string sceneName, string enemyName)
    {
        foreach (Enemy enemy in sceneEnemies[sceneName].enemies)
        {
            if (enemy.enemyName == enemyName)
            {
                enemy.isAlive = false;
                return;
            }
        }
    }

    public void SetEnemies(string sceneName, bool hasSpawned, Dictionary<string, GameObject> enemies)
    {
        List<Enemy> enemiesList = new List<Enemy>();

        foreach (KeyValuePair<string, GameObject> item in enemies)
        {
            EnemyController enemyController = item.Value.GetComponent<EnemyController>();
            string enemyName = item.Key;
            Vector3 position = item.Value.transform.position;
            Vector3 velocity = enemyController.CurrentVelocity;
            Vector3 targetLocation = enemyController.TargetLocation;
            Quaternion rotation = item.Value.transform.rotation;
            float absorbHealth = enemyController.AbsorbHealth;
            float stunHealth = enemyController.StunHealth;
            enemiesList.Add(new Enemy(enemyName, position, velocity, targetLocation, rotation, true, absorbHealth, stunHealth, false, false, false));
        }
        SceneEnemies sceneEnemy = new SceneEnemies(sceneName, hasSpawned, enemiesList.ToArray());

        if (sceneEnemies.ContainsKey(sceneName))
        {
            sceneEnemies.Remove(sceneName);
        }

        sceneEnemies.Add(sceneName, sceneEnemy);
    }

    public Enemies GetEnemiesList()
    {
        GetLatestSceneEnemies();

        List<SceneEnemies> sceneEnemy = new List<SceneEnemies>();
        foreach (KeyValuePair<string, SceneEnemies> item in sceneEnemies)
        {
            sceneEnemy.Add(item.Value);
        }

        return new Enemies(sceneEnemy.ToArray());
    }

    public void GetLatestSceneEnemies()
    {
        EnemyManager enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        string sceneName = enemyManager.SceneName;
        Dictionary<string, GameObject> enemies = enemyManager.Enemies;

        if (!sceneEnemies.ContainsKey(sceneName)) return;

        foreach (Enemy item in sceneEnemies[sceneName].enemies)
        {
            if (!enemies.ContainsKey(item.enemyName)) continue;

            EnemyController enemyController = enemies[item.enemyName].GetComponent<EnemyController>();
            item.position.ToFloat(enemies[item.enemyName].transform.position);
            item.rotation.ToFloat(enemies[item.enemyName].transform.rotation);
            item.velocity.ToFloat(enemyController.CurrentVelocity);
            item.targetLocation.ToFloat(enemyController.TargetLocation);
            item.absorbHealth = enemyController.AbsorbHealth;
            item.stunHealth = enemyController.StunHealth;
            item.isStunned = enemyController.IsStunned;
            item.beenStaggered = enemyController.BeenStaggered;
            item.isStagger = enemyController.IsStagger;
        }
    }

    public void LoadEnemies(Enemies newEnemies)
    {
        sceneEnemies = new Dictionary<string, SceneEnemies>();

        foreach (SceneEnemies item in newEnemies.sceneEnemies)
        {
            sceneEnemies.Add(item.sceneName, item);
        }
    }
}
