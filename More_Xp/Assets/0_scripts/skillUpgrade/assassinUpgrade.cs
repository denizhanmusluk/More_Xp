using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;

public class assassinUpgrade : MonoBehaviour
{
    [SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] GameObject assassinImage;
    [SerializeField] TextMeshProUGUI skillLevelText;
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
    public int assassinLevel;
    [SerializeField] int[] coolDownLevel;
    [SerializeField] int[] damageLevel;
    [SerializeField] int[] assassinAmountLevel;
    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.assassinLevel = PlayerPrefs.GetInt("assassinLevel");
        currentCost = cost[Globals.assassinLevel];
        assassinLevel = Globals.assassinLevel;
        //}

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.assassinLevel];
            costText.text = cost[Globals.assassinLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.assassinCooldown = coolDownLevel[Globals.assassinLevel];
        Globals.assassinDamage = damageLevel[Globals.assassinLevel];
        Globals.assassinAmount = assassinAmountLevel[Globals.assassinLevel];
        if (Globals.assassinLevel > 0)
        {
            skillLevelText.text = Globals.assassinLevel.ToString();
            iconSet();
            assassinOpen();
        }
        if (Globals.assassinLevel == cost.Length - 1)
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
        upgradeIcons[assassinLevel % 3].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
        if (Globals.assassinLevel == 0)
        {
            assassinOpen();
        }
        assassinLevel++;
        Globals.assassinLevel = assassinLevel;
        PlayerPrefs.SetInt("assassinLevel", Globals.assassinLevel);

        currentCost = cost[Globals.assassinLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.assassinCooldown = coolDownLevel[Globals.assassinLevel];
        Globals.assassinDamage = damageLevel[Globals.assassinLevel];
        Globals.assassinAmount = assassinAmountLevel[Globals.assassinLevel];
        if (Globals.assassinLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();
        skillLevelText.text = Globals.assassinLevel.ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > 49 && Globals.assassinLevel < cost.Length - 1)
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
    void assassinOpen()
    {
        assassinImage.SetActive(true);
        skillManager.Instance.assassinCooldown();
    }
}
