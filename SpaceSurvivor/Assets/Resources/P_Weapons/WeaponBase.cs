using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio; 

public abstract class WeaponBase : MonoBehaviour
{
[Header("Base_Audio")]
    [SerializeField] public EventReference Shoot_soundEvent;
[Header("Base_Param")]
    [SerializeField] public float fireCooldown;
    [SerializeField] public float fireForce;
    [SerializeField] public GameObject bulletPrefab;

    [HideInInspector] public float lastFireTime;
    [HideInInspector] public Boolean hasLineOfSight;
    [HideInInspector] public Rigidbody2D rb ;
    [HideInInspector] public int Level;
    [HideInInspector] public FMOD.Studio.EventInstance ShootSoundInstance;
    [HideInInspector] public List<Upgrade> availableUpgrades;
    [HideInInspector] public List<Upgrade> TierUpgrades;

    [HideInInspector] public Vector2 aimDirection;

    public abstract void Fire();

    public virtual void CancelFire(){}

    public virtual void Upgrade(){}

    public virtual void ApplyUpgrade(Upgrade selectedUpgrade){}

    public virtual void PlayShootSound()
    {
        ShootSoundInstance = RuntimeManager.CreateInstance(Shoot_soundEvent);
        ShootSoundInstance.start();
    }

}
