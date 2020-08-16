using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualCell : MonoBehaviour
    {
        private Cell _cellLink;
        private VisualBlock _visualBlock;

        public void Init(Cell cell)
        {
            _cellLink = cell;
        }

        public void SetBlock(VisualBlock block)
        {
            _visualBlock = block;
        }

        private void OnMouseDown()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
