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
            collision.transform.GetComponent<Rigidbody>().AddForce(new Vector3( forcDir.x , -1 , forcDir.z )* 2000);
        }
    }
}
