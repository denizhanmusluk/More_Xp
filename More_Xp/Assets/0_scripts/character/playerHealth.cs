using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Cinemachine;
public class playerHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;
    [SerializeField] Slider healthBar;
    bool fillActive = false;
    [SerializeField] float coolDownSpeed;
    [SerializeField] Image sliderImage;
    bool cooldownActive = true;
    [SerializeField] Ragdoll _ragdoll;
    bool playerAlive = true;
    [SerializeField] CinemachineVirtualCamera cam;
    void Start()
    {
        health = maxHealth;
        healthBar.value = health;
        StartCoroutine(characterCountCooldown());
        sliderImage.color = new Color32(0, 255, 0, 255);
    }

    // Update is called once per frame
    public void characterDamage(int damage)
    {
        StartCoroutine(_coolDownFill(-damage, 1f));
        Debug.Log("damage");
        if(health < 2)
        {
            GameManager.Instance.Notify_LoseObservers();
            playerAlive = false;
            _ragdoll.RagdollActivateWithForce(true, new Vector3(0,1,0));
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Collider>().enabled = false;
            healthBar.gameObject.SetActive(false);
            cam.Follow = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform;
            cam.LookAt = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform;
        }
    }
    IEnumerator characterCountCooldown()
    {
        while (playerAlive)
        {
            if (health < maxHealth && cooldownActive)
            {
                StartCoroutine(_coolDownFill(1, 1f / coolDownSpeed));
            }
            yield return new WaitForSeconds(1f / coolDownSpeed);
        }
    }

    IEnumerator _coolDownFill(int miktar, float cooldownSpeed)
    {
        fillActive = false;
        yield return null;
        fillActive = true;
        float fillOld = (float)health;
        health = health + miktar;
        if (miktar > 0)
        {
            while (fillOld < health && fillActive)
            {
                fillOld += cooldownSpeed * Time.deltaTime;

                healthBar.value = (float)fillOld / (float)maxHealth;
                float G = 128 * healthBar.value + 127;
                float R = 128 - 128 * healthBar.value + 127;
                sliderImage.color = new Color32((byte)R, (byte)G, 0, 255);

                yield return null;
            }
            healthBar.value = (float)health / (float)maxHealth;
        }
        else
        {
            cooldownActive = false;
            while (fillOld > health && fillActive)
            {
                fillOld -= cooldownSpeed * Time.deltaTime;

                healthBar.value = (float)fillOld / (float)maxHealth;
                float G = 128 * healthBar.value + 127;
                float R = 128 - 128 * healthBar.value + 127;
                sliderImage.color = new Color32((byte)R, (byte)G, 0, 255);
                yield return null;
            }
            healthBar.value = (float)health / (float)maxHealth;
            cooldownActive = true;
        }

    }
    public void characterHealthUp(int heal)
    {
        if (health < maxHealth)
        {
            StartCoroutine(_coolDownFill(heal, 1f));
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<coin>() != null)
        {
            other.GetComponent<coin>().collect(transform);
        }
    }
}
