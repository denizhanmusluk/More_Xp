using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;
public class meteorUpgrade : MonoBehaviour
{
    [SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] GameObject meteorImage;
    [SerializeField] int[] cost;
    [SerializeField] int currentCost;
    int currentAmount;
    [SerializeField] public Image outline;
    [SerializeField] public TextMeshProUGUI costText;
    bool sellActive = true;
    //[SerializeField] GameObject buildPrefab;
    //[SerializeField] Vector3 buildPositionOffset;
    public bool isbuy = true;
    [SerializeField] string currentCostSkill;
    float counterTime = 0;
    public int meteorLevel;
    [SerializeField] int[] coolDownLevel;
    [SerializeField] int[] damageLevel;
    [SerializeField] float[] meteorTimeLevel;
    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.meteorLevel = PlayerPrefs.GetInt("meteorLevel");
        currentCost = cost[Globals.meteorLevel];
        meteorLevel = Globals.meteorLevel;
        //}

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.meteorLevel];
            costText.text = cost[Globals.meteorLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.meteorCooldown = coolDownLevel[Globals.meteorLevel];
        Globals.meteorDamage = damageLevel[Globals.meteorLevel];
        Globals.meteorTime = meteorTimeLevel[Globals.meteorLevel];
        if (Globals.meteorLevel > 0)
        {
            iconSet();
            meteorOpen();
        }
        if (Globals.meteorLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void iconSet()
    {
        buyIcon.SetActive(false);
        /////////
        for (int i = 0; i < upgradeIcons.Length; i++)
        {
            upgradeIcons[i].SetActive(false);
        }
        ////////
        upgradeIcons[meteorLevel % 3].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
        if (Globals.meteorLevel == 0)
        {
            meteorOpen();
        }
        meteorLevel++;
        Globals.meteorLevel = meteorLevel;
        PlayerPrefs.SetInt("meteorLevel", Globals.meteorLevel);

        currentCost = cost[Globals.meteorLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.meteorCooldown = coolDownLevel[Globals.meteorLevel];
        Globals.meteorDamage = damageLevel[Globals.meteorLevel];
        Globals.meteorTime = meteorTimeLevel[Globals.meteorLevel];
        if (Globals.meteorLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > 49 && Globals.meteorLevel < cost.Length - 1)
            {
                if (sellActive && isbuy)
                {
                    StartCoroutine(buy());
                }
                //GameManager.Instance.MoneyUpdate(-cost);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sellActive = true;
        }
    }

    IEnumerator buy()
    {
        isbuy = false;
        currentAmount -= 50;
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-50);
        PlayerPrefs.SetInt(currentCostSkill, currentAmount);
        if (currentAmount == 0)
        {
            outline.fillAmount = 0;
            sellActive = false;
            levelUp();
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
    void meteorOpen()
    {
        meteorImage.SetActive(true);
        skillManager.Instance.meteorCooldown();
    }
}
