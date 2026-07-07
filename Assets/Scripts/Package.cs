using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Package : MonoBehaviour
{
    //healthbar govna ne rabotaet


    private float health;
    private float maxHealth = 10f;

    private int penalty;
    private bool isDelivered = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        penalty = -((int) maxHealth / 10);
        health = maxHealth;

    }


    void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            UpdateScore(penalty);
            Die();
        }
    }
    //void UpdateHealth()
    //{
    //    if (hpBar != null)
    //        hpBar.UpdateHealth(health);
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (isDelivered) return;

        float dmg = collision.relativeVelocity.magnitude;
        Debug.Log($"dmg taken: {dmg}, cur hp: {health}");

        TakeDamage(dmg);
        //UpdateHealth();

    }
    void UpdateScore(int score)
    {
        if (SimpleScore.Instance != null)
        {
            SimpleScore.Instance.UpdateScore(score);
        }
    }
    void Die()
    {
        //if (hpBar != null)
        //    Destroy(hpBar.gameObject);
        Destroy(gameObject);
        

        //Destroy(textHp);
        //Destroy(barInstance);

        Debug.Log($"package {gameObject} destroyed");
    }
    public void MarkDelivered()
    {
        isDelivered = true;
        Die();
        Debug.Log("package delivered");
    }
    // Update is called once per frame 
    void Update()
    {
    }
}
