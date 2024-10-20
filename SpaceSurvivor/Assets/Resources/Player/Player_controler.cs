using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD.Studio; 

using UnityEngine.Rendering.Universal;

public class Player_controler : MonoBehaviour
{  
    public static Player_controler Instance { get; private set; }

[Header("Audio")]
    [SerializeField] private EventReference engine_soundEvent;    
    [SerializeField] private EventReference turboEngine_soundEvent;
[Header("Param")] 
    [Tooltip("")] 
    [SerializeField] public float globalSpeed;
    [SerializeField] public float GlowIntensity;
    [SerializeField] public float maxHealth;
    [SerializeField] public float maxXP;
[Header("Other")] 
    [SerializeField] public ParticleSystem particlePrefab; 
    [SerializeField] public GameObject Particle_spawn_point;

    [HideInInspector] public float health;
    [HideInInspector] public float XP;
    [HideInInspector] public WeaponBase[] weapons;
    [HideInInspector] public Boolean playerMort;
    [HideInInspector] public bool upgraded;

    [HideInInspector] private Rigidbody2D rb ;
    [HideInInspector] private float speed ;
    [HideInInspector] private float maxVelocity;
    [HideInInspector] private float sprintSpeed ;
    [HideInInspector] private float maxSprintVelocity;
    [HideInInspector] private float breakForce;
    [HideInInspector] private float rotationSpeed;
    [HideInInspector] private bool OnTurbo;
    [HideInInspector] private bool OnMove;
    [HideInInspector] private Vector2 moveDirection;
    [HideInInspector] private Vector2 mousePosition;
    [HideInInspector] private float currentVelocity;
    [HideInInspector] private float aimAngle;
    [HideInInspector] private float GlowDuration = 0.1f;
    [HideInInspector] private float initialIntensity;
    [HideInInspector] private FMOD.Studio.EventInstance engineSoundInstance;
    [HideInInspector] private FMOD.Studio.EventInstance turboEngineSoundInstance;
    

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        speed = 10000f * Time.fixedDeltaTime * globalSpeed;
        maxVelocity = 250f * Time.fixedDeltaTime * globalSpeed;
        sprintSpeed = 20000f * Time.fixedDeltaTime * globalSpeed;
        maxSprintVelocity = 500f * Time.fixedDeltaTime * globalSpeed;
        breakForce = 1000f * Time.fixedDeltaTime;
        rotationSpeed = 10f * Time.fixedDeltaTime;
        playerMort = false;
        UpdateWeapon();
        health = maxHealth;
        XP = 0;
        initialIntensity = GetComponent<Light2D>().intensity;
        engineSoundInstance = RuntimeManager.CreateInstance(engine_soundEvent);
        OnMove = false;
        turboEngineSoundInstance = RuntimeManager.CreateInstance(turboEngine_soundEvent);
        OnTurbo = false;
    }

    public void UpdateWeapon()
    {
        weapons = GetComponentsInChildren<WeaponBase>();
    }

    private void FixedUpdate() 
    {
        float moveX = Input.GetAxisRaw ("Horizontal");
        float moveY = Input.GetAxisRaw ("Vertical");

        moveDirection = new Vector2 (moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (health <= 0)
        {
            foreach (WeaponBase weapon in weapons)
            {
                weapon.CancelFire();
            }
            playerMort = true;
            rb.constraints = RigidbodyConstraints2D.None;
        }

        if (!playerMort)
        {
            // Rotation
            if (Input.GetMouseButton(0))
            {
                if (moveDirection != Vector2.zero)
                {
                    EmitIndependentParticles();
                }

                Vector2 aimDirection = mousePosition - rb.position;
                aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;
                rb.rotation = aimAngle;
            }
            else if (moveDirection != Vector2.zero)
            {

                EmitIndependentParticles();

                if (rb.rotation > 180)
                {
                    rb.rotation = rb.rotation - 360;
                }
                else if (rb.rotation < -180)
                {
                    rb.rotation = rb.rotation + 360;
                }

                float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
                float smoothedAngle =  Mathf.SmoothDampAngle(rb.rotation, targetAngle, ref currentVelocity, rotationSpeed);
                rb.rotation = smoothedAngle;
            }

            // Déplacements
            if (moveDirection != Vector2.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift)) 
                {
                    StopEngineSound();
                    PlayTurboSound();
                    rb.AddForce(moveDirection * sprintSpeed);
                    if (rb.velocity.magnitude > maxSprintVelocity)
                    {
                        rb.velocity = rb.velocity.normalized * maxVelocity;
                    }
                }
                else
                {
                    StopTurboSound();
                    PlayEngineSound();
                    rb.AddForce(moveDirection * speed);
                    if (rb.velocity.magnitude > maxVelocity)
                    {
                        rb.velocity = rb.velocity.normalized * maxVelocity;
                    }
                }
            }
            else {
                StopEngineSound();
                StopTurboSound();
                rb.AddForce(-rb.velocity*breakForce, ForceMode2D.Force);
            }

            // Shoot
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(ShortDelayFire());
            }
            if(!Input.GetMouseButton(0))
            {
                foreach (WeaponBase weapon in weapons)
                {
                    weapon.CancelFire();
                }
            }
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            playerMort = true;
        }
        else
        {
            StartCoroutine(GlowOnHit());
        }
    }

    void EmitIndependentParticles()
    {
        // Instantiate the particle system at the player's position and rotation
        ParticleSystem particleInstance = Instantiate(particlePrefab, new Vector3 (Particle_spawn_point.transform.position.x,Particle_spawn_point.transform.position.y,Particle_spawn_point.transform.position.z), Quaternion.Euler(0,0,0));
        var mainModule = particleInstance.main;
        particleInstance.Play();
        Destroy(particleInstance.gameObject, mainModule.duration + mainModule.startLifetime.constantMax);
    }

    IEnumerator ShortDelayFire()
    {
        //délai de 0.1 seconde
        yield return new WaitForSeconds(0.00f);
        foreach (WeaponBase weapon in weapons)
        {
            weapon.Fire();
        }
    }

    IEnumerator GlowOnHit()
    {
        float elapsedTime = 0f;
        
        //Debug.Log("Intensity INIT" + GetComponent<Light2D>().intensity);

        while (elapsedTime < GlowDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / GlowDuration;

            // Augmenter le glow
            GetComponent<Light2D>().intensity = Mathf.Lerp(initialIntensity, initialIntensity + GlowIntensity, lerpFactor);

            yield return null;
        }

        // Revenir à la valeur d'origine
        elapsedTime = 0f;
        while (elapsedTime < GlowDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / GlowDuration;

            GetComponent<Light2D>().intensity = Mathf.Lerp(initialIntensity + GlowIntensity, initialIntensity, lerpFactor);

            yield return null;
        }

        //Debug.Log("Intensity INIT" + GetComponent<Light2D>().intensity);
    }

    public void GainXP(int Amount)
    {
        XP += Amount;
    }

    public void LevelUp()
    {
        upgraded = true;
    }

    public void PlayEngineSound()
    {
        if (!OnMove)
        {
            engineSoundInstance.start(); // Démarre la lecture du son
            OnMove = true;
        }
    }
    public void StopEngineSound()
    {
        if (OnMove)
        {
            engineSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //engineSoundInstance.release();
            OnMove = false;
        }
    }

    public void PlayTurboSound()
    {
        if (!OnTurbo)
        {
            turboEngineSoundInstance.start(); // Démarre la lecture du son
            OnTurbo = true;
        }
    }
    public void StopTurboSound()
    {
        if (OnTurbo)
        {
            turboEngineSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //turboEngineSoundInstance.release();
            OnTurbo = false;
        }

        /*FMOD.Studio.PLAYBACK_STATE playbackState;
        turboEngineSoundInstance.getPlaybackState(out playbackState);

        if (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            turboEngineSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }*/
    }
}
