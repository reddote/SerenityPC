using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoutMovement : MonoBehaviour
{
    Quaternion toRotation;
    Vector3 relativePos;
    Vector3 pathTarget;
    public float speed=50;
    UnityEvent onTargetChange = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        pathTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, pathTarget, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, pathTarget) < 0.001f)
        {
            // Swap the position of the cylinder.
            pathTarget = getNewRandomPosAndSpeed();
        }

        relativePos = pathTarget - transform.position;
        toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        Ray ForwardRay = new Ray(transform.position, transform.forward * 10);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red, Time.deltaTime);

        Ray BackwardRay = new Ray(transform.position, -transform.forward * 10);
        Debug.DrawRay(transform.position, -transform.forward * 10, Color.red, Time.deltaTime);

        Ray RightRay = new Ray(transform.position, transform.right * 10);
        Debug.DrawRay(transform.position, transform.right * 10, Color.red, Time.deltaTime);

        Ray LeftRay = new Ray(transform.position, -transform.right * 10);
        Debug.DrawRay(transform.position, -transform.right * 10, Color.red, Time.deltaTime);

        Ray DownRay = new Ray(transform.position, -transform.right * 10);
        Debug.DrawRay(transform.position, -transform.up * 10, Color.red, Time.deltaTime);


        if (Physics.Raycast(ForwardRay,10))
        {
            pathTarget = AvoidHitForRandom(pathTarget.x, pathTarget.y, transform.position.z - 1);
        }
        if (Physics.Raycast(BackwardRay,10))
        {
            pathTarget = AvoidHitForRandom(pathTarget.x, pathTarget.y, transform.position.z + 1);
        }
        if (Physics.Raycast(RightRay,10))
        {
            pathTarget = AvoidHitForRandom(transform.position.x-1, pathTarget.y, pathTarget.z);
        }
        if (Physics.Raycast(LeftRay,10))
        {
            pathTarget = AvoidHitForRandom(transform.position.x + 1, pathTarget.y, pathTarget.z);
        }
        if (Physics.Raycast(DownRay,10))
        {
            pathTarget = AvoidHitForRandom(transform.position.x, pathTarget.y+1, pathTarget.z);
        }


    }


    Vector3 getNewRandomPosAndSpeed()
    {

        speed = Random.Range(10, 15);
        Vector3 pos = new Vector3(this.transform.position.x + Random.Range(-40, 40), Random.Range(50,70), this.transform.position.z + Random.Range(-40, 40));
        return pos;
    }

    Vector3 AvoidHitForRandom(float x , float y , float z)
    {
        
        Vector3 pos = new Vector3(x,y,z);
        return pos;
    }

   
}
