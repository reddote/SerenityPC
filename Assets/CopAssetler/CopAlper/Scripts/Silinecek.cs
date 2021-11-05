using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silinecek : MonoBehaviour
{
    public Animator animator;
    public Transform hipsPos;
    public Transform reload;
    public Vector3 walkingPosValue;
    public Quaternion rotation;


    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = hipsPos.localPosition + walkingPosValue;
        
        if (animator.GetBool("Reload"))
        {
            this.transform.localRotation = reload.localRotation;
        }
        else
        {
            this.transform.localRotation = rotation;
        }
    }

}
