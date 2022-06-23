using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject loadedLevel;
    [SerializeField] List<GameObject> levels;
    //[SerializeField] public int LevelCount;
    public TextMeshProUGUI levelText;

    void Start()
    {
        Globals.LevelCount = levels.Count;
        if (levels.Count > 0)
        {
            levelLoad();
        }
    }
    public void levelLoad()
    {
        if (PlayerPrefs.GetInt("levelIndex") != 0)
        {
            Globals.currentLevel = PlayerPrefs.GetInt("levelIndex");
            levelText.text = Globals.currentLevel.ToString();
        }
        else
        {
            PlayerPrefs.SetInt("levelIndex", Globals.currentLevel);

        }
        //PlayerPrefs.SetInt("level", 0);
        if (PlayerPrefs.GetInt("level") != 0)
        {
            Debug.Log(PlayerPrefs.GetInt("level"));
            Globals.currentLevelIndex = PlayerPrefs.GetInt("level");

        }

        //LevelsPrefab = (GameObject)Instantiate(Resources.Load("Level" + Globals.currentLevelIndex.ToString()));
        loadedLevel = Instantiate(levels[Globals.currentLevelIndex], transform.position, Quaternion.identity);

        Debug.Log("level " + Globals.currentLevel);
        Debug.Log("LEVEL " + PlayerPrefs.GetInt("levelIndex"));
    }
}
