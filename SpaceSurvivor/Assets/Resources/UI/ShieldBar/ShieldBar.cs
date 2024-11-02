using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private Image ShieldSlider;
    [SerializeField] private Gradient ShieldSliderColor;
    [HideInInspector] public GameObject target;
    public float fillSpeed;
    
    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    void Update()
    {
        float fillAmount = target.GetComponent<Player_controler>().shield / target.GetComponent<Player_controler>().maxShield;
        ShieldSlider.DOFillAmount(fillAmount,fillSpeed);
        ShieldSlider.DOColor(ShieldSliderColor.Evaluate(fillAmount),fillAmount);
    }


}
