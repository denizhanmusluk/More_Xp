using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assassinAttack : MonoBehaviour
{
    public playerBehaviour _playerBeh;

    public Vector3 moveTarget;
    public List<GameObject> enemies;
    void Start()
    {
        //Destroy(gameObject.GetComponent<Collider>(), 2);
        //Destroy(gameObject, 5);
        StartCoroutine(forwardMove());
        GetComponent<Collider>().enabled = false;
    }
    IEnumerator forwardMove()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GetComponent<Collider>().enabled = false;
            while (Vector3.Distance(transform.position, enemies[i].transform.position) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemies[i].transform.position, 307 * Time.deltaTime);
                yield return null;
            }
            GetComponent<Collider>().enabled = true;
            yield return new WaitForSeconds(0.2f);

        }
        while (Vector3.Distance(transform.position, _playerBeh.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerBeh.transform.position, 307 * Time.deltaTime);
            yield return null;
        }
        _playerBeh.activeCharacter();
        Destroy(gameObject, 0.1f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().dead(Globals.assassinDamage, 2.3f * (transform.up + (other.transform.position - transform.position).normalized));
            //Vector3 forceDirection = (other.transform.position - transform.position).normalized;
            //other.GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 1f, 0)));
            //_playerBeh.enemies.Remove(other.gameObject);
        }
    }
}
