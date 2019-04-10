using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    // configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f; // use to adjust the speed of the ship
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;

    [Header("Sound FX")]
    [SerializeField] AudioClip deathSoundFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f; //forces a range from 0 - 1
    [SerializeField] AudioClip shootingSoundFX;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f; //forces a range from 0 - 1

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.2f;

    // capture our firing coroutine to make it easy to turn on and off w/o messing w/other coroutines
    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    // this coroutine allows for auto-fire
    private IEnumerator FireContinuously()
    {
        while (true)
        {        
            // eg Istantiate(gameobject, position, rotation) Quarternion.identity = no rotation
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            //PlayClipAtPoint = sound FX doesn't get cut off; volume parameter goes from 0 - 1
            AudioSource.PlayClipAtPoint(shootingSoundFX, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        // GetAxis uses keywords from Edit/Project Settings/Input
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; // Time.deltaTime makes movement framerate independent
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } //protects against null (if other doesn't have a DamageDealer script attached to it)
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        //PlayClipAtPoint = sound FX doesn't get cut off; volume parameter goes from 0 - 1
        AudioSource.PlayClipAtPoint(deathSoundFX, Camera.main.transform.position, deathSoundVolume);
    }

}
