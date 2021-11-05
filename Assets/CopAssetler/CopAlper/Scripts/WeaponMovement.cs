using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponMovement : MonoBehaviour
{

    public string mouseXInputName;
    public string mouseYInputName;
    

    [Space(10)]
    public float weaponSwayValue;
    public float weaponSwayMaxValue;
    public float weaponSwaySmoothValue;

    public float timer = 2f;
    public float tempTimer = 0f;

    private Vector3 weaponLocalPosition;
    private ButtonKeyController buttonKeyController;

    private void Awake()
    {
        AwakeSettings();
    }

    void Start()
    {
        StartSettings();
    }

    void Update()
    {
        UpdateSettings();
    }

    void AwakeSettings()
    {
        mouseXInputName = ButtonKeyController.mouseXInput;
        mouseYInputName = ButtonKeyController.mouseYInput;
    }

    void StartSettings()
    {
        weaponLocalPosition = transform.localPosition;
    }

    void UpdateSettings()
    {
        WeaponMouseMovement();
    }
    
    void WeaponMouseMovement()
    {
        //silahın ekran döndürüldüğünde hareketli olmasını sağlıyor
        float movementX = -Input.GetAxis(mouseXInputName) * weaponSwayValue;
        float movementY = -Input.GetAxis(mouseYInputName) * weaponSwayValue;
        //sağa sola çok hızlı döndürüldüğünde görüntüde bozukluk olmaması için sınırlandırıyoruz.
        movementX = Mathf.Clamp(movementX, -weaponSwayMaxValue, weaponSwayMaxValue);
        movementY = Mathf.Clamp(movementY, -weaponSwayMaxValue, weaponSwayMaxValue);

        Vector3 weaponLastPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, (weaponLastPosition + weaponLocalPosition), Time.deltaTime * weaponSwaySmoothValue);
    }

}
