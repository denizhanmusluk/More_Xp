using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    playerControl playerControlling;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] List<GameObject> attackListEnemies;
    bool attacking = true;
    [SerializeField] Animator animator;
    public float attackSpeed;
    public float health;
    [SerializeField] GameObject swordParticle;

    void Start()
    {
        playerControlling = GetComponent<playerControl>();
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
            other.GetComponent<enemy>().currentBehaviour = enemy.States.followPlayer;
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().currentBehaviour = enemy.States.idle;
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
            if (Vector3.Distance(enemies[i].transform.position, transform.position) < 7)
            {
                attacking = false;
                attack(enemies[i].transform);
                break;
            }
        }
    }
    void attack(Transform _enemy)
    {
        animator.SetFloat("attackspeed", attackSpeed);
        int animationSelect = Random.Range(0, 2);
        string[] atck = { "attack1", "attack2" };
        swordParticle.SetActive(true);
        animator.SetTrigger(atck[animationSelect]);
        StartCoroutine(_attack(_enemy));
    }
    public void attackFunction()
    {
        swordParticle.SetActive(false);
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) < 7)
            {
                enemies[i].GetComponent<enemy>().dead();
                Vector3 forceDirection = (enemies[i].transform.position - transform.position).normalized;
                enemies[i].GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 0.5f, 0)));

                enemies.Remove(enemies[i]);
            }
        }
        attacking = true;
    }
    IEnumerator _attack(Transform _enemy)
    {
        float counter = 0f;
        while(counter < 1/ attackSpeed)
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
        Debug.Log("rotttt" + newSteer);
    }
}