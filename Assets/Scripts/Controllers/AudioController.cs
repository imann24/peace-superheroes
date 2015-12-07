﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour, System.IComparable<AudioController> {
	public static AudioController Instance;

	public AudioSource Music;
	public AudioSource SFX1;
	public AudioSource SFX2;

	public enum Channel {Music, SFX1, SFX2};
	private Channel currentChannel;

	private Dictionary<Channel, AudioSource> allAudioSources;

	private bool _muted;
	public bool Muted {
		set {
			ToggleMute(value);
			_muted = !value;
		}

		get {
			return _muted;
		}
	}

	// ---------------------------------------------
	// SFX:
	public AudioClip Teleport;
	public AudioClip PhraseCorrect;
	public AudioClip PhraseIncorrect;

	public static int Count = 0;
	public int ID = Count++;

	// ---------------------------------------------
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
		if (Muted) {
			return;
		}
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


	public void ToggleMute (bool unmuted) {
		for (int i = 0; i < Enum.GetNames(typeof(Channel)).Length; i++) {
			allAudioSources[(Channel) i].mute = !unmuted;
		}
	}

	public int CompareTo (AudioController other) {
		return other.ID == this.ID ? 0 : -1;
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

	private void playTeleportSFX (int lane) {
		if (allAudioSources[currentChannel].isPlaying && currentClip() == Teleport) {
			return;
		}
		SetChannel(Channel.SFX1);
		SetClip(Teleport);
		PlayCurrentClip();
	}

	private void playCorrectPhraseSFX () {
		SetChannel(Channel.SFX1);
		SetClip(PhraseCorrect);
		PlayCurrentClip();
	}

	private void playIncorrectPhraseSFX () {
		SetChannel(Channel.SFX1);
		SetClip(PhraseIncorrect);
		PlayCurrentClip();
	}

	private void playPhraseSelectionSFX (Quality phraseQuality) {
		if (phraseQuality == Quality.Good ||
		    phraseQuality == Quality.Great) {
			playCorrectPhraseSFX();
		} else {
			playIncorrectPhraseSFX();
		}
	}

	void SubscribeEvents () {
		LaneSwitchController.OnSwitchToLane += playTeleportSFX;
		PhraseApprover.OnPhraseChoice += playPhraseSelectionSFX;
		PhraseSelector.OnPhraseChoice += playPhraseSelectionSFX;
	}

	void UnsubscribeEvents () {
		LaneSwitchController.OnSwitchToLane -= playTeleportSFX;
		PhraseApprover.OnPhraseChoice -= playPhraseSelectionSFX;
		PhraseSelector.OnPhraseChoice -= playPhraseSelectionSFX;
	}

}
