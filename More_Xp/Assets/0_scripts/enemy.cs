using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour,ILoseObserver
{
    [SerializeField] Material deadMat;
    [SerializeField] SkinnedMeshRenderer mesh;
    public int Health;
    [SerializeField] int earnMoney;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int coinCount;
    [SerializeField] float attackSpeed;
    [SerializeField] int damage;
    public GameObject player;
    public enum States { spawning,idleMov , idle, followPlayer, death, attack,failPlayer}
    public States currentBehaviour;

    public NavMeshAgent agent;
    Animator _animator;
    public enemyCreator _enemyCreator;
    bool attackActive = true;
    public bool idleMove = true;
    Vector3 idleMoveTarget;
    Vector3 currentPos;
    Vector3 previousPos;
    float idleMoveCounter = 0;
    void Start()
    {
      GameManager.Instance.Add_LoseObserver(this);
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        transform.position = new Vector3(transform.position.x, -6.44f, transform.position.z);
        StartCoroutine(inm());
    }
    IEnumerator inm()
    {
        yield return new WaitForSeconds(3.85f);
        climbAnim();
    }
    public void climbAnim()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        agent.enabled = true;
        GetComponent<Collider>().enabled = true;
        if (idleMove)
        {
            currentBehaviour = States.idle;
            StartCoroutine(waitingTime());
        }
    }
    public void idleEnum()
    {
        currentBehaviour = States.idle;
        StartCoroutine(waitingTime());
    }
    IEnumerator waitingTime()
    {
        float counter = 0f;
        float waitingTimer = Random.Range(2f, 4f);
        while (idleMove && counter < waitingTimer)
        {
            if(Vector3.Distance( transform.position, player.transform.position) > 100)
            {
                enemyFarDead();
            }
            counter += Time.deltaTime;
            yield return null;
        }
        currentPos = Vector3.zero;
        previousPos = Vector3.zero;
        if (idleMove)
        {
            idleMoveTarget = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            currentBehaviour = States.idleMov;
        }
    }
    void idleMoving()
    {
        if (Vector3.Distance(idleMoveTarget, transform.position) >= 0.2f)
        {
            agent.SetDestination(idleMoveTarget);
            _animator.SetBool("run", true);
            idleMoveCounter += Time.deltaTime;
            if (idleMoveCounter > 2f)
            {
                idleMoveCounter = 0;
                currentBehaviour = States.idle;
                StartCoroutine(waitingTime());
            }
        }
        else
        {
            currentBehaviour = States.idle;
            StartCoroutine(waitingTime());
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentBehaviour)
        {
            case States.spawning:
                {
              
                }
                break;
            case States.idleMov:
                {
                    if(agent.enabled && idleMove)
                    idleMoving();
                }
                break;

            case States.idle:
                {
                    if (agent.enabled)
                    {
                        agent.SetDestination(transform.position);
                        _animator.SetBool("run", false);
                    }
                }
                break;
            case States.followPlayer:
                {
                    if (Globals.isGameActive && agent.enabled)
                    {
                        following();
                    }
                }
                break;
            case States.death:
                {

                }
                break;
            case States.attack:
                {
                    _attack();
                }
                break;    
            case States.failPlayer:
                {

                }
                break;
        }
    }
    public void enemyFarDead()
    {
        _enemyCreator.enemyAll.Remove(gameObject);
        player.transform.parent.GetComponent<playerBehaviour>().enemies.Remove(this.gameObject);
        Destroy(gameObject, 0.1f);

    }
    public void dead(int damage, Vector3 forceDirection)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GetComponent<Collider>().enabled = false;
            currentBehaviour = States.death;
            agent.enabled = false;
            _enemyCreator.enemyAll.Remove(gameObject);
            StartCoroutine(deadRemoveList());
            GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * forceDirection);
            for (int i = 0; i < coinCount; i++)
            {
              GameObject coins =  Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 7, Random.Range(-0.5f, 0.5f)), Quaternion.identity);
                coins.GetComponent<coin>().moneyAmount = earnMoney;
            }
            mesh.material = deadMat;
            Destroy(gameObject, 2f);
        }
        else
        {
            _animator.SetTrigger("hit");
            currentBehaviour = States.death;

            agent.enabled = false;

            StartCoroutine(hitEnemyimpulse(forceDirection));
            StartCoroutine(hitEnemy());
        }
    }
    IEnumerator hitEnemyimpulse(Vector3 direction)
    {
        float counter = 0f;
        while(counter < 2f)
        {
            counter += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction.x, 0, direction.z), (2 - counter) * 5 * Time.deltaTime);

            yield return null;
        }
    }
    IEnumerator hitEnemy()
    {
        yield return new WaitForSeconds(5.24f);
        agent.enabled = true;
        currentBehaviour = States.idle;

    }
    IEnumerator deadRemoveList()
    {
        yield return new WaitForSeconds(0.05f);
        player.transform.parent.GetComponent<playerBehaviour>().enemies.Remove(this.gameObject);

    }
    void following()
    {
        if (Vector3.Distance(player.transform.position, transform.position) >= 6f)
        {
            agent.SetDestination(player.transform.position);
            _animator.SetBool("run", true);
        }
        else
        {
            currentBehaviour = States.attack;
        }
    }
    void _attack()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 6f)
        {
            agent.SetDestination(transform.position);
            _animator.SetBool("attack", true);
            if (attackActive)
            {
                StartCoroutine(attackHit());
            }
        }
        else
        {
            _animator.SetBool("attack", false);
            currentBehaviour = States.followPlayer;
        }
    }
    IEnumerator attackHit()
    {
        player.transform.parent.GetComponent<playerHealth>().characterDamage(damage);
        attackActive = false;
        yield return new WaitForSeconds(1f / attackSpeed);
        attackActive = true;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.transform.GetComponent<characterControl>() != null)
    //    {
    //        currentBehaviour = States.followPlayer;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.GetComponent<characterControl>() != null)
    //    {
    //        currentBehaviour = States.idle;
    //    }
    //}
    public void LoseScenario()
    {
        GameManager.Instance.Remove_LoseObserver(this);
        currentBehaviour = States.failPlayer;

    }
}
