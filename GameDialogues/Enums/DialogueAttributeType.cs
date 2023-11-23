using Marshtown.Attributes;

namespace Marshtown.DialogueComponents
{
    public enum DialogueAttributeType
    {
        None,
        [StringValueAttribute("LineType")]
        LineType,
        [StringValueAttribute("Speaker")]
        Speaker,
        [StringValueAttribute("Emotion")]
        Emotion,
        [StringValueAttribute("FeelsLike")]
        FeelsLike,
        [StringValueAttribute("Position")]
        Position
    }
}
