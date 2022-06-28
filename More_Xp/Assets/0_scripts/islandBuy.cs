using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;

public class islandBuy : MonoBehaviour
{
    [SerializeField] int healthUp;

    [SerializeField] GameObject particlePrefab;
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
    [SerializeField] enemyCreator enemyCreat;
    [SerializeField] GameObject playerParn;
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
            if (Globals.moneyAmount > (cost / 50) - 1)
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
        currentAmount -= cost/50;
        outline.fillAmount = 1 - (float)currentAmount / (float)cost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-cost / 50);
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
        playerParn.GetComponent<playerHealth>().maxHealth += healthUp;
        var partEff = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        partEff.transform.rotation = Quaternion.Euler(-90, 0, 0);
        sellActive = false;
        PlayerPrefs.SetInt(islandName, 1);
        StartCoroutine(unlocked());
        canvas.SetActive(false);
        enemyCreat.spawn();
        ////////////////////////////
    }
    IEnumerator unlocked()
    {
        obstacle.transform.GetChild(0).gameObject.SetActive(false);
        obstacle.transform.GetChild(1).gameObject.SetActive(true);
        float counter = 0f;
        float val = 0f;
        float firstPosY = obstacle.transform.GetChild(1).transform.position.y;
        while (counter < 3 * Mathf.PI / 2)
        {
            counter += 4 * Time.deltaTime;
            val = Mathf.Sin(counter);
            obstacle.transform.GetChild(1).transform.position = new Vector3 (obstacle.transform.GetChild(1).transform.position.x, firstPosY + val * 2 * counter, obstacle.transform.GetChild(1).transform.position.z);

            yield return null;
        }
        //yield return new WaitForSeconds(1f);
        obstacle.SetActive(false);

    }
}
