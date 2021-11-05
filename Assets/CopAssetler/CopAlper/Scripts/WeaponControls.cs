using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

public class WeaponControls : MonoBehaviour
{
    [Header("Weapon Control Variables")]
    public float weaponRecoil;
    public float weaponSway;
    
    [Header("Camera Control Variables")]
    public float camRecoilSmoothness;

    [Header("GameObjects Variables")] 
    public GameObject sandwichCamGO;
    public GameObject bulletSpawnPoint;

    private Quaternion _targetRecoilRot;
    private Quaternion _defaultRecoilRot;
    private bool notShooting;


    private void Awake()
    {
       _defaultRecoilRot = Quaternion.Euler(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (!notShooting)
        {
            sandwichCamGO.transform.localRotation = Quaternion.Slerp(sandwichCamGO.transform.localRotation, _targetRecoilRot, camRecoilSmoothness*Time.fixedDeltaTime);
        }
        if (notShooting)
        {
            sandwichCamGO.transform.localRotation = Quaternion.Slerp(sandwichCamGO.transform.localRotation, _defaultRecoilRot, camRecoilSmoothness*Time.fixedDeltaTime);
        }
       
    }

    public void CameraRecoil(float camUpwardRecoil, float camSideRecoil)
    {
        //playerCamera.m_MouseLook.recoilRotSmoothness = camRecoilSmoothness; 
        float side = Random.Range(-camSideRecoil, camSideRecoil);
        //playerCamera.m_MouseLook.AddCamRecoil(camUpwardRecoil, side);
        _targetRecoilRot = Quaternion.Euler(-camUpwardRecoil,side,0);
    }

    void BulletRecoil(bool recCheck)
    {
        if (recCheck)
        {
            weaponRecoil *= 0.3f;
            
        }
        else
        {
            weaponRecoil *= 0.3f;

        }
        var spawnRot = bulletSpawnPoint.transform;
        var randomRangeX = Random.Range(-weaponRecoil, weaponRecoil);
        var randomRangeY = Random.Range(-weaponSway, weaponSway);
        spawnRot.Rotate(randomRangeX,randomRangeY,0);
    }

    public void Fire(bool fireCheck, bool fireBurst)
    {
        if (fireCheck)
        {
            BulletRecoil(fireBurst);
            notShooting = false;
        }
        if (!fireCheck)
        {
            bulletSpawnPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
            notShooting = true;
        }
    }
    
    
}
