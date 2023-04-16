using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Tank : MonoBehaviour
{
    [SerializeField]private GameObject bullet;
    [SerializeField]private Transform SpawnPoint;
    [SerializeField]private HealthBehaviour healthBar;
    [SerializeField] public Transform turret;
    [SerializeField] public Transform gun;

    public float maxHealth;
    [HideInInspector]public float currentHealth;

    public float damage;
    public float SHOOT_DELAY;
    private float shootTimer = 0f;

    //list of tanks that recently attacked you
    [HideInInspector]public JustAttackedMe justAttackedMe;

    //for respanwing
    public float spawnRange = 5f;      // the range of positions along the x-axis to spawn
    public float zSpawnpoint = 0;

    public int kills = 0;
    public string team = "";
    public int score = 0;

    public bool justDied = false;

    private float lastDamaged = 0f;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
        justAttackedMe = gameObject.AddComponent<JustAttackedMe>();
    }

    void Update()
    {
        shootTimer = (shootTimer + Time.deltaTime) % 10;

        if (Time.deltaTime - lastDamaged > 1f)
        {
            currentHealth += 0.05f;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    public void Shoot()
    {
        if (shootTimer < SHOOT_DELAY)return;
        var bulletInstance = Instantiate(bullet, SpawnPoint);
        bulletInstance.GetComponent<Bullet>().owner = this;
        shootTimer = 0;
    }

    public void ReceiveDamage(GameObject bulletObject)
    {
        lastDamaged = Time.time;
        justAttackedMe.AddTank(ref bulletObject.gameObject.GetComponent<Bullet>().owner);
        bulletObject.gameObject.GetComponent<Bullet>().owner.score += 100;

        currentHealth -= bulletObject.gameObject.GetComponent<Bullet>().owner.damage;
        currentHealth = Mathf.Max(0, currentHealth);
        healthBar.SetHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            bulletObject.gameObject.GetComponent<Bullet>().owner.kills += 1;
            justDied = true;
            respawn();
            //Destroy(gameObject, 0.1f);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>() != null)
        {
            if(collision.gameObject.GetComponent<Bullet>().owner.team != team)
                ReceiveDamage(collision.gameObject);
        }
    }

    public void respawn()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);

        transform.rotation = Quaternion.identity;
        turret.rotation = Quaternion.identity;
        gun.rotation = Quaternion.identity;

        float spawnPositionX = Random.Range(-spawnRange, spawnRange);
        transform.position = new Vector3(spawnPositionX, transform.position.y, zSpawnpoint);
        
    }
}


public class JustAttackedMe : MonoBehaviour
{
    public float disappearTime = 2;
    public List<Tank> tankList = new List<Tank>();
    private List<float> timerList = new List<float>();

    private void Update()
    {
        for (int i = 0; i < tankList.Count; i++)
        {
            timerList[i] += Time.deltaTime;
            if (timerList[i] > disappearTime)
            {
                timerList.RemoveAt(i);
                tankList.RemoveAt(i);
            }
        }

    }

    public void AddTank(ref Tank tank)
    {
        tankList.Add(tank);
        timerList.Add(0);
    }

}
