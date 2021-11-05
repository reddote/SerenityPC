using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    
    public float Health;
    public float jetPackSpeed;
    public float playerMoveSpeed;
    public float playerStamina;
    public float playerJumpPower;
    public float jetPackCoolDown;
    public float sprintSpeedMultiplier = 1.75f;
    private Rigidbody _rigidbody;

    public CameraShaker cameraShaker;

    private void Awake()
    {
        cameraShaker = GetComponentInChildren<CameraShaker>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public TextMeshProUGUI discoveryUI;


    public void takeDamage(float hitDamage)
    {
        Health -= hitDamage;
        CameraShakeInstance c = cameraShaker.ShakeOnce(hitDamage*1.5f,hitDamage*1.5f, 0,4);
        c.PositionInfluence = new Vector3(0.25f, 0.25f, 0.25f);
        c.RotationInfluence = new Vector3(1f, 1f, 1f);
        if (Health < 1)
        {
            Destroy(this.gameObject);
        }

    }

    public void Knockback(Vector3 direction ,float power)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(-direction*power,ForceMode.Force);
        _rigidbody.isKinematic = true;
    }
    

    public void Discover(string Discovery)
    {
        discoveryUI.text = Discovery;
        discoveryUI.gameObject.GetComponent<Animation>().Play();
    }


}
