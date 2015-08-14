using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotController
{
    public class Robot
    {
        public int Direction;
        public int[] Position = { 0, 0 };

        /// <summary>
        /// Updates the Robot position and direction based on its movement instructions property
        /// </summary>
        /// <param name="grid">Grid to be traversed upon</param>
        public void Move(string instruction, Dictionary<int[], IObstacle> grid)
        {
            var movement = GetMovement(instruction.ToUpper(), Direction);
            UpdatePosition(movement.Item1, grid);
            Direction = movement.Item2;
        }

        public Robot UpdatePosition(int[] positionUpdate, Dictionary<int[], IObstacle> grid)
        {
            var updatedPosition = new[] { Position[0] + positionUpdate[0], Position[1] + positionUpdate[1] };
            if (!grid.Keys.Any(g => g.SequenceEqual(updatedPosition))) return this; // new position is out of bounds

            var obstacle = grid.FirstOrDefault(g => g.Key.SequenceEqual(updatedPosition)).Value;
            if (obstacle != null) return obstacle.TakeAction(this); // return the obstacle action if encountered obstacle
            
            Position = updatedPosition;
            return this;
        }

        /// <summary>
        /// Updates the position of the robot to the given position, without accounting for other factors
        /// *NOTE* This should only be used if there is a high level of confidence that the position is valid!
        /// </summary>
        public Robot UpdatePosition(int[] newPosition)
        {
            Position = newPosition;
            return this;
        }

        public Robot UpdateDirection(int angle)
        {
            Direction = (Direction + GetDirection(angle)) % 4;
            return this;
        }

        /// <summary>
        /// Translates the movement command and direction to a movement directive and new direction
        /// </summary>
        /// <param name="instruction">Will accept F (move forward), L (move left) and R (move right)</param>
        /// <param name="direction">Will accept 0 through 3, translating to the angle faced (as direciton * 90 degrees)</param>
        /// <returns>Tuple with Item1 being movement directive and Item2 as new direction (0 through 3)</returns>
        public static Tuple<int[], int> GetMovement(string instruction, int direction)
        {
            instruction = instruction.ToUpper()[0].ToString();

            if ((instruction == "F" && direction == 0) || 
                (instruction == "L" && direction == 1) || 
                (instruction == "R" && direction == 3))
                return new Tuple<int[], int>(new [] { 0, -1 }, 0);
            
            if ((instruction == "R" && direction == 0) || 
                (instruction == "F" && direction == 1) || 
                (instruction == "L" && direction == 2))
                return new Tuple<int[], int>(new[] { 1, 0 }, 1);
            
            if ((instruction == "R" && direction == 1) || 
                (instruction == "F" && direction == 2) || 
                (instruction == "L" && direction == 3))
                return new Tuple<int[], int>(new[] { 0, 1 }, 2);
            
            if ((instruction == "L" && direction == 0) || 
                (instruction == "R" && direction == 2) || 
                (instruction == "F" && direction == 3))
                return new Tuple<int[], int>(new[] { -1, 0 }, 3);
            
            return new Tuple<int[], int>(new [] { 0, 0 }, direction); // make no change if parameters are outside what's expected
        }

        /// <summary>
        /// Translates a given integer to its corresponding direction, favoring towards the left
        /// </summary>
        /// <param name="angle">Integer reflecting the angle amount to turn</param>
        /// <returns>Integer between 0 and 3 correspoinding to cardinal directions (0 being North)</returns>
        public static int GetDirection(int angle)
        {
            return (((int)Math.Ceiling(Math.Abs((double)angle / 360)) * 360 + angle) / 90) % 4;
        }
    }
}
