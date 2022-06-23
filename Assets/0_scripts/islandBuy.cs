using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;

public class islandBuy : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject canvas;
    bool sellActive = true;
    public bool isbuy = true;
    int currentAmount;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI costText;
    [SerializeField] int cost;
    [SerializeField] string currentCost;
    [SerializeField] string islandName;
    float counterTime = 0;

    void Start()
    {

        //}
        if (PlayerPrefs.GetInt(islandName) != 0)
        {
            openIsland();
        }


        if (PlayerPrefs.GetInt(currentCost) == 0)
        {
            currentAmount = cost;
            costText.text = cost.ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCost);
            costText.text = currentAmount.ToString();

        }
        outline.fillAmount = 1 - (float)currentAmount / (float)cost;

 

        /*
        if (Globals.swordLevel > 0)
        {
            iconSet();
            //stompOpen();
        }
        */

    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > 49 )
            {
                if (sellActive && isbuy)
                {
                    StartCoroutine(buy());
                }
                //GameManager.Instance.MoneyUpdate(-cost);
            }
        }
    }
    IEnumerator buy()
    {
        isbuy = false;
        currentAmount -= 50;
        outline.fillAmount = 1 - (float)currentAmount / (float)cost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-50);
        PlayerPrefs.SetInt(currentCost, currentAmount);
        if (currentAmount == 0)
        {
            outline.fillAmount = 0;
            openIsland();
            //StartCoroutine(buildScaling());
            //GetComponent<Collider>().enabled = false;
        }
        counterTime += Time.deltaTime;
        if (counterTime > 0.15f)
        {
            counterTime = 0f;
            TapticManager.Impact(ImpactFeedback.Light);
        }

        yield return null;
        isbuy = true;
    }
    void openIsland()
    {
        sellActive = false;
        PlayerPrefs.SetInt(islandName, 1);
        obstacle.SetActive(false);
        canvas.SetActive(false);
    }
}