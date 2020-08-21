using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "Settings/Bool")]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        public string DeveloperDescription = "";
#endif
        [SerializeField] private bool value = false;

        public bool Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}

