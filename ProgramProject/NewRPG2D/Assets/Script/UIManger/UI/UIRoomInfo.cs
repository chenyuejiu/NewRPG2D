﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public abstract class UIRoomInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_Level;
    public GameObject roleGrid;
    public Transform roleTrans;

    public Button btn_Close;
    protected RoomMgr roomData;

    private void Awake()
    {
        btn_Close.onClick.AddListener(ChickClose);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
        UpdateInfo(roomData);
        UpdateName(roomData);
    }

    /// <summary>
    /// 刷新房间名称和等级
    /// </summary>
    /// <param name="data"></param>
    protected virtual void UpdateName(RoomMgr data)
    {
        txt_Name.text = data.BuildingData.RoomName.ToString();
        txt_Level.text = data.BuildingData.Level.ToString();
    }

    /// <summary>
    /// 检查角色卡数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="roleGrids"></param>
    protected virtual void ChickRoleNumber<T>(List<T> roleGrids)
    {
        int index = roomData.BuildingData.RoomRole - roleGrids.Count;
        if (index > 0) //证明已有角色卡数量不足
        {
            for (int i = 0; i < index; i++)
            {
                GameObject go = Instantiate(roleGrid, roleTrans) as GameObject;
                T grid = go.GetComponent<T>();
                roleGrids.Add(grid);
            }
        }
    }

    protected abstract void UpdateInfo(RoomMgr roomMgr);
    protected virtual void ChickClose()
    {
        System.Type type = GetType();
        UIPanelManager.instance.ClosePage(type);
    }
}
