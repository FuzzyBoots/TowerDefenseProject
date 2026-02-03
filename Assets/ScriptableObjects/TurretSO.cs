using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Turret", menuName = "Scriptable Objects/Turret")]
public class TurretSO : ScriptableObject
{
    public float _cost;
    GameObject _prefab;
    Image _thumbnail;    
}
