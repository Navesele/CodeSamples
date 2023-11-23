using Marshtown.PuzzleController;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Marshtown.UnitTests.PlayMode
{
    public class NeuralNetworkController_PlayModeTests
    {
        #region Private methods
        private NeuralNetworkController CreateNeuralNetwrok(List<NeuronController> childNeurons)
        {
            var neuralNetwork = new GameObject()
                .ThenSetActive(false)
                .ThenAddComponent<NeuralNetworkController>();
            neuralNetwork.SetCompleteCondition();

            foreach (var child in childNeurons)
            {
                child.transform.SetParent(neuralNetwork.transform);
            }

            neuralNetwork.gameObject.SetActive(true);
            return neuralNetwork;
        }

        private NeuronController CreateNeuronController()
        {
            var neuron = new GameObject()
                .ThenSetActive(false)
                .ThenAddComponent<NeuronController>();
            neuron.AddAxon();
            neuron.AddTerminal();

            neuron.gameObject.SetActive(true);
            return neuron;
        }
        #endregion Private methods

        #region Tests
        [Test]
        public void Create_NeuronController_Basic()
        {
            NeuronDefectFlags expectedDefects = NeuronDefectFlags.None;
            SignalMoveDirectionType expectedDirection = SignalMoveDirectionType.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.Normal;

            var neuron = CreateNeuronController();

            Assert.IsNotNull(neuron);
            Assert.AreEqual(expectedDefects, neuron.Defects);
            Assert.AreEqual(expectedDirection, neuron.SignalDirection);
            Assert.AreEqual(expectedExcitation, neuron.ExcitationLevel);
        }

        [Test]
        public void Create_NeuronController_With_One_Defect([Values] NeuronDefectFlags defects)
        {
            SignalMoveDirectionType expectedDirection = SignalMoveDirectionType.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.Normal;

            var neuron = CreateNeuronController();
            neuron.SetDefaultDefects(defects);

            Assert.IsNotNull(neuron);
            Assert.AreEqual(defects, neuron.Defects);
            Assert.AreEqual(expectedDirection, neuron.SignalDirection);
            Assert.AreEqual(expectedExcitation, neuron.ExcitationLevel);
        }

        [Test]
        public void Create_NeuronController_With_Multiple_Defects()
        {
            NeuronDefectFlags expectedDefects = NeuronDefectFlags.Blocked | NeuronDefectFlags.Broken;
            SignalMoveDirectionType expectedDirection = SignalMoveDirectionType.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.Normal;

            var neuron = CreateNeuronController();
            neuron.SetDefaultDefects(expectedDefects);

            Assert.IsNotNull(neuron);
            Assert.AreEqual(expectedDefects, neuron.Defects);
            Assert.AreEqual(expectedDirection, neuron.SignalDirection);
            Assert.AreEqual(expectedExcitation, neuron.ExcitationLevel);
        }

        [Test]
        public void Create_NeuronController_With_ExcitationLevel([Values]NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expectedDefects = NeuronDefectFlags.None;
            SignalMoveDirectionType expectedDirection = SignalMoveDirectionType.None;

            var neuron = CreateNeuronController();
            neuron.SetDefaultExcitationLevel(excitationLevel);

            Assert.IsNotNull(neuron);
            Assert.AreEqual(expectedDefects, neuron.Defects);
            Assert.AreEqual(expectedDirection, neuron.SignalDirection);
            Assert.AreEqual(excitationLevel, neuron.ExcitationLevel);
        }

        [Test]
        public void Create_NeuronController_With_SignalDirection([Values]SignalMoveDirectionType signalDirection)
        {
            NeuronDefectFlags expectedDefects = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.Normal;

            var neuron = CreateNeuronController();
            neuron.SetDefaultSignalDirection(signalDirection);

            Assert.IsNotNull(neuron);
            Assert.AreEqual(expectedDefects, neuron.Defects);
            Assert.AreEqual(signalDirection, neuron.SignalDirection);
            Assert.AreEqual(expectedExcitation, neuron.ExcitationLevel);
        }

        [Test]
        public void Create_NeuronController_With_Defects_ExcitationLevel_Direction([Values]NeuronDefectFlags defects, [Values]NeuronExcitationType excitationLevel, [Values]SignalMoveDirectionType signalDirection)
        {
            var neuron = CreateNeuronController();
            neuron
                .SetDefaultDefects(defects)
                .SetDefaultExcitationLevel(excitationLevel)
                .SetDefaultSignalDirection(signalDirection);

            Assert.IsNotNull(neuron);
            Assert.AreEqual(defects, neuron.Defects);
            Assert.AreEqual(signalDirection, neuron.SignalDirection);
            Assert.AreEqual(excitationLevel, neuron.ExcitationLevel);
        }

        [Test]
        public void Create_NeuralNetwork_Basic()
        {
            int expectedNeuronCount = 1;
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.None;

            List<NeuronController> neurons = new List<NeuronController>() 
            {
                CreateNeuronController()
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(expectedNeuronCount, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
        }

        [Test]
        public void Completed_NeuralNetwork_Basic()
        {
            int expectedNeuronCount = 1;
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.None;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(expectedNeuronCount, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsTrue(neuralNetwork.IsCompleted);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Completed_NeuralNetwork_Basic_With_Multiple_Neurons(int neuronsCount)
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.None;

            List<NeuronController> neurons = new List<NeuronController>();

            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(CreateNeuronController());
            }

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neuronsCount, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsTrue(neuralNetwork.IsCompleted);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Not_Completed_NeuralNetwork_With_One_Broken_Neuron(int neuronsCount)
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.None;

            List<NeuronController> neurons = new List<NeuronController>();
            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(CreateNeuronController());
            }

            int neuronIdx = neuronsCount >= 1 ? 0 : neuronsCount - 1;
            neurons[neuronIdx].SetDefaultDefects(NeuronDefectFlags.Broken);

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsFalse(neuralNetwork.IsCompleted);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Not_Completed_NeuralNetwork_With_All_Broken_Neurons(int neuronsCount)
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.None;

            List<NeuronController> neurons = new List<NeuronController>();
            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(CreateNeuronController().SetDefaultDefects(NeuronDefectFlags.Broken));
            }

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsFalse(neuralNetwork.IsCompleted);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Completed_NeuralNetwork_With_DefectsToIgnore_Broken(int neuronsCount)
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.Broken;

            List<NeuronController> neurons = new List<NeuronController>();
            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(CreateNeuronController());
            }

            int neuronIdx = neuronsCount >= 1 ? 0 : neuronsCount - 1;
            neurons[neuronIdx].SetDefaultDefects(expectedDefectsToIgnore);

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedDefectsToIgnore);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsTrue(neuralNetwork.IsCompleted);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Not_Completed_NeuralNetwork_With_DefectsToIgnore_Broken_But_Neurons_Blocked(int neuronsCount)
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.Broken;

            List<NeuronController> neurons = new List<NeuronController>();
            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(CreateNeuronController());
            }

            int neuronIdx = neuronsCount >= 1 ? 0 : neuronsCount - 1;
            neurons[neuronIdx].SetDefaultDefects(NeuronDefectFlags.Blocked);

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedDefectsToIgnore);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsFalse(neuralNetwork.IsCompleted);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Not_Completed_NeuralNetwork_With_DefectsToIgnore_Broken_But_Neurons_IsEverything(int neuronsCount)
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.Broken;

            List<NeuronController> neurons = new List<NeuronController>();
            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(CreateNeuronController());
            }

            int neuronIdx = neuronsCount >= 1 ? 0 : neuronsCount - 1;
            neurons[neuronIdx].SetDefaultDefects(NeuronDefectFlags.Blocked | NeuronDefectFlags.Broken);

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedDefectsToIgnore);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsFalse(neuralNetwork.IsCompleted);
        }

        [Test]
        public void Completed_NeuralNetwork_With_DefectsToIgnore_IsEverything()
        {
            NeuronExcitationType expectedExctitationToComplete = NeuronExcitationType.Normal;
            NeuronDefectFlags expectedDefectsToIgnore = NeuronDefectFlags.Broken | NeuronDefectFlags.Blocked;

            List<NeuronController> neurons = new List<NeuronController>();
            neurons.Add(CreateNeuronController().SetDefaultDefects(NeuronDefectFlags.None));
            neurons.Add(CreateNeuronController().SetDefaultDefects(NeuronDefectFlags.None | NeuronDefectFlags.Broken));
            neurons.Add(CreateNeuronController().SetDefaultDefects(NeuronDefectFlags.None | NeuronDefectFlags.Broken | NeuronDefectFlags.Blocked));

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedDefectsToIgnore);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.IsNotNull(neuralNetwork.CompleteCondition);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(expectedExctitationToComplete, neuralNetwork.CompleteCondition.ExcitationLevelToComplete);
            Assert.AreEqual(expectedDefectsToIgnore, neuralNetwork.CompleteCondition.DefectsToIgnore);
            Assert.IsTrue(neuralNetwork.IsCompleted);
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_CriticalLow([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.CriticalLow;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_VeryLow([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.VeryLow;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_Low([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.Low;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_Normal([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.Normal;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_High([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.High;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_VeryHigh([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.VeryHigh;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }

        [Test]
        public void Check_Completed_NeuralNetwork_Condition_ExcitationLevel_CriticalHigh([Values] NeuronDefectFlags defect, [Values] NeuronExcitationType excitationLevel)
        {
            NeuronDefectFlags expextedDefect = NeuronDefectFlags.None;
            NeuronExcitationType expectedExcitation = NeuronExcitationType.CriticalHigh;

            List<NeuronController> neurons = new List<NeuronController>()
            {
                CreateNeuronController()
                .SetDefaultDefects(defect)
                .SetDefaultExcitationLevel(excitationLevel)
            };

            var neuralNetwork = CreateNeuralNetwrok(neurons);
            neuralNetwork.SetCompleteCondition(expectedExcitation);
            neuralNetwork.CheckIsCompleted();

            Assert.IsNotNull(neuralNetwork);
            Assert.AreEqual(neurons.Count, neuralNetwork.Neurons.Count);
            Assert.AreEqual(1, neuralNetwork.Neurons.Count);

            if (neurons[0].Defects == expextedDefect && neurons[0].ExcitationLevel == expectedExcitation)
            {
                Assert.IsTrue(neuralNetwork.IsCompleted);
            }
            else
            {
                Assert.IsFalse(neuralNetwork.IsCompleted);
            }
        }
        #endregion Tests
    }
}