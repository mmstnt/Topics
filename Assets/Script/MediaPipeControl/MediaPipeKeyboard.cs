using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class NewBehaviourScript : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    static extern void keybd_event(byte bVK, byte bScan, int dwFlags, int dwExtraInfo);
}
