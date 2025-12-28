using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TraversalAgent : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] Transform _target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateTarget());
    }

    IEnumerator UpdateTarget()
    {
        while (true)
        {
            _agent.destination = _target.position;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
