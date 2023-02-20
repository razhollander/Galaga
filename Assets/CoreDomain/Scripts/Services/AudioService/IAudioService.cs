using Cysharp.Threading.Tasks;

namespace CoreDomain.Scripts.Services.Audio
{
    public interface IAudioService
    {
        UniTask PlayAudio(string audioClipName, AudioChannelType audioChannel, AudioPlayType audioPlayType);

        void StopLoopingSound(string audioClipName, AudioChannelType audioChannel);
        void SetChannelVolume(AudioChannelType channel, float volume);
    }
}