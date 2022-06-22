using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] int earnMoney;
    public GameObject player;
    public enum States { idle, followPlayer, death, attack }
    public States currentBehaviour;

    public NavMeshAgent agent;
    Animator _animator;
    public enemyCreator _enemyCreator;
    public int Health;
    void Start()
    {
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
                        agent.SetDestination(player.transform.position);
                        _animator.SetBool("run", true);
                    }
                }
                break;
            case States.death:
                {

                }
                break;
            case States.attack:
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
            GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 0.5f, 0)));
            GameManager.Instance.MoneyUpdate(earnMoney);

            Destroy(gameObject, 2f);
        }
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
}
