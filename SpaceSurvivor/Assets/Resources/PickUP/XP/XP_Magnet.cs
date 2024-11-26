using UnityEngine;
using FMODUnity; 

public class XP_Magnet : MonoBehaviour
{

    [Header("Audio")]
        [SerializeField] public EventReference Small_XP_soundEvent;
        [SerializeField] public EventReference Medium_XP_soundEvent;
        [SerializeField] public EventReference Large_XP_soundEvent;

    [Header("Attraction")]
        public float attractionRadius; 
        public float attractionSpeed;  

    [Header("Other")] 
        public GameObject player;

    [HideInInspector] private FMOD.Studio.EventInstance ShootSoundInstance;

  
    void OnTriggerStay2D(Collider2D other)
    {
        // Vérifier si l'objet touché est un objet XP
        if (other.CompareTag("XP"))
        {
            // Attraction de l'objet XP vers le joueur
            other.transform.position = Vector3.Lerp(other.transform.position, player.transform.position, Time.deltaTime * attractionSpeed);

            float distance = Vector3.Distance(other.transform.position, player.transform.position);
            if (distance < 0.9f)  // Distance d'absorption
            {
                CollectXP(other.gameObject);
            }
        }
    }

    void CollectXP(GameObject xpObject)
    {
        if (xpObject.GetComponent<XP>().type == "Large")
        {
            player.GetComponent<Player_controler>().GainXP(xpObject.GetComponent<XP>().Value);
            PlayLargeXPSound();
        }
        else if (xpObject.GetComponent<XP>().type == "Medium")
        {
            player.GetComponent<Player_controler>().GainXP(xpObject.GetComponent<XP>().Value);
            PlayMediumXPSound();
        }
        else if (xpObject.GetComponent<XP>().type == "Small")
        {
            player.GetComponent<Player_controler>().GainXP(xpObject.GetComponent<XP>().Value);
            PlaySmallXPSound();
        }


        Destroy(xpObject);
    }

    public virtual void PlaySmallXPSound()
    {
        ShootSoundInstance = RuntimeManager.CreateInstance(Small_XP_soundEvent);
        ShootSoundInstance.start();
    }

    public virtual void PlayMediumXPSound()
    {
        ShootSoundInstance = RuntimeManager.CreateInstance(Medium_XP_soundEvent);
        ShootSoundInstance.start();
    }

    public virtual void PlayLargeXPSound()
    {
        ShootSoundInstance = RuntimeManager.CreateInstance(Large_XP_soundEvent);
        ShootSoundInstance.start();
    }
}
