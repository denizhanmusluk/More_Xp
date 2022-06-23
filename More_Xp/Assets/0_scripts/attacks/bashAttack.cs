using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bashAttack : MonoBehaviour
{
    //[SerializeField] List<GameObject> enemies;
    public playerBehaviour _playerBeh;

    public Vector3 moveTarget;
    void Start()
    {
        Destroy(gameObject.GetComponent<Collider>(), 1);
        Destroy(gameObject, 4);
        StartCoroutine(forwardMove());
    }
    IEnumerator forwardMove()
    {
        while (Vector3.Distance(transform.position, moveTarget) >0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, moveTarget, 2 * Time.deltaTime);
                yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().dead(Globals.bashDamage, transform.forward * Globals.bashForce + (other.transform.position - transform.position).normalized);
            //Vector3 forceDirection = (other.transform.position - transform.position).normalized;
            //other.GetComponent<Ragdoll>().RagdollActivateWithForce(true, 0.35f * (forceDirection + new Vector3(0, 1f, 0)));
            //_playerBeh.enemies.Remove(other.gameObject);
        }
    }
}
