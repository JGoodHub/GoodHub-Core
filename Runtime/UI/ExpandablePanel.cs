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
        [SerializeField] private Button _expandButton;
        [SerializeField] private Button _collapseButton;
        [SerializeField] private Vector2 _collapsedSize;
        [SerializeField] private Vector2 _expandedSize;
        [SerializeField] private float _transitionDuration;
        [SerializeField] private RectTransform _expandablePanel;
        [SerializeField] private GameObject _collapsedContent;
        [SerializeField] private GameObject _expandedContent;
        [SerializeField] private CanvasGroup _collapsedCanvasGroup;
        [SerializeField] private CanvasGroup _expandedCanvasGroup;

        private bool _isExpanded;
        private bool _isTweening;

        protected virtual void Start()
        {
            _expandButton.onClick.AddListener(Expand);
            _collapseButton.onClick.AddListener(Collapse);

            SetToCollapsedState(true);
        }

        private void Expand()
        {
            if (_isTweening || _isExpanded)
                return;

            _isTweening = true;

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
                })
                .AddTrigger(_transitionDuration * 0.75f, () =>
                {
                    AsyncTween.Float(0f, 1f, _transitionDuration * 0.25f, expandedAlpha =>
                    {
                        _expandedCanvasGroup.alpha = expandedAlpha;
                    });
                });

            AsyncTween.Float(1f, 0f, _transitionDuration * 0.25f, collapsedAlpha =>
            {
                _collapsedCanvasGroup.alpha = collapsedAlpha;
            });
        }

        private void Collapse()
        {
            if (_isTweening || _isExpanded == false)
                return;

            _isTweening = true;

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
                })
                .AddTrigger(_transitionDuration * 0.75f, () =>
                {
                    AsyncTween.Float(0f, 1f, _transitionDuration * 0.25f, collapsedAlpha =>
                    {
                        _collapsedCanvasGroup.alpha = collapsedAlpha;
                    });
                });

            AsyncTween.Float(1f, 0f, _transitionDuration * 0.25f, expandedAlpha =>
            {
                _expandedCanvasGroup.alpha = expandedAlpha;
            });
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