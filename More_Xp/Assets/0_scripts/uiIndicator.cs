using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiIndicator : MonoBehaviour
{

    Vector3 direction;
    Vector3 distance;
    bool followActive = true;
  [SerializeField]  Transform player;
    [SerializeField] GameObject selectionTarget;
    bool troubleActive = false;
    private void Start()
    {
        if (PlayerPrefs.GetInt("skiller") == 1)
        {
            Destroy(transform.parent.gameObject);
        }

    }
    public void selectTarget()
    {
        GetComponent<Image>().enabled = true;
   
        followActive = true;
    }


    private void Update()
    {
        if (Globals.moneyAmount >= 50)
        {
            GetComponent<Image>().enabled = true;
            arrowUIPos();
        }
        if (PlayerPrefs.GetInt("skiller") == 1)
        {
            Destroy(transform.parent.gameObject);
        }
    }
    public void arrowUIPos()
    {
        direction = (player.transform.position - selectionTarget.transform.position ).normalized;
        distance =selectionTarget.transform.position - player.transform.position;
        float distZ = Mathf.Clamp(distance.z, -20, 20);
        float distX = Mathf.Clamp(distance.x, -2, 2);
        //distX = Mathf.Abs(distX);
        //distZ = Mathf.Abs(distZ);
        int magnX;
        int magnZ;
        if (distX > 0)
        {
            magnX = 50;
        }
        else
        {
            magnX = -50;
        }

        if (distZ > 0)
        {
            magnZ = 100;
        }
        else
        {
            magnZ = -100;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector3(direction.x * Mathf.Abs(distX) * Screen.width / 15 + magnX, direction.z * Mathf.Abs(distZ) * Screen.height / 240 + magnZ, 0);


        float angle;
        if (direction == Vector3.zero)
        {
            angle = 0;
        }
        else
        {
            angle = Mathf.Atan(direction.x / direction.z);
        }
        angle = angle * 180 / 3.14f;
        if (direction.z < 0)
        {
            angle += 180;
        }

        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -angle);
    
    }


}
