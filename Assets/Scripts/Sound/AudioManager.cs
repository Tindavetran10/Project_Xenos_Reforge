using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager Instance;

	public Sound[] sounds;

	void Awake ()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		} else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = s.mixer;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play (string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Play();
	}
	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Stop();
	}
}
