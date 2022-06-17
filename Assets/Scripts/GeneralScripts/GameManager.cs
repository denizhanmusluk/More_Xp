using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour, IWinObserver, ILoseObserver
{
    public bool gameActive;
    public static GameManager Instance;
    [SerializeField] public GameObject startButton;
    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject restartButton;
    [SerializeField] Image moneyPanel;

    public TextMeshProUGUI moneyLabel;
    

    [SerializeField] RectTransform successImage, failImage;
    float firstImageScale = 10;
    float lastImageScale = 0.7f;
    public LevelManager lvlManager;
    //[SerializeField] CinemachineVirtualCamera camFirst, camMain;
    //[SerializeField] GameObject confetti;


 
    //[SerializeField] TextMeshProUGUI population;
    //[SerializeField] public Image downArrow;
    //[SerializeField] public Image upArrow;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        winObservers = new List<IWinObserver>();
        loseObservers = new List<ILoseObserver>();
        startObservers = new List<IStartGameObserver>();
        finishObservers = new List<IFinish>();
    }
    public void MoneyUpdate(int miktar)
    {
        int moneyOld = Globals.moneyAmount;
        Globals.moneyAmount = Globals.moneyAmount + miktar;
        LeanTween.value(moneyOld, Globals.moneyAmount, 0.2f).setOnUpdate((float val) =>
        {
            moneyLabel.text = val.ToString("N0");
        });//.setOnComplete(() =>{});
        PlayerPrefs.SetInt("money", Globals.moneyAmount);

    }

    void Start()
    {
        //downArrow.gameObject.SetActive(false);
        //Application.targetFrameRate = 60;
        Globals.moneyAmount = PlayerPrefs.GetInt("money");
        moneyPanel.enabled = true;


        startButton.SetActive(true);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        Add_WinObserver(this);
        Add_LoseObserver(this);
        moneyLabel.text =Globals.moneyAmount.ToString();
    }


    IEnumerator Scaling(Transform bagel)
    {
        float counter = 0f;
        float firstSize = 0.75f;
        float sizeDelta;
        while (counter < Mathf.PI)
        {
            counter += 10 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 2f;
            bagel.GetComponent<RectTransform>().localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        bagel.localScale = new Vector3(firstSize, firstSize, firstSize);
    }
    ///////////////////////////////////////////////////


    public void moneyUp(int banknotVal)
    {
        Globals.moneyAmount += banknotVal;
        PlayerPrefs.SetInt("money", Globals.moneyAmount);
        moneyLabel.text = Globals.moneyAmount.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt("currentDoctorCount", 0);
            PlayerPrefs.SetInt("hospitalLevel", 0);            
            PlayerPrefs.SetInt("policeStationLevel", 0);
            PlayerPrefs.SetInt("currentPoliceCount", 0);
            PlayerPrefs.SetInt("currentFarmerCount", 0);
            PlayerPrefs.SetInt("farmvillelevel", 0);
            PlayerPrefs.SetInt("universityLevel", 0);
            PlayerPrefs.SetInt("currentTeacherCount", 0);

            PlayerPrefs.SetInt("money", 0);

            PlayerPrefs.SetInt("levelIndex", 1);
            PlayerPrefs.SetInt("level", 0);

        }
    }


    public void StartButton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(startDelay());
        }
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Globals.isGameActive = true;
        Globals.finish = false;
        startButton.SetActive(false);
        Notify_GameStartObservers();
        yield return new WaitForSeconds(1f);

        //doctorText.text = Globals.currentDoctorCount / 
        //    policeText.text 
        //    farmerText.text
        //    teacheText.text
    }
    public void RestartButton()
    {
    
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevelbutton()
    {
        Globals.currentLevel++;
        PlayerPrefs.SetInt("levelIndex", Globals.currentLevel);
        

        Globals.currentLevelIndex++;
        if (Globals.LevelCount - 1< Globals.currentLevelIndex)
        {
            Globals.currentLevelIndex = Random.Range(0, Globals.LevelCount - 1);
            PlayerPrefs.SetInt("level", Globals.currentLevelIndex);

        }
        else
        {
            PlayerPrefs.SetInt("level", Globals.currentLevelIndex);

        }
        StartCoroutine(levelLoad());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Globals.isGameActive = true;
    }
    public void failLevelbutton()
    {
        PlayerPrefs.SetInt("levelIndex", Globals.currentLevel);
        



        PlayerPrefs.SetInt("level", Globals.currentLevelIndex);
        //StartCoroutine(levelLoad());

        Destroy(lvlManager.loadedLevel);

        lvlManager.levelLoad();



        //Start();

        Notify_GameStartObservers();


        //Start();
        //Destroy(lvlManager.loadedLevel);
        //lvlManager.levelLoad();
        //Globals.isGameActive = true;
    }
    IEnumerator levelLoad()
    {
        //yield return null;
        Destroy(lvlManager.loadedLevel);

        lvlManager.levelLoad();



        //Start();

        Notify_GameStartObservers();

        yield return null;

    }
    public void LoseScenario()
    {
        //GameEvents.fightEvent.RemoveAllListeners();
        Globals.isGameActive = false;


        StartCoroutine(Fail_Delay());
    }
    IEnumerator Fail_Delay()
    {
        yield return new WaitForSeconds(3f);

        failPanel.SetActive(true);
        failImage.localScale = new Vector3(firstImageScale, firstImageScale, firstImageScale);
        StartCoroutine(panelScaleSet(failImage));

    }
    public void WinScenario()
    {
        //GameEvents.fightEvent.RemoveAllListeners();

        Globals.isGameActive = false;

        StartCoroutine(win_Delay());

        //Globals.currentLevel++;
        //PlayerPrefs.SetInt("level", Globals.currentLevel);

    }
    IEnumerator win_Delay()
    {
        yield return new WaitForSeconds(3f);

        successPanel.SetActive(true);
        successImage.localScale = new Vector3(firstImageScale, firstImageScale, firstImageScale); 
        StartCoroutine(panelScaleSet(successImage));
    }
    IEnumerator panelScaleSet(RectTransform image)
    {
        float counter = firstImageScale;
        while (counter > lastImageScale)
        {
            counter -= 20 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);
        counter = 0f;
        float scale = 0;
        while (counter < Mathf.PI)
        {
            counter += 10 * Time.deltaTime;
            scale = Mathf.Sin(counter);
            scale *= 0.3f;
            image.localScale = new Vector3(lastImageScale - scale, lastImageScale - scale, lastImageScale - scale);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);

    }
    public void GameEnd()
    {
        moneyPanel.enabled = false;

    }





    #region Observer Funcs

    private List<IWinObserver> winObservers;
    private List<ILoseObserver> loseObservers;
    private List<IStartGameObserver> startObservers;
    private List<IFinish> finishObservers;
    #region Finish Observer
    public void Add_FinishObserver(IFinish observer)
    {
        finishObservers.Add(observer);
    }

    public void Remove_FinishObserver(IFinish observer)
    {
        finishObservers.Remove(observer);
    }

    public void Notify_GameFinishObservers()
    {
        foreach (IFinish observer in finishObservers.ToArray())
        {
            if (finishObservers.Contains(observer))
                observer.finishRunner();
        }
    }
    #endregion



    #region Start Observer
    public void Add_StartObserver(IStartGameObserver observer)
    {
        startObservers.Add(observer);
    }

    public void Remove_StartObserver(IStartGameObserver observer)
    {
        startObservers.Remove(observer);
    }

    public void Notify_GameStartObservers()
    {
        foreach (IStartGameObserver observer in startObservers.ToArray())
        {
            if (startObservers.Contains(observer))
                observer.StartGame();
        }
    }
    #endregion

    #region End Observer

    #endregion

    #region Win Observer
    public void Add_WinObserver(IWinObserver observer)
    {
        winObservers.Add(observer);
    }

    public void Remove_WinObserver(IWinObserver observer)
    {
        winObservers.Remove(observer);
    }

    public void Notify_WinObservers()
    {
        foreach (IWinObserver observer in winObservers.ToArray())
        {
            if (winObservers.Contains(observer))
                observer.WinScenario();
        }
    }
    #endregion

    #region Lose Observer
    public void Add_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Add(observer);
    }

    public void Remove_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Remove(observer);
    }

    public void Notify_LoseObservers()
    {
        foreach (ILoseObserver observer in loseObservers.ToArray())
        {
            if (loseObservers.Contains(observer))
                observer.LoseScenario();
        }
    }
    #endregion
    #endregion
}
