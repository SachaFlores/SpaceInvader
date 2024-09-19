using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misile : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    public float speed;
    public float damage;
    bool persec = true;
    public float timebtw = 1f;
    float timer = 0;
    public GameObject ExplosionEffect;

    [Header("Sonidos")]
    public AudioClip crashSound;
    // Start is called before the first frame update
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
        verify();
        if (target != null && persec)
        {
            Vector2 dir = target.position - transform.position;
            float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angleZ);
        }
        transform.Translate(0, speed * Time.deltaTime, 0);
    }
    void verify()
    {
        if (persec)
        {
            if (timer < timebtw)
            {
                timer += Time.deltaTime;
            }
            else
            {
                persec = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player p = collision.gameObject.GetComponent<Player>();
                if (p.TakeDamage(damage))
                {
                    AudioManager.instance.PlaySound(crashSound);
                    Instantiate(ExplosionEffect, transform.position, Quaternion.Euler(0, 0, 0));
                    Destroy(gameObject);
                }
            }
        }
        catch
        {
            Destroy(gameObject);
        }  
    }
}
