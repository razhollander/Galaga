using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using CoreDomain.Services;
using UnityEngine;
using UnityEngine.Audio;
using Untils;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Services;
using Systems;

namespace CoreDomain.Scripts.Services.Audio
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        private const int MaxDecibelLevel = 0;
        private const int MinDecibelLevel = -80;
        private const string FxVolumeName = "FxVolume";
        private const string MasterVolumeName = "MasterVolume";
        private const string MusicVolumeName = "MusicVolume";
        private const string PitchBend = "pitchBend";
        private const string MasterSource = "audioSource_master";
        private const string FXSource = "audioSource_FX";
        private const string MusicSource = "audioSource_music";
        private const string Mixer = "Mixer";
        
        private static readonly List<string> _audioClipsAssetNames = new()
        {
            "GalagaThemeSong",
            "GalagaLevelStartSoundEffect",
            "GalagaFiringSoundEffect",
            "GalagaCoinSoundEffect"
        };

        private Dictionary<AudioChannelType, AudioSource> _channelsByType = new();
        private Dictionary<string, AudioClip> _audioClipsByName = new();
        [SerializeField] private AudioClipsScriptableObject _audioClipsScriptableObject;
        [SerializeField] private AudioMixer _audioMixer;
        
        private IAssetBundleLoaderService _assetsService;
        public AudioService(IAssetBundleLoaderService assetsService)
        {
            _assetsService = assetsService;
        }
        
        public async UniTask SetupAsync()
        {
            if(!(_assetsService.TryInstantiateAssetFromBundle<AudioMixer>(Mixer, Mixer, out _audioMixer) &&
            _assetsService.TryInstantiateAssetFromBundle<AudioSource>(MasterSource, MasterSource, out var masterSource) &&
            _assetsService.TryInstantiateAssetFromBundle<AudioSource>(FXSource, FXSource, out var fxSource) &&
            _assetsService.TryInstantiateAssetFromBundle<AudioSource>(MusicSource, FXSource, out var musicSource)))
            {
                return;
            }

            var rootGameObject = new GameObject("AudioRoot");

            // for clear order, set parent all GameObjects (masterSource, fxSource, musicSource) under rootGameObject
            var tracksToLoad = new List<Task>();

            foreach (var audioClipAssetName in _audioClipsAssetNames)
            {
                AddAudioClip(audioClipAssetName);
            }

            _channelsByType.Add(AudioChannelType.Master, masterSource);
            _channelsByType.Add(AudioChannelType.Fx, fxSource);
            _channelsByType.Add(AudioChannelType.Music, musicSource);
        }
        
        public async UniTask PlayAudio(string audioClipName, AudioChannelType audioChannel, AudioPlayType audioPlayType = AudioPlayType.OneShot)
        {
            if (audioClipName.IsNullOrEmpty())
            {
                return;
            }

            if (!_audioClipsByName.TryGetValue(audioClipName.ToLower(), out var clip))
            {
                LogService.LogError($"No clip of name <{audioClipName}> found");

                return;
            }

            if (!_channelsByType.TryGetValue(audioChannel, out var audioSource))
            {
                LogService.LogError($"No audioChannel of name <{audioChannel}> found");

                return;
            }

            //No point playing sound if were muted
            if (audioSource.mute || !audioSource.enabled)
            {
                return;
            }

            switch (audioPlayType)
            {
                case AudioPlayType.OneShot:
                    audioSource.loop = false;
                    audioSource.PlayOneShot(clip);
                    await UniTask.Delay(System.TimeSpan.FromSeconds(clip.length), false);
                    break;
                case AudioPlayType.Loop:
                    audioSource.clip = clip;
                    audioSource.loop = true;
                    audioSource.Play();
                    break;
            }
        }

        public void StopLoopingSound(string audioClipName, AudioChannelType audioChannel)
        {
            if (!_audioClipsByName.TryGetValue(audioClipName.ToLower(), out var clip))
            {
                LogService.LogError($"No clip of name <{audioClipName}> found");

                return;
            }

            if (!_channelsByType.TryGetValue(audioChannel, out var audioSource))
            {
                LogService.LogError($"No audioChannel of name <{audioChannel}> found");

                return;
            }

            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.Stop();
        }
        
        public void SetChannelVolume(AudioChannelType channel, float volume)
        {
            if (volume is < 0 or > 1)
            {
                LogService.LogError("ChannelVolume must be between 0 and 1");

                return;
            }

            var decibelVolume = NormalizedVolumeToDecibel(volume);

            switch (channel)
            {
                case AudioChannelType.Master:
                    _audioMixer.SetFloat(MasterVolumeName, decibelVolume);

                    break;
                case AudioChannelType.Fx:
                    _audioMixer.SetFloat(FxVolumeName, decibelVolume);

                    break;
                case AudioChannelType.Music:
                    _audioMixer.SetFloat(MusicVolumeName, decibelVolume);

                    break;
            }
        }
        
        private void AddAudioClip(string assetName)
        {
            var clip = _assetsService.InstantiateAssetFromBundle<AudioClip>(assetName, FXSource);
            _audioClipsByName.Add(clip.name, clip);
        }

        private static float NormalizedVolumeToDecibel(float volume)
        {
            return UnityMathUtils.Remap(0, 1, MinDecibelLevel, MaxDecibelLevel, volume);
        }

        private void SetChannelSpeed(AudioChannelType audioChannelType, float speed)
        {
            _channelsByType[audioChannelType].pitch = speed;
            _channelsByType[AudioChannelType.Fx].outputAudioMixerGroup.audioMixer.SetFloat(PitchBend, 1f / speed);
        }
    }

    public class AudioServiceConstants
    {
        public const string FxGoalCelebration = "Fx_GoalCelebration";
        public const string FxMouseClickA = "Fx_MouseClickA";
        public const string FxMouseClickB = "Fx_MouseClickB";
        public const string FxGoalSequence = "Fx_GoalSequence";
        public const string FxTurn = "Fx_Turn";
        public const string MusicElectricBeat = "Music_NadavTrack";
    }
}