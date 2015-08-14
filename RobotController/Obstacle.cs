using System;
using System.Collections.Generic;

namespace RobotController
{
    public interface IObstacle
    {
        int[] Position { get; }
        Robot TakeAction(Robot robot);
    }

    public class Obstacle : IObstacle
    {
        public string Type;
        public Func<Robot, Robot> RobotAction = r => r; // default for all obstacles, known or unknown, will be to not move there
        public int[] ObstaclePosition;
        public Dictionary<int[], IObstacle> Grid;

        public int[] Position { get { return ObstaclePosition; } }

        public Robot TakeAction(Robot robot)
        {
            return RobotAction(robot);
        }
    }

    public class Obstacle<T> : Obstacle, IObstacle
    {
        public T ActionData;
        new public Func<Robot, T, Robot> RobotAction = (r, data) => r; // default for all obstacles, known or unknown, will be not move there

        new public Robot TakeAction(Robot robot)
        {
            return RobotAction(robot, ActionData);
        }
    }

    public class Rock : Obstacle
    {
        public Rock(int[] position, Dictionary<int[], IObstacle> grid)
        {
            Type = "Rock";
            ObstaclePosition = position;
            Grid = grid;
        }
    }

    public class Hole : Obstacle<int[]>
    {
        public Hole(int[] endPoint, int[] position, Dictionary<int[], IObstacle> grid)
        {
            Type = "Hole";
            ObstaclePosition = position;
            ActionData = endPoint;
            RobotAction = (r, endpoint) => r.UpdatePosition(new[] { endpoint[0] - r.Position[0], endpoint[1] - r.Position[1] }, grid);
            Grid = grid;
        }
    }

    public class Spinner : Obstacle<int>
    {
        public Spinner(int rotationAngle, int[] position, Dictionary<int[], IObstacle> grid)
        {
            Type = "Spinner";
            ObstaclePosition = position;
            ActionData = rotationAngle;
            RobotAction = (r, angle) => r.UpdateDirection(angle).UpdatePosition(Position);
            Grid = grid;
        }
    }
}
