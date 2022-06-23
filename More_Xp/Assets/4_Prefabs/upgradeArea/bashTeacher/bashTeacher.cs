using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bashTeacher : MonoBehaviour
{
    [SerializeField] GameObject[] bashEffect;
    Vector3 target;
    int counter = 0;

    void Start()
    {
        target= transform.position + transform.forward * 20;

    }

    // Update is called once per frame
    public  void bashSkill()
    {
        GameObject bashEffect1 = Instantiate(bashEffect[counter % bashEffect.Length], transform.position, transform.rotation);
        StartCoroutine(forwardMove(bashEffect1));
        counter++;
    }
    IEnumerator forwardMove(GameObject bash)
    {
        while (Vector3.Distance(bash.transform.position, target) > 0.1f)
        {
            bash.transform.position = Vector3.Lerp(bash.transform.position, target, 2 * Time.deltaTime);
            yield return null;
        }
    }
}
