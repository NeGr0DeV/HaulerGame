using UnityEngine;

public class TruckCargoSystem : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Transform cargoHoldPoint;
    [SerializeField] private int maxCargoCount = 1;

    private Transform currentCargo = null;

    public bool CanPickupCargo() => currentCargo == null;

    public void LoadCargo(Transform cargo)
    {
        if (currentCargo != null) return;

        currentCargo = cargo;
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