using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marshtown.UnitTests.PlayMode
{
    internal static class GameObjectTestExtension
    {
        internal static GameObject ThenSetActive(this GameObject gameObj, bool isActive)
        {
            gameObj.SetActive(isActive);
            return gameObj;
        }

        internal static T ThenAddComponent<T>(this GameObject gameObj) where T : Component
        {
            return gameObj.AddComponent<T>();
        }
    }
}
