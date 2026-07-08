using Unity.VisualScripting;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private int pts = 30; //change later
    //public Color zoneColor = Color.yellow;

    private void OnTriggerEnter(Collider other)
    {
        Cargo box = other.GetComponent<Cargo>();
        if (box)
        {
            MarkDelivered(box);
            //Destroy(other.gameObject);
        }
        else return;
    }
    void MarkDelivered(Cargo box) 
    {
        if (SimpleScore.Instance != null)
        {
            SimpleScore.Instance.UpdateScore(pts);
            SimpleScore.Instance.UpdateCargosDelivered();
        }
        if (Timer.Instance != null)
        {
            Timer.Instance.AddTime();
        }
        box.MarkDelivered();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
