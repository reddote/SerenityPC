using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class XRayReplacement : MonoBehaviour
{
    public Shader DroneShader;
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void OnEnable()
    {
       
    }

    private void Update()
    {
        
       
            
           camera.SetReplacementShader(DroneShader, "XRay");
           //camera.clearFlags = CameraClearFlags.Skybox;
           //camera.clearFlags = CameraClearFlags.Nothing;
        
        
//        if (Input.GetButton("Fire2"))
//        {
//            
//            GetComponent<Camera>().SetReplacementShader(DroneShader, "XRay");
//            GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
//            GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
//        }
//        else
//        {
//            GetComponent<Camera>().ResetReplacementShader();
//        }
    }

}