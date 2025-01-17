using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoodHub.Core.Runtime
{
    public class SelectionController : SceneSingleton<SelectionController>
    {
        private HashSet<SelectableEntity> _allEntities = new HashSet<SelectableEntity>();

        private SelectableEntity _selectedEntity;

        public SelectableEntity SelectedEntity => _selectedEntity;

        public delegate void SelectionChange(SelectionController sender, SelectableEntity oldEntity, SelectableEntity newEntity);

        public event SelectionChange OnSelectionChanged;

        private void Start()
        {
            TouchInput.OnTouchClick += OnTouchClick;
        }

        private void OnDestroy()
        {
            TouchInput.OnTouchClick -= OnTouchClick;
        }

        public void RegisterEntity(SelectableEntity entity)
        {
            _allEntities.Add(entity);
        }

        public void UnregisterEntity(SelectableEntity entity)
        {
            if (entity == SelectedEntity)
                Singleton.ClearSelection();

            if (_allEntities.Contains(entity))
                _allEntities.Remove(entity);
        }

        private void OnTouchClick(TouchInput.TouchData touchData)
        {
            if (touchData.DownOverUI || touchData.UpOverUI)
                return;

            Vector2 selectionPoint = RaycastPlane.QueryPlaneAsXY();
            SelectableEntity newlySelectedEntity = GetFirstSelectedEntity(selectionPoint);

            SetSelection(newlySelectedEntity);
        }

        public void SetSelection(SelectableEntity newSelection)
        {
            // Empty to empty so no change
            if (_selectedEntity == null && newSelection == null)
                return;

            // Selecting an entity from empty
            if (_selectedEntity == null && newSelection != null)
            {
                _selectedEntity = newSelection;

                _selectedEntity.SelectionStatusChanged(true);

                OnSelectionChanged?.Invoke(this, null, newSelection);
                return;
            }

            // Changing from one entity to another
            if (_selectedEntity != null && newSelection != null && _selectedEntity != newSelection)
            {
                SelectableEntity oldSelection = _selectedEntity;
                _selectedEntity = newSelection;

                oldSelection.SelectionStatusChanged(false);
                newSelection.SelectionStatusChanged(true);

                OnSelectionChanged?.Invoke(this, oldSelection, newSelection);
                return;
            }

            // Deselecting the current entity
            if (_selectedEntity != null && newSelection == null)
            {
                SelectableEntity oldSelection = _selectedEntity;
                _selectedEntity = null;

                oldSelection.SelectionStatusChanged(false);

                OnSelectionChanged?.Invoke(this, oldSelection, null);

                return;
            }
        }

        public void ClearSelection()
        {
            if (_selectedEntity == null)
                return;

            SelectableEntity oldSelectedEntity = _selectedEntity;
            _selectedEntity = null;

            oldSelectedEntity.SelectionStatusChanged(false);

            OnSelectionChanged?.Invoke(this, oldSelectedEntity, null);
        }

        public SelectableEntity GetFirstSelectedEntity(Vector2 selectionPoint)
        {
            foreach (SelectableEntity entity in _allEntities)
            {
                float entityDistanceFromSelectionPoint = Vector2.Distance(new Vector2(entity.transform.position.x, entity.transform.position.z), selectionPoint);

                if (entityDistanceFromSelectionPoint <= entity.SelectionRadius)
                {
                    return entity;
                }
            }

            return null;
        }

        public List<SelectableEntity> GetEntitiesWithinRadius(Vector3 queryPosition, float radius)
        {
            List<SelectableEntity> nearbyEntities = new List<SelectableEntity>();

            foreach (SelectableEntity entity in _allEntities)
            {
                if (Vector3.Distance(queryPosition, entity.transform.position) <= radius)
                    nearbyEntities.Add(entity);
            }

            return nearbyEntities;
        }
    }
}