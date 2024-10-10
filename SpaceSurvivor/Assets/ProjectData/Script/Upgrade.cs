using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public string PieceName;
    public string Name;
    public float fireCooldownReduction;
    public Color colorBonus;
    public int intensityBonus;
    public int damageBonus;

    public Upgrade(string pieceName, string name, float cooldown, Color color, int intensity, int damage)
    {
        PieceName = pieceName;
        Name = name;
        fireCooldownReduction = cooldown;
        colorBonus = color;
        intensityBonus = intensity;
        damageBonus = damage;
    }
}
