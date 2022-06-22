using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;
public class tornadoUpgrade : MonoBehaviour
{
    [SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] GameObject tornadoImage;
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
    public int tornadoLevel;
    [SerializeField] int[] coolDownLevel;
    [SerializeField] int[] damageLevel;
    [SerializeField] int[] tornadoDistanceLevel;
    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.tornadoLevel = PlayerPrefs.GetInt("tornadoLevel");
        currentCost = cost[Globals.tornadoLevel];
        tornadoLevel = Globals.tornadoLevel;
        //}

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.tornadoLevel];
            costText.text = cost[Globals.tornadoLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.tornadoCooldown = coolDownLevel[Globals.tornadoLevel];
        Globals.tornadoDamage = damageLevel[Globals.tornadoLevel];
        Globals.tornadoDistance = tornadoDistanceLevel[Globals.tornadoLevel];
        if (Globals.tornadoLevel > 0)
        {
            iconSet();
            tornadoOpen();
        }
        if (Globals.tornadoLevel == cost.Length - 1)
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
        upgradeIcons[tornadoLevel % 3].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
        if (Globals.tornadoLevel == 0)
        {
            tornadoOpen();
        }
        tornadoLevel++;
        Globals.tornadoLevel = tornadoLevel;
        PlayerPrefs.SetInt("tornadoLevel", Globals.tornadoLevel);

        currentCost = cost[Globals.tornadoLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.tornadoCooldown = coolDownLevel[Globals.tornadoLevel];
        Globals.tornadoDamage = damageLevel[Globals.tornadoLevel];
        Globals.tornadoDistance = tornadoDistanceLevel[Globals.tornadoLevel];
        if (Globals.tornadoLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > 49 && Globals.tornadoLevel < cost.Length - 1)
            {
                if (sellActive && isbuy)
                {
                    StartCoroutine(buy());
                }
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
    void tornadoOpen()
    {
        tornadoImage.SetActive(true);
        skillManager.Instance.tornadoCooldown();
    }
}
