using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementTower : MonoBehaviour
{

    public float CamSpeed; 
    Camera cam;
    float mouseY;
    float mouseX;
    [SerializeField] private float playerMouseSensivity;
    // Start is called before the first frame update
    void Start()
    {

        setComponents();
    }

    // Update is called once per frame
    void Update()
    {
        moveCam();
    }



    void setComponents()
    {

        cam = GetComponent<Camera>();
        

    }


    void moveCam()
    {
        mouseX = Input.GetAxis("Mouse X") * playerMouseSensivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * playerMouseSensivity * Time.deltaTime;


        //Sağ clicke basıldıysa kamera hareketine izin ver
        if (Input.GetMouseButton(1))
        {

            
            transform.Translate(Vector3.forward*Input.GetAxis("Vertical")*CamSpeed, Space.Self);

            transform.Translate(Vector3.right*Input.GetAxis("Horizontal") * CamSpeed, Space.Self);

            transform.Rotate(Vector3.left * mouseY);
            transform.Rotate(Vector3.up * mouseX);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,    0);

        }


        

    }




}
