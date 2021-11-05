using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    public Shader ethanGray; // Highlight Shader'ı
    private float radius;
    public float border;
    private bool effectIsOn;
    private Renderer rend;
    private bool inCoroutine = false;
    public Camera cameratwo,cam;
    // ---------------------
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(!inCoroutine)
                StartCoroutine(HighlightAll());
        }
    }

    IEnumerator HighlightAll()
    {
        inCoroutine = true;
        yield return new WaitForSeconds(0.2f);
        cameratwo.depth = -1;// Player kamerası, Highlighted gözükmesi için depth -1 yapılıp efekt sonrası 0 olarak değiştiriyoruz.
        FindAll(this.transform.position,border); // Player etrafındaki bütün colliderları bulup(bordera göre), shaderlarını değiştiriyor.
        cam.SetReplacementShader(ethanGray, "XRay");// //
        inCoroutine = false;
    }

        
    void FindAll(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        var i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].GetComponent<Renderer>()!=null)
                hitColliders[i].GetComponent<Renderer>().material.shader = ethanGray;
            i++;
        }
    }
}
