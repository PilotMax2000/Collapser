using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Collapser
{
    public class CoroutineQueue
    {
        // Maximum number of corouites to run at once
        private readonly uint _maxActive;

        // Delegate to start coroutines with
        private readonly Func<IEnumerator, Coroutine> _coroutineStarter;

        private readonly Queue<IEnumerator> _queue;

        //Number of current active coroutines
        private uint numActive;

        public CoroutineQueue(uint maxActive, Func<IEnumerator, Coroutine> coroutineStarter)
        {
            if(maxActive == 0)
            {
                throw new ArgumentException("Must be at least one", "maxActive");
            }
            this._maxActive = maxActive;
            this._coroutineStarter = coroutineStarter;
            _queue = new Queue<IEnumerator>();
        }

        // if the number of active coroutines if sunder the limit specified in the counstructor, run the given coroutine.
        //...Otherwise, queue it to be run when other coroutines finish.
        public void Run(IEnumerator coroutine)
        {
            if(numActive < _maxActive)
            {
                var runner = CoroutineRunner(coroutine);
                _coroutineStarter(runner);
            }
            else
            {
                _queue.Enqueue(coroutine);
            }
        }

        // Runs a coroutine then runs the next queued  coroutine if available
        //...Increments numActive before running the coroutine and decrement if after.
        private IEnumerator CoroutineRunner(IEnumerator coroutine)
        {
            numActive++;
            while (coroutine.MoveNext())
            {
                yield return coroutine.Current;
            }
            numActive--;
            if (_queue.Count > 0)
            {
                var next = _queue.Dequeue();
                Run(next);
            }
        }
    }
}
