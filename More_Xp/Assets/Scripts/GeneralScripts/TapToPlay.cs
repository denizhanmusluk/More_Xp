using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TapToPlay : MonoBehaviour
{
    [SerializeField(), Range(0f, 5f)] private float scaleFactor;
    [SerializeField(), Range(0f, 10f)] private float scaleSpeed;

    void Start()
    {
        StartCoroutine(swipeMove());
    }
    IEnumerator swipeMove()
    {
        float counter = 0f;
        float value = 0;
        while (true)
        {
            counter += scaleSpeed * Time.deltaTime;
            value = Mathf.Abs(Mathf.Sin(counter));
            value *= 0.05f * scaleFactor;
            transform.localScale = new Vector3(1 + value, 1 + value, 1 + value);

            yield return null;
        }
    }
}