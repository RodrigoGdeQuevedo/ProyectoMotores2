using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [Header("Timeline Reference")]
    public PlayableDirector timeline;
    
    [Header("Trigger Settings")]
    public string playerTag = "Player";
    
    private void Start()
    {
        if (timeline == null)
        {
            timeline = GetComponent<PlayableDirector>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (timeline != null)
            {
                timeline.Play();
                Debug.Log("Timeline activada por trigger");
            }
            else
            {
                Debug.LogError("No hay Timeline asignada en el objeto: " + gameObject.name);
            }
        }
    }
}