using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CoreDomain.Scripts.Extensions;

namespace CoreDomain.Services
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioClipsScriptableObject _audioClipsScriptableObject;
        [SerializeField] private AudioSource _masterAudioSource;
        [SerializeField] private AudioSource _FxAudioSource;
        [SerializeField] private AudioSource _MusicAudioSource;
        
        private Dictionary<AudioChannelType, AudioSource> _audioSourceByChannel = new();
        private Dictionary<string, AudioClip> _audioClipsByName = new();
        
        private void Awake()
        {
            foreach (var clip in _audioClipsScriptableObject.AudioClips)
            {
                _audioClipsByName.Add(clip.name, clip);
            }

            _audioSourceByChannel.Add(AudioChannelType.Master, _masterAudioSource);
            _audioSourceByChannel.Add(AudioChannelType.Fx, _FxAudioSource);
            _audioSourceByChannel.Add(AudioChannelType.Music, _MusicAudioSource);
        }
        
        public async UniTask PlayAudio(string audioClipName, AudioChannelType audioChannel, AudioPlayType audioPlayType = AudioPlayType.OneShot)
        {
            if (audioClipName.IsNullOrEmpty())
            {
                return;
            }

            if (!_audioClipsByName.TryGetValue(audioClipName, out var clip))
            {
                LogService.LogError($"No clip of name {audioClipName} found");

                return;
            }

            if (!_audioSourceByChannel.TryGetValue(audioChannel, out var audioSource))
            {
                LogService.LogError($"No audioChannel of name {audioChannel} found");

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

        public void StopAllSounds()
        {
            foreach (var keyValuePair in _audioSourceByChannel)
            {
                keyValuePair.Value.Stop();
            }
        }
    }
}