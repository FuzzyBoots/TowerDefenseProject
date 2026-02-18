using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int _warFunds;
    [SerializeField] int _warPurse;

    private void Start()
    {
        EventManager.OnFundsChange?.Invoke(_warFunds);
        EventManager.OnPurseChange?.Invoke(_warPurse);
    }

    public void DeductFunds(int amount)
    {
        _warFunds -= amount;
        EventManager.OnFundsChange.Invoke(_warFunds); 
    }

    public void AddPurse(int amount)
    {
        _warPurse += amount;
        EventManager.OnPurseChange.Invoke(_warPurse);
    }

    public void TransferFromPurseToFunds()
    {
        _warPurse += _warFunds;
        _warFunds = 0;

        EventManager.OnFundsChange.Invoke(_warFunds);
        EventManager.OnPurseChange.Invoke(_warPurse);
    }

    public int GetWarFunds()
    {
        return _warFunds;
    }
}
