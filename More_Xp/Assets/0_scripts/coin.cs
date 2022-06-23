using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapticPlugin;

public class coin : MonoBehaviour
{
    float motionSpeed = 20;
    Transform target;
    GameObject particle;
    public int moneyAmount;
    void Start()
    {
        particle = transform.GetChild(0).gameObject;
        particle.SetActive(true);
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2f, 2f), 0.5f, Random.Range(-2f, 2f)) * 200);
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f)) * 1000);
        StartCoroutine(rotateCoin());
    }
    IEnumerator rotateCoin()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().AddForce(-Vector3.up * 500);
        yield return new WaitForSeconds(0.3f);
        Destroy(GetComponent<Rigidbody>());

        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        yield return null;
        //particle.SetActive(true);
        particle.transform.position -= new Vector3(0, 1, 0);
        particle.transform.parent = null;
        while (true)
        {
            transform.Rotate(50 * Time.deltaTime, 200 * Time.deltaTime, 50 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator targetMotion()
    {
        particle.SetActive(false);

        GetComponent<SphereCollider>().enabled = false;
        float counter = 0f;
        float angle = 0f;
        Vector3 dirVect = (transform.position - target.position).normalized;



        while (counter < Mathf.PI / 2)
        {
            counter += 3 * Time.deltaTime;
            angle = 2 * Mathf.Cos(counter);

            transform.position = Vector3.MoveTowards(transform.position, transform.position + dirVect * angle + new Vector3(0, 0.5f, 0), counter * motionSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, target.position) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, (3 + Mathf.Abs(5 - 0.3f * Vector3.Distance(transform.position, target.position))) * motionSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target.position, (40 / Vector3.Distance(transform.position, target.position)) * motionSpeed * Time.deltaTime);
            yield return null;
        }
        TapticManager.Impact(ImpactFeedback.Light);


        GameManager.Instance.MoneyUpdate(moneyAmount);

        /////////////////
        //target.GetComponent<playerHealth>().characterHealthUp(2);
        /////////////////

        Destroy(particle);

        //target.GetComponent<PlayerParent>().playerYearSet(1);
        GameObject money = gameObject;
        money.transform.parent = null;
        Destroy(money);
    }
    public void collect(Transform moneyTarget)
    {
        target = moneyTarget;
        StartCoroutine(targetMotion());
    }
}
