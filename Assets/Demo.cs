using KcpProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zero;

public class Demo : MonoBehaviour
{
    public InputField input;
    public Text textPackageInfo;
    public Text textUnpackageInfo;
    public Toggle toggleAutoUpdateEnable;
    public Toggle toggleReceive;

    KCPHelper _a = new KCPHelper();
    KCPHelper _b = new KCPHelper();

    //byte[] _netPack;

    private void Start()
    {
        Application.targetFrameRate = 200;

        _a.onToSend += A2B;
        _a.onReceived += OnReceivedBytesA;

        _b.onToSend += B2A;
        _b.onReceived += OnReceivedBytesB;

        Debug.Log($"最大传输单元:{_a.MSS}");
        input.text = _a.MSS.ToString();
    }

    private void A2B(byte[] bytes)
    {
        if (toggleReceive.isOn)
        {
            _b.KcpInput(bytes);
        }
    }

    private void OnReceivedBytesA(byte[] bytes)
    {
        Debug.Log($"A收到业务数据 size:{bytes.Length}");
    }

    private void B2A(byte[] bytes)
    {
        _a.KcpInput(bytes);
    }

    private void OnReceivedBytesB(byte[] bytes)
    {
        //Debug.Log($"B收到业务数据 size:{bytes.Length}");
    }

    public void Package()
    {
        //Debug.Log("封包");

        string content = input.text;
        var bytes = Encoding.UTF8.GetBytes(content);       
        bytes = new byte[int.Parse(content)];
        for(var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = 7;
        }
        
        _a.Send(bytes);
    }


    public void ManualUpdate()
    {
        _a.Update();
        _b.Update();
    }

    public void Update()
    {
        if (toggleAutoUpdateEnable.isOn)
        {
            ManualUpdate();
        }
    }
}
