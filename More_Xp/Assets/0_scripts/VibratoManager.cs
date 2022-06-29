using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapticPlugin;

public class VibratoManager : MonoBehaviour
{
    public static VibratoManager Instance;
    bool lightVibratoActive = true;
    bool mediumVibratoActive = true;
    bool heavyVibratoActive = true;
    [SerializeField] float frequency;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Update is called once per frame
    public void LightViration()
    {
        if (lightVibratoActive)
        {
            TapticManager.Impact(ImpactFeedback.Light);
            Debug.Log("LightViration");
            StartCoroutine(lightVibratoActivator());
        }
    }
    public void MediumViration()
    {
        if (mediumVibratoActive)
        {
            TapticManager.Impact(ImpactFeedback.Medium);
            Debug.Log("MediumViration");
            StartCoroutine(mediumVibratoActivator());
        }
    }
    public void HeavyViration()
    {
        if (heavyVibratoActive)
        {
            TapticManager.Impact(ImpactFeedback.Heavy);
            Debug.Log("HeavyViration");
            StartCoroutine(heavyVibratoActivator());
        }
    }
    IEnumerator lightVibratoActivator()
    {
        if (lightVibratoActive)
        {
            lightVibratoActive = false;
            yield return new WaitForSeconds(frequency);
            lightVibratoActive = true;
        }
    }
    IEnumerator mediumVibratoActivator()
    {
        if (mediumVibratoActive)
        {
            mediumVibratoActive = false;
            yield return new WaitForSeconds(frequency);
            mediumVibratoActive = true;
        }
    }
    IEnumerator heavyVibratoActivator()
    {
        if (heavyVibratoActive)
        {
            heavyVibratoActive = false;
            yield return new WaitForSeconds(frequency);
            heavyVibratoActive = true;
        }
    }
}