using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorTeacher : MonoBehaviour
{
    [SerializeField] GameObject meteorEffect;
    [SerializeField] Transform effectPoint;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(meteorSkill());
    }

    // Update is called once per frame
    IEnumerator meteorSkill()
    {
        while (true)
        {
            anim.SetTrigger("meteor");
            StartCoroutine(meteorAttack());
            yield return new WaitForSeconds(4f);
        }
    }
    IEnumerator meteorAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            meteorAttacking();
            yield return new WaitForSeconds(0.5f);
        }
    }
    void meteorAttacking()
    {
        Vector3 pos =new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        GameObject meteor = Instantiate(meteorEffect, pos + effectPoint.position, Quaternion.identity);
        Destroy(meteor, 2.5f);



    }
}
