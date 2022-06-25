using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assassinTeacher : MonoBehaviour
{
    [SerializeField] GameObject[] assassinEffect;
    [SerializeField] Transform[] assassinPoints;
    int counter = 0;
    [SerializeField] SkinnedMeshRenderer skin;
    Material mat;
    int[] rgb = { 255, 0, 0 };
    int counterRGB = 0;
    void Start()
    {
        mat = skin.material;
        StartCoroutine(setColor());
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
    IEnumerator setColor()
    {
        while (true)
        {
            for(int i = 0; i < 3; i++)
            {
                counterRGB = 0;
                while (counterRGB<255)
                {
                    counterRGB += 1;
                    int val = (i + 1) % 3;
                    rgb[val] = counterRGB;
                    mat.color = new Color32((byte)rgb[0], (byte)rgb[1], (byte)rgb[2], 255);
                    yield return null;
                }
                counterRGB = 255;
                while (counterRGB > 0)
                {
                    counterRGB -= 1;
                    int val = (i) % 3;
                    rgb[val] = counterRGB;
                    mat.color = new Color32((byte)rgb[0], (byte)rgb[1], (byte)rgb[2], 255);

                    yield return null;
                }
                counterRGB = 0;
            }
            yield return null;
        }
    }
}
