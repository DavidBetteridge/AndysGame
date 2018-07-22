using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomApp
{
    class Program
    {
        enum Direction
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3
        }

        enum Piece
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

        class GameState
        {
            readonly int _size;
            readonly int _seed;

            Piece[,] _board;
            int _healthPoints = 3;
            int _gold = 0;
            int _playerX = 0;
            int _playerY = 0;

            bool _playerAtExit = false;

            public GameState(int size, int seed)
            {
                _size = size;
                _seed = seed;

                var rnd = new Random(seed);

                _board = new Piece[size, size];

                for (int column = 0; column < size; column++)
                {
                    for (int row = 0; row < size; row++)
                    {
                        _board[column, row] = (Piece)rnd.Next(9);
                    }
                }
                _board[0, 0] = Piece.Player;
                _board[size - 1, size - 1] = Piece.Exit;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                for (int row = _size - 1; row >= 0; row--)
                {
                    var line = "";
                    for (int column = 0; column < _size; column++)
                    {
                        switch (_board[column, row])
                        {
                            case Piece.Bridge:
                                line += "~~";
                                break;
                            case Piece.Monster1:
                                line += "-1";
                                break;
                            case Piece.Monster2:
                                line += "-2";
                                break;
                            case Piece.Monster3:
                                line += "-3";
                                break;
                            case Piece.Gold:
                                line += "AU";
                                break;
                            case Piece.Heart1:
                                line += "+1";
                                break;
                            case Piece.Heart2:
                                line += "+2";
                                break;
                            case Piece.Heart3:
                                line += "+3";
                                break;
                            case Piece.Exit:
                                line += "XX";
                                break;
                            case Piece.Player:
                                line += ":)";
                                break;
                            case Piece.Wall:
                                line += "--";
                                break;
                            default:
                                break;
                        }
                    }
                    sb.AppendLine(line);
                }

                return sb.ToString();
            }

            public GameState(GameState gameState)
            {
                this._size = gameState._size;
                this._seed = gameState._seed;
                this._board = (Piece[,])gameState._board.Clone();
                this._healthPoints = gameState._healthPoints;
                this._gold = gameState._gold;
                this._playerX = gameState._playerX;
                this._playerY = gameState._playerY;
            }

            internal bool IsAWin() => _playerAtExit;
            internal bool IsALoss() => (_healthPoints <= 0);

            internal IEnumerable<Direction> FindAvailableMoves()
            {
                var moves = new List<Direction>();
                if ((_playerX > 0) && (_board[_playerX - 1, _playerY] != Piece.Wall)) moves.Add(Direction.Left);
                if ((_playerX < _size - 1) && (_board[_playerX + 1, _playerY] != Piece.Wall)) moves.Add(Direction.Right);

                if ((_playerY > 0) && (_board[_playerX, _playerY - 1] != Piece.Wall)) moves.Add(Direction.Down);
                if ((_playerY < _size - 1) && (_board[_playerX, _playerY + 1] != Piece.Wall)) moves.Add(Direction.Up);

                return moves;
            }

            internal GameState MakeMove(Direction moveToTry)
            {
                var newState = new GameState(this);
                newState.ApplyMove(moveToTry);
                return newState;
            }

            void ApplyMove(Direction moveToTry)
            {
                var newPlayerX = 0;
                var newPlayerY = 0;
                switch (moveToTry)
                {
                    case Direction.Left:
                        newPlayerX = _playerX - 1;
                        newPlayerY = _playerY;
                        break;
                    case Direction.Right:
                        newPlayerX = _playerX + 1;
                        newPlayerY = _playerY;
                        break;
                    case Direction.Up:
                        newPlayerX = _playerX;
                        newPlayerY = _playerY + 1;
                        break;
                    case Direction.Down:
                        newPlayerX = _playerX;
                        newPlayerY = _playerY - 1;
                        break;
                    default:
                        break;
                }

                switch (_board[newPlayerX, newPlayerY])
                {
                    case Piece.Bridge:
                        break;
                    case Piece.Monster1:
                        _healthPoints -= 1;
                        break;
                    case Piece.Monster2:
                        _healthPoints -= 2;
                        break;
                    case Piece.Monster3:
                        _healthPoints -= 3;
                        break;
                    case Piece.Gold:
                        _gold++;
                        break;
                    case Piece.Heart1:
                        _healthPoints += 1;
                        break;
                    case Piece.Heart2:
                        _healthPoints += 2;
                        break;
                    case Piece.Heart3:
                        _healthPoints += 3;
                        break;
                    case Piece.Exit:
                        _playerAtExit = true;
                        break;
                    default:
                        break;
                }

                _board[newPlayerX, newPlayerY] = Piece.Player;

                var rnd = new Random(_seed);  //This needs a refactor to make it predictable
                switch (moveToTry)
                {
                    case Direction.Left:
                        for (int x = _playerX; x < _size - 1; x++)
                        {
                            _board[x, _playerY] = _board[x + 1, _playerY];
                        }
                        _board[_size - 1, _playerY] = (Piece)rnd.Next(9);
                        break;


                    case Direction.Right:
                        for (int x = _playerX; x > 0; x--)
                        {
                            _board[x, _playerY] = _board[x - 1, _playerY];
                        }
                        _board[0, _playerY] = (Piece)rnd.Next(9);
                        break;

                    case Direction.Up:
                        for (int y = _playerY; y > 0; y--)
                        {
                            _board[_playerX, y] = _board[_playerX, y - 1];
                        }
                        _board[_playerX, 0] = (Piece)rnd.Next(9);
                        break;

                    case Direction.Down:
                        for (int y = _playerY; y < _size - 1; y++)
                        {
                            _board[_playerX, y] = _board[_playerX, y + 1];
                        }
                        _board[_playerX, _size - 1] = (Piece)rnd.Next(9);
                        break;
                    default:
                        break;
                }

                _playerX = newPlayerX;
                _playerY = newPlayerY;
            }
        }


        static void Main(string[] args)
        {
            var initialPosition = new GameState(size: 4, seed: 1234);
            Console.WriteLine(initialPosition);

            var movesToExamine = new Stack<(GameState GameState, Direction Direction)>();

            var availableMovesFromCurrentPosition = initialPosition.FindAvailableMoves();
            foreach (var move in availableMovesFromCurrentPosition)
            {
                movesToExamine.Push((initialPosition, move));
            }


            while (movesToExamine.Any())
            {
                var moveToTry = movesToExamine.Pop();

                var newPosition = moveToTry.GameState.MakeMove(moveToTry.Direction);
                Console.WriteLine("");
                Console.WriteLine(newPosition);

                if (newPosition.IsAWin())
                {
                    Console.WriteLine("It is possible");
                    Console.ReadKey();
                }

                if (!newPosition.IsALoss())
                {
                    var availableMovesFromNewPosition = newPosition.FindAvailableMoves();
                    foreach (var move in availableMovesFromNewPosition)
                    {
                        movesToExamine.Push((newPosition, move));

                    }
                }
            }

            Console.WriteLine("It is NOT possible");
            Console.ReadKey();

        }



    }
}
