using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornadoTeacher : MonoBehaviour
{
    [SerializeField] GameObject[] tornadoEffect;
    [SerializeField] Transform targetPoint;
    Animator anim;
    int counter = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(tornadoAnimation());
    }
    IEnumerator tornadoAnimation()
    {
        while (true)
        {
            anim.SetTrigger("tornado");
            yield return new WaitForSeconds(5f);
            counter++;
        }
    }
    // Update is called once per frame
    public void tornadoSkill()
    {
        GameObject tornado = Instantiate(tornadoEffect[counter % tornadoEffect.Length], transform.position, transform.rotation);
        StartCoroutine(forwardMove(tornado));
    }
    IEnumerator forwardMove(GameObject _tornado)
    {
        yield return new WaitForSeconds(0.65f);
        while (Vector3.Distance(_tornado.transform.position, targetPoint.position) > 1f)
        {
            _tornado.transform.position = Vector3.Lerp(_tornado.transform.position, targetPoint.position, 3 * Time.deltaTime);
            yield return null;
        }
    
        Destroy(_tornado, 0.1f);
    }
}
