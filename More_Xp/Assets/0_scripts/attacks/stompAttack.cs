using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stompAttack : MonoBehaviour
{
    public playerBehaviour _playerBeh;
    void Start()
    {
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().dead(Globals.stompDamage, new Vector3(0,3,0) + (other.transform.position - transform.position).normalized);
            //Vector3 forceDirection = (other.transform.position - transform.position).normalized;
            //other.GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 1f, 0)));
            //_playerBeh.enemies.Remove(other.gameObject);
        }
    }
}
