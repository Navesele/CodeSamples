using System.Collections.Generic;
using Marshtown.Extensions;
using System.Linq;
using System;

namespace Marshtown.DialogueComponents
{
    public class DialogueLine
    {
        public string Text { get; private set; }
        public DialogueLineType LineType { get; private set; }
        public string Speaker { get; private set; }
        public ActorEmotionType Emotion { get; private set; }
        public ActorPositionType Position { get; private set; }
        public string FeelsLike { get; private set; }

        private readonly Dictionary<DialogueAttributeType, string> _attributes;

        public DialogueLine(string textLine)
        {
            Text = textLine;
            _attributes = new Dictionary<DialogueAttributeType, string>();
        }

        public void ReadTags(List<string> dialogueTags)
        {
            _attributes.Clear();

            if (dialogueTags == null || dialogueTags.Count == 0)
            {
                return;
            }

            var tags = dialogueTags.Where(t => !string.IsNullOrWhiteSpace(t) && t.Contains(":") && t.Trim().Length > 2).ToList();

            if (tags == null || tags.Count == 0)
            {
                return;
            }

            foreach (var tag in tags)
            {
                string[] values = tag.Split(':');

                DialogueAttributeType attributType = values[0].Trim().ToEnumOrDefault<DialogueAttributeType>();

                if (attributType != DialogueAttributeType.None)
                {
                    if (_attributes.ContainsKey(attributType))
                    {
                        _attributes[attributType] = values[1].Trim();
                    }
                    else
                    {
                        _attributes.Add(attributType, values[1].Trim());
                    }
                }
            }

            SetupLineAttributes();
        }

        private void SetupLineAttributes()
        {
            LineType = GetAttributeValueOrDefault<DialogueLineType>(DialogueAttributeType.LineType);
            Speaker = GetAttributeStringValue(DialogueAttributeType.Speaker);
            Emotion = GetAttributeValueOrDefault<ActorEmotionType>(DialogueAttributeType.Emotion);
            Position = GetAttributeValueOrDefault<ActorPositionType>(DialogueAttributeType.Position);
            FeelsLike = GetAttributeStringValue(DialogueAttributeType.FeelsLike);
        }

        private string GetAttributeStringValue(DialogueAttributeType attribute)
        {
            if (attribute == DialogueAttributeType.None)
            {
                return string.Empty;
            }

            if (_attributes.TryGetValue(attribute, out string result))
            {
                return result;
            }
            
            return string.Empty;
        }

        private T GetAttributeValueOrDefault<T>(DialogueAttributeType attribute) where T : struct, IConvertible
        {
            string stringValue = string.Empty;

            if (attribute == DialogueAttributeType.None)
            {
                stringValue = string.Empty;
            }

            if (_attributes.TryGetValue(attribute, out string result))
            {
                stringValue = result;
            }

            return stringValue.ToEnumOrDefault<T>();
        }
    }
}
