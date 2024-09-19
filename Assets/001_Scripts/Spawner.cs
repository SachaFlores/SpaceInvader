using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeBtwSpawn = 3f;
    float timer = 0;
    bool IfBoss = false;
    bool stillBoss = false;
    public GameObject boss;

    public List<GameObject>EnemyPreFabs;
    public Transform LeftPoint;
    public Transform RightPoint;

    public int killedEnemies = 0;
    public static Spawner instance;

    int MaxEnemies = 10;
    float contEnemies = 0;
    public TextMeshProUGUI FinishText;
    public TextMeshProUGUI NowText;
    public Image enemyBar;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FinishText.text = MaxEnemies.ToString();
        NowText.text = killedEnemies.ToString();

        float x = contEnemies / MaxEnemies;
        enemyBar.fillAmount = x;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timeBtwSpawn)
        {
            timer += Time.deltaTime;
        }
        else if (!IfBoss)
        {
            timer = 0;
            int enemiesTospawn = 1 + (killedEnemies / MaxEnemies);
            for (int i = 0; i< enemiesTospawn; i++)
            {

                float x = Random.Range(LeftPoint.position.x, RightPoint.position.x);
                int enemy = Random.Range(0, EnemyPreFabs.Count);

                Instantiate(EnemyPreFabs[enemy], new Vector3(x, transform.position.y, 0), Quaternion.Euler(0, 0, 180));
            }
        }
        if(IfBoss && !stillBoss)
        {
            stillBoss = true;
            AudioManager.instance.bossSong();
            Instantiate(boss, new Vector3(transform.position.x, transform.position.y , 0), Quaternion.Euler(0, 0, 180));
        }
    }
    
    public void addKilledEnemies()
    {
        killedEnemies++;

        contEnemies++; // enemigos de la gorda 0 - max osea 10

        FinishText.text = (MaxEnemies - contEnemies).ToString();
        NowText.text = killedEnemies.ToString();

        float x = contEnemies / MaxEnemies;
        enemyBar.fillAmount = x ;

        if (contEnemies > MaxEnemies -1)
        {
            contEnemies = 0;
            enemyBar.fillAmount = 0;
        }
        if(killedEnemies == 50)
        {
            IfBoss = true;
        }
    }
}
