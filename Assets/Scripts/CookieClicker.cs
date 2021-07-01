using UnityEngine;

namespace DefaultNamespace
{
    public class CookieClicker
    {
        private static CookieClicker _clicker;

        public static CookieClicker SingletonAccess
        {
            get
            {
                _clicker ??= new CookieClicker();
                return _clicker;
            }
            private set => _clicker = value;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnGameStart()
        {
            SingletonAccess = new CookieClicker();
        }

        public CookieClicker()
        {
            CurrentValue = 0;
            CurrentAddValue = 1;
        }

        public int CurrentValue { get; private set; }
        public int CurrentAddValue { get; set; }


        public void IncrementValue()
        {
            CurrentValue += CurrentAddValue;
        }
    }
}