using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class XP : MonoBehaviour
{
    [SerializeField] public string type; 
    [SerializeField] public int Value; 
    [SerializeField] public float DecayTime; // Durée pour le premier déclin
    [SerializeField] private float DecayTime2; // Durée pour le second déclin
    [SerializeField] private Color startColor = Color.yellow; // Couleur initiale de la lumière
    [SerializeField] private Color endColor = Color.clear; // Couleur de fin (dissipation)

    private Light2D xpLight;

    public void Start()
    {
        xpLight = GetComponent<Light2D>();
        if (xpLight == null)
        {
            Debug.LogWarning("Light2D non trouvée sur l'XP !");
            return;
        }

        // Définir la valeur d'XP en fonction du type
        Value = type == "Large" ? 45 : type == "Medium" ? 15 : 3;

        // Déclencher l'animation de déclin
        StartDecayEffects();
    }

    private void StartDecayEffects()
    {
        // Premier déclin : réduire l'intensité et la couleur de la lumière, diminuer la valeur de l'XP
        DOTween.To(() => xpLight.intensity, x => xpLight.intensity = x, 3f, DecayTime)
            .SetEase(Ease.OutQuad);
        
        DOTween.To(() => GetComponent<SpriteRenderer>().color, x => GetComponent<SpriteRenderer>().color = x, startColor, DecayTime)
            .SetEase(Ease.OutQuad);

        DOTween.To(() => xpLight.color, x => xpLight.color = x, startColor * 0.5f, DecayTime)
            .SetEase(Ease.OutQuad);
        
        DOTween.To(() => Value, x => Value = x, Mathf.CeilToInt(Value / 2), DecayTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // Deuxième phase de déclin après la première
                DOTween.To(() => xpLight.intensity, x => xpLight.intensity = x, 1, DecayTime2)
                    .SetEase(Ease.OutQuad);

                DOTween.To(() => xpLight.color, x => xpLight.color = x, endColor, DecayTime2)
                    .SetEase(Ease.OutQuad);

                DOTween.To(() => GetComponent<SpriteRenderer>().color, x => GetComponent<SpriteRenderer>().color = x, endColor, DecayTime2)
                    .SetEase(Ease.OutQuad);

                DOTween.To(() => Value, x => Value = x, 1, DecayTime2)
                    .SetEase(Ease.Linear);
            });
    }
}
