using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float runStepLength = 0.4f;
        public float walkSpeed = 2f;
        public float runSpeed = 4f;
        public float jumpSpeed = 10f;
        public float stickToGroundForce = 10f;
        public float gravityMultiplier = 2f;
        public bool isWalking;
        public bool useFovKick = true;
        public bool useHeadBob = true;
        public float stepInterval = 5f;
        public MouseLook mouseLook;
        public CurveControlledBob headBob = new CurveControlledBob();
        public FOVKick fovKick = new FOVKick();
        public LerpControlledBob jumpBob = new LerpControlledBob();
        public Animator animator;
        public bool jump;
        public float yRotation;
        public Vector2 input;
        public Vector3 moveDir = Vector3.zero;
        public CollisionFlags collisionFlags;
        public bool previouslyGrounded;
        public float stepCycle;
        public float nextStep;
        public bool jumping;
        
        CharacterController characterController;
        Camera cameraTarget;
        Vector3 originalCameraPosition;

        public static bool IsWalkingBackward;
        public static bool IsWalkingForward;
     
        public AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        public AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        public AudioClip m_LandSound;           // the sound played when character touches back on ground.
        public AudioSource m_AudioSource;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int IsRun = Animator.StringToHash("IsRun");

        public void Start()
        {
            characterController = GetComponent<CharacterController>();
            cameraTarget = GetComponentInChildren<Camera>();
            originalCameraPosition = cameraTarget.transform.localPosition;
            fovKick.Setup(cameraTarget);
            headBob.Setup(cameraTarget, stepInterval);
            stepCycle = 0f;
            nextStep = stepCycle/2f;
            jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			mouseLook.Init(transform , cameraTarget.transform);
        }

        public void Update()
        {
            RotateView();
            
            // the jump state needs to read here to make sure it is not missed
            if (!jump)
            {
                jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (!previouslyGrounded && characterController.isGrounded)
            {
                StartCoroutine(jumpBob.DoBobCycle());
                PlayLandingSound();
                moveDir.y = 0f;
                jumping = false;
            }
            if (!characterController.isGrounded && !jumping && previouslyGrounded)
            {
                moveDir.y = 0f;
            }

            IsWalkingBackward = CrossPlatformInputManager.GetAxis("Vertical") < 0;
            IsWalkingForward = CrossPlatformInputManager.GetAxis("Vertical") > 0;
            
            //animasyon çalışma conditionları burda bulunmakta
            if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 ||
                CrossPlatformInputManager.GetAxis("Vertical") != 0)
            {
               
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetBool(Walk, false);
                    animator.SetBool(IsRun, true);
                }
                else
                {
                    animator.SetBool(Walk, true);
                    animator.SetBool(IsRun, false);
                }
            }
            else
            {
                animator.SetBool(Walk, false);
                animator.SetBool(IsRun, false);
            }

            previouslyGrounded = characterController.isGrounded;
        }


        public void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            nextStep = stepCycle + .5f;
        }


        public void FixedUpdate()
        {
            var speed = GetInput();

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*input.y + transform.right*input.x;

            // get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out var hitInfo,
                               characterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            if (!jumping)
            {
                moveDir.x = desiredMove.x*speed;
                moveDir.z = desiredMove.z*speed;
            }

            if (characterController.isGrounded)
            {
                moveDir.y = -stickToGroundForce;

                if (jump)
                {
                    if (input.x>0)
                    {
                        moveDir.z = walkSpeed*desiredMove.z;
                    }
                    else if (input.x<0)
                    {
                        moveDir.z = walkSpeed*desiredMove.z;
                    }
                    if (input.y<0)
                    {
                        moveDir.x = walkSpeed*desiredMove.x;
                    }
                    else if (input.y>0)
                    {
                        moveDir.x = walkSpeed* desiredMove.x;
                    }
                    moveDir.y = jumpSpeed;
                    PlayJumpSound();
                    jump = false;
                    jumping = true;
                }
            }
            else
            {
                moveDir += Time.fixedDeltaTime*gravityMultiplier*Physics.gravity;
            }
            collisionFlags = characterController.Move(moveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            mouseLook.UpdateCursorLock();
            //m_MouseLook.CamRecoilBackToDefaultRot(m_Camera.transform);
        }


        public void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        public void ProgressStepCycle(float speed)
        {
            if (characterController.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0))
            {
                stepCycle += (characterController.velocity.magnitude + (speed*(isWalking ? 1f : runStepLength)))*
                             Time.fixedDeltaTime;
            }

            if (!(stepCycle > nextStep))
            {
                return;
            }

            nextStep = stepCycle + stepInterval;

            PlayFootStepAudio();
        }


        public void PlayFootStepAudio()
        {
            if (!characterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        public void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!useHeadBob)
            {
                return;
            }
            if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
            {
                cameraTarget.transform.localPosition =
                    headBob.DoHeadBob(characterController.velocity.magnitude +
                                      (speed*(isWalking ? 1f : runStepLength)));
                newCameraPosition = cameraTarget.transform.localPosition;
                newCameraPosition.y = cameraTarget.transform.localPosition.y - jumpBob.Offset();
            }
            else
            {
                newCameraPosition = cameraTarget.transform.localPosition;
                newCameraPosition.y = originalCameraPosition.y - jumpBob.Offset();
            }
            cameraTarget.transform.localPosition = newCameraPosition;
        }


        public float GetInput()
        {
           
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = isWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            isWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            float speed = isWalking ? walkSpeed : runSpeed;
            input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (input.sqrMagnitude > 1)
            {
                input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (isWalking != waswalking && useFovKick && characterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!isWalking ? fovKick.FOVKickUp() : fovKick.FOVKickDown());
            }

            return speed;
        }


        public void RotateView()
        {
            mouseLook.LookRotation (transform, cameraTarget.transform);
        }


        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (collisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(characterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
