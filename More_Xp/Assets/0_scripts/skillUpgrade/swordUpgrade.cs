using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;

public class swordUpgrade : MonoBehaviour
{
    [SerializeField] GameObject particlePrefab;
    //[SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    //[SerializeField] GameObject swordImage;
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
    public int swordLevel;
    [SerializeField] int[] damageLevel;
    [SerializeField] float[] attackSpeedLevel;
   [SerializeField] playerBehaviour _playerBehaviour;
    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.swordLevel = PlayerPrefs.GetInt("swordLevel");
        currentCost = cost[Globals.swordLevel];
        swordLevel = Globals.swordLevel;
        //}

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.swordLevel];
            costText.text = cost[Globals.swordLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.swordDamage = damageLevel[Globals.swordLevel];
        Globals.swordAttackSpeed = attackSpeedLevel[Globals.swordLevel];
        iconSet();

        /*
        if (Globals.swordLevel > 0)
        {
            iconSet();
            //stompOpen();
        }
        */
        if (Globals.swordLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        _playerBehaviour.swordSet();
    }
    void iconSet()
    {
        //buyIcon.SetActive(false);
        /////////
        for (int i = 0; i < upgradeIcons.Length; i++)
        {
            upgradeIcons[i].SetActive(false);
        }
        ////////
        upgradeIcons[swordLevel % upgradeIcons.Length].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
        PlayerPrefs.SetInt("skiller", 1);
        //if (Globals.stompLevel == 0)
        //{
        //    stompOpen();
        //}
        var partEff = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        partEff.transform.rotation = Quaternion.Euler(-90, 0, 0);
        swordLevel++;
        Globals.swordLevel = swordLevel;
        PlayerPrefs.SetInt("swordLevel", Globals.swordLevel);

        currentCost = cost[Globals.swordLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.swordDamage = damageLevel[Globals.swordLevel];
        Globals.swordAttackSpeed = attackSpeedLevel[Globals.swordLevel];

        if (Globals.swordLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();
        _playerBehaviour.swordSet();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > (cost[Globals.swordLevel] / 50) - 1 && Globals.swordLevel < cost.Length - 1)
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
        currentAmount -= (cost[Globals.swordLevel] / 50);
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-(cost[Globals.swordLevel] / 50));
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
    //void stompOpen()
    //{
    //    lightningImage.SetActive(true);
    //    skillManager.Instance.stompCooldown();
    //}
}
