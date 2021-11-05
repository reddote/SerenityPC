using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public enum DayInfo {Dawn,Noon,Evening,Night}
    public DayInfo dayInfo;
    Light lt;
    public static bool sunny;
    public float rotatinSpeed = 2f;
    bool coroutine = false;
    public Material MarsSky;
    private bool dawn = false;
    public float intensityChangeValue = 0.02f;
    public float exposure;
    //public var exposure = 0;
    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
        var standartExposure = 0.40f;
        RenderSettings.skybox.SetFloat("_Exposure", standartExposure);
        exposure = RenderSettings.skybox.GetFloat("_Exposure");
        
    }
    private IEnumerator intensityIncreaser()
    {
        coroutine = true;
        
        exposure = RenderSettings.skybox.GetFloat("_Exposure");
        exposure += 0.005f;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        
        lt.intensity += intensityChangeValue;
        yield return new WaitForSeconds(0.2f);
        coroutine = false;
    }
    private IEnumerator intensityDecreaser()
    {
        coroutine = true;
        
        exposure = RenderSettings.skybox.GetFloat("_Exposure");
        exposure -= 0.005f;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        
        lt.intensity -= intensityChangeValue/2;
        yield return new WaitForSeconds(0.2f);
        coroutine = false;
    }
    void Update()
    {
        
        transform.RotateAround(Vector3.zero, Vector3.right, rotatinSpeed * Time.deltaTime);
        
        transform.LookAt(Vector3.zero);
        if (gameObject.transform.rotation.eulerAngles.x < 90 && gameObject.transform.rotation.eulerAngles.x > 0)
        {
            if (gameObject.transform.rotation.eulerAngles.y > 90f) // Batarken
            {
                if (!coroutine&&lt.intensity>0.1f&& exposure>0.15f)
                    StartCoroutine(intensityDecreaser());
            }else if (gameObject.transform.rotation.eulerAngles.y < 90f) // Doğarken
            {
                if (!coroutine&&lt.intensity<1.5f && exposure<1 )
                    StartCoroutine(intensityIncreaser());
            }
        }
        else
        {
            if (!coroutine&&lt.intensity>0 && exposure>0.15f)
                StartCoroutine(intensityDecreaser());
        }
                            
    }
    IEnumerator rotateBooster()
    {
        coroutine = false;
        rotatinSpeed = 140f;
        yield return new WaitUntil(()=> sunny == true);
        TowDefTimer.sunSetting = false;
        rotatinSpeed = 3f;
        coroutine = true;
    }
}
