using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGun : BulletWeapon
{


    private void Start()
    {
        //Hiyeraşideki sıralamaya bağlı değişebilir index.
        
        fireRate = 1f;
        nextFire = 0;
        //Bulletlar

        bulletSpeed = 4000;
        
    }


    //public override void Shoot()
    //{
    //    //fire button = ButtonKeyController.fireButtonInputName
    //    if (Input.GetButton(fireButton) && clipSize > 0 && Time.time > nextFire)
    //    {
    //        nextFire = Time.time + fireRate;

    //        GameObject spawnBullet = Instantiate(bulletGO, bulletSpawnPointGO.transform.position, bulletSpawnPointGO.transform.rotation);
    //        recoil.Fire();
    //        //Damage için bu kodu her yere yerleştir.
    //        //+1i bullet damage yap
    //        float damage = Random.Range(4, 7) + 1;
    //        weaponDamage = damage;
    //        // Debug.Log(damage);
    //        Bullet bullet = spawnBullet.GetComponent<Bullet>();
    //        bullet.whichWeapon = this;
    //        clipSize -= 1;
    //        bulletRigidBody = spawnBullet.GetComponent<Rigidbody>();
    //        bulletRigidBody.AddRelativeForce(Vector3.forward * bulletSpeed);
    //        Destroy(spawnBullet, 2f);
    //    }
    //}




}
