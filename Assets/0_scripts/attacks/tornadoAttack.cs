using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornadoAttack : MonoBehaviour
{
    public playerBehaviour _playerBeh;

    public Vector3 moveTarget;
    void Start()
    {
        //Destroy(gameObject.GetComponent<Collider>(), 2);
        //Destroy(gameObject, 5);
        StartCoroutine(forwardMove());
    }
    IEnumerator forwardMove()
    {
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(transform.position, moveTarget) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, moveTarget, 3 * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, _playerBeh.transform.position) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, _playerBeh.transform.position, 4 * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject, 0.1f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().dead(Globals.tornadoDamage, transform.up * Globals.tornadoForce + (other.transform.position - transform.position).normalized);
            //Vector3 forceDirection = (other.transform.position - transform.position).normalized;
            //other.GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 1f, 0)));
            //_playerBeh.enemies.Remove(other.gameObject);
        }
    }
}
