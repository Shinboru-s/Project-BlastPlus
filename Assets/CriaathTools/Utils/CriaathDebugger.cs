using UnityEngine;

namespace Criaath.MiniTools
{
    public static class CriaathDebugger
    {
        public static void Log(object message)
        {
            if (!ShouldLog()) return;
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(Color.white)}>{message}</color>");
        }
        public static void Log(object message, Color color)
        {
            if (!ShouldLog()) return;
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>");
        }

        public static void Log(object header, Color headerColor, object message, Color messageColor)
        {
            if (!ShouldLog()) return;
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(headerColor)}>{header}</color>: \n<color=#{ColorUtility.ToHtmlStringRGBA(messageColor)}>{message}</color>");
        }

        public static void LogWarning(object message, Color color)
        {
            if (!ShouldLog()) return;
            Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>");
        }
        public static void LogWarning(object header, Color headerColor, object message, Color messageColor)
        {
            if (!ShouldLog()) return;
            Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGBA(headerColor)}>{header}</color>: \n<color=#{ColorUtility.ToHtmlStringRGBA(messageColor)}>{message}</color>");
        }
        public static void LogError(object message, Color color)
        {
            if (!ShouldLog()) return;
            Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>");
        }
        public static void LogError(object header, Color headerColor, object message, Color messageColor)
        {
            if (!ShouldLog()) return;
            Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGBA(headerColor)}>{header}</color>: \n<color=#{ColorUtility.ToHtmlStringRGBA(messageColor)}>{message}</color>");
        }


        private static bool ShouldLog()
        {
            return Debug.isDebugBuild;
        }
    }
}
