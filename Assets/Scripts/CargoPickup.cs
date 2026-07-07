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

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        StartCoroutine(MoveToCargoHold());
    }

    private System.Collections.IEnumerator MoveToCargoHold()
    {
        // === КРИТИЧНО ВАЖНЫЙ ПОРЯДОК ===
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;        // только после сброса скорости!
        }

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 targetPos = cargoHoldPoint != null ? cargoHoldPoint.position : transform.position;
        Quaternion targetRot = cargoHoldPoint != null ? cargoHoldPoint.rotation : transform.rotation;

        float duration = 1.3f;
        float elapsed = 0f;

        transform.SetParent(null);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float arc = Mathf.Sin(t * Mathf.PI) * 2.4f;

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos + Vector3.up * arc, t);
            Quaternion currentRot = Quaternion.Lerp(startRot, targetRot, t * t);

            transform.position = currentPos;
            transform.rotation = currentRot;

            yield return null;
        }

        // Финальная постановка в кузов
        if (cargoHoldPoint != null)
        {
            transform.SetParent(cargoHoldPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        Debug.Log("Груз успешно доставлен в кузов");
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