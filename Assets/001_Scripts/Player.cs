using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PowerUp;

using TMPro;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float speed = 7f;
    public GameObject bulletPreFab;
    public Transform firePoint;
    public float timeBtwShoot = 1f;
    float timer = 0;
    bool canShoot = true;
    public float life = 3f;
    public float maxLife = 3f;
    public float extraLifes = 4;
    public float damage =1;
    public float bulletSpeed=7;

    float iframe = 0;
    float timeToDamage = 1;
    bool canTakeDamage = true;

    [Header("UI")]
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI ShootSpeedText;
    public TextMeshProUGUI BulletSpeedText;
    public TextMeshProUGUI MoveSpeedText;
    public Image lifeBar;

    public GameObject ExplosionEffect;
    public GameObject PowerEffect;

    [Header("Sonidos")]
    public AudioClip explosionSound;
    public AudioClip shootSound;
    public AudioClip powerUpSound;

    [Header("nimator")]
    public Animator anim;
    public Animator Elives;
    void Start()
    {
        DamageText.text = "Damage = " + damage;
        ShootSpeedText.text = "Shoot speed = " + timeBtwShoot;
        BulletSpeedText.text = "Bullet speed = " + bulletSpeed ;
        MoveSpeedText.text = "Move speed = " + speed;
        lifeBar.fillAmount = life / maxLife;
        //Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shoot();
        CheckIfCanShoot();
        CheckIfCanTakeDamage();
    }
    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        anim.SetFloat("X",x);
        rb.velocity = new Vector3(x * speed, y * speed, 0);
    }
    void Shoot()
    {
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Shoot");
                AudioManager.instance.PlaySound(shootSound);

                GameObject bullet = Instantiate(bulletPreFab, firePoint.position, firePoint.rotation); //new Vector3(x,y,z)----Quaternion.Euler(x,y,z)
                Bullet b = bullet.GetComponent<Bullet>();
                b.speed = bulletSpeed;
                b.damage = damage;
                canShoot = false;
            }
        }
    }
    void CheckIfCanShoot()
    {
        if (!canShoot)
        {
            if(timer < timeBtwShoot)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                canShoot = true;
            }
        }
    }
    void CheckIfCanTakeDamage()
    {
        if (!canTakeDamage)
        {
            if(iframe < timeToDamage)
            {
                iframe += Time.deltaTime;
            }
            else
            {
                canTakeDamage = true;
            }
        }
    }
    public bool TakeDamage(float amount)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            iframe = 0;
            life -= amount;
            lifeBar.fillAmount = life / maxLife;
            if (life <= 0)
            {
                AudioManager.instance.PlaySound(explosionSound);

                Instantiate(ExplosionEffect, transform.position, transform.rotation);
                if (extraLifes < 1)
                {
                    Destroy(gameObject);
                }
                else if (life <= 0)
                {
                    extraLifes--;
                    Elives.SetFloat("Live", extraLifes);
                    transform.position = new Vector3(0, -3, 0);
                    life = 3;
                    lifeBar.fillAmount = life / maxLife;
                }
            }
            return true;
        }
        else { return false; }
    }

    public void TakePowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.MoveSpeed:
                speed += 1f;
                MoveSpeedText.text = "Move speed = " + speed;
                break;
            case PowerUpType.ShootSpeed:
                
                timeBtwShoot = timeBtwShoot / 2;
                ShootSpeedText.text = "Shoot speed = " + timeBtwShoot;
                break;
            case PowerUpType.BulletSpeed:
                bulletSpeed += 1f;
                BulletSpeedText.text = "Bullet speed = " + bulletSpeed;
                break;
            case PowerUpType.Damage:
                damage += 1f;
                DamageText.text = "Damage = " + damage;
                break;
            case PowerUpType.Heal:
                Debug.Log("Vida recuperada");
                if(life < maxLife)
                {
                    life += 1;
                    lifeBar.fillAmount = life / maxLife;
                }
                break;
        }
        AudioManager.instance.PlaySound(powerUpSound);
        Instantiate(PowerEffect, transform.position, transform.rotation);
    }
}
