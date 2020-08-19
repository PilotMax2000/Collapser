using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualActionsHandler : MonoBehaviour
    {
        private CoroutineQueue _actionsQueue;
        private void Awake()
        {
            _actionsQueue = new CoroutineQueue(1, StartCoroutine);
        }

        private void Start() {
            //TODO: remove after adding start screen
            _actionsQueue.Run(WaitAndDo(delegate{}, 1.0f));
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
