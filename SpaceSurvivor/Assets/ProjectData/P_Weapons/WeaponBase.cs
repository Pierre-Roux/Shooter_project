using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [HideInInspector] public float lastFireTime;
    [HideInInspector] public Boolean hasLineOfSight;
    [HideInInspector] public Rigidbody2D rb ;
    public int Level;
    
    public GameObject bulletPrefab;
    public float fireCooldown;
    public float fireForce;

    public abstract void Fire();

    public virtual void CancelFire(){}

    public virtual void Upgrade(){}

}
