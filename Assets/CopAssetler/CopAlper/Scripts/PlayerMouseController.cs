using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{
    //mouseXInputName değiştirebildiğimiz tuş isimleri. Options Keybindings için kullanımı rahat olur.
    public string mouseXInputName, mouseYInputName;
    [SerializeField] private float playerMouseSensivity;
    //bu değişken kameranın sınırlarını belirlemek için kullanılıyor.
    private float yAxisClamp;
    
    private Transform player;

    private void Awake()
    {
        mouseXInputName = ButtonKeyController.mouseXInput;
        mouseYInputName = ButtonKeyController.mouseYInput;
        LockCursor();
        AwakeSettings();
    }

    void Start()
    {
        StartSettings();
    }

    
    void Update()
    {
        if (!PlayerController.stopMovement)
        {
            CameraRotation();
        }
    }

    //Fonksiyonların Başlangıcı...
    private void AwakeSettings()
    {
        yAxisClamp = 0.0f;
    }

    private void StartSettings()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.transform;
    }

    private void LockCursor()
    {
        //mouse ortaya kitliyorum. crosshair gibi şimdilik böyle kalcak sonra ince ayar çekilecek envanterde bırakılması gibi...
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation()
    {
        //mouse hareket ettirdiğimizdeki pozisyonları alıyoruz
        float mouseX = Input.GetAxis(mouseXInputName) * playerMouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * playerMouseSensivity * Time.deltaTime;

        yAxisClamp += mouseY;

        //kameramızı sınırlandırdık.
        if(yAxisClamp > 80.0f)
        {
            yAxisClamp = 80.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(280.0f);
        }
        else if (yAxisClamp < -60.0f)
        {
            yAxisClamp = -60.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(60.0f);
        }
        transform.Rotate(Vector3.left * mouseY);
        player.Rotate(Vector3.up * mouseX);
    }
    
    //x açısını 80 ve 75 değerine sabitlemek için kullanıyoruz. Bu sayede hiçbir şekilde o sınırı aşamayacak.
    private void ClampXAxisRotationToValue (float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
    //fonksiyonların bitişi...
}
