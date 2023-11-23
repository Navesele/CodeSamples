using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Marshtown.PuzzleController;

public class NeuronController_EditModeTests
{
    private const NeuronDefectFlags _allFlags = NeuronDefectFlags.None | NeuronDefectFlags.Blocked | NeuronDefectFlags.Broken;
    private const NeuronDefectFlags _allFlagsNotNone = NeuronDefectFlags.Blocked | NeuronDefectFlags.Broken;

    private NeuronController CreateNewNeuronController()
    {
        return new GameObject().AddComponent<NeuronController>();
    }

    #region Default defects
    [Test]
    public void DefaultNeuron_Defects_Should_Be_None()
    {
        var controller = CreateNewNeuronController();
        Assert.IsTrue(controller.Defects == NeuronDefectFlags.None);
    }

    [Test]
    public void DefaultNeuron_Defects_Should_Be_IsNothing()
    {
        var controller = CreateNewNeuronController();
        Assert.IsTrue(controller.Defects.IsNothing());
    }

    [Test]
    public void DefaultNeuron_Defects_Shound_Not_Be_IsEverything()
    {
        var controller = CreateNewNeuronController();
        Assert.IsFalse(controller.Defects.IsEverything());
    }

    [Test]
    public void DefaultNeuron_Defects_None_Is_Nothing()
    {
        var controller = CreateNewNeuronController();
        Assert.AreEqual(NeuronDefectFlags.None, controller.Defects);
        Assert.IsTrue(controller.Defects.IsNothing());
    }
    #endregion Default defects

    #region AddDefect
    [Test]
    [TestCase(NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Blocked)]
    [TestCase(_allFlagsNotNone)]
    [TestCase(_allFlags)]
    [TestCase(NeuronDefectFlags.Broken | NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Blocked | NeuronDefectFlags.None)]
    public void AddDefect_Defects_Should_Be_Equal_To_InputFlag(NeuronDefectFlags inputFlag)
    {
        var controller = CreateNewNeuronController();
        controller.AddDefect(inputFlag);

        Assert.AreEqual(inputFlag, controller.Defects);
    }

    [Test]
    [TestCase(_allFlagsNotNone)]
    [TestCase(_allFlags)]
    public void AddDefect_Defects_Should_Be_IsEverything(NeuronDefectFlags inputFlag)
    {
        var controller = CreateNewNeuronController();
        controller.AddDefect(inputFlag);

        Assert.IsTrue(controller.Defects.IsEverything());
    }

    [Test]
    [TestCase(NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Broken | NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Blocked | NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.None)]
    public void AddDefect_Defects_Should_Not_Be_IsEverything(NeuronDefectFlags inputFlag)
    {
        var controller = CreateNewNeuronController();
        controller.AddDefect(inputFlag);

        Assert.IsFalse(controller.Defects.IsEverything());
    }

    [Test]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.Blocked, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.Broken, NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked, _allFlags)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked, _allFlagsNotNone)]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.None, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.None, NeuronDefectFlags.Broken)]
    public void AddDefect_Defects_Should_Be_Equal_To_ExpectedFlag(
        NeuronDefectFlags defaultFlag,
        NeuronDefectFlags addFlag,
        NeuronDefectFlags resultFlag)
    {
        var controller = CreateNewNeuronController();
        controller.AddDefect(defaultFlag);
        controller.AddDefect(addFlag);

        Assert.AreEqual(resultFlag, controller.Defects);
    }

    [Test]
    [TestCase(NeuronDefectFlags.None, NeuronDefectFlags.Blocked, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.None, NeuronDefectFlags.Broken, NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked, _allFlags)]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.Broken, _allFlagsNotNone)]
    public void AddDefect_Defects_Should_Be_IsExact(
        NeuronDefectFlags defaultFlag,
        NeuronDefectFlags addFlag,
        NeuronDefectFlags resultFlag)
    {
        var controller = CreateNewNeuronController();

        controller.AddDefect(defaultFlag);
        controller.AddDefect(addFlag);

        Assert.IsTrue(controller.Defects.IsExact(resultFlag));
    }
    #endregion AddDefect

    #region RemoveDefect
    [Test]
    [TestCase(NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Blocked)]
    [TestCase(_allFlagsNotNone)]
    [TestCase(_allFlags)]
    [TestCase(NeuronDefectFlags.Broken | NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Blocked | NeuronDefectFlags.None)]
    public void RemoveDefect_Defects_Should_Be_IsNothing(NeuronDefectFlags inputFlag)
    {
        var controller = CreateNewNeuronController();

        controller.RemoveDefect(inputFlag);

        Assert.AreEqual(NeuronDefectFlags.None, controller.Defects);
        Assert.IsTrue(controller.Defects.IsNothing());
    }

    [Test]
    [TestCase(NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Blocked)]
    [TestCase(_allFlagsNotNone)]
    [TestCase(_allFlags)]
    [TestCase(NeuronDefectFlags.Broken | NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Blocked | NeuronDefectFlags.None)]
    public void RemoveDefect_Defects_Should_Not_Be_IsEverything(NeuronDefectFlags inputFlag)
    {
        var controller = CreateNewNeuronController();

        controller.RemoveDefect(inputFlag);

        Assert.AreEqual(NeuronDefectFlags.None, controller.Defects);
        Assert.IsFalse(controller.Defects.IsEverything());
    }

    [Test]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.Blocked, NeuronDefectFlags.None)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.Broken, NeuronDefectFlags.None)]
    [TestCase(_allFlags, NeuronDefectFlags.Blocked, NeuronDefectFlags.Broken)]
    [TestCase(_allFlags, NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.None, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.None, NeuronDefectFlags.Broken)]
    [TestCase(_allFlags, NeuronDefectFlags.None, _allFlags)]
    [TestCase(_allFlagsNotNone, NeuronDefectFlags.None, _allFlagsNotNone)]
    public void RemoveDefect_Defects_Should_Be_Equal_To_ExpectedFlag(
        NeuronDefectFlags defaultFlag,
        NeuronDefectFlags removeFlag,
        NeuronDefectFlags resultFlag)
    {
        var controller = CreateNewNeuronController();
        controller.AddDefect(defaultFlag);
        controller.RemoveDefect(removeFlag);

        Assert.AreEqual(resultFlag, controller.Defects);
    }

    [Test]
    [TestCase(_allFlags, NeuronDefectFlags.Blocked, NeuronDefectFlags.Broken)]
    [TestCase(_allFlags, NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.Blocked, NeuronDefectFlags.Broken)]
    [TestCase(NeuronDefectFlags.Blocked, NeuronDefectFlags.None, NeuronDefectFlags.Blocked)]
    [TestCase(NeuronDefectFlags.Broken, NeuronDefectFlags.None, NeuronDefectFlags.Broken)]
    [TestCase(_allFlags, NeuronDefectFlags.None, _allFlags)]
    [TestCase(_allFlagsNotNone, NeuronDefectFlags.None, _allFlagsNotNone)]
    [TestCase(_allFlagsNotNone, NeuronDefectFlags.None, _allFlags)]
    public void RemoveDefect_Defect_Should_Be_IsExact(
        NeuronDefectFlags defaultFlag,
        NeuronDefectFlags removeFlag,
        NeuronDefectFlags resultFlag)
    {
        var controller = CreateNewNeuronController();

        controller.AddDefect(defaultFlag);
        controller.RemoveDefect(removeFlag);

        Assert.IsTrue(controller.Defects.IsExact(resultFlag));
    }
    #endregion RemoveDefect
}
