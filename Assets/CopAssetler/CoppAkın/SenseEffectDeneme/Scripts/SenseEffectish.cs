using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FI.PP
{
    [ExecuteInEditMode]
    public class SenseEffectish : MonoBehaviour
    {
        private Camera cam;

        [NonSerialized]
        private Material worldMaterial;

        [NonSerialized]
        private Vector3[] frustumCorners;

        [NonSerialized]
        private Vector4[] vectorArray;

        [SerializeField]
        private float speed = 50f, multiplierFactor = 1.01f;

        [SerializeField]
        private Color inRadiusColor = Color.green;

        
        private float radius;
        public float border;
        private bool effectIsOn;
        private Renderer rend;
        private bool inCoroutine = false;

        
        // ---------------------
        private void Start()
        {
            cam = this.GetComponent<Camera>();
            
        }

        // ----------------------------------
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                effectIsOn = !effectIsOn;// Sense Effect açık/kapalı
                
            }
            UpdateRadiusEffect();
        }

        // ----------------------------------------------------------
        private void UpdateRadiusEffect()
        {
            if (!cam)
                cam = this.GetComponent<Camera>();

            if (effectIsOn)
            {
                if (!inCoroutine)
                {
                    radius += Time.deltaTime * speed;
                    radius = radius > cam.farClipPlane / 4f ?
                        radius * multiplierFactor : radius;
                    if (radius >= border)
                        StartCoroutine(FieldTime());
                }
                
            }
            else
            {
                radius -= Time.deltaTime * speed;
                radius = radius > cam.farClipPlane / 4f ?
                    radius / multiplierFactor : radius;
                if (radius < 5)
                    cam.depth = 0;

            }

            radius = Mathf.Clamp(radius, 0, cam.farClipPlane * 4f);
        }

        IEnumerator FieldTime()
        {
            inCoroutine = true;
            yield return new WaitForSeconds(2f);
            effectIsOn = !effectIsOn;
            ResetAll(this.transform.position,border); // Tüm Highlighted Shader'ı olan GameObject'lerin shaderını standart shadera dönüştürüyor.
            inCoroutine = false;
        }

        // ------------------------------------------------------------------------
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!worldMaterial)
            {
                vectorArray = new Vector4[4];
                frustumCorners = new Vector3[4];

                worldMaterial = new Material(Shader.Find("FI/WorldPosInFragment"));
                worldMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            UpdateFrustumCorners();
            UpdateMaterialProperties();

            Graphics.Blit(source, destination, worldMaterial);
        }

        // -------------------------------------
        private void UpdateFrustumCorners()
        {
            if (!cam)
                cam = Camera.main;

            cam.CalculateFrustumCorners(
                   new Rect(0f, 0f, 1f, 1f),
                   cam.farClipPlane,
                   cam.stereoActiveEye,
                   frustumCorners);

            vectorArray[0] = frustumCorners[0];
            vectorArray[1] = frustumCorners[3];
            vectorArray[2] = frustumCorners[1];
            vectorArray[3] = frustumCorners[2];
        }

        // ---------------------------------------------------------------
        private void UpdateMaterialProperties()
        {
            worldMaterial.SetFloat("_Radius", radius);
            worldMaterial.SetColor("_InRadiusColor", inRadiusColor);
            worldMaterial.SetVectorArray("_FrustumCorners", vectorArray);
        }
        void ResetAll(Vector3 center, float radius)
            {
                Collider[] hitColliders = Physics.OverlapSphere(center, radius);
                var i = 0;
                while (i < hitColliders.Length)
                {
                    if(hitColliders[i].GetComponent<Renderer>()!=null)
                        hitColliders[i].GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    i++;
                    
                }
            }
    }

}