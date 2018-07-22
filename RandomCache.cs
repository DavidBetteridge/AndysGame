using System;
using System.Collections.Generic;

namespace RandomApp
{
    class RandomCache
    {
        readonly Random _rnd;

        readonly List<int> _numbers;

        public RandomCache(int seed)
        {
            _rnd = new Random(seed);
            _numbers = new List<int>();
        }

        public int FromSequence(int number)
        {
            if (_numbers.Count >= number)
            {
                return _numbers[number];
            }
            else
            {
                var next = _rnd.Next(8);
                _numbers.Add(next);
                return next;
            }
        }
    }
}
