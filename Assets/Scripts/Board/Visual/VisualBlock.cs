using UnityEngine;

namespace Collapser
{
    public class VisualBlock : MonoBehaviour
    {
        [SerializeField] private BlockParams _blockParams;
        private Block _blockLink;
        private SpriteRenderer _sr;

        public void Init(Block block)
        {
            _blockLink = block;
            _blockParams = block.BlockParams;
        }

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _sr.sprite = _blockLink.BlockParams.Sprite;
        }
    }

}
