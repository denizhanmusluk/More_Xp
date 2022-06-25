using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;

public class spinUpgrade : MonoBehaviour
{
    [SerializeField] GameObject emptyTeacher, teacher;
    [SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] GameObject spinImage;
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
    public int spinLevel;
    [SerializeField] int[] coolDownLevel;
    [SerializeField] int[] damageLevel;
    [SerializeField] float[] spinTimeLevel;
    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.spinLevel = PlayerPrefs.GetInt("spinLevel");
        currentCost = cost[Globals.spinLevel];
        spinLevel = Globals.spinLevel;
        //}

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.spinLevel];
            costText.text = cost[Globals.spinLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.spinCooldown = coolDownLevel[Globals.spinLevel];
        Globals.spinDamage = damageLevel[Globals.spinLevel];
        Globals.spinTime = spinTimeLevel[Globals.spinLevel];
        if (Globals.spinLevel > 0)
        {
            skillLevelText.text = Globals.spinLevel.ToString();
            iconSet();
            spinOpen();
        }
        if (Globals.spinLevel == cost.Length - 1)
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
        upgradeIcons[spinLevel % 3].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
        if (Globals.spinLevel == 0)
        {
            spinOpen();
        }
        spinLevel++;
        Globals.spinLevel = spinLevel;
        PlayerPrefs.SetInt("spinLevel", Globals.spinLevel);

        currentCost = cost[Globals.spinLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.spinCooldown = coolDownLevel[Globals.spinLevel];
        Globals.spinDamage = damageLevel[Globals.spinLevel];
        Globals.spinTime = spinTimeLevel[Globals.spinLevel];
        if (Globals.spinLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();
        skillLevelText.text = Globals.spinLevel.ToString();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > (cost[Globals.spinLevel] / 50) - 1 && Globals.spinLevel < cost.Length - 1)
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
        currentAmount -= (cost[Globals.spinLevel] / 50);
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-(cost[Globals.spinLevel] / 50));
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
    void spinOpen()
    {
        emptyTeacher.SetActive(false);
        teacher.SetActive(true);
        spinImage.SetActive(true);
        skillManager.Instance.spinCooldown();
    }
}
