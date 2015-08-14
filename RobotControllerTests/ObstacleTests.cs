using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotController;

namespace RobotControllerTests
{
    [TestClass]
    public class ObstacleTests
    {
        #region Rocks

        [TestMethod]
        public void SampleRockTest_ObstacleClass()
        {
            var grid = Program.InitializeGrid(5, 5);
            var rock = new Obstacle()
            {
                Type = "Rock",
                RobotAction = r => r,
                Grid = grid
            };
            var robot = new Robot()
            {
                Position = new[] { 3, 3 }
            };

            robot = rock.TakeAction(robot);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] {3, 3}));
        }

        [TestMethod]
        public void SampleRockTest_RockClass()
        {
            var grid = Program.InitializeGrid(5, 5);
            var rock = new Rock(new [] { 0, 0 }, grid);
            var robot = new Robot()
            {
                Position = new[] {3, 3}
            };

            robot = rock.TakeAction(robot);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] {3, 3}));
        }
        #endregion
        #region Holes

        [TestMethod]
        public void SampleHoleTest()
        {
            var grid = Program.InitializeGrid(5, 5);
            var hole = new Obstacle<int[]>()
            {
                Type = "Hole",
                ActionData = new[] { 4, 4 },
                RobotAction = (r, endpoint) => r.UpdatePosition(new [] { endpoint[0] - r.Position[0], endpoint[1] - r.Position[1] }, grid),
                Grid = grid
            };
            var robot = new Robot()
            {
                Position = new[] {3, 3}
            };

            robot = hole.TakeAction(robot);

            Assert.IsTrue(robot.Position.SequenceEqual(new[] { 4, 4 }));
        }
        #endregion
        #region Spinners

        [TestMethod]
        public void SampleSpinnerTest()
        {
            var grid = Program.InitializeGrid(5, 5);
            var spinner = new Obstacle<int>()
            {
                Type = "Spinner",
                ActionData = 270,
                RobotAction = (r, angle) => r.UpdateDirection(angle),
                Grid = grid
            };
            var robot = new Robot();

            robot = spinner.TakeAction(robot);

            Assert.IsTrue(robot.Direction == 3);
        }
        #endregion
    }
}
