using UnityEngine;
using UnityEngine.Playables;

public class SimpleCutsceneEnd : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject cinematicCam;   // Cámara de la cinemática (o padre de varias)
    public GameObject playerCam;      // Cámara del jugador
    public MonoBehaviour playerControl; // Script de control del jugador
    public GameObject cinematicClone; // Opcional

    void Start()
    {
        timeline.stopped += OnTimelineEnd;
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        cinematicCam.SetActive(false);
        playerCam.SetActive(true);
        playerControl.enabled = true;

        if (cinematicClone != null)
            cinematicClone.SetActive(false);
    }
}
