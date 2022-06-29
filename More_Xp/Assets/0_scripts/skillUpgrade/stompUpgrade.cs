using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;
public class stompUpgrade : MonoBehaviour
{
    [SerializeField] GameObject particlePrefab;
    [SerializeField] GameObject emptyTeacher, teacher;
    [SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] GameObject lightningImage;
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
    public int stompLevel;
    [SerializeField] int[] coolDownLevel;
    [SerializeField] int[] amountLevel;
    [SerializeField] int[] damageLevel;

    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.stompLevel = PlayerPrefs.GetInt("stompLevel");
        currentCost = cost[Globals.stompLevel];
        stompLevel = Globals.stompLevel;
        //}

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.stompLevel];
            costText.text = cost[Globals.stompLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.stompCooldown = coolDownLevel[Globals.stompLevel];
        Globals.stompDamage = damageLevel[Globals.stompLevel];
        Globals.lightningAmount = amountLevel[Globals.stompLevel];
        if (Globals.stompLevel > 0)
        {
            skillLevelText.text = Globals.stompLevel.ToString();
            iconSet();
            stompOpen();
        }
        if (Globals.stompLevel == cost.Length - 1)
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
        upgradeIcons[stompLevel % 3].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
        VibratoManager.Instance.MediumViration();
        PlayerPrefs.SetInt("skiller", 1);
        var partEff = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        partEff.transform.rotation = Quaternion.Euler(-90, 0, 0);
        if (Globals.stompLevel == 0)
        {
            stompOpen();
        }
        stompLevel++;
        Globals.stompLevel = stompLevel;
        PlayerPrefs.SetInt("stompLevel", Globals.stompLevel);

        currentCost = cost[Globals.stompLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.stompCooldown = coolDownLevel[Globals.stompLevel];
        Globals.stompDamage = damageLevel[Globals.stompLevel];
        Globals.lightningAmount = amountLevel[Globals.stompLevel];
        if (Globals.stompLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();
        skillLevelText.text = Globals.stompLevel.ToString();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > (cost[Globals.stompLevel] / 50) - 1 && Globals.stompLevel < cost.Length - 1)
            {
                if (sellActive && isbuy)
                {
                    VibratoManager.Instance.LightViration();
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
        currentAmount -= (cost[Globals.stompLevel] / 50);
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-(cost[Globals.stompLevel] / 50));
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
    void stompOpen()
    {
        emptyTeacher.SetActive(false);
        teacher.SetActive(true);
        lightningImage.SetActive(true);
        skillManager.Instance.stompCooldown();
    }
}
