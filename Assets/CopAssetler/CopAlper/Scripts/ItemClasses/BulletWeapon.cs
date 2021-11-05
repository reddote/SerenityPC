using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeapon : Weapon
{
    public RecoilScript recoil;
    public WeaponControls weaponControls;
    [Space(10)]
    //Bullet Typeları tut;
    public float fireRate;
    public float nextFire = 0f;
    public GameObject bulletGO;
    public Rigidbody bulletRigidBody;
    public float bulletSpeed;
    public float camUpwardRecoil;
    public float camSideRecoil;
    public PlayerLoadOut playerLoadOut;

    public override void Shoot()
    {
        if (Input.GetButtonDown(fireButton) && playerLoadOut.tempClipSize > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            weaponControls.Fire(true,true);
            weaponControls.CameraRecoil(camUpwardRecoil, camSideRecoil);
            GameObject spawnBullet = Instantiate(bulletGO, bulletSpawnPointGO.transform.position, bulletSpawnPointGO.transform.rotation);
            weaponControls.bulletSpawnPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
            recoil.Fire();
            //Damage için bu kodu her yere yerleştir.
            //+1i bullet damage yap
            float damage = Random.Range(4, 7) + 1;
            weaponDamage = damage;
            Bullet bullet = spawnBullet.GetComponent<Bullet>();
            bullet.whichWeapon = this;
            clipSize -= 1;
            bulletRigidBody = spawnBullet.GetComponent<Rigidbody>();
            bulletRigidBody.AddRelativeForce(Vector3.forward * bulletSpeed);
            Destroy(spawnBullet, 2f);
        }
        //fire button = ButtonKeyController.fireButtonInputName
        if (Input.GetButton(fireButton) && playerLoadOut.tempClipSize > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            weaponControls.Fire(true,false);
            weaponControls.CameraRecoil(camUpwardRecoil, camSideRecoil);
            GameObject spawnBullet = Instantiate(bulletGO, bulletSpawnPointGO.transform.position, bulletSpawnPointGO.transform.rotation);
            weaponControls.bulletSpawnPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
            recoil.Fire();
            //Damage için bu kodu her yere yerleştir.
            //+1i bullet damage yap
            float damage = Random.Range(4, 7) + 1;
            weaponDamage = damage;
            Bullet bullet = spawnBullet.GetComponent<Bullet>();
            bullet.whichWeapon = this;
            clipSize -= 1;
            bulletRigidBody = spawnBullet.GetComponent<Rigidbody>();
            bulletRigidBody.AddRelativeForce(Vector3.forward * bulletSpeed);
            Destroy(spawnBullet, 2f);
        }
        else if (!Input.GetButton(fireButton))
        {
            weaponControls.Fire(false,false);

        }
    }


}
