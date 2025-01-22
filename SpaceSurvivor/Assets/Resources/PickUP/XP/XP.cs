using UnityEngine;
using UnityEngine.Rendering.Universal;

public class XP : MonoBehaviour
{
    [SerializeField] public string type; 
    [SerializeField] public float Value; 
    [SerializeField] public float LifeGain;
    [SerializeField] private float BaseValue; 
    [SerializeField] public float DecayTime1;
    [SerializeField] public float DecayTime2;
    [SerializeField] public float DecayTime3;
    [SerializeField] public float DecayTime4;
    [SerializeField] public Color ColorStadeFresh;
    [SerializeField] public Color ColorStadeGood;
    [SerializeField] public Color ColorStadeMedium;
    [SerializeField] public Color ColorStadeMediocre;
    [SerializeField] public Color ColorStadePoor;

    [HideInInspector] private float startTime;

    private Light2D xpLight;
    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        startTime = Time.time;
        xpLight = GetComponent<Light2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Définir la valeur d'XP en fonction du type
        BaseValue = type == "Large" ? 1000 : type == "Medium" ? 500 : 100;
    }

    public void Update()
    {
        DefineValue();
    }

    private void DefineValue()
    {
        if (Time.time > DecayTime4)
        {
            Value = BaseValue * 0.3f;
            xpLight.intensity = 1f;
            xpLight.color = ColorStadePoor;
            spriteRenderer.color = ColorStadePoor;
            LifeGain = 0;
        }
        else if (Time.time > DecayTime3)
        {
            Value = BaseValue * 0.5f;
            xpLight.intensity = 2f;
            xpLight.color = ColorStadeMediocre;
            spriteRenderer.color = ColorStadeMediocre;
            LifeGain = 0;
        }
        else if (Time.time > DecayTime2)
        {
            Value = BaseValue * 0.75f;
            xpLight.intensity = 3f;
            xpLight.color = ColorStadeMedium;
            spriteRenderer.color = ColorStadeMedium;
            LifeGain = 0;
        }
        else if (Time.time > DecayTime1)
        {
            Value = BaseValue;
            xpLight.intensity = 4f;
            xpLight.color = ColorStadeGood;
            spriteRenderer.color = ColorStadeGood;
            LifeGain = 0;
        }
        else
        {
            Value = BaseValue * 1.2f;
            xpLight.intensity = 5f;
            xpLight.color = ColorStadeFresh;
            spriteRenderer.color = ColorStadeFresh;
            LifeGain = 1;
        }
    }
}
