using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    class Snake
    {
        private char _snake = (char)214;
        private char _snakeBody = (char)111;
        private char _apple = (char)64;

        private int _wallWidth = 40;
        private int _wallHeight = 70;

        private int[] _xPosition = new int[50];
        private int[] _yPosition = new int[50];

        private bool _isGameOn = true;
        private decimal _gameSpeed = 150m;

        private int _appleXDim = 10;
        private int _appleYDim = 10;
        private bool _appleEaten = true;
        private int _numberApplesEaten = 0;

        private Random _random = new Random();
        private ConsoleKey _keyboardCommand;

        public void InitGame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.SetWindowSize(1, 1);

            if (Console.WindowLeft + Console.WindowWidth < _wallWidth && Console.WindowTop + Console.WindowHeight < _wallHeight) {
                //System.Console.SetBufferSize(_wallWidth, _wallHeight);
            }

            if (_isGameOn == true) {    
                Console.SetWindowSize(_wallWidth, _wallHeight);
                BuildWall();
                InitSnake();
           }
        }

        public void InitSnake()
        {
            _xPosition[0] = 35;
            _yPosition[0] = 20;
        }

        public void BuildWall()
        {
            for (int i = 1; i < _wallWidth; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(1, i);
                Console.Write("#");
                Console.SetCursorPosition(_wallHeight-1, i);
                Console.Write("#");
            }

            for (int i = 1; i < _wallHeight; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(i, 1);
                Console.Write("#");
                Console.SetCursorPosition(i, _wallWidth-1);
                Console.Write("#");
            }
        }

        public void WriteSnake()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(_xPosition[0], _yPosition[0]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(_snake);

            for (int i = 1; i < _numberApplesEaten + 1; i++)
            {
                Console.SetCursorPosition(_xPosition[i], _yPosition[i]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(_snakeBody);
            }

            Console.SetCursorPosition(_xPosition[_numberApplesEaten + 1], _yPosition[_numberApplesEaten + 1]);
            Console.Write(" ");

            for (int i = _numberApplesEaten + 1; i > 0; i--)
            {
                _xPosition[i] = _xPosition[i - 1];
                _yPosition[i] = _yPosition[i - 1];
            }
        }

        public void ClearSnake()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(_xPosition[0], _yPosition[0]);
            Console.Write(" ");
        }

        public void DidSnakeHitWall()
        {
            if (_xPosition[0] == 1 || _xPosition[0] == _wallHeight-1 || _yPosition[0] == 1 || _yPosition[0] == _wallWidth-1)
            {
                _isGameOn = false;
                Console.Clear();
                Console.SetCursorPosition(0, 20);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("The snake hit the wall and died. You have eaten " + _numberApplesEaten + " apples.");
            }
        }

        public void SetApplePositionOnScreen()
        {
            if (_appleEaten)
            {
                _appleXDim = _random.Next(0 + 2, _wallHeight - 2);
                _appleYDim = _random.Next(0 + 2, _wallWidth - 2);
                _appleEaten = false;
                PaintApple();
            }
        }

        public void PaintApple()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(_appleXDim, _appleYDim);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(_apple);
        }

        public void WasAppleEaten()
        {
            if (_xPosition[0] == _appleXDim && _yPosition[0] == _appleYDim)
            {
                _appleEaten = true;
                _numberApplesEaten++;
                _gameSpeed *= .925m;
            }
        }

        public void StartGame()
        {
            InitGame();
            if (_isGameOn == true) {
               WriteSnake();
               SetApplePositionOnScreen();
               MoveSnake();
               WriteSnake();
            } else {
                Console.WriteLine("Game Over!");
            }
        }

        public void MoveSnake()
        {
            _keyboardCommand = Console.ReadKey().Key;

            do
            {
                switch (_keyboardCommand)
                {
                    case ConsoleKey.LeftArrow:
                        ClearSnake();
                        _xPosition[0]--;
                        break;

                    case ConsoleKey.UpArrow:
                        ClearSnake();
                        _yPosition[0]--;
                        break;

                    case ConsoleKey.RightArrow:
                        ClearSnake();
                        _xPosition[0]++;
                        break;

                    case ConsoleKey.DownArrow:
                        ClearSnake();
                        _yPosition[0]++;
                        break;
                }

                WriteSnake();
                DidSnakeHitWall();
                WasAppleEaten();
                SetApplePositionOnScreen();

                if (Console.KeyAvailable) _keyboardCommand = Console.ReadKey().Key;
                System.Threading.Thread.Sleep(Convert.ToInt32(_gameSpeed));

            } while (_isGameOn);
        }
    }
}