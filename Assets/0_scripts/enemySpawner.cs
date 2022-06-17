using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour, IStartGameObserver
{
    public static enemySpawner Instance;
    public List<GameObject> enemyAll;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float spawnPeriod;
    public bool spawnActive = true;
    int spawnPointSelect;
    int spawnPointCount;
    public GameObject player;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int[] zombieLevel;
    public int level;

    public int[] pink;
    public int[] orange;
    public int[] red;
    public int[] purple;
    public int[] green;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
        Globals.killedEnemy = 0;
        Globals.maxEnemyCount = 150;
        spawnPointCount = transform.childCount;
    }
    public void StartGame()
    {
        StartCoroutine(spawning());

    }
    IEnumerator spawning()
    {
        yield return null;

        while (spawnActive)
        {
            if (Globals.killedEnemy < zombieLevel[zombieLevel.Length - 1])
            {
                if (Globals.killedEnemy > zombieLevel[level])
                {
                    level++;
                }
            }
            if (enemyAll.Count < Globals.maxEnemyCount)
            {
                enemySpawn();
            }
            yield return new WaitForSeconds(1000f / ((float)Globals.killedEnemy + 200f));
        }
    }
    void enemySpawn()
    {
        for (int i = 0; i < pink[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<playerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var pinkEnemy = Instantiate(enemyPrefab[0], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                pinkEnemy.GetComponent<enemy>().player = player.GetComponent<playerControl>().players[Random.Range(0, player.GetComponent<playerControl>().players.Count)];
                enemyAll.Add(pinkEnemy);
                //zombie.GetComponent<Zombie>().player = player;
            }
        }
        for (int i = 0; i < orange[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<playerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var orangeEnemy = Instantiate(enemyPrefab[1], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                orangeEnemy.GetComponent<enemy>().player = player.GetComponent<playerControl>().players[Random.Range(0, player.GetComponent<playerControl>().players.Count)];
                enemyAll.Add(orangeEnemy);

            }
        }
        for (int i = 0; i < red[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<playerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var redEnemy = Instantiate(enemyPrefab[2], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                redEnemy.GetComponent<enemy>().player = player.GetComponent<playerControl>().players[Random.Range(0, player.GetComponent<playerControl>().players.Count)];
                enemyAll.Add(redEnemy);
            }
        }
        for (int i = 0; i < purple[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<playerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var purpleEnemy = Instantiate(enemyPrefab[3], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                purpleEnemy.GetComponent<enemy>().player = player.GetComponent<playerControl>().players[Random.Range(0, player.GetComponent<playerControl>().players.Count)];
                enemyAll.Add(purpleEnemy);
            }
        }
        for (int i = 0; i < green[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<playerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var greenEnemy = Instantiate(enemyPrefab[4], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                greenEnemy.GetComponent<enemy>().player = player.GetComponent<playerControl>().players[Random.Range(0, player.GetComponent<playerControl>().players.Count)];
                enemyAll.Add(greenEnemy);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.GetChild(0).rotation;
    }
}
