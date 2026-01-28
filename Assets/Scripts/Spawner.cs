using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject _targetPrefab;
    [SerializeField] Transform _destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(_targetPrefab, "No target prefab set");
        Assert.IsNotNull(_destination, "No destination set");

        StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets()
    {
        WaitForSeconds time = new WaitForSeconds(1f);
        while (true)         {
            GameObject newEnemy = Instantiate(_targetPrefab, transform.position, Quaternion.identity);

            NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
            agent.destination = _destination.position;

            yield return time;
        }
    }
}
