using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogColorSet : MonoBehaviour
{
    BoxCollider boxCol;
    [SerializeField] float frontR, frontG, frontB;
    [SerializeField] float backR, backG, backB;
    [SerializeField] Camera cam;
    void Start()
    {
         boxCol = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(setCollider(other.transform));
            boxCol.enabled = false;
        }
        if (other.transform.GetComponent<enemy>() != null)
        {
            other.GetComponent<enemy>().idleMove = true;
            other.GetComponent<enemy>().idleEnum();
            other.GetComponent<enemy>().player.transform.parent.GetComponent<playerBehaviour>().enemies.Remove(other.gameObject);

        }
    }
    // Update is called once per frame
    IEnumerator setCollider(Transform player)
    {

        yield return new WaitForSeconds(1f);
        if(player.position.z > transform.position.z)
        {
            frontfogColorR();
        }
        else
        {
            backfogColorR();
        }

        boxCol.enabled = true;

    }
    public void frontfogColorR()
    {
        LeanTween.value(RenderSettings.fogColor.r, frontR, 2f).setOnUpdate((float val) =>
        {
            RenderSettings.fogColor = new Color( val, RenderSettings.fogColor.g, RenderSettings.fogColor.b);
            cam.backgroundColor = new Color( val, RenderSettings.fogColor.g, RenderSettings.fogColor.b);
        });
        LeanTween.value(RenderSettings.fogColor.g, frontG, 2f).setOnUpdate((float val) =>
        {
            RenderSettings.fogColor = new Color( RenderSettings.fogColor.r, val, RenderSettings.fogColor.b);
            cam.backgroundColor = new Color( RenderSettings.fogColor.r, val, RenderSettings.fogColor.b);
        });
        LeanTween.value(RenderSettings.fogColor.g, frontB, 2f).setOnUpdate((float val) =>
        {
            RenderSettings.fogColor = new Color(RenderSettings.fogColor.r, RenderSettings.fogColor.g, val);
            cam.backgroundColor = new Color(RenderSettings.fogColor.r, RenderSettings.fogColor.g, val);
        });
    }
    public void backfogColorR()
    {
        LeanTween.value(RenderSettings.fogColor.r, backR, 2f).setOnUpdate((float val) =>
        {
            RenderSettings.fogColor = new Color(val, RenderSettings.fogColor.g, RenderSettings.fogColor.b);
            cam.backgroundColor = new Color(val, RenderSettings.fogColor.g, RenderSettings.fogColor.b);
        });
        LeanTween.value(RenderSettings.fogColor.g, backG, 2f).setOnUpdate((float val) =>
        {
            RenderSettings.fogColor = new Color(RenderSettings.fogColor.r, val, RenderSettings.fogColor.b);
            cam.backgroundColor = new Color(RenderSettings.fogColor.r, val, RenderSettings.fogColor.b);
        });
        LeanTween.value(RenderSettings.fogColor.g, backB, 2f).setOnUpdate((float val) =>
        {
            RenderSettings.fogColor = new Color(RenderSettings.fogColor.r, RenderSettings.fogColor.g, val);
            cam.backgroundColor = new Color(RenderSettings.fogColor.r, RenderSettings.fogColor.g, val);
        });
    }


}
