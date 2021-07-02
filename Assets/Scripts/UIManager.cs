using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace DefaultNamespace
{
    public class UIManager : MonoBehaviour
    {
        private EventSystem _eventSystem;
        private InputSystemUIInputModule _newInputModule;
        private Camera _camera;
        private Coroutine _coroutine;
        private RectTransform _rectTransform;

        public TMP_Text textPrefab;
        public TMP_Text scoreCounter;

        private void Awake()
        {
            _eventSystem = EventSystem.current;
            _camera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _newInputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        }

        private void Update()
        {
            UpdateScore();
        }

        public void UpdateScore()
        {
            if (scoreCounter)
            {
                scoreCounter.text = CookieClicker.SingletonAccess.CurrentValue.ToString();
            }
        }


        public void SpawnText(string text)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(StartUpdatingText(text));
        }

        private IEnumerator StartUpdatingText(string text)
        {
            TMP_Text textClone = ObjectPooler.Instantiate(textPrefab, 20, transform);
            textClone.transform.localPosition = Vector3.zero;
            textClone.gameObject.SetActive(true);

            textClone.text = $"+{text}";

            DoTweenManager manager = textClone.GetComponent<DoTweenManager>();
            yield return manager.Tween();
            
        }

        private Vector3 GetWorldPositionFromScreen(Vector2 mousePosition)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, mousePosition, _camera,
                out Vector3 localPoint);
            localPoint *= 100;
            Debug.Log(localPoint);
            return localPoint;
        }
    }
}