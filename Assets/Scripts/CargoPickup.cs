using UnityEngine;

public class CargoPickup : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float pickupDistance = 6f;
    [SerializeField] private string pickupKey = "e";
    [SerializeField] private Transform cargoHoldPoint;

    [Header("Визуал")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseIntensity = 0.4f;

    private Renderer[] renderers;
    private Rigidbody rb;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        rb = GetComponentInParent<Rigidbody>(); // находим Rigidbody на родителе
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(pickupKey))
        {
            Debug.Log("Кнопка нажата");
            PickupCargo();
        }

        if (isPlayerNearby)
            PulseEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ShowPickupPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            ShowPickupPrompt(false);
            ResetHighlight();
        }
    }

    private void PickupCargo()
    {
        ShowPickupPrompt(false);
        ResetHighlight();

        StartCoroutine(MoveToCargoHold());
    }

    private System.Collections.IEnumerator MoveToCargoHold()
    {
        if (cargoHoldPoint == null)
        {
            Debug.LogError("CargoHoldPoint не назначен!");
            yield break;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Debug.Log("Начинаем мгновенное перемещение");

        // Мгновенно ставим в точку
        transform.SetParent(null);
        transform.position = cargoHoldPoint.position + Vector3.up * 0.5f;
        transform.rotation = cargoHoldPoint.rotation;

        yield return new WaitForSeconds(0.3f); // небольшая пауза

        // Финально фиксируем
        transform.SetParent(cargoHoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = false;
        Debug.Log("Груз ПРИНУДИТЕЛЬНО поставлен в кузов");
    }

    private void PulseEffect()
    {
        float intensity = Mathf.PingPong(Time.time * pulseSpeed, pulseIntensity) + 0.6f;
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_EmissionColor"))
                r.material.SetColor("_EmissionColor", Color.white * intensity);
        }
    }

    private void ResetHighlight()
    {
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_EmissionColor"))
                r.material.SetColor("_EmissionColor", Color.black);
        }
    }

    private void ShowPickupPrompt(bool show)
    {
        if (show)
            Debug.Log("<color=yellow>Нажмите [E] чтобы подобрать груз</color>");
        // Здесь позже подключишь UI
    }
}