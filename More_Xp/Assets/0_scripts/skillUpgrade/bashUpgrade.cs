using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapticPlugin;

public class bashUpgrade : MonoBehaviour
{
    [SerializeField] GameObject particlePrefab;
    [SerializeField] GameObject emptyTeacher, teacher;
    [SerializeField] GameObject buyIcon;
    [SerializeField] GameObject[] upgradeIcons;
    [SerializeField] GameObject bashImage;
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
    public int bashLevel;
    [SerializeField] int[] coolDownLevel;
    [SerializeField] int[] damageLevel;
    [SerializeField] int[] distanceLevel;
    void Start()
    {

        //if (PlayerPrefs.GetInt("bashLevel") != 0)
        //{
        Globals.bashLevel = PlayerPrefs.GetInt("bashLevel");
        currentCost = cost[Globals.bashLevel];
        bashLevel = Globals.bashLevel;
        //}


  
        

        if (PlayerPrefs.GetInt(currentCostSkill) == 0)
        {
            currentAmount = cost[Globals.bashLevel];
            costText.text = cost[Globals.bashLevel].ToString();
        }
        else
        {
            currentAmount = PlayerPrefs.GetInt(currentCostSkill);
            costText.text = currentAmount.ToString();
        }
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.bashCooldown = coolDownLevel[Globals.bashLevel];
        Globals.bashDamage = damageLevel[Globals.bashLevel];
        Globals.bashDistance = distanceLevel[Globals.bashLevel];
        if (Globals.bashLevel > 0)
        {
            skillLevelText.text = Globals.bashLevel.ToString();
            iconSet();
            bashOpen();
        }
        if (Globals.bashLevel == cost.Length - 1)
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
        upgradeIcons[bashLevel % 3].SetActive(true);
    }
    // Update is called once per frame
    void levelUp()
    {
      var partEff =  Instantiate(particlePrefab, transform.position, Quaternion.identity);
        partEff.transform.rotation = Quaternion.Euler(-90, 0, 0);
        if (Globals.bashLevel == 0)
        {
            bashOpen();
        }
        bashLevel++;
        Globals.bashLevel = bashLevel;
        PlayerPrefs.SetInt("bashLevel", Globals.bashLevel);

        currentCost = cost[Globals.bashLevel];
        currentAmount = currentCost;

        costText.text = currentAmount.ToString();
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;

        Globals.bashCooldown = coolDownLevel[Globals.bashLevel];
        Globals.bashDamage = damageLevel[Globals.bashLevel];
        Globals.bashDistance = distanceLevel[Globals.bashLevel];
        if(Globals.bashLevel == cost.Length - 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        iconSet();
        skillLevelText.text = Globals.bashLevel.ToString();

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Globals.moneyAmount > (cost[Globals.bashLevel] / 50) - 1 && Globals.bashLevel < cost.Length - 1)
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
        currentAmount -= cost[Globals.bashLevel]/50;
        outline.fillAmount = 1 - (float)currentAmount / (float)currentCost;
        costText.text = currentAmount.ToString();
        GameManager.Instance.MoneyUpdate(-cost[Globals.bashLevel] / 50);
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
    void bashOpen()
    {
        emptyTeacher.SetActive(false);
        teacher.SetActive(true);
        bashImage.SetActive(true);
        skillManager.Instance.bashCooldown();
    }
}
