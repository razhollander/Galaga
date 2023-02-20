using System.Collections.Generic;
using UnityEngine;

namespace CoreDomain.Scripts.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioClips", menuName = "Game/Audio/AudioClips")]
    public class AudioClipsScriptableObject : ScriptableObject
    {
        public List<AudioClip> AudioClips;
    }
}