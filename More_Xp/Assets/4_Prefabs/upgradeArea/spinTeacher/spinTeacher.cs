using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinTeacher : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(spinAnimation());
    }

   IEnumerator spinAnimation()
    {
        while (true)
        {
            anim.SetBool("spin", true);
            yield return new WaitForSeconds(3);
            anim.SetBool("spin", false);
            yield return new WaitForSeconds(2);

        }
    }
}
