using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assassinTeacher : MonoBehaviour
{
    [SerializeField] GameObject[] assassinEffect;
    [SerializeField] Transform[] assassinPoints;
    int counter = 0;

    void Start()
    {
        StartCoroutine(assassinAnimation());
    }
    IEnumerator assassinAnimation()
    {
        while (true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            assassinSkill();
            yield return new WaitForSeconds(5f);
            counter++;

        }
    }
    public void assassinSkill()
    {
        GameObject assassin = Instantiate(assassinEffect[counter % assassinEffect.Length], transform.position, transform.rotation);
        StartCoroutine(forwardMove(assassin));
    }
    IEnumerator forwardMove(GameObject _assassin)
    {
        for (int i = 0; i < assassinPoints.Length; i++)
        {
            while (Vector3.Distance(_assassin.transform.position, assassinPoints[i].transform.position) > 0.5f)
            {
                _assassin. transform.position = Vector3.MoveTowards(_assassin.transform.position, assassinPoints[i].transform.position, 50 * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);

        }
        while (Vector3.Distance(_assassin.transform.position, transform.position) > 0.1f)
        {
            _assassin. transform.position = Vector3.MoveTowards(_assassin.transform.position, transform.position, 50 * Time.deltaTime);
            yield return null;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        Destroy(_assassin, 0.2f);
    }
}
