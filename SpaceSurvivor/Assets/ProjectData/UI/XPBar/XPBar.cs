using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class XPBar : MonoBehaviour
{
    [SerializeField] private Image XpSlider;
    [SerializeField] private Gradient XpSliderColor;
    [HideInInspector] public GameObject target;
    public float fillSpeed;
    public int augmentXP;
    private bool isLevelingUp = false;

    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    void Update()
    {
        if (!isLevelingUp) // Only update the XP bar if not leveling up
        {
            float originalValue = target.GetComponent<Player_controler>().XP / target.GetComponent<Player_controler>().maxXP;
            float fillAmount = Mathf.Clamp01(originalValue);
            float overflowXP = target.GetComponent<Player_controler>().XP - target.GetComponent<Player_controler>().maxXP;

            // Animation de la montée de la barre d'XP
            XpSlider.DOFillAmount(fillAmount, fillSpeed);
            XpSlider.DOColor(XpSliderColor.Evaluate(fillAmount), fillSpeed);

            if (target.GetComponent<Player_controler>().XP >= target.GetComponent<Player_controler>().maxXP)
            {
                StartCoroutine(ProcessLevelUp());
            }
        }
    }    

    IEnumerator ProcessLevelUp()
    {
        isLevelingUp = true;

        float overflowXP = target.GetComponent<Player_controler>().XP;
        while (overflowXP >= target.GetComponent<Player_controler>().maxXP)
        {
            float maxXP = target.GetComponent<Player_controler>().maxXP;

            // Animation du remplissage jusqu'à 100%
            Sequence xpSequence = DOTween.Sequence();
            XpSlider.DOColor(XpSliderColor.Evaluate(1), fillSpeed);
            xpSequence.Append(XpSlider.DOFillAmount(1, fillSpeed));
            
            yield return xpSequence.WaitForCompletion();

            target.GetComponent<Player_controler>().XP -= maxXP;
            target.GetComponent<Player_controler>().maxXP += augmentXP;
            overflowXP = target.GetComponent<Player_controler>().XP;
            target.GetComponent<Player_controler>().LevelUp();

            // Réinitialiser la barre d'XP pour la prochaine montée
            XpSlider.fillAmount = 0;
            XpSlider.color = XpSliderColor.Evaluate(0);
        }

        isLevelingUp = false;
    }
}
