using System.Collections.Generic;
using UnityEngine;

namespace CoreDomain.Services
{
    [CreateAssetMenu(fileName = "AudioClips", menuName = "Game/Audio/AudioClips")]
    public class AudioClipsScriptableObject : ScriptableObject
    {
        public List<AudioClip> AudioClips;
    }
}