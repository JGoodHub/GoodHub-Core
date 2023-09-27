using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.PopupSystem
{

    public class PopupBase : MonoBehaviour
    {
        private bool _popupReadyForDestruction;

        public bool PopupReadyForDestruction => _popupReadyForDestruction;

        public virtual bool CanBeShown()
        {
            return true;
        }

        public virtual void PopupActivated()
        {
        }

        public virtual void PopupBeginDismissal()
        {
            PopupFinishedDismissal();
        }

        protected void PopupFinishedDismissal()
        {
            _popupReadyForDestruction = true;
        }
    }

}