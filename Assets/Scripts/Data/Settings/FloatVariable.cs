using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "Settings/Float")]
    public class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        public string DeveloperDescription = "";
#endif
        [SerializeField] private float value = 0f;

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}