using UnityEngine;

public class ZoneSpawner : MonoBehaviour
{
    private GameObject[] deliveryZones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindZones();
        DeactivateAll();
    }
    void FindZones()
    {
        deliveryZones = GameObject.FindGameObjectsWithTag("DeliveryZone");
    }
    public void DeactivateAll()
    {
        foreach (GameObject zone in deliveryZones)
        {
            zone.SetActive(false);
        }
    }
    public void ActivateRandom()
    {
        int idx = Random.Range(0, deliveryZones.Length);  
        deliveryZones[idx].SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
