using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Button upgradeRaptorButton;
    public Button upgradeStegoButton;
    public Button upgradeTrexButton;

    private int raptorUpgradeCost = 200;
    private int stegoUpgradeCost = 200;
    private int trexUpgradeCost = 200;

    private MeatManager meatManager;

    void Start()
    {
        meatManager = FindObjectOfType<MeatManager>();

        upgradeRaptorButton.onClick.AddListener(() => UpgradeRaptor());
        upgradeStegoButton.onClick.AddListener(() => UpgradeStego());
        upgradeTrexButton.onClick.AddListener(() => UpgradeTrex());
    }

    void UpgradeRaptor()
    {
        if (MeatManager.meat >= raptorUpgradeCost)
        {
            meatManager.AddMeat(-raptorUpgradeCost);  //Spend meat

            RaptorDefenderNew[] raptorDefenders = FindObjectsOfType<RaptorDefenderNew>();
            foreach (var raptor in raptorDefenders)
            {
                raptor.Upgrade();
                raptor.ResetHealth();
            }

            DefenderFactoryNew.UpgradeDefender(DefenderFactoryNew.DefenderType.RaptorDefenderNew);
            raptorUpgradeCost += 100; //Increase cost
        }
    }

    void UpgradeStego()
    {
        if (MeatManager.meat >= stegoUpgradeCost)
        {
            meatManager.AddMeat(-stegoUpgradeCost);  //Spend meat

            StegoDefenderNew[] stegoDefenders = FindObjectsOfType<StegoDefenderNew>();
            foreach (var stego in stegoDefenders)
            {
                stego.Upgrade();
                stego.ResetHealth();
            }

            DefenderFactoryNew.UpgradeDefender(DefenderFactoryNew.DefenderType.StegoDefenderNew);
            stegoUpgradeCost += 100; //Increase cost
        }
    }

    void UpgradeTrex()
    {
        if (MeatManager.meat >= trexUpgradeCost)
        {
            meatManager.AddMeat(-trexUpgradeCost);  //Spend meat

            TrexDefenderNew[] trexDefenders = FindObjectsOfType<TrexDefenderNew>();
            foreach (var trex in trexDefenders)
            {
                trex.Upgrade();
                trex.ResetHealth();
            }

            DefenderFactoryNew.UpgradeDefender(DefenderFactoryNew.DefenderType.TrexDefenderNew);
            trexUpgradeCost += 100; //Increase cost
        }
    }
}
