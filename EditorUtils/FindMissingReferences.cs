using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Marshtown.EditorUtils
{
    public static class FindMissingReferences
    {
        /// <summary>
        /// Search for missing UnityEvent references in the scene
        /// </summary>
        [MenuItem("Tools/Search for missing UnityEvent references in scene")]
        private static void FindMissingUnityEventReferencesInScene()
        {
            bool isReferenceMissing = false;

            // Get all the gameObjects in the scene. Active and disabled
            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
            {
                // Skip the gameObject if for some reason it is null
                if (gameObject == null)
                {
                    Debug.LogWarning("Skipping missing gameObject", gameObject);
                    continue;
                }

                // Get all components on the gameObject
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    // Skip the component if for some reason it is null
                    if (component == null)
                    {
                        Debug.LogWarning($"Skipping missing component on gameObject {gameObject.name}", gameObject);
                        continue;
                    }

                    // Get all private and public fields of the component
                    FieldInfo[] fieldInfos = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        var fieldValue = fieldInfo.GetValue(component);

                        // Looking for UnityEvent fields only
                        if (fieldValue is not UnityEventBase)
                        {
                            continue;
                        }

                        UnityEventBase unityEvent = fieldValue as UnityEventBase;
                        int numberOfAssignedEvents = unityEvent.GetPersistentEventCount();

                        // No need to continue if there are no assigned events
                        if (numberOfAssignedEvents == 0) 
                        {
                            continue;
                        }

                        string eventName = fieldInfo.Name;

                        // Go through each event and check for possible problems
                        for (int i = 0; i < numberOfAssignedEvents; i++)
                        {
                            var targetObject = unityEvent.GetPersistentTarget(i);

                            // Target object is missing or removed
                            if (targetObject == null)
                            {
                                isReferenceMissing = true;
                                ShowDetailedErrorMessage(
                                    errorMessage: "UnityEvent reference is missing! Target Object is null",
                                    component: component,
                                    eventName: eventName);
                                break;
                            }

                            string methodName = unityEvent.GetPersistentMethodName(i);

                            // No Function selected
                            if (string.IsNullOrWhiteSpace(methodName))
                            {
                                isReferenceMissing = true;
                                ShowDetailedErrorMessage(
                                    errorMessage: "UnityEvent reference is missing! No function selected",
                                    component: component,
                                    eventName: eventName);
                                break;
                            }

                            // The assigned method is missing
                            if (!IsMethodNameExist(targetObject, methodName))
                            {
                                isReferenceMissing = true;
                                ShowDetailedErrorMessage(
                                    errorMessage: $"UnityEvent reference is missing! Method name \"{targetObject.GetType().Name}.{methodName}\" does not exist",
                                    component: component,
                                    eventName: eventName);
                                break;
                            }
                        }
                    }
                }
            }

            if (!isReferenceMissing)
            {
                Debug.Log("No missing references were found in the scene");
            }
        }

        private static bool IsMethodNameExist(UnityEngine.Object targetObject, string methodName)
        {
            // Get any method with the required name
            // There might be more than one if the method was overloaded
            // And this is the main issue: if there are any overloaded methods, then we still will return true
            //   even though in the inspector it will be missing
            return targetObject
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Any(m => m.Name == methodName);
        }

        private static void ShowDetailedErrorMessage(string errorMessage, Component component, string eventName)
        {
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.AppendLine(errorMessage);
            sbuilder.AppendLine($"GameObject: {component.gameObject.name}");
            sbuilder.AppendLine($"-  Component: {component.GetType().Name}");
            sbuilder.AppendLine($"-     Event name: {eventName}");

            Debug.LogError(sbuilder.ToString(), component);
        }
    }
}
