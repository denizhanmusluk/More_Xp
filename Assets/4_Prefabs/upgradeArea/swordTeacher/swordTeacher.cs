using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordTeacher : MonoBehaviour
{
    [SerializeField] GameObject[] swords;
    int counter = 0;
    Animator anim;
    string[] attackAnimSelect = { "attack1" , "attack2" };
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(attacTeacher());
    }

   IEnumerator attacTeacher()
    {
        while (true)
        {
            counter++;
            anim.SetTrigger(attackAnimSelect[counter % 2]);
            swords[counter % swords.Length].SetActive(true);
            yield return new WaitForSeconds(2);
            swords[counter % swords.Length].SetActive(false);

        }
    }
}
