using UnityEngine;

public class Cross_Canon : WeaponBase
{
    public Transform firepoint1;
    public Transform firepoint2;
    public Transform firepoint3;
    public Transform firepoint4;

    void Start()
    {
        
    }

    public override void Fire()
    {
        PlayShootSound();
        GameObject bullet = Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint1.up * fireForce,ForceMode2D.Impulse);

        bullet = Instantiate(bulletPrefab, firepoint2.position, firepoint2.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(-firepoint2.right * fireForce,ForceMode2D.Impulse);

        bullet = Instantiate(bulletPrefab, firepoint3.position, firepoint3.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(-firepoint3.up * fireForce,ForceMode2D.Impulse);

        bullet = Instantiate(bulletPrefab, firepoint4.position, firepoint4.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint4.right * fireForce,ForceMode2D.Impulse);
    }
}
