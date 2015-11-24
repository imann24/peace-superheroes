﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {
	public static AudioController Instance;

	public AudioSource Music;
	public AudioSource SFX1;
	public AudioSource SFX2;

	public enum Channel {Music, SFX1, SFX2};
	private Channel currentChannel;

	private Dictionary<Channel, AudioSource> allAudioSources;
	public bool Muted {
		set {
			ToggleMute(value);
		}
	}
	void Awake () {
		// Singleton implementation
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {
		allAudioSources = CreateAudioSourceDictionary();
		SubscribeEvents();
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}

	public void PlayCurrentClip () {
		allAudioSources[currentChannel].Play();
	}

	public void StopCurrentClip () {
		allAudioSources[currentChannel].Stop();
	}

	public void ToggleLoopCurrentClip (bool active) {
		allAudioSources[currentChannel].loop = active;
	}

	public void ToggleMuteCurrentClip (bool muted) {
		allAudioSources[currentChannel].mute = muted;
	}

	public void SetClipVolume (float volume = 1.0f) {
		allAudioSources[currentChannel].volume = volume;
	}

	public void SetClip (AudioClip clip) {
		allAudioSources[currentChannel].clip = clip;
	}

	// Sets the current audio source being modified
	public void SetChannel (Channel currentChannel) {
		this.currentChannel = currentChannel;
	}

	
	// The three audiosources are controller by an enum
	Dictionary<Channel, AudioSource> CreateAudioSourceDictionary () {
		Dictionary<Channel, AudioSource> allAudioSources = new Dictionary<Channel, AudioSource>();
		allAudioSources.Add(Channel.Music, Music);
		allAudioSources.Add(Channel.SFX1, SFX1);
		allAudioSources.Add(Channel.SFX2, SFX2);
		return allAudioSources;

	}

	public void SetMusic (GameState state) {
		SetChannel(Channel.Music);


		    
		PlayCurrentClip();

	}


	public void ToggleMute (bool muted) {
		for (int i = 0; i < Enum.GetNames(typeof(Channel)).Length; i++) {
			allAudioSources[(Channel) i].mute = muted;
		}
	}

	private void toggleMuteMusic (bool muted) {
		SetChannel(Channel.Music);
		ToggleMuteCurrentClip(muted);
	}


	private void unmuteMusic () {
		toggleMuteMusic(false);
	}

	private AudioClip currentClip () {
		return allAudioSources[currentChannel].clip;
	}

	void SubscribeEvents () {

	}

	void UnsubscribeEvents () {

	}

}
