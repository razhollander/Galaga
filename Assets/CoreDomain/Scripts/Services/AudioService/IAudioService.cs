using Cysharp.Threading.Tasks;

namespace CoreDomain.Services
{
    public interface IAudioService
    {
        UniTask PlayAudio(string audioClipName, AudioChannelType audioChannel, AudioPlayType audioPlayType);
        void StopAllSounds();
    }
}