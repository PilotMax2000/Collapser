﻿using DG.Tweening;
using UnityEngine;

namespace Collapser
{
    public class VisualCell : MonoBehaviour
    {
        [SerializeField] private VisualBlock _visualBlock;
        [SerializeField] private BoardsBridge _boardsBridge;

        [Header("Settings")] 
        [SerializeField] private BoolVariable _isBlockingInput;
        [SerializeField] private FloatVariable _globalAnimationDuration;
        public VisualBlock VisualBlock => _visualBlock;
        
        private Vector2Int _boardPos;

        public void Init(Cell cell)
        {
            _boardPos = cell.BoardPos;
        }

        public void SetBlock(VisualBlock block, bool showSetAnim = true)
        {
            _visualBlock = block;
            var blockTransform = block.transform;
            if (showSetAnim == false)
            {
                blockTransform.parent = transform;
                blockTransform.localPosition = Vector3.zero;
                return;
            }
            
            blockTransform.DOMove(transform.position, _globalAnimationDuration.Value)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    blockTransform.parent = transform;
                    blockTransform.localPosition = Vector3.zero;
                });

        }

        public void UnbindBlock()
        {
            _visualBlock = null;
        }

        public void RemoveBlock()
        {
            _visualBlock.transform.DOScale(Vector3.zero, _globalAnimationDuration.Value).SetEase(Ease.InCubic).OnComplete(() =>
            {
                Destroy(_visualBlock.gameObject);
                UnbindBlock();
            });
            
        }

        private void OnMouseDown()
        {
            if (_isBlockingInput.Value)
            {
                return;
            }
            Debug.Log($"[OnClick] Player clicked cell{_boardPos}");
            _boardsBridge.LogicActionOnClicked(_boardPos);
        }
    }

}
