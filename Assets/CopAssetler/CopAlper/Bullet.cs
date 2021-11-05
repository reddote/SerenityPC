using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : Item
{
    public float bulletHoleX = 9f;
    public float bulletHoleY = 9f;
    public float bulletHoleZ = 9f;
    public GameObject bulletHole;
    public Weapon whichWeapon;
    //public UnityEvent onBulletCollision= new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        var monster = collision.gameObject.GetComponent<TowDefMonster>();

        if (monster != null)
        {
            //Debug.Log(whichWeapon.weaponDamage);
            if (collision.collider.CompareTag("crit"))
            {
                monster.takeDamage(whichWeapon.weaponDamage, DamageType.physical,true);
            }
            else
            {
                monster.takeDamage(whichWeapon.weaponDamage, DamageType.physical,false);

            }
            Destroy(this.gameObject);
            return;
            //onBulletCollision.Invoke();
        }

        GameObject instance = Instantiate(bulletHole, collision.contacts[0].point, Quaternion.identity);
       
        //Debug.Log("Önce" + instance.transform.localPosition);
        //Debug.Log(collision.contacts[0].normal);
        //instance.transform.localPosition = instance.transform.localPosition + new Vector3(bulletHoleX, bulletHoleY, bulletHoleZ);
        instance.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal / 100);
        instance.transform.localPosition += collision.contacts[0].normal/100;


        //  Debug.Log("Sonra" + instance.transform.localPosition);
        Destroy(this.gameObject);
    }


}
