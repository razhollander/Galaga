using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Services.Logs.Base;
using UnityEngine;
using UnityEngine.Audio;
using Untils;
using CoreDomain.Scripts.Extensions;
using Systems;

namespace CoreDomain.Scripts.Services.AudioService
{
    public class AudioService : IAudioService
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
        private static readonly List<string> _allMusicTracks = new()
        {
            
        };
        private static readonly List<string> _allSoundFXs = new()
        {
            
        };
        private AudioMixer _audioMixer;

        private float _gameAnimationSpeed = 1.3f;

        private Dictionary<AudioChannelType, AudioSource> _channelsByType = new();
        private Dictionary<string, AudioClip> _audioClipsByName = new();

        private AssetBundleSystem _assetsService;
        public AudioService(AssetBundleSystem assetsService)
        {
            _assetsService = assetsService;
        }
        
        public async UniTask SetupAsync()
        {
            _audioMixer = _assetsService.LoadAssetFromBundle<AudioMixer>(Mixer, Mixer);
            var rootGameObject = new GameObject("AudioRoot");

            var masterSource = _assetsService.LoadAssetFromBundle<AudioSource>(MasterSource, MasterSource);
            var fxSource = _assetsService.LoadAssetFromBundle<AudioSource>(FXSource, FXSource);
            var musicSource = _assetsService.LoadAssetFromBundle<AudioSource>(MusicSource, FXSource);

            // for clear order, set parent all GameObjects (masterSource, fxSource, musicSource) under rootGameObject
            var tracksToLoad = new List<Task>();

            foreach (var musicTrack in _allMusicTracks)
            {
                tracksToLoad.Add(AddAudioClip(musicTrack));
            }

            foreach (var fxClip in _allSoundFXs)
            {
                tracksToLoad.Add(AddAudioClip(fxClip));
            }

            await Task.WhenAll(tracksToLoad);
            
            _channelsByType.Add(AudioChannelType.Master, masterSource);
            _channelsByType.Add(AudioChannelType.Fx, fxSource);
            _channelsByType.Add(AudioChannelType.Music, musicSource);
            
            SetChannelSpeed(AudioChannelType.Fx, _gameAnimationSpeed);
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
        
        private async Task AddAudioClip(string name)
        {
            var clip = _assetsService.LoadAssetFromBundle<AudioClip>(name, FXSource);
            _audioClipsByName.Add(name.ToLower(), clip);
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