using UnityEngine;


public class Weapon : MonoBehaviour
{
    [System.NonSerialized]public float lastFireTime;
    public GameObject bulletSpawnPointGO;
    public Camera fpsCamera;
    ButtonKeyController buttonController;
    public string fireButton;
    public int clipSize;
    public int bulletLimit;
    public float weaponDamage;
    public float reloadTimer;
    [Header("ITEM ID"), Space(5)] 
    [Space(10)]
    public Animator animator;

    public void Awake()
    {
        fireButton = ButtonKeyController.fireButtonInputName;
        bulletSpawnPointGO = transform.root.GetChild(0).GetChild(0).GetChild(1).gameObject;
        animator.SetBool("Reload", false);
    }

    public void Start()
    {
        fpsCamera = GetComponentInParent<Camera>();
    }
    public virtual void Shoot()
    {
        return;
    }

    public void Reload(bool set)
    {
        //animator.SetBool("Reload", set);
    }

}
