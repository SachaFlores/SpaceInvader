using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Enemy;

public class PowerUp : MonoBehaviour
{
    public float speed = 3f;
    public PowerUpType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakePowerUp(type);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public enum PowerUpType
    {
        MoveSpeed,
        ShootSpeed,
        BulletSpeed,
        Damage,
        Heal
    }
}
