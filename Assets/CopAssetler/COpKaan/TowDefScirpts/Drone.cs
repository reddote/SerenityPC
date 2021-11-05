    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Drone : MonoBehaviour
    {

        public float droneMovespeed;
        public float ySpeed;
        public Rigidbody droneRigid;
        private float xInput;
        private float yInput;
        private PlayerController playerController;
        public Camera droneCamera;



        // Start is called before the first frame update
        void Start()
        {

            setStartComponents();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayType.type == PlayType.playType.drone)
            {
                 droneMoveX();
                 droneMoveY();

                if (Input.GetKeyDown(KeyCode.L))
                {
                    playerController.toFpsGameplay();
                    droneCamera.gameObject.SetActive(false);
                }

            }
        

        }




        public void droneMoveX()
        {
            yInput = Input.GetAxis("Horizontal");
            xInput = Input.GetAxis("Vertical");
            
            droneRigid.AddRelativeForce(new Vector3(xInput*droneMovespeed, 0 , -yInput * droneMovespeed));
        }

        public void droneMoveY()
        {


        if (droneRigid.velocity.y > 0)
        {
            droneRigid.useGravity = true;
        }
        else
        {
            droneRigid.useGravity = false;
        }


        
        if (Input.GetKey(KeyCode.Space))
        {

            
            droneRigid.AddRelativeForce(new Vector3(0, ySpeed, 0));
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
           
            droneRigid.AddRelativeForce(new Vector3(0, -ySpeed, 0));
        }

    }

        void setStartComponents()
        {
            droneRigid = GetComponent <Rigidbody>();
            playerController = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
            

        }




    }
