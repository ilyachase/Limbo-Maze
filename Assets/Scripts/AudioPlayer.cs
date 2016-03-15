using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	public AudioClip[] Soundtracks;
	public float fadeSpeed;
	AudioSource audio;

	void Start() {
		audio = GetComponent<AudioSource>();
		audio.volume = 0;
		audio.PlayOneShot(Soundtracks[Random.Range(0, Soundtracks.Length - 1)]);
	}

	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying) {
			audio.PlayOneShot(Soundtracks[Random.Range(0, Soundtracks.Length - 1)]);
		}
		if (audio.volume < 1)
			FadeIn();
	}

	void FadeIn() {
		audio.volume += fadeSpeed * Time.deltaTime;
	}

	void FadeOut() {
		audio.volume += fadeSpeed * Time.deltaTime;
	}
}
