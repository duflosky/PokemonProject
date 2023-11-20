using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;

        public EventSystem eventSystem;

        private void Awake()
        {
            if (Instance != null) DestroyImmediate(this);
            else Instance = this;
        }
    }
}