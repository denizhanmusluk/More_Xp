using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour,ILoseObserver
{
    public int Health;
    [SerializeField] int earnMoney;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int coinCount;
    [SerializeField] float attackSpeed;
    [SerializeField] int damage;
    public GameObject player;
    public enum States { idle, followPlayer, death, attack,failPlayer}
    public States currentBehaviour;

    public NavMeshAgent agent;
    Animator _animator;
    public enemyCreator _enemyCreator;
    bool attackActive = true;
    void Start()
    {
      GameManager.Instance.Add_LoseObserver(this);
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentBehaviour)
        {
            case States.idle:
                {
                    agent.SetDestination(transform.position);
                    _animator.SetBool("run", false);
                }
                break;
            case States.followPlayer:
                {
                    if (Globals.isGameActive)
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
    public void dead(int damage, Vector3 forceDirection)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GetComponent<Collider>().enabled = false;
            currentBehaviour = States.death;
            agent.enabled = false;
            _enemyCreator.enemyAll.Remove(gameObject);
            player.transform.parent.GetComponent<playerBehaviour>().enemies.Remove(this.gameObject);
            GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * forceDirection);
            for (int i = 0; i < coinCount; i++)
            {
              GameObject coins =  Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 7, Random.Range(-0.5f, 0.5f)), Quaternion.identity);
                coins.GetComponent<coin>().moneyAmount = earnMoney;
            }
            Destroy(gameObject, 2f);
        }
    }
    void following()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 4f)
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
        if (Vector3.Distance(player.transform.position, transform.position) < 4f)
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
