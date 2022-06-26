using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillManager : MonoBehaviour
{
    public static skillManager Instance;

    [SerializeField] Image bashImage, stompImage, spinImage, meteorImage, tornadoImage, assassinImage;
    [SerializeField] GameObject bashIconEffect, stompIconEffect, spinIconEffect, meteorIconEffect, tornadoIconEffect, assassinIconEffect;
    bool bash = false, stomp = false, spin = false, meteor = false, tornado = false, assassin = false;
    public playerBehaviour _playerBehaviour;
  [SerializeField]  List<int> skilId;
    int skillSelectNo;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        //bashImage.fillAmount = 0;
        //stompImage.fillAmount = 0;
        //spinImage.fillAmount = 0;
        //meteorImage.fillAmount = 0;
        //tornadoImage.fillAmount = 0;
        //assassinImage.fillAmount = 0;
        //bashCooldown();
        //spinCooldown();
        //stompCooldown();
        //meteorCooldown();
        //tornadoCooldown();
        //assassinCooldown();

    }

    public void bashCooldown()
    {
        bashIconEffect.SetActive(false);
        StartCoroutine(_bashCooldown());
    }
    IEnumerator _bashCooldown()
    {
        bash = false;
        float counter = 0f;
        while(counter < Globals.bashCooldown)
        {
            counter += Time.deltaTime;
            bashImage.fillAmount = counter / Globals.bashCooldown;
            yield return null;
        }
        bash = true;
        skillSelect();
        bashIconEffect.SetActive(true);
    }


    public void spinCooldown()
    {
        spinIconEffect.SetActive(false);
        StartCoroutine(_spinCooldown());
    }
    IEnumerator _spinCooldown()
    {
        spin = false;
        float counter = 0f;
        while (counter < Globals.spinCooldown)
        {
            counter += Time.deltaTime;
            spinImage.fillAmount = counter / Globals.spinCooldown;
            yield return null;
        }
        spin = true;
        skillSelect();
        spinIconEffect.SetActive(true);
    }


    public void stompCooldown()
    {
        stompIconEffect.SetActive(false);
        StartCoroutine(_stompCooldown());
    }
    IEnumerator _stompCooldown()
    {
        stomp = false;
        float counter = 0f;
        while (counter < Globals.stompCooldown)
        {
            counter += Time.deltaTime;
            stompImage.fillAmount = counter / Globals.stompCooldown;
            yield return null;
        }
        stomp = true;
        skillSelect();
        stompIconEffect.SetActive(true);
    }


    public void meteorCooldown()
    {
        meteorIconEffect.SetActive(false);
        StartCoroutine(_meteorCooldown());
    }
    IEnumerator _meteorCooldown()
    {
        meteor = false;
        float counter = 0f;
        while (counter < Globals.meteorCooldown)
        {
            counter += Time.deltaTime;
            meteorImage.fillAmount = counter / Globals.meteorCooldown;
            yield return null;
        }
        meteor = true;
        skillSelect();
        meteorIconEffect.SetActive(true);
    }


    public void tornadoCooldown()
    {
        tornadoIconEffect.SetActive(false);
        StartCoroutine(_tornadoCooldown());
    }
    IEnumerator _tornadoCooldown()
    {
        tornado = false;
        float counter = 0f;
        while (counter < Globals.tornadoCooldown)
        {
            counter += Time.deltaTime;
            tornadoImage.fillAmount = counter / Globals.tornadoCooldown;
            yield return null;
        }
        tornado = true;
        skillSelect();
        tornadoIconEffect.SetActive(true);
    }



    public void assassinCooldown()
    {
        assassinIconEffect.SetActive(false);
        StartCoroutine(_assassinCooldown());
    }
    IEnumerator _assassinCooldown()
    {
        assassin = false;
        float counter = 0f;
        while (counter < Globals.assassinCooldown)
        {
            counter += Time.deltaTime;
            assassinImage.fillAmount = counter / Globals.assassinCooldown;
            yield return null;
        }
        assassin = true;
        skillSelect();
        assassinIconEffect.SetActive(true);
    }


    public void skillSelect()
    {
        Debug.Log("skill select");
        skillSelectNo = 0;
        if (skilId.Count != 0)
        skilId.Clear();
        if (bash)
        {
            skilId.Add(1);
        }
        if (stomp)
        {
            skilId.Add(2);

        }
        if (spin)
        {
            skilId.Add(3);

        }
        if (meteor)
        {
            skilId.Add(4);

        }
        if (tornado)
        {
            skilId.Add(5);

        }
        if (assassin)
        {
            skilId.Add(6);

        }


            skillSelectNo = Random.Range(0, skilId.Count);
        if (skilId.Count == 0)
        {
            Debug.Log("normal  ");

            _playerBehaviour.currentAttack = playerBehaviour.States.normalAttack;
        }
        else
        {
            if (skilId[skillSelectNo] == 1)
            {
                Debug.Log("stomp  " + skilId[skillSelectNo]);

                _playerBehaviour.currentAttack = playerBehaviour.States.bash;
            }
            if (skilId[skillSelectNo] == 2)
            {
                Debug.Log("bash  " + skilId[skillSelectNo]);

                _playerBehaviour.currentAttack = playerBehaviour.States.stomp;
            }
            if (skilId[skillSelectNo] == 3)
            {
                Debug.Log("spin  " + skilId[skillSelectNo]);

                _playerBehaviour.currentAttack = playerBehaviour.States.spin;
            }
            if (skilId[skillSelectNo] == 4)
            {
                Debug.Log("meteor  " + skilId[skillSelectNo]);

                _playerBehaviour.currentAttack = playerBehaviour.States.meteor;
            }
            if (skilId[skillSelectNo] == 5)
            {
                Debug.Log("tornado  " + skilId[skillSelectNo]);

                _playerBehaviour.currentAttack = playerBehaviour.States.tornado;
            }
            if (skilId[skillSelectNo] == 6)
            {
                Debug.Log("assassin  " + skilId[skillSelectNo]);

                _playerBehaviour.currentAttack = playerBehaviour.States.assassin;
            }
        }

     


        //if (bash)
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.bash;
        //}
        //else if (stomp)
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.stomp;
        //}
        //else if (spin)
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.spin;
        //}
        //else if (meteor)
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.meteor;
        //}
        //else if (tornado)
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.tornado;
        //}
        //else if (assassin)
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.assassin;
        //}
        //else
        //{
        //    _playerBehaviour.currentAttack = playerBehaviour.States.normalAttack;
        //}
    }
}
