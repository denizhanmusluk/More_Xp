using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCreator : MonoBehaviour,IStartGameObserver
{
    public List<GameObject> enemyAll;
    [SerializeField] GameObject[] enemyPrefab;
    public bool spawnActive = true;
    public GameObject player;


    [SerializeField] int maxEnemyCount;
    [SerializeField] bool creating;
    [SerializeField] float spawnTime;
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
        Globals.killedEnemy = 0;
    }
    public void StartGame()
    {
        if (creating)
        {
            StartCoroutine(spawning());
        }
    }
    public void spawn()
    {
        StartCoroutine(spawning());
    }
    IEnumerator spawning()
    {
        yield return null;

        while (spawnActive)
        {
            //if (Globals.killedEnemy < zombieLevel[zombieLevel.Length - 1])
            //{
            //    if (Globals.killedEnemy > zombieLevel[level])
            //    {
            //        level++;
            //    }
            //}
            if (enemyAll.Count < maxEnemyCount)
            {
                enemySpawn();
            }
            float spawnTimeFactor = 0.1f * ((float)Globals.swordLevel + (float)Globals.stompLevel + (float)Globals.meteorLevel + (float)Globals.spinLevel + (float)Globals.tornadoLevel + (float)Globals.assassinLevel) / 6;
            yield return new WaitForSeconds(1 / (spawnTime + spawnTimeFactor));
        }
    }
    void enemySpawn()
    {

        Vector3 spawnPointSelect = new Vector3(Random.Range(-35f, 35f), 0, Random.Range(-35f, 35f));
        //while(Mathf.Abs(player.transform.position.x - spawnPointSelect.x) < 23)
        //{
        //    spawnPointSelect = new Vector3(Random.Range(-40f, 40f), 0, Random.Range(-40f, 40f));
        //}


        //while ((Camera.main.WorldToScreenPoint(spawnPointSelect).x < Screen.width && Camera.main.WorldToScreenPoint(spawnPointSelect).x > 0) && (Camera.main.WorldToScreenPoint(spawnPointSelect).y < Screen.height && Camera.main.WorldToScreenPoint(spawnPointSelect).y > 0))
        //{
        //    spawnPointSelect = new Vector3(Random.Range(-35f, 35f), 0, Random.Range(-35f, 35f));
        //}

        if (player.GetComponent<playerControl>().players.Count > 0 && Globals.isGameActive)
        {
            var _enemy = Instantiate(enemyPrefab[0], transform.position + spawnPointSelect, Quaternion.identity);
            _enemy.GetComponent<enemy>().player = player.GetComponent<playerControl>().players[Random.Range(0, player.GetComponent<playerControl>().players.Count)];
            enemyAll.Add(_enemy);
            _enemy.GetComponent<enemy>()._enemyCreator = this;
            //zombie.GetComponent<Zombie>().player = player;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position;
        //transform.rotation = player.transform.GetChild(0).rotation;
    }
}
