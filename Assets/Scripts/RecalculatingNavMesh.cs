using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class RecalculatingNavMesh : MonoBehaviour
{
    private NavMeshSurface[] _navMeshes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PeriodicallyRefresh());
        _navMeshes = GetComponents<NavMeshSurface>();
    }

    private IEnumerator PeriodicallyRefresh()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (true)
        {
            yield return wait;
            foreach (var navMesh in _navMeshes)
            {
                Debug.Log("Refreshing NavMesh");
                navMesh.BuildNavMesh();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
