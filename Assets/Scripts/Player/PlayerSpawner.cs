using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Vector3 _spawnPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spawnPoint = transform.position;
    }
    
    public Vector3 GetSpawnPoint() => _spawnPoint;
}
