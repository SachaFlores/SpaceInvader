using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using static Enemy;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
    float timer = 0;
    float top = 4;
    float MaxLife = 200;
    Transform target;

    bool shootP = false;
    bool shootR = false;
    bool shootS = false;
    public float bulletSpeed = 8;
    bool canRotate = false;
    [Header("Principal")]
    int maxShootP = 4;
    int countShootsP = 0;
    float timeP = 0;
    float timeToshootP = 0;
    float timeBtwShootP = 1;
    [Header("Secundary")]
    int maxShootS = 2;
    int countShootsS = 0;
    float timeS = 0;
    float timeToshootS = 0;
    float timeBtwShootS = 3;
    [Header("Rocket")]
    int maxShootR = 6;
    int countShootsR = 0;
    float timeR = 0;
    float timeToshootR = 0;
    float timeBtwShootR = 6;

    [Header("Pre Fabs")]
    public GameObject bulletBluePreFab;
    public GameObject bulletRedPreFab;
    public GameObject misilePreFab;
    [Header("FirePoints")]
    public Transform firePointR;
    public Transform firePointR2;
    public Transform firePointL;
    public Transform firePointL2;

    [Header("Sonidos")]
    public AudioClip shootSoundPrincipal;
    public AudioClip shootSound;
    

    public BossFase type ;
    void Start()
    {
        GameObject targetGo = GameObject.FindGameObjectWithTag("Player");
        if (targetGo != null)
        {
            target = targetGo.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
        canShoot();
        updateBossFase();
        rotateToTarget();
        switch (type)
        {
            case BossFase.Quiet:
                maxShootP = 1;
                shootPrincipal();
                break;
            case BossFase.RafagaQuiet:
                maxShootP = 4;
                shootPrincipal();
                break;
            case BossFase.Rage:
                maxShootP = 4;
                shootPrincipal();
                shootSecundary();
                break;
            case BossFase.Sniper:
                maxShootS = 3;
                bulletSpeed = 15;
                shootSecundary();
                shootRocket();
                break;
            case BossFase.Rocket:
                maxShootP = 2;
                bulletSpeed = 15;
                shootPrincipal();
                shootRocket();
                break;
        }
    }
    void move()
    {
        if (timer <= top)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            timer += Time.deltaTime;
        }
        else
        {
            canRotate = true;
        }
    }
    void updateBossFase()
    {
        if(MaxLife > 180)
        {
            type = BossFase.Quiet;
        }
        else if (MaxLife > 150)
        {
            type = BossFase.RafagaQuiet;
        }
        else if (MaxLife > 100)
        {
            type = BossFase.Rage;
        }
        else if (MaxLife > 70)
        {
            type = BossFase.Sniper;
        }
        else if (MaxLife > 50)
        {
            type = BossFase.Rocket;
        }
    }
    void rotateToTarget()
    {
        if (target != null && canRotate)
        {
            Vector2 dir = target.position - transform.position;
            float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angleZ);
        }
    }
    void shootPrincipal()
    {
        if(shootP && countShootsP < maxShootP)
        {
            
            if(timeP < 0.125)
            {
                timeP += Time.deltaTime;
            }
            else
            {
                countShootsP++;
                AudioManager.instance.PlaySound(shootSoundPrincipal);
                GameObject bulletP = Instantiate(bulletBluePreFab, firePointR.position, firePointR.rotation);
                Bullet bP = bulletP.GetComponent<Bullet>();
                bP.speed = bulletSpeed;
                GameObject bulletP2 = Instantiate(bulletBluePreFab, firePointL.position, firePointL.rotation);
                Bullet bP2 = bulletP2.GetComponent<Bullet>();
                bP2.speed = bulletSpeed;
                timeP = 0;
            }
        }
        else if(countShootsP >= maxShootP)
        {
            countShootsP = 0;
            shootP = false;
        }
    }
    void shootSecundary()
    {
        if (shootS && countShootsS < maxShootS)
        {

            if (timeS < 0.25)
            {
                timeS += Time.deltaTime;
            }
            else
            {
                countShootsS++;
                AudioManager.instance.PlaySound(shootSound);
                GameObject bulletS = Instantiate(bulletRedPreFab, firePointR2.position, firePointR2.rotation);
                Bullet bP = bulletS.GetComponent<Bullet>();
                bP.speed = bulletSpeed;
                GameObject bulletS2 = Instantiate(bulletRedPreFab, firePointL2.position, firePointR2.rotation);
                Bullet bP2 = bulletS2.GetComponent<Bullet>();
                bP2.speed = bulletSpeed;
                timeS = 0;
            }
        }
        else if (countShootsS >= maxShootS)
        {
            countShootsS = 0;
            shootS = false;
        }
    }
    void shootRocket()
    {
        if (shootR && countShootsR < maxShootR)
        {

            if (timeR < 0.125)
            {
                timeR += Time.deltaTime;
            }
            else
            {
                countShootsR++;
                AudioManager.instance.PlaySound(shootSoundPrincipal);
                GameObject bulletP = Instantiate(misilePreFab, firePointR2.position, firePointR2.rotation);
                Misile bP = bulletP.GetComponent<Misile>();
                bP.speed = 5;
                GameObject bulletP2 = Instantiate(misilePreFab, firePointL2.position, firePointL2.rotation);
                Misile bP2 = bulletP2.GetComponent<Misile>();
                bP2.speed = 5;
                timeR = 0;
            }
        }
        else if (countShootsR >= maxShootR)
        {
            countShootsR = 0;
            shootR = false;
        }
    }
    void canShoot()
    {
        if (!shootP)
        {
            if (timeToshootP < timeBtwShootP)
            {
                timeToshootP += Time.deltaTime;
            }
            else
            {
                timeToshootP = 0;
                shootP = true;
            }
        }
        if (!shootS)
        {
            if (timeToshootS < timeBtwShootS)
            {
                timeToshootS += Time.deltaTime;
            }
            else
            {
                timeToshootS = 0;
                shootS = true;
            }
        }
        if (!shootR)
        {
            if (timeToshootR < timeBtwShootR)
            {
                timeToshootR += Time.deltaTime;
            }
            else
            {
                timeToshootR = 0;
                shootR = true;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        MaxLife-= amount;
        if(MaxLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    public enum BossFase
    {
        Quiet,
        Rocket,
        RafagaQuiet,
        Rage,
        Sniper
    }
}
