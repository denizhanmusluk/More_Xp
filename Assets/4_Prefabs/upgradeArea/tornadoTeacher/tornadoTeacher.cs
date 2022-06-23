using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornadoTeacher : MonoBehaviour
{
    [SerializeField] GameObject tornadoEffect;
    [SerializeField] Transform targetPoint;
    Animator anim;
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
        }
    }
    // Update is called once per frame
    public void tornadoSkill()
    {
        GameObject tornado = Instantiate(tornadoEffect, transform.position, transform.rotation);
        StartCoroutine(forwardMove(tornado));
    }
    IEnumerator forwardMove(GameObject _tornado)
    {
        yield return new WaitForSeconds(0.6f);
        while (Vector3.Distance(_tornado.transform.position, targetPoint.position) > 1f)
        {
            _tornado.transform.position = Vector3.Lerp(_tornado.transform.position, targetPoint.position, 2 * Time.deltaTime);
            yield return null;
        }
        while (Vector3.Distance(_tornado.transform.position, transform.position) > 1f)
        {
            _tornado.transform.position = Vector3.Lerp(_tornado.transform.position, transform.position, 2 * Time.deltaTime);
            yield return null;
        }
        Destroy(_tornado, 0.2f);
    }
}
