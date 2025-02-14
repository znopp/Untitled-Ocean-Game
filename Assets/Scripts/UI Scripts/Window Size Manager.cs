using UnityEngine;

namespace UI_Scripts
{
    public class WindowSizeManager : MonoBehaviour
    {
        public int minWidth = 800;
        public int minHeight = 600;

        private void Start()
        {
            CheckAndSetWindowSize();
        }

        private void Update()
        {
            CheckAndSetWindowSize();
        }

        private void CheckAndSetWindowSize()
        {
            if (!Screen.fullScreen && (Screen.width < minWidth || Screen.height < minHeight))
            {
                int newWidth = Mathf.Max(Screen.width, minWidth);
                int newHeight = Mathf.Max(Screen.height, minHeight);
                Screen.SetResolution(newWidth, newHeight, FullScreenMode.Windowed);
            }
        }
    }
}