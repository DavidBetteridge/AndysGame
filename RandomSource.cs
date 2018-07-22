using System;
using System.Collections.Generic;

namespace RandomApp
{
    class RandomSource
    {
        readonly Random _rnd;

        readonly List<int> _numbers;

        public RandomSource(int seed)
        {
            _rnd = new Random(seed);
            _numbers = new List<int>();
        }

        public int FromSequence(int number)
        {
            if (_numbers.Count <= number)
            {
                _numbers.Add(_rnd.Next(9));
            }

            return _numbers[number];
        }
    }
}
