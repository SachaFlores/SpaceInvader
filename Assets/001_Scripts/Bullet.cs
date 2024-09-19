using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed ;
    public bool playerBullet = false;
    public float damage ;

    public GameObject ExplosionEffect;

    [Header("Sonidos")]
    public AudioClip crashSound;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,speed * Time.deltaTime,0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerBullet && collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if(p.TakeDamage(damage))
            {
                AudioManager.instance.PlaySound(crashSound);
                Instantiate(ExplosionEffect, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
            }
        }
        else if (playerBullet && collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySound(crashSound);
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(damage);
            Instantiate(ExplosionEffect, transform.position, Quaternion.Euler(0, 0, 180));
            Destroy(gameObject);
        }
        else if (playerBullet && collision.gameObject.CompareTag("Boss"))
        {
            AudioManager.instance.PlaySound(crashSound);
            Boss e = collision.gameObject.GetComponent<Boss>();
            e.TakeDamage(damage);
            Instantiate(ExplosionEffect, transform.position, Quaternion.Euler(0, 0, 180));
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
