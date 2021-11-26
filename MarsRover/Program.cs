using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MarsRover
{
    class Program
    {
        public class Rover
        {
            public int PositionX { get; set; }
            public int PositionY { get; set; }
            public char Direction { get; set; }
            public string Rule { get; set; }

            public Rover(int x, int y, char direction,string rule)
            {
                this.PositionX = x;
                this.PositionY = y;
                this.Direction = direction;
                this.Rule = rule;
            }
        }

        public readonly static char[] navigator = new[] { 'N', 'E', 'S', 'W' };
        public readonly static List<Tuple<char, char, int>> moveOperator = new List<Tuple<char, char, int>>
        {
            {new Tuple <char, char, int>('X', 'E', 1)},
            {new Tuple <char, char, int>('X', 'W', -1)},
            {new Tuple <char, char, int>('Y', 'N', 1)},
            {new Tuple <char, char, int>('Y', 'S', -1)}
        };

        public static char[,] plateau;

        public static void SetRoverDirection(char direction, Rover rover)
        {
            var directionIndex = Array.FindIndex(navigator, x => x == rover.Direction);

            if (direction == 'L')
            {
                directionIndex -= 1;
                directionIndex = directionIndex < 0 ? navigator.Length - 1 : directionIndex;
            }
            else if (direction == 'R')
            {
                directionIndex = (directionIndex + 1) % navigator.Length;
            }
            else
            {
                throw new Exception("Incorrect direction...");
            }

            rover.Direction = navigator[directionIndex];
        }

        public static void SetRoverPosition(Rover rover, char coordinateType, int operation)
        {
            Type roverType = typeof(Rover);
            PropertyInfo roverInstance = roverType.GetProperty("Position" + coordinateType);
            var coordinateValue = (int)roverInstance.GetValue(rover);
            roverInstance.SetValue(rover, coordinateValue + operation);
        }

        public static void MoveRover(Rover rover)
        {
            List<char> rule = new List<char>();
            rule.AddRange(rover.Rule);

            foreach (var r in rule)
            {
                if (r != 'M')
                {
                    SetRoverDirection(r, rover);
                    continue;
                }

                if (moveOperator.Any(t => t.Item2 == rover.Direction))
                {
                    var tMover = moveOperator.Find(t => t.Item2 == rover.Direction);
                    SetRoverPosition(rover, tMover.Item1, tMover.Item3);
                    CheckSpaceBorder(plateau, rover.PositionX, rover.PositionY);
                }
                else
                {
                    throw new Exception("Incorrect direction...");
                }
            }
        }

        public static void CheckSpaceBorder(char[,] plateau, int positionX, int positionY)
        {
            if (plateau.GetLength(0) < positionX || plateau.GetLength(1) < positionY || positionX < 0 || positionY < 0)
            {
                throw new Exception("Rover is out of space...");
            }
        }

        static void Main(string[] args)
        {
            plateau = new char[5, 5];
            Rover r1 = new Rover(1, 2, 'N', "LMLMLMLMM");
            Rover r2 = new Rover(3, 3, 'E', "MMRMMRMRRM");

            IList<Rover> rovers = new List<Rover> { r1,r2};

            foreach(var rover in rovers)
            {
                CheckSpaceBorder(plateau, rover.PositionX, rover.PositionY);
                MoveRover(rover);
                Console.WriteLine(rover.PositionX + " " + rover.PositionY + " " + rover.Direction);
            }

            Console.ReadLine();
        }
    }
}
