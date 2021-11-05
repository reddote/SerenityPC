using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroneMouseController : MonoBehaviour
{

    //mouseXInputName değiştirebildiğimiz tuş isimleri. Options Keybindings için kullanımı rahat olur.
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float droneMouseSensivity;
    //bu değişken kameranın sınırlarını belirlemek için kullanılıyor.
    private float lerpCam;
    private float xAxisClamp;
    private float yAxisClamp;
    public Vector3 camTopPos;
    public Vector3 camBotPos;
    public Vector3 camBotRot;
    public Vector3 camTopRot;
    Vector3 droneBotRot;
    Vector3 droneTopRot;
    
    

    private Transform droneTransform;
    private Transform droneRootTransform;

    private void Awake()
    {
        LockCursor();
        AwakeSettings();
    }

    void Start()
    {
        StartSettings();
    }


    void Update()
    {
        //CameraRotation();
        cameraLerp();
    }

    //Fonksiyonların Başlangıcı...
    private void AwakeSettings()
    {
        xAxisClamp = 0.0f;
    }

    private void StartSettings()
    {
        GameObject droneGO = GameObject.Find("Drone");
        GameObject droneGOroot = GameObject.FindGameObjectWithTag("Drone");
        droneTransform = droneGO.transform;
        droneRootTransform = droneGOroot.transform;
        lerpCam = 0.5f;
       
    }

    private void LockCursor()
    {
        //mouse ortaya kitliyorum. crosshair gibi şimdilik böyle kalcak sonra ince ayar çekilecek envanterde bırakılması gibi...
        Cursor.lockState = CursorLockMode.Locked;
    }



    void cameraLerp()
    {
        float mouseY = Input.GetAxis(mouseYInputName) * droneMouseSensivity * Time.deltaTime;
        float mouseX = Input.GetAxis(mouseXInputName) * droneMouseSensivity * Time.deltaTime;

        //lerpCam -= mouseY / 100;
        //Mathf.Clamp(lerpCam, 0, 1);
        ////Mathf.Clamp(yAxisClamp, 11, 33);
        //if (lerpCam > 1)
        //{
        //    lerpCam = 1;
        //}
        //else if (lerpCam < 0)
        //{
        //    lerpCam = 0;
        //}

        //transform.localPosition = Vector3.Lerp(camBotPos, camTopPos, lerpCam); 
        //transform.localEulerAngles= Vector3.Lerp(camBotRot, camTopRot, lerpCam);

   

        droneRootTransform.Rotate(Vector3.up * mouseX);
     
        droneRootTransform.Rotate(Vector3.forward * mouseY);


        if (droneTransform.localEulerAngles.x > 320)
        {
            droneRootTransform.localEulerAngles = new Vector3(320, droneTransform.localEulerAngles.y, droneTransform.localEulerAngles.z);
        }



    }


    private void LateUpdate()
    {
        droneRootTransform.transform.eulerAngles = new Vector3(0, droneRootTransform.transform.eulerAngles.y, droneRootTransform.transform.eulerAngles.z);

    }








    private void CameraRotation()
    {
        //mouse hareket ettirdiğimizdeki pozisyonları alıyoruz
        float mouseX = Input.GetAxis(mouseXInputName) * droneMouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * droneMouseSensivity * Time.deltaTime;

        yAxisClamp += mouseY;
        

        //kameramızı sınırlandırdık.
        //if (yAxisClamp > 20.0f)
        //{
        //    yAxisClamp = 20.0f;
        //    mouseY = 0.0f;
        //    ClampXAxisRotationToValue(340.0f);
        //}
        //else if (yAxisClamp < -45.0f)
        //{
        //    yAxisClamp = -45.0f;
        //    mouseY = 0.0f;
        //    ClampXAxisRotationToValue(45.0f);
        //}

        transform.Rotate(Vector3.left * mouseY);
        droneTransform.Rotate(Vector3.up * mouseX);

    }

    //x açısını 80 ve 75 değerine sabitlemek için kullanıyoruz. Bu sayede hiçbir şekilde o sınırı aşamayacak.
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
    //fonksiyonların bitişi...


}

/*

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroneMouseController : MonoBehaviour
{

    //mouseXInputName değiştirebildiğimiz tuş isimleri. Options Keybindings için kullanımı rahat olur.
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float droneMouseSensivity;
    //bu değişken kameranın sınırlarını belirlemek için kullanılıyor.
    private float lerpCam;
    private float xAxisClamp;
    private float yAxisClamp;
    public Vector3 camTopPos;
    public Vector3 camBotPos;
    public Vector3 camBotRot;
    public Vector3 camTopRot;


    private Transform droneTransform;

    private void Awake()
    {
        LockCursor();
        AwakeSettings();
    }

    void Start()
    {
        StartSettings();
    }


    void Update()
    {
        CameraRotation();
        cameraLerp();
    }

    //Fonksiyonların Başlangıcı...
    private void AwakeSettings()
    {
        xAxisClamp = 0.0f;
    }

    private void StartSettings()
    {
        GameObject droneGO = GameObject.Find("Drone");
        droneTransform = droneGO.transform;
        lerpCam = 0.5f;

    }

    private void LockCursor()
    {
        //mouse ortaya kitliyorum. crosshair gibi şimdilik böyle kalcak sonra ince ayar çekilecek envanterde bırakılması gibi...
        Cursor.lockState = CursorLockMode.Locked;
    }



    void cameraLerp()
    {
        float mouseY = Input.GetAxis(mouseYInputName) * droneMouseSensivity * Time.deltaTime;
        float mouseX = Input.GetAxis(mouseXInputName) * droneMouseSensivity * Time.deltaTime;

        lerpCam -= mouseY / 100;
        Mathf.Clamp(lerpCam, 0, 1);
        Mathf.Clamp(yAxisClamp, 11, 33);
        if (lerpCam > 1)
        {
            lerpCam = 1;
        }
        else if (lerpCam < 0)
        {
            lerpCam = 0;
        }

        transform.localPosition = Vector3.Lerp(camBotPos, camTopPos, lerpCam); ;
        transform.localEulerAngles = Vector3.Lerp(camBotRot, camTopRot, lerpCam);

        //transform.Rotate(Vector3.left * mouseY);
        droneTransform.Rotate(Vector3.up * mouseY);
        droneTransform.Rotate(Vector3.forward * mouseX);



    }











    private void CameraRotation()
    {
        //mouse hareket ettirdiğimizdeki pozisyonları alıyoruz
        float mouseX = Input.GetAxis(mouseXInputName) * droneMouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * droneMouseSensivity * Time.deltaTime;

        yAxisClamp += mouseY;


        //kameramızı sınırlandırdık.
        if (yAxisClamp > 20.0f)
        {
            yAxisClamp = 20.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(340.0f);
        }
        else if (yAxisClamp < -45.0f)
        {
            yAxisClamp = -45.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(45.0f);
        }

        transform.Rotate(Vector3.left * mouseY);

        transform.Rotate(Vector3.up * mouseX);
        //droneTransform.Rotate(Vector3.up * mouseX);
        //droneTransform.Rotate(Vector3.forward * mouseY);

    }

    //x açısını 80 ve 75 değerine sabitlemek için kullanıyoruz. Bu sayede hiçbir şekilde o sınırı aşamayacak.
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
    //fonksiyonların bitişi...


}


    */