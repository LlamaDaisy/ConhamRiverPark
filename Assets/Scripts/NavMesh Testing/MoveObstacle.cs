using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] float xSpeed, ySpeed, zSpeed, maxDistance;

    Vector3 startingPos;

    bool isReversing;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    void Reverse()
    {
        xSpeed *= -1;
        ySpeed *= -1;
        zSpeed *= -1;
        isReversing = !isReversing;
    }
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(startingPos, transform.position) >= maxDistance) 
        {
            Reverse();
        }
        else if(isReversing == true && transform.position == startingPos) 
        {
            Reverse();
        }    
            transform.position = transform.position + new Vector3(xSpeed,ySpeed,zSpeed);      
    }
}
