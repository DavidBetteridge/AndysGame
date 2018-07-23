using System.Collections.Generic;
using System.Text;

namespace RandomApp
{
    class GameState
    {
        readonly int _size;
        readonly RandomSource _randomSource;


        Piece[,] _board;
        int _healthPoints = 5;
        int _gold = 0;
        int _playerX = 0;
        int _playerY = 0;
        int _randomOffset = 0;

        string _moves = "";
        bool _playerAtExit = false;

        public GameState(int size, RandomSource randomSource)
        {
            _size = size;
            _randomSource = randomSource;

            _board = new Piece[size, size];

            for (int column = 0; column < size; column++)
            {
                for (int row = 0; row < size; row++)
                {
                    _board[column, row] = GenerateRandomPiece();
                }
            }
            _board[0, 0] = Piece.Player;
            _board[size - 1, size - 1] = Piece.Exit;
        }

        Piece GenerateRandomPiece()
        {
            var number = _randomSource.FromSequence(_randomOffset);
            _randomOffset++;
            return (Piece)number;
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
            _size = gameState._size;
            _randomOffset = gameState._randomOffset;
            _board = (Piece[,])gameState._board.Clone();
            _healthPoints = gameState._healthPoints;
            _gold = gameState._gold;
            _playerX = gameState._playerX;
            _playerY = gameState._playerY;
            _randomSource = gameState._randomSource;
            _moves = gameState._moves;
        }

        internal bool IsAWin()
        {
            return _playerAtExit;
        }

        internal bool IsALoss()
        {
            return (_healthPoints <= 0);
        }

        internal string Moves()
        {
            return _moves;
        }

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
                    _moves += "L ";
                    break;
                case Direction.Right:
                    newPlayerX = _playerX + 1;
                    newPlayerY = _playerY;
                    _moves += "R ";
                    break;
                case Direction.Up:
                    newPlayerX = _playerX;
                    newPlayerY = _playerY + 1;
                    _moves += "U ";
                    break;
                case Direction.Down:
                    newPlayerX = _playerX;
                    newPlayerY = _playerY - 1;
                    _moves += "D ";
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

            switch (moveToTry)
            {
                case Direction.Left:
                    for (int x = _playerX; x < _size - 1; x++)
                    {
                        _board[x, _playerY] = _board[x + 1, _playerY];
                    }
                    _board[_size - 1, _playerY] = GenerateRandomPiece();
                    break;


                case Direction.Right:
                    for (int x = _playerX; x > 0; x--)
                    {
                        _board[x, _playerY] = _board[x - 1, _playerY];
                    }
                    _board[0, _playerY] = GenerateRandomPiece();
                    break;

                case Direction.Up:
                    for (int y = _playerY; y > 0; y--)
                    {
                        _board[_playerX, y] = _board[_playerX, y - 1];
                    }
                    _board[_playerX, 0] = GenerateRandomPiece();
                    break;

                case Direction.Down:
                    for (int y = _playerY; y < _size - 1; y++)
                    {
                        _board[_playerX, y] = _board[_playerX, y + 1];
                    }
                    _board[_playerX, _size - 1] = GenerateRandomPiece();
                    break;
                default:
                    break;
            }

            _playerX = newPlayerX;
            _playerY = newPlayerY;
        }
    }
}
