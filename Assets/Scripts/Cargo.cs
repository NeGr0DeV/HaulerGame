using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Cargo : MonoBehaviour
{
    //healthbar govna ne rabotaet
    //public UnityEvent onPackagePickedUp;
    public UnityEvent onCargoDelivered;

    private float health;
    private float maxHealth = 100f;

    private int penalty;

    //private bool isPickedUp = false;
    private bool isDelivered = false;

    //public bool IsPickedUp
    //{
    //    get => isPickedUp; 
    //    set 
    //    {
    //        if (isPickedUp != value)
    //        {
    //            isPickedUp = value;
    //            if (isPickedUp)
    //            {
    //                onPackagePickedUp?.Invoke();
    //                Debug.Log($"Package picked up, event fired");
    //            }
    //        }
    //    }
    //}
    public bool IsDelivered
    {
        get => isDelivered;
        set
        {
            if (isDelivered != value)
            {
                isDelivered = value;
                if (isDelivered)
                {
                    onCargoDelivered?.Invoke();
                    Debug.Log($"Package delivered, event fired");
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //adding event listeners
        TaskText taskText = GameObject.FindFirstObjectByType<TaskText>();
        if (taskText != null )
        {
            onCargoDelivered.AddListener(taskText.PickupMessage);
        }
        ZoneSpawner zoneSpawner = GameObject.FindFirstObjectByType<ZoneSpawner>();
        if (zoneSpawner != null)
        {
            onCargoDelivered.AddListener(zoneSpawner.DeactivateAll);
        }
    }
    void Start()
    {
        penalty = -((int) maxHealth / 10);
        health = maxHealth;
    }
    void TakeDamage(float dmg)
    {
        health -= dmg;
        Debug.Log($"dmg taken: {dmg}, cur hp: {health}");
        if (health <= 0)
        {
            UpdateScore(penalty);
            Die();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (IsDelivered) return;

        float dmg = collision.relativeVelocity.magnitude;
        if (dmg > 3)
            TakeDamage(dmg);
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
        Destroy(gameObject);
        Debug.Log($"cargo {gameObject} destroyed");
    }
    public void MarkDelivered()
    {
        IsDelivered = true;
        Die();
        Debug.Log("cargo delivered");
    }
    private void OnDestroy()
    {
        onCargoDelivered.RemoveAllListeners();
    }
    // Update is called once per frame 
    void Update()
    {
    }
}
