using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    public skillManager _skillManager;
    playerControl playerControlling;
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] List<GameObject> attackListEnemies;
    bool attacking = true;
    [SerializeField] Animator animator;
    public float attackSpeed;
    public float health;
    //[SerializeField] GameObject swordParticle;

    public enum States { normalAttack,bash,stomp,spin,meteor,tornado,assassin }
    public States currentAttack;


    [SerializeField] GameObject bashAreaPrefab;
    [SerializeField] GameObject stompAreaPrefab;
    [SerializeField] GameObject meteorAreaPrefab;
    [SerializeField] GameObject tornadoPrefab;
    [SerializeField] GameObject assassinPrefab;

    [SerializeField] List< GameObject> bashEffect;
    [SerializeField] List<GameObject> lightingEffects;
    [SerializeField] List<GameObject> meteorEffects;
    [SerializeField] List<GameObject> tornadoEffects;
    [SerializeField] List<GameObject> assassinEffects;

    public GameObject[] swords;
    [SerializeField] GameObject[] clothes;

    void Start()
    {
        currentAttack = States.normalAttack;
        playerControlling = GetComponent<playerControl>();
    }
    public void swordSet()
    {
        for(int i = 0; i < swords.Length; i++)
        {
            swords[i].SetActive(false);
        }
        swords[Globals.swordLevel].SetActive(true);

        for (int i = 0; i < clothes.Length; i++)
        {
            clothes[i].SetActive(false);
        }
        if (Globals.swordLevel > 2)
        {
            for(int i = 0; i< Globals.swordLevel / 3; i++)
            clothes[i].SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<enemy>() != null)
        {
            //playerControlling.playerAttack(collision.transform);
            //collision.transform.GetComponent<enemy>().dead();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().idleMove = false;
            other.GetComponent<enemy>().currentBehaviour = enemy.States.followPlayer;
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().idleMove = true;
            other.GetComponent<enemy>().idleEnum();
            enemies.Remove(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            if (attacking)
            {
                distanceEnemyCheck();
            }
        }
    }

    void distanceEnemyCheck()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) < 9f)
            {
                attacking = false;


                switch (currentAttack)
                {
                    case States.normalAttack:
                        {
                            attack(enemies[i].transform);
                        }
                        break;
                    case States.bash:
                        {
                            Debug.Log("current bash");
                            bashAttack(enemies[i].transform);

                        }
                        break;
                    case States.stomp:
                        {
                            stompAttack(enemies[i].transform);

                        }
                        break;
                    case States.spin:
                        {
                            spinAttack(enemies[i].transform);

                        }
                        break;   
                    case States.meteor:
                        {
                            meteorAttack(enemies[i].transform);

                        }
                        break;
                    case States.tornado:
                        {
                            tornadoAttack(enemies[i].transform);

                        }
                        break;     
                    case States.assassin:
                        {
                            assassinAttack(enemies[i].transform);

                        }
                        break;
                }


                break;
            }
        }
    }
    #region normalAttack
    void attack(Transform _enemy)
    {
        StartCoroutine(_attack(_enemy,Globals.swordAttackSpeed));
        animator.SetFloat("attackspeed", Globals.swordAttackSpeed);
        int animationSelect = Random.Range(0, 2);
        string[] atck = { "attack1", "attack2" };
        swords[Globals.swordLevel].transform.GetChild(0).gameObject.SetActive(true);
        //swordParticle.SetActive(true);
        animator.SetTrigger(atck[animationSelect]);
    }
    public void attackAnimationEvent()
    {
        swords[Globals.swordLevel].transform.GetChild(0).gameObject.SetActive(false);
        //swordParticle.SetActive(false);
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                if (Vector3.Distance(enemies[i].transform.position, transform.position) < 9f)
                {
                    enemies[i].GetComponent<enemy>().dead(Globals.swordDamage, (enemies[i].transform.position - transform.position).normalized + new Vector3(0, -0.3f, 0));
                    //Vector3 forceDirection = (enemies[i].transform.position - transform.position).normalized;
                    //enemies[i].GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 0.5f, 0)));

                    //enemies.Remove(enemies[i]);
                }
            }
            else
            {
                enemies.Remove(enemies[i]);
            }
        }
        attacking = true;
    }
 
    #endregion

    #region bash attack
    public void bashAttack(Transform _enemy)
    {
        Globals.isGameActive = false;
        StartCoroutine(_attack(_enemy , 1));
        animator.SetTrigger("bash");
    }
    public void bashAttackAnimationEvent()
    {
        _skillManager.bashCooldown();
        StartCoroutine(bashAttacking());
    }
    IEnumerator bashAttacking()
    {
        GameObject bash1 = Instantiate(bashAreaPrefab, transform.position + transform.GetChild(0).forward, transform.GetChild(0).rotation);
        bash1.GetComponent<bashAttack>()._playerBeh = this;
        bash1.GetComponent<bashAttack>().moveTarget = transform.position + transform.GetChild(0).forward * Globals.bashDistance;

        GameObject bashEffect1 = Instantiate(bashEffect[(Globals.bashLevel - 1) % bashEffect.Count], new Vector3(bash1.transform.position.x, 1, bash1.transform.position.z), transform.GetChild(0).rotation);
        bashEffect1.transform.parent = bash1.transform;
 

        yield return new WaitForSeconds(2f);
        _skillManager.skillSelect();
        Globals.isGameActive = true;
        attacking = true;
    }
    #endregion
    #region stomp attack
    public void stompAttack(Transform _enemy)
    {
        Globals.isGameActive = false;
        StartCoroutine(_attack(_enemy, 1));
        animator.SetTrigger("stomp");

    }
    public void stompAttackAnimationEvent()
    {
        _skillManager.stompCooldown();
        StartCoroutine(_stompAttacking());
    }

    IEnumerator _stompAttacking()
    {

        //currentAttack = States.normalAttack;
        for (int i = 0; i < Globals.lightningAmount; i++)
        {
            stompAttacking();
            yield return new WaitForSeconds(0.2f);
        }
        _skillManager.skillSelect();
        attacking = true;

        Globals.isGameActive = true;
    }
    void stompAttacking()
    {
        GameObject stomp0 = Instantiate(stompAreaPrefab, transform.position + transform.GetChild(0).forward * Random.Range(-20f, 20f) + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        stomp0.GetComponent<stompAttack>()._playerBeh = this;
        GameObject lighting1 = Instantiate(lightingEffects[(Globals.stompLevel - 1) % lightingEffects.Count], new Vector3(stomp0.transform.position.x, 37, stomp0.transform.position.z), Quaternion.identity);
        Destroy(lighting1, 2);
        //yield return new WaitForSeconds(0.2f);

        //GameObject stomp1 = Instantiate(stompAreaPrefab, transform.position + transform.GetChild(0).forward * Random.Range(5f, 10f) + new Vector3(Random.Range(-10f,10f),0, Random.Range(-10f, 10f)), Quaternion.identity);
        //stomp1.GetComponent<stompAttack>()._playerBeh = this;
        //GameObject lighting2 = Instantiate(lightingEffects[1], new Vector3(stomp1.transform.position.x, 33, stomp1.transform.position.z), Quaternion.identity);
        //Destroy(lighting2, 2);

        //yield return new WaitForSeconds(0.2f);

        //GameObject stomp2 = Instantiate(stompAreaPrefab, transform.position + transform.GetChild(0).forward * Random.Range(10f, 15f) + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        //stomp2.GetComponent<stompAttack>()._playerBeh = this;
        //GameObject lighting3 = Instantiate(lightingEffects[2], new Vector3(stomp2.transform.position.x, 33, stomp2.transform.position.z), Quaternion.identity);
        //Destroy(lighting3, 2);

        //yield return new WaitForSeconds(0.2f);

        //GameObject stomp3 = Instantiate(stompAreaPrefab, transform.position + transform.GetChild(0).forward * Random.Range(15f, 20f) + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        //stomp3.GetComponent<stompAttack>()._playerBeh = this;
        //GameObject lighting4 = Instantiate(lightingEffects[3], new Vector3(stomp3.transform.position.x, 33, stomp3.transform.position.z), Quaternion.identity);
        //Destroy(lighting4, 2);

        //yield return new WaitForSeconds(1f);
        //_skillManager.skillSelect();

        //currentAttack = States.normalAttack;
        //Globals.isGameActive = true;
        //attacking = true;
    }
    #endregion

    #region spin attack
    public void spinAttack(Transform _enemy)
    {
        _skillManager.spinCooldown();
        playerControlling.speed /= 3;
        swords[Globals.swordLevel].transform.GetChild(0).gameObject.SetActive(true);
        //swordParticle.SetActive(true);
        StartCoroutine(_attack(_enemy, 1));
        animator.SetBool("spin2", true);
        //currentAttack = States.normalAttack;
        StartCoroutine(spinHitEnemy());
    }
    IEnumerator spinHitEnemy()
    {
        int enemyAmont = enemies.Count;
        float counter = 0f;
        while (counter < Globals.spinTime)
        {
            counter += 0.2f;
            enemyAmont = enemies.Count;
            for (int i = 0; i < enemyAmont; i++)
            {
                Debug.Log("enemyyyy");
                if (Vector3.Distance(enemies[i].transform.position, transform.position) < 9f)
                {
                    enemies[i].GetComponent<enemy>().dead(Globals.spinDamage, 2 * (enemies[i].transform.position - transform.position).normalized);
                }
                //Vector3 forceDirection = (enemies[0].transform.position - transform.position).normalized;
                //enemies[0].GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 0.5f, 0)));

                //enemies.Remove(enemies[0]);
            }
            yield return new WaitForSeconds(0.2f);
        }


        //yield return new WaitForSeconds(0.5f);

        swords[Globals.swordLevel].transform.GetChild(0).gameObject.SetActive(false);
        //swordParticle.SetActive(false);
        playerControlling.speed *= 3;


        //yield return new WaitForSeconds(3.5f);
        _skillManager.skillSelect();
        //currentAttack = States.normalAttack;
        animator.SetBool("spin2", false);
        attacking = true;

    }
    public void spinAttackAnimationEvent()
    {
        //_skillManager.spinCooldown();
        //StartCoroutine(spinHitEnemy());


    }

    #endregion
    #region assassin attack
    public void assassinAttack(Transform _enemy)
    {
        Globals.isGameActive = false;
        StartCoroutine(_attack(_enemy, 10));
        //animator.SetTrigger("tornado");
        _skillManager.assassinCooldown();
        StartCoroutine(assassinAttacking());
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }
    public void assassinAttackAnimationEvent()
    {

    }
    IEnumerator assassinAttacking()
    {
        GameObject assassin = Instantiate(assassinPrefab, transform.position + transform.GetChild(0).forward, transform.GetChild(0).rotation);
        assassin.GetComponent<assassinAttack>()._playerBeh = this;
        for (int i = 0; i < Globals.assassinAmount; i++)
        {
            assassin.GetComponent<assassinAttack>().enemies.Add(enemies[i]);
            if (i == enemies.Count - 1)
            {
                break;
            }

        }
        //assassin.GetComponent<assassinAttack>().moveTarget = transform.position + transform.GetChild(0).forward * Globals.tornadoDistance;

        GameObject assassinEffect = Instantiate(assassinEffects[(Globals.assassinLevel - 1) % assassinEffects.Count], new Vector3(assassin.transform.position.x, 1, assassin.transform.position.z), transform.GetChild(0).rotation);
        assassinEffect.transform.parent = assassin.transform;
        assassinEffect.transform.localPosition = new Vector3(0, 2, 0);

        yield return new WaitForSeconds(2f);
        //Globals.isGameActive = true;
        //attacking = true;
    }
    #endregion
    #region meteor attack
    public void meteorAttack(Transform _enemy)
    {
        Globals.isGameActive = false;
        StartCoroutine(_attack(_enemy, 1));
        animator.SetTrigger("meteor");
        _skillManager.meteorCooldown();
        StartCoroutine(_meteorAttacking());
    }
    public void meteorAttackAnimationEvent()
    {
        Globals.isGameActive = true;

        _skillManager.skillSelect();
    }
    IEnumerator _meteorAttacking()
    {

        //currentAttack = States.normalAttack;
        for (int i = 0; i < Globals.meteorTime; i++)
        {
            StartCoroutine(meteorAttacking());
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        attacking = true;

    }
    IEnumerator meteorAttacking()
    {
        Vector3 pos = transform.position + transform.GetChild(0).forward * Random.Range(0, 8f) + new Vector3(Random.Range(-8, 8f), 0, Random.Range(-8f, 8f));

        GameObject meteorEffect = Instantiate(meteorEffects[(Globals.meteorLevel - 1) % meteorEffects.Count], pos, Quaternion.identity);
        Destroy(meteorEffect, 2.5f);
        yield return new WaitForSeconds(2.14f);
        GameObject meteor = Instantiate(meteorAreaPrefab, pos, Quaternion.identity);
        meteor.GetComponent<meteorAttack>()._playerBeh = this;


    }
    #endregion

    #region tornado attack
    public void tornadoAttack(Transform _enemy)
    {
        Globals.isGameActive = false;
        StartCoroutine(_attack(_enemy, 10));
        animator.SetTrigger("tornado");

 
    }
    public void tornadoAttackAnimationEvent()
    {
        _skillManager.tornadoCooldown();
        StartCoroutine(tornadoAttacking());
    }
    IEnumerator tornadoAttacking()
    {
        GameObject tornado = Instantiate(tornadoPrefab, transform.position + transform.GetChild(0).forward, transform.GetChild(0).rotation);
        tornado.GetComponent<tornadoAttack>()._playerBeh = this;
        tornado.GetComponent<tornadoAttack>().moveTarget = transform.position + transform.GetChild(0).forward * Globals.tornadoDistance;

        GameObject tornadoEffect = Instantiate(tornadoEffects[(Globals.tornadoLevel - 1) % tornadoEffects.Count], new Vector3(tornado.transform.position.x, 1, tornado.transform.position.z), transform.GetChild(0).rotation);
        tornadoEffect.transform.parent = tornado.transform;


        yield return new WaitForSeconds(1.4f);
        _skillManager.skillSelect();
        Globals.isGameActive = true;
        attacking = true;
    }
    #endregion

    public void activeCharacter()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        _skillManager.skillSelect();
        Globals.isGameActive = true;
        attacking = true;
    }

    IEnumerator _attack(Transform _enemy,float affectTime)
    {
        float counter = 0f;
        while (counter < 1 / affectTime)
        {
            counter += Time.deltaTime;
            lookTarget(transform.GetChild(0), _enemy);
            yield return null;
        }

    }
    void lookTarget(Transform player, Transform target)
    {
      
        Vector3 relativeVector = player.transform.InverseTransformPoint(target.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 50;
        player.Rotate(0, newSteer * Time.deltaTime * 20, 0);
    }
}