using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualActionsHandler : MonoBehaviour
    {
        [SerializeField] private BoardsBridge _bridge;
        private CoroutineQueue _actionsQueue;
        private void Awake()
        {
            _actionsQueue = new CoroutineQueue(1, StartCoroutine);
            _bridge.InitVisualActionHandler(this);
        }

        public  void DoAndWait(Queue<Action> actions, float time)
        {
            _actionsQueue.Run(WaitAndDo(() =>
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue().Invoke();
                }
            }, time));
        }

        public void DoWithoutWaiting(Action action)
        {
            _actionsQueue.Run(Do(action));
        }
        
        private static IEnumerator DoAndWait(System.Action task, float time)
        {
            task();
            yield return new WaitForSeconds(time);
        }

        private static IEnumerator WaitAndDo(System.Action task, float time)
        {
            yield return new WaitForSeconds(time);
            task();
        }

        private static IEnumerator Do(System.Action task)
        {
            task();
            yield break;
        }
    }

}
