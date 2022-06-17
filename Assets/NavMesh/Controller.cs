using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    [SerializeField] Transform destination;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            agent.SetDestination(destination.position);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
