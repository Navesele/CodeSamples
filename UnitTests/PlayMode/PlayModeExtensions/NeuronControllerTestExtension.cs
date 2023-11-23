using UnityEngine;
using Marshtown.PuzzleController;
using System.Reflection;

namespace Marshtown.UnitTests.PlayMode
{
    internal static class NeuronControllerTestExtension
    {
        internal static NeuronController SetDefaultDefects(this NeuronController controller, NeuronDefectFlags defects)
        {
            FieldInfo defectsField = controller.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance);
            defectsField.SetValue(controller, defects);
            return controller;
        }

        internal static NeuronController SetDefaultExcitationLevel(this NeuronController controller, NeuronExcitationType excitation)
        {
            FieldInfo excitationField = controller.GetType().GetField("_excitationLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            excitationField.SetValue(controller, excitation);
            return controller;
        }

        internal static NeuronController SetDefaultSignalDirection(this NeuronController controller, SignalMoveDirectionType direction)
        {
            FieldInfo directionField = controller.GetType().GetField("_signalDirection", BindingFlags.NonPublic | BindingFlags.Instance);
            directionField.SetValue(controller, direction);
            return controller;
        }

        internal static void AddAxon(this NeuronController controller)
        {
            GameObject axonObject = new GameObject();
            var axon = axonObject.AddComponent<NeuronAxon>();
            axon.transform.parent = controller.transform;
        }

        internal static void AddTerminal(this NeuronController controller)
        {
            GameObject terminalObject = new GameObject();
            var terminal = terminalObject.AddComponent<NeuronTerminal>();
            terminal.transform.parent = controller.transform;
        }
    }
}