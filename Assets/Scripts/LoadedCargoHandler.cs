using UnityEngine;

public class LoadedCargoHandler : MonoBehaviour
{
    private GameObject player;
    private TruckCargoSystem truckCargoSystem;
    private ZoneSpawner zoneSpawner;

    private int numCargos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        truckCargoSystem = player.GetComponent<TruckCargoSystem>();
        zoneSpawner = GameObject.FindFirstObjectByType<ZoneSpawner>();
        numCargos = 0;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        GetLoadedCargos();
    //    }
    //}
    public int GetNumCargos()
    {
        return truckCargoSystem.loadedCargos.Count;
    }
    public void ClearAllCargo()
    {
        truckCargoSystem.ClearAllCargo();
    }
    //{
    //public void DeactivateZones()
    //    Debug.Log("Zone deactivation started (loaded cargo handler)");
    //    //GetLoadedCargos();
    //    if (zoneSpawner)
    //    {
    //        if (numCargos == 0)
    //        {
    //            zoneSpawner.DeactivateAll();
    //        }
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        //GetLoadedCargos();
    }
}
