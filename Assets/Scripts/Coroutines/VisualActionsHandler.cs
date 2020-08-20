using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualActionsHandler : MonoBehaviour
    {
        private CoroutineQueue _actionsQueue;
        [SerializeField] private BoardsBridge _bridge;
        private void Awake()
        {
            _actionsQueue = new CoroutineQueue(1, StartCoroutine);
            _bridge.InitVisualActionHandler(this);
        }

        private void Start() {
            // //TODO: remove after adding start screen
            // _actionsQueue.Run(WaitAndDo(delegate{}, 1.0f));
            // _actionsQueue.Run(WaitAndDo(() => Debug.LogError("!!!!!"), 1.0f));
            // _actionsQueue.Run(WaitAndDo(() => Debug.LogError("@@@@@@"), 1.0f));
            // _actionsQueue.Run(WaitAndDo(() => Debug.LogError("______"), 1.0f));
        }

        public void DoAndWait(Queue<Action> actions, float time)
        {
            _actionsQueue.Run(WaitAndDo(() =>
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue().Invoke();
                }
            }, time));
        }
        
        private IEnumerator DoAndWait(System.Action task, float time)
        {
            task();
            yield return new WaitForSeconds(time);
        }

        private IEnumerator WaitAndDo(System.Action task, float time)
        {
            yield return new WaitForSeconds(time);
            task();
        }
    }

}
