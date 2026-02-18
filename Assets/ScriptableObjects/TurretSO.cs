using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Turret", menuName = "Scriptable Objects/Turret")]
public class TurretSO : ScriptableObject
{
    public int _cost;
    public GameObject _prefab;
    public Image _thumbnail;

    public float _attackInterval = 1f;
    public float _rotationSpeed = 5f;
    public float _attackRange = 10f;
    public float _attackDamage = 10f;

    public TurretSO _upgradeOption;
    public int _upgradeCost;
    public float _scrapValue;
}
