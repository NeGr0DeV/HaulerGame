using NUnit.Framework;
using UnityEngine;

public class TruckCargoSystem : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Transform cargoHoldPoint;
    [SerializeField] private int maxCargoCount = 1;
    private CargoPickup cargoPickup;
    public float massCargo = 0f;

    public Transform currentCargo = null;

    public bool CanPickupCargo() => currentCargo == null;

    public void LoadCargo(Transform cargo)
    {
        if (currentCargo != null) return;

        currentCargo = cargo;
        CargoPickup pickup = cargo.GetComponent<CargoPickup>();
        if (pickup != null) { massCargo = pickup.massCargo; }
        Debug.Log("Груз успешно загружен в кузов");
    }

    public void UnloadCurrentCargo()
    {
        if (currentCargo == null) return;

        currentCargo.SetParent(null);

        currentCargo = null;
        Debug.Log("Груз выгружен");
    }

    public bool HasCargo() => currentCargo != null;
}