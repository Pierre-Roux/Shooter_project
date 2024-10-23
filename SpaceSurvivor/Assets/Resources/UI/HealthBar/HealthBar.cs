using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthSlider;
    [SerializeField] private Gradient healthSliderColor;
    [HideInInspector] public GameObject target;
    public float fillSpeed;
    
    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    void Update()
    {
        float fillAmount = target.GetComponent<Player_controler>().health / target.GetComponent<Player_controler>().maxHealth;
        healthSlider.DOFillAmount(fillAmount,fillSpeed);
        healthSlider.DOColor(healthSliderColor.Evaluate(fillAmount),fillAmount);
    }


}
