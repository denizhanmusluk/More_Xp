using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    Animator animator;
    public playerBehaviour _playerBehaviour;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
   public void attackAnim()
    {
        _playerBehaviour.attackFunction();
    }

    public void playerStop()
    {
        float firstAngleX = Mathf.Cos((transform.localEulerAngles.y + 90) * Mathf.Deg2Rad);
        float firstAngleY = Mathf.Sin((transform.localEulerAngles.y + 90) * Mathf.Deg2Rad);
        LeanTween.value(firstAngleX, 0, 0.2f).setOnUpdate((float val) =>
        {
            if (animator != null)
                animator.SetFloat("x", val);
        });
        LeanTween.value(firstAngleY, 0, 0.2f).setOnUpdate((float val) =>
        {
            if (animator != null)
                animator.SetFloat("y", val);
        });
    }

    public void playerMovingDirection(float angle)
    {
        //animator.SetFloat("x", Mathf.Cos((transform.localEulerAngles.y + 90) * Mathf.Deg2Rad));
        //animator.SetFloat("y", Mathf.Sin((transform.localEulerAngles.y + 90) * Mathf.Deg2Rad));

        animator.SetFloat("x", Mathf.Cos((angle + 90) * Mathf.Deg2Rad));
        animator.SetFloat("y", Mathf.Sin((angle + 90) * Mathf.Deg2Rad));
    }
}
