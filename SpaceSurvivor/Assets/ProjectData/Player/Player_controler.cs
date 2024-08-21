using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_controler : MonoBehaviour
{
    Rigidbody2D rb ;
    float speed ;
    float maxVelocity;
    float sprintSpeed ;
    float maxSprintVelocity;
    float breakForce;
    float rotationSpeed;
    public WeaponBase weapon;
    Vector2 moveDirection;
    Vector2 mousePosition;
    private float currentVelocity;
    public ParticleSystem particlePrefab; 
    public GameObject Particle_spawn_point;
    public int PlayerHealth;
    public Boolean playerMort;
    float aimAngle;
    WeaponBase[] weapons;
    public static Player_controler Instance { get; private set; }

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
        speed = 10000f * Time.fixedDeltaTime;
        maxVelocity = 250f * Time.fixedDeltaTime;
        sprintSpeed = 20000f * Time.fixedDeltaTime;
        maxSprintVelocity = 500f * Time.fixedDeltaTime;
        breakForce = 1000f * Time.fixedDeltaTime;
        rotationSpeed = 10f * Time.fixedDeltaTime;
        playerMort = false;
        weapons = GetComponentsInChildren<WeaponBase>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw ("Horizontal");
        float moveY = Input.GetAxisRaw ("Vertical");

        moveDirection = new Vector2 (moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (PlayerHealth <= 0)
        {
            foreach (WeaponBase weapon in weapons)
            {
                weapon.CancelFire();
            }
            playerMort = true;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void FixedUpdate() 
    {
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
                    rb.AddForce(moveDirection * sprintSpeed);
                    if (rb.velocity.magnitude > maxSprintVelocity)
                    {
                        rb.velocity = rb.velocity.normalized * maxVelocity;
                    }
                }
                else
                {
                    rb.AddForce(moveDirection * speed);
                    if (rb.velocity.magnitude > maxVelocity)
                    {
                        rb.velocity = rb.velocity.normalized * maxVelocity;
                    }
                }
            }
            else {
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
        yield return new WaitForSeconds(0.02f);
        foreach (WeaponBase weapon in weapons)
        {
            weapon.Fire();
        }
    }
}