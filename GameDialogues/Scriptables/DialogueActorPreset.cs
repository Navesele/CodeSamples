using System.Collections.Generic;
using UnityEngine;

namespace Marshtown.DialogueComponents
{
    [CreateAssetMenu(fileName = "DialogueActor", menuName = "Custom tools/Dialogues/New dialogue actor")]
    public class DialogueActorPreset : ScriptableObject
    {
        public string DisplayName => _displayName;
        public string InternalName => _internalName;

        [SerializeField]
        private string _internalName;
        [SerializeField]
        private string _displayName;
        [SerializeField]
        private Sprite _defaultEmotion;
        [SerializeField]
        private List<ActorEmotion> _actorEmotions;

        private Dictionary<ActorEmotionType, Sprite> _emotions = new Dictionary<ActorEmotionType, Sprite>();

        void OnEnable()
        {
            InitializeEmotionDictionary(_actorEmotions);
        }

        public Sprite GetEmotionSpriteByType(ActorEmotionType emotionType)
        {
            if (_emotions.TryGetValue(emotionType, out Sprite result))
            {
                return result;
            }

            return _defaultEmotion;
        }

        private void InitializeEmotionDictionary(List<ActorEmotion> emotions)
        {
            if (emotions is null)
            {
                return;
            }

            _emotions.Clear();

            foreach (var emotion in emotions)
            {
                if (!_emotions.ContainsKey(emotion.EmotionType))
                {
                    _emotions.Add(emotion.EmotionType, emotion.EmotionImage);
                }
            }
        }
    }
}
