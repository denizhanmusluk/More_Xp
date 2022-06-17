using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.GetComponent<enemy>() != null)
        {
            Vector3 forcDir = (collision.transform.position - transform.position).normalized;
            collision.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0) * 10000);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<enemy>() != null)
        {
            Vector3 forcDir = (other.transform.position - transform.position).normalized;
            other.transform.GetComponent<Rigidbody>().AddForce(forcDir * 5000);
        }
    }
}
