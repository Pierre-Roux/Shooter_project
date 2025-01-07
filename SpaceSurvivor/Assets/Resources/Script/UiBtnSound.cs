using FMODUnity;
using UnityEngine;

public class UiBtnSound : MonoBehaviour
{

    [SerializeField] public EventReference BtnClick_soundEvent;
    [HideInInspector] public FMOD.Studio.EventInstance BtnClickInstance;
    // Start is called before the first frame update

    public virtual void PlayBtnSound()
    {
        BtnClickInstance = RuntimeManager.CreateInstance(BtnClick_soundEvent);
        BtnClickInstance.start();
    }
}
