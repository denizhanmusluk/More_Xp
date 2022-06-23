using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stompTeacher : MonoBehaviour
{
    [SerializeField] GameObject[] stompEffect;
    [SerializeField] Transform effectPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void stompSkill()
    {
        GameObject lighting1 = Instantiate(stompEffect[Random.Range(0, stompEffect.Length)], new Vector3(effectPoint.transform.position.x, 33, effectPoint.transform.position.z), Quaternion.identity);
        Destroy(lighting1, 2);
    }
}
