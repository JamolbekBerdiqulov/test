using DG.Tweening;
using JigsawMemory.Utils.Input.Drag;
using System;
using UnityEngine;

namespace JigsawMemory.Jigsaw
{
    public class PuzzlePiece : MonoBehaviour
    {
        public event Action Rotated;

        public PuzzlePieceData PieceData { get; private set; }

        public Vector2 PuzzlePosition { get; private set; }

        public PiecesGroup JoinedGroup { get; private set; }

        public bool IsGrouped => JoinedGroup != null;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private DragHandler _dragHandler;
        [SerializeField] private float _rotationDuration = 0.5f;

        private Tweener _rotationTweener;

        private int _rotationCount = 0;
        private const float _rotationAngle = -360f / 4;

        public void Setup(PuzzlePieceData pieceData, Vector2 puzzlePosition, int rotationCount)
        {
            PieceData = pieceData;
            PuzzlePosition = puzzlePosition;

            _rotationCount = rotationCount;
            var rotationAngle = _rotationCount * _rotationAngle;
            transform.Rotate(0f, 0f, rotationAngle);

            _spriteRenderer.sprite = PieceData.PieceSprite;
        }

        /// <summary>
        /// Callback from inspector
        /// </summary>
        public void Rotate()
        {
            _rotationCount = (_rotationCount + 1) % _rotationCount;

            var rotation = transform.eulerAngles;
            rotation.z = _rotationCount * _rotationAngle;

            _rotationTweener?.Kill();
            _rotationTweener = transform.DORotate(rotation, _rotationDuration)
                .OnComplete(() =>
                {
                    Rotated?.Invoke();
                });
        }

        public void SetGroup(PiecesGroup group)
        {
            transform.SetParent(group.transform);

            _dragHandler.enabled = false;
            JoinedGroup = group;
        }
    }
}