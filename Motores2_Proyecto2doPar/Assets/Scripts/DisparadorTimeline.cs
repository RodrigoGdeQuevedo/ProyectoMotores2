using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class DisparadorTimeline : MonoBehaviour
{
    [Tooltip("La tecla que disparará la animación de Timeline.")]
    public KeyCode teclaParaActivar = KeyCode.T;

    private PlayableDirector miTimeline;

    void Awake()
    {
        miTimeline = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaParaActivar))
        {
            Debug.Log($"¡Tecla {teclaParaActivar} presionada! Reproduciendo Timeline.");
            miTimeline.Play();
        }
    }
}