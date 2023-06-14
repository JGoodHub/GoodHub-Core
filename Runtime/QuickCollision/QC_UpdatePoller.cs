using System;
using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class QC_UpdatePoller : MonoBehaviour
    {
        private void LateUpdate()
        {
            QC_Manager.CheckForCollisions();
        }
    }
}
