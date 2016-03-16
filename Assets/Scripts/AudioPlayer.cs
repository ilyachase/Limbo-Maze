using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	public AudioClip[] Soundtracks;
	public float fadeSpeed;
	AudioSource aud;

	void Start() {
		aud = GetComponent<AudioSource>();
		aud.volume = 0;
		aud.PlayOneShot(Soundtracks[Random.Range(0, Soundtracks.Length - 1)]);
	}

	// Update is called once per frame
	void Update () {
		if (!aud.isPlaying) {
			aud.PlayOneShot(Soundtracks[Random.Range(0, Soundtracks.Length - 1)]);
		}
		if (aud.volume < 1)
			FadeIn();
	}

	void FadeIn() {
		aud.volume += fadeSpeed * Time.deltaTime;
	}

	void FadeOut() {
		aud.volume += fadeSpeed * Time.deltaTime;
	}
}
