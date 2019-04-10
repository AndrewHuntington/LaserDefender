using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;

    [Header("Sound FX")]
    [SerializeField] AudioClip deathSoundFX;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.75f; //forces a range from 0 - 1
    [SerializeField] AudioClip shootingSoundFX;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f; //forces a range from 0 - 1


    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Fire Rate")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
                projectile,
                transform.position,
                Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootingSoundFX, Camera.main.transform.position, shootSoundVolume);
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
        GameObject explosion = Instantiate(
                        deathVFX,
                        transform.position,
                        transform.rotation) as GameObject; //trasform.rotation = Quaternion.identity
        Destroy(explosion, durationOfExplosion);
        //PlayClipAtPoint = sound FX doesn't get cut off; volume parameter goes from 0 - 1
        AudioSource.PlayClipAtPoint(deathSoundFX, Camera.main.transform.position, deathSoundVolume);
    }
}
