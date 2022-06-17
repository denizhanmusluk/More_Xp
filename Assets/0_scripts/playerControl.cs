using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [SerializeField] public List<GameObject> players;
    public float speed;

    public enum States { idle, move, death, attack }
    public States currentBehaviour;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    public bool pressed = false;
    float attackTimer = 0f;
    [SerializeField] Animator animator;
    Transform targetEnemy;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentBehaviour)
        {
            case States.idle:
                {
                }
                break;
            case States.move:
                {
                    if (Globals.isGameActive)
                    {
                        Control();
                    }
                }
                break;
            case States.death:
                {

                }
                break;
            case States.attack:
                {
                    attack();
                }
                break;
        }
    }
   public void playerAttack(Transform enemy)
    {
        targetEnemy = enemy;
        StartCoroutine(enemyKill());
        //pressed = false;

        //for (int i = 0; i < players.Count; i++)
        //{
        //    players[i].transform.GetChild(0).GetComponent<characterControl>().playerStop();
        //}

        animator.SetTrigger("attack");
        attackTimer = 0;
        //currentBehaviour = States.attack;
    }
    IEnumerator enemyKill()
    {
        yield return new WaitForSeconds(1f);
        targetEnemy.transform.GetComponent<Ragdoll>().RagdollActivate(true);
    }
    void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {

            firstPressPos = (Vector2)Input.mousePosition;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = false;

            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.GetChild(0).GetComponent<characterControl>().playerStop();
            }
        }

        attackTimer += Time.deltaTime;
        if(attackTimer >= 2f)
        {
            currentBehaviour = States.move;

        }
        for (int i = 0; i < players.Count; i++)
        {
            lookTarget(players[i].transform, targetEnemy);

        }
    }
    public void Control()
    {
        if (Input.GetMouseButtonDown(0))
        {

            firstPressPos = (Vector2)Input.mousePosition;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = false;

            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.GetChild(0).GetComponent<characterControl>().playerStop();
            }
        }

        if (pressed == true)
        {


            //foreach (var anim in GetComponentsInChildren<Animator>())
            //{
            //    anim.SetBool("walk", true);
            //}
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            currentSwipe.Normalize();


            Vector3 direction = new Vector3(currentSwipe.x, 0f, currentSwipe.y);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);

            if (direction != Vector3.zero)
            {
                /*   enemySpawner.rotation = Quaternion.RotateTowards(enemySpawner.rotation, newRot, 300 * Time.deltaTime); */
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].transform.rotation = Quaternion.RotateTowards(players[i].transform.rotation, newRot, 200 * Time.deltaTime);
                    players[i].transform.GetChild(0).GetComponent<characterControl>().playerMovingDirection(targetAngle - players[i].transform.eulerAngles.y);
                    //Debug.Log("players[i].transform.rotation =" + (targetAngle - players[i].transform.eulerAngles.y));
                    //Debug.Log("players[i].transform.rotation =" + players[i].transform.eulerAngles.y+      "   newrot = " + targetAngle);
                }
            }

            transform.position = transform.position + (direction * speed * Time.deltaTime);

        }
        else
        {
            //foreach (var anim in GetComponentsInChildren<Animator>())
            //{
            //    anim.SetBool("walk", false);
            //}
            /*
            for (int i = 0; i < players.Count; i++)
            {
                players[i].transform.GetChild(0).GetComponent<PlayerController>().playerStop();
            }
            */
        }
    }


    void lookTarget(Transform player, Transform target)
    {
        Debug.Log("rotttt");
        Vector3 relativeVector = player.transform.InverseTransformPoint(target.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 50;
        player.Rotate(0, newSteer * Time.deltaTime * 20, 0);
    }
}