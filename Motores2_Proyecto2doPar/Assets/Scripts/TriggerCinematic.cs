using UnityEngine;
using UnityEngine.Playables;

public class TriggerCinematic : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject player;
    public MonoBehaviour playerControl;
    public GameObject playerCam;
    public GameObject cinematicCam;
    public AudioSource mainMusic; // música o audio que suena antes (intro)

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.gameObject == player)
        {
            hasPlayed = true;

            // Detener música anterior (opcional)
            if (mainMusic != null)
                mainMusic.Stop();

            // Desactivar control del jugador y cambiar cámara
            playerControl.enabled = false;
            playerCam.SetActive(false);
            cinematicCam.SetActive(true);

            // Reproducir el nuevo timeline (con su propio audio)
            timeline.Play();
        }
    }
}
