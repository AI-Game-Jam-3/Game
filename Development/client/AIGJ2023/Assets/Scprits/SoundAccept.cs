using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.IO;

public class SoundAccept : MonoBehaviour
{
    private static SoundAccept instance;
    public static SoundAccept Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundAccept>();
            }
            return instance;
        }
    }

    //[HideInInspector] public bool isFire = false;
    //[HideInInspector] public bool isShout = false;
    //[HideInInspector] public bool isBreak = false;
    public event Action OnShout;
    public event Action OnFire;
    public event Action OnBreak;
    
    //public OtherScript otherScript;
    [HideInInspector]public string mes;

    // Thread
    Thread receiveThread;
    TcpClient client;
    TcpListener listener;
    int port = 5066;
    Process process;
    void Start()
    {
        UnityEngine.Debug.Log("InitTCP()");
        StartCoroutine(InitTCP());
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator InitTCP()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        UnityEngine.Debug.Log("Thread.Start");
        yield return new WaitForSeconds(5f);

        // 下面的代码延迟五秒执行
        //下面调用python项目打包成的exe文件
        //try
        //{
        //    UnityEngine.Debug.Log("Process.Start");
        //    // 创建一个 Process 实例
        //    process = new Process();

        //    string assetsPath = Application.dataPath;
        //    string exeFileName = "VoiceFinalVersion.exe";
        //    string exePath = assetsPath+"/"+exeFileName;
        //    UnityEngine.Debug.Log(exePath);

        //    // 配置 ProcessStartInfo
        //    ProcessStartInfo startInfo = new ProcessStartInfo(exePath);
        //    startInfo.UseShellExecute = false; // 禁用外壳启动

        //    // 将 ProcessStartInfo 分配给 Process 实例
        //    process.StartInfo = startInfo;


        //    // 启动进程
        //    process.Start();
        //    UnityEngine.Debug.Log("Process.Start End");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Error: " + ex.Message);
        //}

    }
    // 在 Unity 停止执行时调用此协程来停止进程
    private IEnumerator StopProcessCoroutine()
    {
        yield return new WaitForEndOfFrame(); // 等待当前帧结束，确保进程已经启动

        if (process != null && !process.HasExited)
        {
            process.Kill();
            process.Dispose();
        }
        if (listener != null)
        {
            listener.Stop();

        }
    }
    private void OnApplicationQuit()
    {
        StartCoroutine(StopProcessCoroutine());
    }
    private void ReceiveData()
    {
        try
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            UnityEngine.Debug.Log("listener.Start");
            Byte[] bytes = new Byte[1024];

            while (true)
            {
                using (client = listener.AcceptTcpClient())
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            startInvoke(clientMessage);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    void startInvoke(string str)
    {
        switch (str)
        {
            case "hello":
                // 执行声波点亮
                //isShout = true;
                OnShout?.Invoke();
                break;
            case "break":
                // 执行打破墙壁
                //isBreak = true;
                OnBreak?.Invoke();
                break;
            case "fire":
                // 执行点亮火把
                //isFire = true;
                OnFire?.Invoke();
                break;
            default:
                // 如果上述条件都不满足，则执行默认操作
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
