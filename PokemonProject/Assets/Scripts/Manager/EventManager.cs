using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;

        private EventSystem eventSystem;

        private void Awake()
        {
            if (Instance != null) DestroyImmediate(this);
            else Instance = this;
        }

        private void Start()
        {
            eventSystem = GetComponent<EventSystem>();
        }

        public void SelectObject(GameObject obj)
        {
            eventSystem.SetSelectedGameObject(obj);
        }
    }
}