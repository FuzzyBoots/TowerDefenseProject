using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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

    [SerializeField] TMP_Text _warFundsText;
    [SerializeField] TMP_Text _warPurseText;

    private void Start()
    {
        Assert.IsNotNull(_warFundsText, "War Funds text object not specified.");
        Assert.IsNotNull(_warPurseText, "War Purse text object not specified.");

        EventManager.OnFundsChange += ModifyFunds;
        EventManager.OnPurseChange += ModifyPurse;
    }

    private void OnDestroy()
    {
        EventManager.OnFundsChange -= ModifyFunds;
        EventManager.OnPurseChange -= ModifyPurse;
    }

    private void ModifyFunds(int amount)
    {
        // Eventually, we want to do some special effect like tweening
        _warFundsText.text = $"Warfunds:\n${amount}";
    }

    private void ModifyPurse(int amount)
    {
        // Eventually, we want to do some special effect like tweening
        _warPurseText.text = $"WarPurse:\n${amount}";
    }
}
