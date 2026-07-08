using System.Collections;
using TMPro;
using UnityEngine;

public class TaskText : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public bool isCargoPicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        taskText = GetComponent<TextMeshProUGUI>();
        taskText.text = "You need to pick up a cargo!";
        StartCoroutine(HideAfterDelay());
    }
    IEnumerator HideAfterDelay()
    {
        Debug.Log("HAD called");
        yield return new WaitForSeconds(3);
        taskText.text = "";
    }
    public void PickupMessage()
    {
        taskText.text = "You need to pick up a cargo!";
        StartCoroutine(HideAfterDelay());
    }
    public void DeliverMessage()
    {
        taskText.text = "Deliver a cargo to a delivery zone";
        StartCoroutine(HideAfterDelay());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
