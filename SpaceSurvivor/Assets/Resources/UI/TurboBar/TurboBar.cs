using UnityEngine;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class TurboBar : MonoBehaviour
{
    [SerializeField] private Image turboSlider;
    [SerializeField] private Gradient turboSliderColor;
    [HideInInspector] public GameObject target;
    public float fillSpeed;
    
    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    void Update()
    {
        float fillAmount = target.GetComponent<Player_controler>().turbo / target.GetComponent<Player_controler>().maxTurbo;
        turboSlider.DOFillAmount(fillAmount,fillSpeed);
        turboSlider.DOColor(turboSliderColor.Evaluate(fillAmount),fillAmount);
    }


}
