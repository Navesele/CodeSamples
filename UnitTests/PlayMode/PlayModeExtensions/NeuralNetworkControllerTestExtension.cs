using UnityEngine;
using Marshtown.PuzzleController;
using System.Reflection;

namespace Marshtown.UnitTests.PlayMode
{
    internal static class NeuralNetworkControllerTestExtension
    {
        internal static void SetCompleteCondition(this NeuralNetworkController neuralNetwork)
        {
            var condition = ScriptableObject.CreateInstance<NeuralNetworkCondition>();
            PropertyInfo propCondition = neuralNetwork.GetType().GetProperty("CompleteCondition");
            propCondition.SetValue(neuralNetwork, condition);
        }

        internal static void SetCompleteCondition(this NeuralNetworkController neuralNetwork, NeuronExcitationType excitationType)
        {
            var condition = ScriptableObject.CreateInstance<NeuralNetworkCondition>();
            PropertyInfo propExcitationLevel = condition.GetType().GetProperty("ExcitationLevelToComplete");
            propExcitationLevel.SetValue(condition, excitationType);
            PropertyInfo propCondition = neuralNetwork.GetType().GetProperty("CompleteCondition");
            propCondition.SetValue(neuralNetwork, condition);
        }

        internal static void SetCompleteCondition(this NeuralNetworkController neuralNetwork, NeuronDefectFlags defectsToIgnore)
        {
            var condition = ScriptableObject.CreateInstance<NeuralNetworkCondition>();
            PropertyInfo propDefectsToIgnore = condition.GetType().GetProperty("DefectsToIgnore");
            propDefectsToIgnore.SetValue(condition, defectsToIgnore);
            PropertyInfo propCondition = neuralNetwork.GetType().GetProperty("CompleteCondition");
            propCondition.SetValue(neuralNetwork, condition);
        }

        internal static void SetCompleteCondition(this NeuralNetworkController neuralNetwork, NeuronExcitationType excitationType, NeuronDefectFlags defectsToIgnore)
        {
            var condition = ScriptableObject.CreateInstance<NeuralNetworkCondition>();
            PropertyInfo propExcitationLevel = condition.GetType().GetProperty("ExcitationLevelToComplete");
            propExcitationLevel.SetValue(condition, excitationType);
            PropertyInfo propDefectsToIgnore = condition.GetType().GetProperty("DefectsToIgnore");
            propDefectsToIgnore.SetValue(condition, defectsToIgnore);
            PropertyInfo propCondition = neuralNetwork.GetType().GetProperty("CompleteCondition");
            propCondition.SetValue(neuralNetwork, condition);
        }
    }
}
