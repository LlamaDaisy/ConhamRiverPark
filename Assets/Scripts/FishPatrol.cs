using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// NOT CURRENTLY USED IN PROJECT
/// </summary>
public class FishPatrol : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float speed;
    [SerializeField] float prox;
    int currentIndex;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveToCurrentPoint();
        UpdatePatrol();
    }

/*    void MoveToCurrentPoint()
    {
        Transform targetPoint = waypoints[currentIndex].transform;

        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.velocity = direction * speed;
    }*/


    void UpdatePatrol()
    {
        if(CloseToCurrentPoint())
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }

    bool CloseToCurrentPoint()
    {
        Transform targetPoint = waypoints[currentIndex].transform;
        return Vector2.Distance(transform.position, targetPoint.position) > prox;
    }
}
