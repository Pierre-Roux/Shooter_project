using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public string ID;
    public string PieceName;
    public string Name;
    public string Description;
    public float fireCooldownReduction;
    public Color colorBonus;
    public int intensityBonus;
    public int damageBonus;
    public int BulletNumber;
    public float Range;

    public Upgrade(string Identifier ,string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, float range, string description)
    {
        ID = Identifier;
        PieceName = pieceName;
        Name = name;
        fireCooldownReduction = cooldown;
        colorBonus = color;
        intensityBonus = intensity;
        damageBonus = damage;
        Description = description;
        BulletNumber = bulletNumber;
        Range = range;
    }
}
