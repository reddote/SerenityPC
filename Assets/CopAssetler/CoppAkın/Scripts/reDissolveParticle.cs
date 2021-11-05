using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reDissolveParticle : MonoBehaviour
{
    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;
    public GameObject afterDissolveGO;
    //ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;

    int shaderProperty;

    void Start ()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
      
    }
	
    void Update ()
    {
        
        SpawningWithEffect();
    }
    //ÇöpAkın klasöründeki reDissolveTurret prefabını kullanmalıyız bu efekt için.(hepsine implemente edilecek.)
    void SpawningWithEffect()
    {
        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            afterDissolveGO.SetActive(true);// Ters Dissolve gerçekleştikten sonra asıl turret objelerini aktifleştirip,
            this.gameObject.SetActive(false); // Efekt için oluşan turret objelerini kapatıyoruz.
        }
        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate( Mathf.InverseLerp(0, spawnEffectTime, timer)));
    }
}
