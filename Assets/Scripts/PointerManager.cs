using System;
using UnityEngine;

namespace TiltBrush
{
    [ExecuteInEditMode]
    public class PointerManager : MonoBehaviour
    {
        public static PointerManager m_Instance;
        public GameObject MainPointer;

        private void Awake()
        {
            m_Instance = this;
        }
    }
}