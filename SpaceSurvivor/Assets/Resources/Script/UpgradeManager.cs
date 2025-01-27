using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] public EventReference LevelUp_soundEvent;
    [SerializeField] public GameObject LevelUpPanel;
    [SerializeField] public Button Button1;               
    [SerializeField] public Button Button2;               
    [SerializeField] public Button Button3;              
    [SerializeField] public TMP_Text Button1Text;             
    [SerializeField] public TMP_Text Button2Text;             
    [SerializeField] public TMP_Text Button3Text;      
    [SerializeField] public TMP_Text Description1Text;             
    [SerializeField] public TMP_Text Description2Text;             
    [SerializeField] public TMP_Text Description3Text;          
    [SerializeField] public TMP_Text WeaponName1Text;             
    [SerializeField] public TMP_Text WeaponName2Text;             
    [SerializeField] public TMP_Text WeaponName3Text;   
    
    [HideInInspector] public GameObject Player;
    [HideInInspector] public FMOD.Studio.EventInstance LevelUpInstance;

    [HideInInspector] private bool ChooseUpgrade;

    void Start()
    {
        Player = Player_controler.Instance.gameObject; 
    }

    void FixedUpdate()
    {
        if (Player.GetComponent<Player_controler>().upgraded)
        {
            if (!ChooseUpgrade)
            {
                ChooseUpgrade = true;
                LevelUp();
            }
        }
    }

    public void LevelUp()
    {
        // Met le jeu en pause
        Time.timeScale = 0f;
        ShuffleUpgrade();
        LevelUpPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        // Remet le temps à la normale
        Time.timeScale = 1f;  
        LevelUpPanel.SetActive(false);
        ChooseUpgrade = false;  
        Player.GetComponent<Player_controler>().upgraded = false;
    }

    public void ShuffleUpgrade()
    {
        List<Upgrade> allUpgrades = new List<Upgrade>();

        foreach (var weapon in Player.GetComponent<Player_controler>().weapons)
        {
            allUpgrades.AddRange(weapon.availableUpgrades);
        }

        // Sélectionner trois upgrades aléatoires
        var randomUpgrades = allUpgrades.OrderBy(x => Random.value).Take(3).ToList();

        DisplayUpgrades(randomUpgrades);
    }

    public void DisplayUpgrades(List<Upgrade> upgrades)
    {
        RectTransform ButtonTransform1 = Button1.GetComponent<RectTransform>();
        RectTransform ButtonTransform2 = Button2.GetComponent<RectTransform>();
        RectTransform ButtonTransform3 = Button3.GetComponent<RectTransform>();

        PlayLevelUpSound();

        int upgradeCount = upgrades.Count;
        switch (upgradeCount)
        {
            case 1:
                Button2.gameObject.SetActive(false);
                Button3.gameObject.SetActive(false);

                Button1Text.text = upgrades[0].Name;

                WeaponName1Text.text = upgrades[0].PieceName;

                Description1Text.text = upgrades[0].Description;

                // Ajouter des listeners pour les boutons
                Button1.onClick.RemoveAllListeners();

                Button1.onClick.AddListener(() => MakeUpgrade(upgrades[0], upgrades[0].PieceName));

                ButtonTransform1.anchoredPosition = new Vector2(0, -55);

                break;
            case 2:
                Button2.gameObject.SetActive(true);
                Button3.gameObject.SetActive(false);

                Button1Text.text = upgrades[0].Name;
                Button2Text.text = upgrades[1].Name;

                WeaponName1Text.text = upgrades[0].PieceName;
                WeaponName2Text.text = upgrades[1].PieceName;

                Description1Text.text = upgrades[0].Description;
                Description2Text.text = upgrades[1].Description;

                // Ajouter des listeners pour les boutons
                Button1.onClick.RemoveAllListeners();
                Button2.onClick.RemoveAllListeners();

                Button1.onClick.AddListener(() => MakeUpgrade(upgrades[0], upgrades[0].PieceName));
                Button2.onClick.AddListener(() => MakeUpgrade(upgrades[1], upgrades[1].PieceName));

                ButtonTransform1.anchoredPosition = new Vector2(-280, -55);
                ButtonTransform2.anchoredPosition = new Vector2(280, -55);

                break;
            default:
                Button2.gameObject.SetActive(true);
                Button3.gameObject.SetActive(true);

                Button1Text.text = upgrades[0].Name;
                Button2Text.text = upgrades[1].Name;
                Button3Text.text = upgrades[2].Name;

                WeaponName1Text.text = upgrades[0].PieceName;
                WeaponName2Text.text = upgrades[1].PieceName;
                WeaponName3Text.text = upgrades[2].PieceName;

                Description1Text.text = upgrades[0].Description;
                Description2Text.text = upgrades[1].Description;
                Description3Text.text = upgrades[2].Description;

                // Ajouter des listeners pour les boutons
                Button1.onClick.RemoveAllListeners();
                Button2.onClick.RemoveAllListeners();
                Button3.onClick.RemoveAllListeners();

                Button1.onClick.AddListener(() => MakeUpgrade(upgrades[0], upgrades[0].PieceName));
                Button2.onClick.AddListener(() => MakeUpgrade(upgrades[1], upgrades[1].PieceName));
                Button3.onClick.AddListener(() => MakeUpgrade(upgrades[2], upgrades[2].PieceName));

                // Positionner les trois boutons de manière équilibrée
                ButtonTransform1.anchoredPosition = new Vector2(-560, -55);
                ButtonTransform2.anchoredPosition = new Vector2(0, -55);
                ButtonTransform3.anchoredPosition = new Vector2(560, -55);

                break;
        }
    }

    public void MakeUpgrade(Upgrade selectedUpgrade, string weaponName)
    {
        // Trouver l'arme correspondant à `weaponName`
        foreach (var weapon in Player.GetComponent<Player_controler>().weapons)
        {
            if (weapon.GetType().Name == weaponName)
            {
                weapon.ApplyUpgrade(selectedUpgrade);  // Appliquer l'upgrade
                break;
            }
        }

        ResumeGame();
    }

    public virtual void PlayLevelUpSound()
    {
        LevelUpInstance = RuntimeManager.CreateInstance(LevelUp_soundEvent);
        LevelUpInstance.start();
    }
}
