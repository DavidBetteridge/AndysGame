using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RandomApp
{

    public enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

    public enum Piece
    {
        Bridge = 0,
        Monster1 = 1,
        Monster2 = 2,
        Monster3 = 3,
        Gold = 4,
        Heart1 = 5,
        Heart2 = 6,
        Heart3 = 7,
        Wall = 8,
        Exit = 9,
        Player = 10
    }

    class Program
    {
        static void Main(string[] args)
        { 
            var randomSource = new RandomSource(seed: 1234);
            var initialPosition = new GameState(size: 4, randomSource: randomSource);
            Console.WriteLine(initialPosition);

            var movesToExamine = new Stack<StateAndDirection>();

            var availableMovesFromCurrentPosition = initialPosition.FindAvailableMoves();
            foreach (var move in availableMovesFromCurrentPosition)
            {
                movesToExamine.Push(new StateAndDirection(initialPosition, move));
            }


            while (movesToExamine.Any())
            {
                var moveToTry = movesToExamine.Pop();

                var newPosition = moveToTry.GameState.MakeMove(moveToTry.Direction);
                Console.WriteLine("");
                Console.WriteLine(newPosition);

                if (newPosition.IsAWin())
                {
                    Console.WriteLine("It is possible :: " + newPosition.Moves());
                    Console.ReadKey();
                }

                if (!newPosition.IsALoss())
                {
                    var availableMovesFromNewPosition = newPosition.FindAvailableMoves();
                    foreach (var move in availableMovesFromNewPosition)
                    {
                        movesToExamine.Push(new StateAndDirection(newPosition, move));

                    }
                }
            }

            Console.WriteLine("It is NOT possible");
            Console.ReadKey();

        }



    }
}
