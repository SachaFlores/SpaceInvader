using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public GameObject bulletPreFab;

    public Transform firePoint;
    float life ;
    public float maxLife ;
    public float timeBtwShoot = 2f;
    float timer = 0;
    public EnemyType type = EnemyType.normal;

    bool distance = false;
    Transform target;
    public float damage ;
    public float bulletSpeed;
    public Image lifeBar;

    public GameObject ExplosionEffect;


    public List<GameObject> PowerUpPrefabs;

    [Header("Sonidos")]
    public AudioClip explosionSound;
    public AudioClip shootSound;
    public AudioClip crashSound;
    
    void Start()
    {
        life = maxLife;
        lifeBar.fillAmount = life / maxLife;
        GameObject targetGo = GameObject.FindGameObjectWithTag("Player");
        if (targetGo != null)
        {
            target = targetGo.transform;
        }
    }

    void Update()
    {
        try
        {
            switch (type)
            {
                case EnemyType.normal:
                    NormalMovement();
                    break;
                case EnemyType.normalShoot:
                    NormalMovement();
                    NormalShoot();
                    break;
                case EnemyType.sniper:
                    if (Vector2.Distance(target.position, transform.position) < 5f || distance)
                    {
                        distance = true;
                        UpdateTarget();
                        NormalShoot();
                    }
                    else
                    {
                        NormalMovement();
                    }
                    break;
                case EnemyType.kamikase:
                    if (Vector2.Distance(target.position, transform.position) < 4f || distance)
                    {
                        distance = true;
                        KamikaseMovement();
                    }
                    else
                    {
                        NormalMovement();
                    }
                    break;
            }
        }
        catch
        {

        }
        
    }

    void NormalMovement()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }
    void NormalShoot()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            AudioManager.instance.PlaySound(shootSound);
            GameObject bullet =  Instantiate(bulletPreFab, firePoint.position, firePoint.rotation);
            Bullet b = bullet.GetComponent<Bullet>();
            b.speed = bulletSpeed;
            b.damage = damage;
        }
    }

    void UpdateTarget()
    {
        if (target != null)
        {
            Vector2 dir = target.position - transform.position;
            float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angleZ);
        }
    }
    void KamikaseMovement()
    {
        if (target != null)
        {
            Vector2 dir = target.position - transform.position;
            float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angleZ);

            transform.Translate(0, (speed * Time.deltaTime) * 2, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == EnemyType.kamikase && collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(1f);
            AudioManager.instance.PlaySound(crashSound);
            Instantiate(ExplosionEffect, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 180));
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(1f);
            AudioManager.instance.PlaySound(crashSound);
            Instantiate(ExplosionEffect, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 180));
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float amount)
    {

        life -= amount;
        lifeBar.fillAmount = life / maxLife;
        if (life <= 0)
        {
            AudioManager.instance.PlaySound(explosionSound);
            int x = Random.Range(0,4);
            Debug.Log("Enemy is dead");
            Instantiate(ExplosionEffect, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 180));
            if (x == 1)
            {
                int PowerUp = Random.Range(0, PowerUpPrefabs.Count);
                Instantiate(PowerUpPrefabs[PowerUp], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 180));
            }
            Spawner.instance.addKilledEnemies();
            Destroy(gameObject); 
        }
    }
    public enum EnemyType
    {
        normal,
        normalShoot,
        sniper,
        kamikase
    }
}
