using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Vector3[] locations;
    [SerializeField] private GameObject prefabs;
    [SerializeField] private string questName;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    private bool isSpawned;
    private Dictionary<string, GameObject> enemies;
    private PlayerController playerController;
    private string sceneName;
    private EnemiesManager enemiesManager;

    public string SceneName { get => sceneName; }
    public Dictionary<string, GameObject> Enemies { get => enemies; }

    // Start is called before the first frame update
    void Start()
    {
        enemiesManager = GameObject.Find("EnemiesManager").GetComponent<EnemiesManager>();
        sceneName = gameObject.scene.name;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        isSpawned = false;
        enemies = new Dictionary<string, GameObject>();

        if (enemiesManager.HasSpawned(sceneName))
        {
            isSpawned = true;
            LoadEnemies();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawned)
        {
            spawnEnemies();
        }

    }

    private void spawnEnemies()
    {
        if (playerController.QuestManager.QuestInProgress(questName))
        {
            for (int i = 0; i < locations.Length; i++)
            {
                GameObject enemy = Instantiate(prefabs, locations[i], Quaternion.Euler(0, 0, 0), transform);
                enemy.name = "Enemy" + i;
                enemies.Add(enemy.name, enemy);
            }

            isSpawned = true;
            enemiesManager.SetEnemies(sceneName, isSpawned, enemies);
        }
    }

    public void RemoveEnemy(string enemyName)
    {
        enemies.Remove(enemyName);
        enemiesManager.SetDead(sceneName, enemyName);

        if (enemies.Count > 0) return;

        playerController.InventoryManager.AddNewItem((itemName, itemDescription));
    }

    private void LoadEnemies()
    {
        Enemy[] enemys = enemiesManager.GetEnemies(sceneName);

        foreach (Enemy item in enemys)
        {
            if (item.isAlive)
            {
                GameObject enemy = Instantiate(prefabs, item.position.GetVector(), item.rotation.GetRotation(), transform);
                enemy.name = item.enemyName;
                enemy.GetComponent<EnemyController>().SetEnemy(item);
                enemies.Add(enemy.name, enemy);
            }
        }
    }
}
