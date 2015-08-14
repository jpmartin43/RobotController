using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotController;

namespace RobotControllerTests
{
    [TestClass]
    public class RobotTests
    {
        #region Move Tests

        [TestMethod]
        public void Move_PositiveCase()
        {
            var robot = new Robot()
            {
                Position = new [] {3, 3},
                Direction = 0
            };
            var instructions = "FRRR"; // ends up back at [3, 3] with direction 3
            var grid = Program.InitializeGrid(5, 5);
            
            foreach (var instruction in instructions)
                robot.Move(instruction.ToString(), grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new [] { 3, 3 }));
            Assert.IsTrue(robot.Direction == 3);
        }

        [TestMethod]
        public void Move_OutOfBounds()
        {
            var robot = new Robot()
            {
                Position = new[] {3, 3},
                Direction = 0
            };
            var instructions = "FFFF"; // will attempt to move to [3, -1] with direction 0
            var grid = Program.InitializeGrid(5, 5);

            foreach (var instruction in instructions)
                robot.Move(instruction.ToString(), grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new [] { 3, 0 }));
            Assert.IsTrue(robot.Direction == 0);
        }
        #endregion
        #region UpdatePosition Tests

        [TestMethod]
        public void UpdatePosition_MoveToEmptyPosition()
        {
            var grid = Program.InitializeGrid(5, 5);
            var robot = new Robot();

            robot = robot.UpdatePosition(new[] {1, 0}, grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] {1, 0}));
        }

        [TestMethod]
        public void UpdatePosition_MoveToPositionWithRock()
        {
            var grid = Program.InitializeGrid(5, 5);
            var rock = new Rock(new [] { 0, 0 }, grid);
            grid[grid.FirstOrDefault(g => g.Key.SequenceEqual(rock.Position)).Key] = rock;
            var robot = new Robot();

            robot = robot.UpdatePosition(rock.Position, grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] { 0, 0 }));
        }

        [TestMethod]
        public void UpdatePosition_MoveToPositionWithHole()
        {
            var grid = Program.InitializeGrid(5, 5);
            var hole = new Hole(new[] {3, 3}, new[] {1, 0}, grid);
            grid[grid.FirstOrDefault(g => g.Key.SequenceEqual(hole.Position)).Key] = hole;
            var robot = new Robot();

            robot = robot.UpdatePosition(hole.Position, grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] { 3, 3 }));
        }

        [TestMethod]
        public void UpdatePosition_MoveToPositionWithSpinner()
        {
            var grid = Program.InitializeGrid(5, 5);
            var spinner = new Spinner(270, new [] { 1, 0 }, grid);
            grid[grid.FirstOrDefault(g => g.Key.SequenceEqual(spinner.Position)).Key] = spinner;
            var robot = new Robot();

            robot = robot.UpdatePosition(spinner.Position, grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] { 1, 0 }));
        }

        [TestMethod]
        public void UpdatePosition_MoveOutOfBounds()
        {
            var grid = Program.InitializeGrid(5, 5);
            var robot = new Robot();

            robot = robot.UpdatePosition(new[] { -1, -1 }, grid);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] { 0, 0 }));
        }
        #endregion
        #region UpdateDirection Tests

        [TestMethod]
        public void UpdateDirection_MultiplesOf90()
        {
            var robot = new Robot();

            Assert.IsTrue(robot.UpdateDirection(0).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(90).Direction == 1);
            Assert.IsTrue(robot.UpdateDirection(90).Direction == 2);
            Assert.IsTrue(robot.UpdateDirection(180).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(360).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(180).Direction == 2);
            Assert.IsTrue(robot.UpdateDirection(270).Direction == 1);
        }

        [TestMethod]
        public void UpdateDirection_NotMultiplesOf90()
        {
            var robot = new Robot();

            Assert.IsTrue(robot.UpdateDirection(1).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(45).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(91).Direction == 1);
            Assert.IsTrue(robot.UpdateDirection(179).Direction == 2);
            Assert.IsTrue(robot.UpdateDirection(181).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(361).Direction == 0);
            Assert.IsTrue(robot.UpdateDirection(271).Direction == 3);
            Assert.IsTrue(robot.UpdateDirection(316).Direction == 2);
        }
        #endregion
        #region GetMovement Tests

        [TestMethod]
        public void GetMovement_YMinus1PositiveCases()
        {
            var F0 = Robot.GetMovement("F", 0);
            var L1 = Robot.GetMovement("L", 1);
            var R3 = Robot.GetMovement("R", 3);

            var result = new Tuple<int[], int>(new[] {0, -1}, 0);

            Assert.IsTrue(result.Item1.SequenceEqual(F0.Item1));
            Assert.AreEqual(F0.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(L1.Item1));
            Assert.AreEqual(L1.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(R3.Item1));
            Assert.AreEqual(R3.Item2, result.Item2);
        }

        [TestMethod]
        public void GetMovement_XPlus1PositiveCases()
        {
            var R0 = Robot.GetMovement("R", 0);
            var F1 = Robot.GetMovement("F", 1);
            var L2 = Robot.GetMovement("L", 2);

            var result = new Tuple<int[], int>(new[] {1, 0}, 1);

            Assert.IsTrue(result.Item1.SequenceEqual(R0.Item1));
            Assert.AreEqual(R0.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(F1.Item1));
            Assert.AreEqual(F1.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(L2.Item1));
            Assert.AreEqual(L2.Item2, result.Item2);
        }

        [TestMethod]
        public void GetMovement_YPlus1PositiveCases()
        {
            var R1 = Robot.GetMovement("R", 1);
            var F2 = Robot.GetMovement("F", 2);
            var L3 = Robot.GetMovement("L", 3);

            var result = new Tuple<int[], int>(new[] {0, 1}, 2);

            Assert.IsTrue(result.Item1.SequenceEqual(R1.Item1));
            Assert.AreEqual(R1.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(F2.Item1));
            Assert.AreEqual(F2.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(L3.Item1));
            Assert.AreEqual(L3.Item2, result.Item2);
        }

        [TestMethod]
        public void GetMovement_XMinus1PositiveCases()
        {
            var L0 = Robot.GetMovement("L", 0);
            var R2 = Robot.GetMovement("R", 2);
            var F3 = Robot.GetMovement("F", 3);

            var result = new Tuple<int[], int>(new[] {-1, 0}, 3);

            Assert.IsTrue(result.Item1.SequenceEqual(L0.Item1));
            Assert.AreEqual(L0.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(R2.Item1));
            Assert.AreEqual(R2.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(F3.Item1));
            Assert.AreEqual(F3.Item2, result.Item2);
        }

        [TestMethod]
        public void GetMovement_LowercaseInstructions()
        {
            var L0 = Robot.GetMovement("l", 0);
            var R2 = Robot.GetMovement("r", 2);
            var F3 = Robot.GetMovement("f", 3);

            var result = new Tuple<int[], int>(new[] { -1, 0 }, 3);

            Assert.IsTrue(result.Item1.SequenceEqual(L0.Item1));
            Assert.AreEqual(L0.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(R2.Item1));
            Assert.AreEqual(R2.Item2, result.Item2);

            Assert.IsTrue(result.Item1.SequenceEqual(F3.Item1));
            Assert.AreEqual(F3.Item2, result.Item2);
        }

        [TestMethod]
        public void GetMovement_InvalidDirection()
        {
            var F5 = Robot.GetMovement("f", 5);

            var result = new Tuple<int[], int>(new[] {0, 0}, 5);

            Assert.IsTrue(result.Item1.SequenceEqual(F5.Item1));
            Assert.AreEqual(F5.Item2, result.Item2);
        }

        [TestMethod]
        public void GetMovement_InvalidInstruction()
        {
            var Q1 = Robot.GetMovement("q", 1);

            var result = new Tuple<int[], int>(new[] {0, 0}, 1);

            Assert.IsTrue(result.Item1.SequenceEqual(Q1.Item1));
            Assert.AreEqual(Q1.Item2, result.Item2);
        }
        #endregion
        #region GetDirection Tests

        [TestMethod]
        public void GetDirection_NormalPositiveCases()
        {
            var angle = 0;
            var result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 0);

            angle = 90;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 1);

            angle = 180;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 2);

            angle = 270;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 3);
        }

        [TestMethod]
        public void GetDirection_361Degrees()
        {
            var angle = 361;
            var result = Robot.GetDirection(angle);

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetDirection_45Degrees()
        {
            var angle = 45;
            var result = Robot.GetDirection(angle);

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetDirection_NegativeDegrees()
        {
            var angle = -90;
            var result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 3);

            angle = -45;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 3);

            angle = -1;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 3);

            angle = -361;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 3);

            angle = 2340;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 2);

            angle = -2340;
            result = Robot.GetDirection(angle);
            Assert.AreEqual(result, 2);
        }
        #endregion
    }
}
