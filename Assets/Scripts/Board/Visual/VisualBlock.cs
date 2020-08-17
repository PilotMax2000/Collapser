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
        [SerializeField] private BlockParams _blockParams;

        public void Init(Block block)
        {
            _blockLink = block;
            _blockParams = block.BlockParams;
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
