using System;
using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime.UI
{
    public class ExpandablePanel : MonoBehaviour
    {
        [Header("Expandable Panel Settings")]
        [SerializeField] protected Button _expandButton;
        [SerializeField] protected Button _collapseButton;
        [SerializeField] protected Vector2 _collapsedSize;
        [SerializeField] protected Vector2 _expandedSize;
        [SerializeField] protected float _transitionDuration;
        [SerializeField] protected RectTransform _expandablePanel;
        [SerializeField] protected GameObject _collapsedContent;
        [SerializeField] protected GameObject _expandedContent;
        [SerializeField] protected CanvasGroup _collapsedCanvasGroup;
        [SerializeField] protected CanvasGroup _expandedCanvasGroup;

        private bool _isExpanded;
        private bool _isTweening;

        public event Action OnExpandStarted;
        public event Action OnExpandFinished;

        public event Action OnCollapseStarted;
        public event Action OnCollapseFinished;

        protected virtual void Awake()
        {
            _expandButton.onClick.AddListener(Expand);
            _collapseButton.onClick.AddListener(Collapse);

            SetToCollapsedState(true);
        }

        protected void Expand()
        {
            if (_isTweening || _isExpanded)
                return;

            OnExpandStarted?.Invoke();

            _isTweening = true;
            float transitionDurationQuarter = _transitionDuration * 0.25f;

            _expandedContent.SetActive(true);
            _collapsedContent.SetActive(true);

            _expandedCanvasGroup.alpha = 0f;
            _collapsedCanvasGroup.alpha = 1f;

            Vector2 baseSizeDelta = _expandablePanel.sizeDelta;

            AsyncTween.Vector2(baseSizeDelta, _expandedSize, _transitionDuration, newSizeDelta =>
                {
                    _expandablePanel.sizeDelta = newSizeDelta;
                })
                .OnCompleted(() =>
                {
                    _isExpanded = true;
                    _isTweening = false;

                    _collapsedContent.SetActive(false);

                    OnExpandFinished?.Invoke();
                })
                .AddTrigger(_transitionDuration - transitionDurationQuarter, () =>
                {
                    AsyncTween.Float(0f, 1f, transitionDurationQuarter, expandedAlpha =>
                    {
                        _expandedCanvasGroup.alpha = expandedAlpha;
                    });
                })
                .SetEasing(Easing.OutQuart);

            AsyncTween.Float(1f, 0f, transitionDurationQuarter, collapsedAlpha =>
            {
                _collapsedCanvasGroup.alpha = collapsedAlpha;
            });
        }

        protected void Collapse()
        {
            if (_isTweening || _isExpanded == false)
                return;

            OnCollapseStarted?.Invoke();

            _isTweening = true;
            float transitionDurationQuarter = _transitionDuration * 0.25f;

            _expandedContent.SetActive(true);
            _collapsedContent.SetActive(true);

            _expandedCanvasGroup.alpha = 1f;
            _collapsedCanvasGroup.alpha = 0f;

            Vector2 baseSizeDelta = _expandablePanel.sizeDelta;

            AsyncTween.Vector2(baseSizeDelta, _collapsedSize, _transitionDuration, newSizeDelta =>
                {
                    _expandablePanel.sizeDelta = newSizeDelta;
                })
                .OnCompleted(() =>
                {
                    _isExpanded = false;
                    _isTweening = false;

                    _expandedContent.SetActive(false);

                    OnCollapseFinished?.Invoke();
                })
                .AddTrigger(_transitionDuration - transitionDurationQuarter, () =>
                {
                    AsyncTween.Float(0f, 1f, transitionDurationQuarter, collapsedAlpha => _collapsedCanvasGroup.alpha = collapsedAlpha);
                })
                .SetEasing(Easing.OutQuart);

            AsyncTween.Float(1f, 0f, transitionDurationQuarter, expandedAlpha => _expandedCanvasGroup.alpha = expandedAlpha);
        }

        private void SetToExpandedState(bool snap = false)
        {
            if (snap)
                _expandablePanel.sizeDelta = _expandedSize;

            _expandedCanvasGroup.alpha = 1f;
            _collapsedCanvasGroup.alpha = 0f;

            _collapsedContent.SetActive(false);
            _expandedContent.SetActive(true);
        }

        private void SetToCollapsedState(bool snap = false)
        {
            if (snap)
                _expandablePanel.sizeDelta = _collapsedSize;

            _expandedCanvasGroup.alpha = 0f;
            _collapsedCanvasGroup.alpha = 1f;

            _collapsedContent.SetActive(true);
            _expandedContent.SetActive(false);
        }

        [ContextMenu("Set To Expanded State")]
        private void SetToExpandedStateMenuItem() => SetToExpandedState(true);

        [ContextMenu("Set To Collapsed State")]
        private void SetToCollapsedStateMenuItem() => SetToCollapsedState(true);
    }
}