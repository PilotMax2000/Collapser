using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualBlock : MonoBehaviour
    {
        private Block _blockLink;
        private SpriteRenderer _sr;

        public void Init(Block block)
        {
            _blockLink = block;
            //Init picture
        }

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _sr.sprite = _blockLink.BlockParams.Sprite;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
