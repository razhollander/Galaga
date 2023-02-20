using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using CoreDomain.Services;
using UnityEngine;
using UnityEngine.Audio;
using Untils;
using CoreDomain.Scripts.Extensions;

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

        [SerializeField] private AudioClipsScriptableObject _audioClipsScriptableObject;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _masterAudioSource;
        [SerializeField] private AudioSource _FxAudioSource;
        [SerializeField] private AudioSource _MusicAudioSource;
        
        private Dictionary<AudioChannelType, AudioSource> _channelsByType = new();
        private Dictionary<string, AudioClip> _audioClipsByName = new();
        
        private void Awake()
        {
            foreach (var clip in _audioClipsScriptableObject.AudioClips)
            {
                _audioClipsByName.Add(clip.name, clip);
            }

            _channelsByType.Add(AudioChannelType.Master, _masterAudioSource);
            _channelsByType.Add(AudioChannelType.Fx, _FxAudioSource);
            _channelsByType.Add(AudioChannelType.Music, _MusicAudioSource);
        }
        
        public async UniTask PlayAudio(string audioClipName, AudioChannelType audioChannel, AudioPlayType audioPlayType = AudioPlayType.OneShot)
        {
            if (audioClipName.IsNullOrEmpty())
            {
                return;
            }

            if (!_audioClipsByName.TryGetValue(audioClipName, out var clip))
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
            if (!_audioClipsByName.TryGetValue(audioClipName, out var clip))
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
}