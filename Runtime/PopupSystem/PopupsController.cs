using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoodHub.Core.Runtime.Utils;
using UnityEngine;

namespace GoodHub.Core.Runtime.PopupSystem
{

    public enum PopupPriority
    {
        LEVEL_0 = 0,
        LEVEL_10 = 10,
        LEVEL_20 = 20,
        LEVEL_30 = 30,
        LEVEL_40 = 40,
        LEVEL_50 = 50,
        LEVEL_60 = 60,
        LEVEL_70 = 70,
        LEVEL_80 = 80,
        LEVEL_90 = 90,
        LEVEL_100 = 100,
    }

    public class PopupsController : GlobalSingleton<PopupsController>
    {
        [SerializeField] private string _popupResourcesFolder;
        [SerializeField] private RectTransform _popupContainer;

        private PriorityQueue<PopupBase> _popupQueue;
        private Dictionary<string, GameObject> _popupPrefabsCache;

        private PopupBase _activePopup;

        private Coroutine _dismissalRoutine;

        public PopupBase ActivePopup => _activePopup;

        public event Action<PopupBase> OnPopupActivated;
        public event Action<PopupBase> OnPopupDismissed;

        protected override void Awake()
        {
            base.Awake();
            
            _popupQueue = new PriorityQueue<PopupBase>();
            _popupPrefabsCache = new Dictionary<string, GameObject>();
            
            if (string.IsNullOrWhiteSpace(_popupResourcesFolder) == false && _popupResourcesFolder.EndsWith("/") == false)
            {
                _popupResourcesFolder += "/";
            }
        }

        /// <summary>
        /// Fetches and creates an instance of the popup, adding it to the priority queue if another popup is already active. NOTE: Popups in the queue exist in a deactivated state.
        /// </summary>
        /// <param name="popupKey">The name of the popup prefab in the resource folder.</param>
        /// <param name="popupPriority">The priority of the popup in the queue, higher means more important.</param>
        /// <param name="initialisationCallback">Callback used to initialise popup with dedicated logic/params.</param>
        /// <typeparam name="T">Popup subtype inherited from PopupBas.e</typeparam>
        /// <returns>The instance of the popup.</returns>
        public T EnqueuePopup<T>(string popupKey, PopupPriority popupPriority = PopupPriority.LEVEL_50, Action<T> initialisationCallback = null) where T : PopupBase
        {
            GameObject popupPrefab = GetPopupPrefab(popupKey);

            if (popupPrefab == null)
                return null;

            // Create the popup instance

            GameObject popupObject = Instantiate(popupPrefab, _popupContainer);
            popupObject.name = popupPrefab.name;

            if (popupObject.TryGetComponent(out T popupScript) == false)
            {
                Debug.LogError($"[{GetType()}]: Popup does not contain component of type '{typeof(T)}'");
                return null;
            }

            initialisationCallback?.Invoke(popupScript);

            // No other popups, leave it active

            if (_activePopup == null && _popupQueue.Count == 0)
            {
                if (popupScript.CanBeShown() == false)
                {
                    Destroy(_activePopup.gameObject);
                    _activePopup = null;

                    Debug.LogError($"[{GetType()}]: Popup failed the CanBeShown check and was aborted '{typeof(T)}'");
                    return null;
                }

                _activePopup = popupScript;
                _activePopup.PopupActivated();
                
                OnPopupActivated?.Invoke(_activePopup);

                return popupScript;
            }

            // Otherwise add it to the priority queue

            popupObject.SetActive(false);
            _popupQueue.Enqueue((int) popupPriority, popupScript);

            return popupScript;
        }

        /// <summary>
        /// Destroy the active popup and activate the next if one is waiting.
        /// </summary>
        public void DismissActivePopup()
        {
            if (_activePopup == null)
                return;

            if (_dismissalRoutine != null)
                return;

            _dismissalRoutine = StartCoroutine(DismissActivePopupRoutine());
        }

        private IEnumerator DismissActivePopupRoutine()
        {
            _activePopup.PopupBeginDismissal();

            yield return new WaitUntil(() => _activePopup.PopupReadyForDestruction);

            OnPopupDismissed?.Invoke(_activePopup);
            Destroy(_activePopup.gameObject);

            _activePopup = null;

            if (_popupQueue.Count == 0)
            {
                _dismissalRoutine = null;
                yield break;
            }

            // Activate the next popup in the queue

            PopupBase nextActivePopup = _popupQueue.Dequeue();

            while (nextActivePopup.CanBeShown() == false)
            {
                Debug.LogError($"[{GetType()}]: Popup failed the CanBeShown check and was aborted '{nextActivePopup.GetType()}'");

                Destroy(nextActivePopup.gameObject);

                if (_popupQueue.Count == 0)
                    yield break;

                nextActivePopup = _popupQueue.Dequeue();
            }

            _activePopup = nextActivePopup;
            _activePopup.gameObject.SetActive(true);
            _activePopup.PopupActivated();
            OnPopupActivated?.Invoke(_activePopup);

            _dismissalRoutine = null;
        }

        /// <summary>
        /// Checks if any popups are currently waiting in the queue.
        /// </summary>
        public bool IsAnyPopupEnqueued(bool includeActive = false)
        {
            if (includeActive && _activePopup != null)
                return true;

            return _popupQueue.Count > 0;
        }

        /// <summary>
        /// Checks if any popups of the given type are currently waiting in the queue.
        /// </summary>
        public bool IsPopupOfTypeEnqueued(string popupKey, bool includeActive = false)
        {
            if (includeActive && _activePopup != null && _activePopup.name == popupKey)
                return true;

            if (_popupQueue.Count == 0)
                return false;

            return _popupQueue.Values.Find(item => item.value.name == popupKey).value;
        }

        private GameObject GetPopupPrefab(string popupKey)
        {
            if (string.IsNullOrEmpty(popupKey))
                return null;

            if (_popupPrefabsCache.TryGetValue(popupKey, out GameObject cachedPopupPrefab))
                return cachedPopupPrefab;

            string fullPopupPath = _popupResourcesFolder + popupKey;

            GameObject popupPrefab = Resources.Load<GameObject>(fullPopupPath);

            if (popupPrefab == null)
            {
                Debug.LogError($"[{GetType()}]: Unable to load prefab at path '{fullPopupPath}'");
                return null;
            }

            _popupPrefabsCache.Add(popupKey, popupPrefab);

            return popupPrefab;
        }
    }

}