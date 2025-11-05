using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TrackIRBridge : MonoBehaviour
{

    private const string DllName = "NPClient64.dll";


    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    private static extern int NP_RegisterWindowHandle(IntPtr hWnd);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    private static extern int NP_QueryVersion(out ushort version);


    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    private static extern int NP_GetData(ref TrackIRData data);

    [StructLayout(LayoutKind.Sequential)]
    public struct TrackIRData
    {
        public float roll;
        public float pitch;
        public float yaw;
    }

    public bool isReady;
    public Vector3 rotationDeg;

    void Start()
    {
        try
        {
            int reg = NP_RegisterWindowHandle(IntPtr.Zero);
            ushort ver;
            NP_QueryVersion(out ver);
            Debug.Log($"[TrackIR] Registered. Version: {ver}");
            isReady = (reg == 0);
        }
        catch (Exception e)
        {
            Debug.LogWarning("[TrackIR] Init failed: " + e.Message);
            isReady = false;
        }
    }

    void Update()
    {
        if (!isReady) return;

        try
        {
            var data = new TrackIRData();
            int ok = NP_GetData(ref data);
            if (ok == 0)
            {

                rotationDeg = new Vector3(data.pitch, data.yaw, data.roll);
            }
        }
        catch (Exception e)
        {

            Debug.LogWarning("[TrackIR] NP_GetData failed: " + e.Message);
            isReady = false;
        }
    }
}
