// using UnityEngine;

// public class DebugLoggerOnScreen : MonoBehaviour
// {
//     string debugText = "";

//     private void OnEnable()
//     {
//         Application.logMessageReceived += HandleLog;
//     }

//     private void OnDisable()
//     {
//         Application.logMessageReceived -= HandleLog;
//     }

//     void HandleLog(string logString, string stackTrace, LogType type)
//     {
//         debugText += logString + "\n";
//         if (debugText.Length > 5000) // Limit the text buffer size to prevent overflow
//         {
//             debugText = debugText.Substring(debugText.Length - 5000);
//         }
//     }

//     private void OnGUI()
//     {
//         GUI.Label(new Rect(10, 10, Screen.width, Screen.height), debugText);
//     }
// }
