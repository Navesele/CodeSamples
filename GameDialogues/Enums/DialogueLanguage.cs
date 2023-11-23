using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Marshtown.Attributes;

namespace Marshtown.DialogueComponents
{
    public enum DialogueLanguage
    {
        [StringValueAttribute("en")]
        English,
        [StringValueAttribute("ru")]
        Russian
    }
}
