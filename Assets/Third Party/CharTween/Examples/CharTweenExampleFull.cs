﻿using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharTween.Examples
{
    public class CharTweenExampleFull : MonoBehaviour
    {
        public TMP_Text Target;
        private CharTweener _tweener;

        [Header("Tween 1")]
        public float radius = 1;
        [Header("Tween 2")]
        public float strenght = 1;
        public int vibrato = 1;
        public float randomness = 1;
        [Header("Tween 3")]
        public float height = 1;

        Sequence sequence;

        public void Start()
        {
            _tweener = Target.GetCharTweener();
            ApplyTweenToLine(0, Tween1);
            ApplyTweenToLine(1, Tween2);
            ApplyTweenToLine(2, Tween3);
            ApplyTweenToLine(3, Tween4);
            ApplyTweenToLine(4, Tween5);
        }

        // Basic example 1, oscillating characters and color
        private void Tween1(int start, int end)
        {
            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                var circleTween = _tweener.DOCircle(i, radius, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart);
                var colorTween = _tweener.DOColor(i, Color.yellow, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo);
                circleTween.fullPosition = timeOffset;
                colorTween.fullPosition = timeOffset;
            }
        }

        // Basic example 2, jittery characters + random color
        private void Tween2(int start, int end)
        {
            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                _tweener.DOShakePosition(i, 1, strenght, vibrato, randomness, false, false)
                    .SetLoops(-1, LoopType.Restart);
                var colorTween = _tweener.DOColor(i, UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1),
                        UnityEngine.Random.Range(0.1f, 0.5f))
                    .SetLoops(-1, LoopType.Yoyo);
                colorTween.fullPosition = timeOffset;
            }
        }

        // Sequence example, bubbly fade-in + bounce
        private void Tween3(int start, int end)
        {
            var sequence = DOTween.Sequence();

            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                var charSequence = DOTween.Sequence();
                charSequence.Append(_tweener.DOLocalMoveY(i, height, 0.5f).SetEase(Ease.InOutCubic))
                    .Join(_tweener.DOFade(i, 0, 0.5f).From())
                    .Join(_tweener.DOScale(i, 0, 0.5f).From().SetEase(Ease.OutBack, 5))
                    .Append(_tweener.DOLocalMoveY(i, 0, 0.5f).SetEase(Ease.OutBounce));
                sequence.Insert(timeOffset, charSequence);
            }

            sequence.SetLoops(-1, LoopType.Yoyo);
        }

        // Rotation example
        private void Tween4(int start, int end)
        {
            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                var rotationTween = _tweener.DOLocalRotate(i, UnityEngine.Random.onUnitSphere * 360, 2, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental);
                rotationTween.fullPosition = timeOffset;
            }
        }


        private void Tween5(int start, int end)
        {
            sequence = DOTween.Sequence();
            Vector3 move = new Vector3(-0.5f, -0.3f, 0);
            float duration = 0.2f;

            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                var charSequence = DOTween.Sequence();
                charSequence.Append(_tweener.DOLocalMove(i, move, 0.5f).SetEase(Ease.InOutQuad))
                    .Join(_tweener.DOFade(i, 0, duration * 2).From().SetEase(Ease.InOutCubic))
                    .Join(_tweener.DOScale(i, 0, duration).From().SetEase(Ease.OutBack, 3))
                    .Append(_tweener.DOLocalMove(i, Vector3.zero, duration).SetEase(Ease.InOutQuad));
                sequence.Insert(timeOffset, charSequence);
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.IsPressed() == true)
            {
                sequence.Kill(true);
            }
        }

        private void ApplyTweenToLine(int line, Action<int, int> tween)
        {
            if (line >= Target.textInfo.lineCount)
                return;

            var lineInfo = Target.textInfo.lineInfo[line];
            tween(lineInfo.firstCharacterIndex, lineInfo.lastCharacterIndex);
        }
    }
}

