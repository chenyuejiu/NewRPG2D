﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenRoleGrid : MonoBehaviour
{
    public Button btn_Photo;
    public Image image_TypeIcon;
    public Text txt_Point;
    public Text txt_Name;
    public Text txt_Level;
    public Text txt_NeedDiamonds;
    public Text txt_Tip;
    public Text txt_Time;
    public GameObject TrainType;
    public HallRoleData currentRole;

    public void UpdateInfo(HallRoleData data, RoleAttribute needAtr)
    {
        if (data != currentRole)
        {
            HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
        }
        TrainType.SetActive(false);
        txt_Name.text = data.Name;
        txt_Level.text = data.GetAtrProduce(needAtr).ToString();
        if (data.currentRoom != null)
        {
            txt_Point.text = data.currentRoom.RoomName.ToString();
            image_TypeIcon.sprite = GetSpriteAtlas.insatnce.ChickRoomIcon(data.currentRoom.RoomName);
            if (data.currentRoom.BuildingData.RoomType == RoomType.Training)
            {
                //如果是训练类的房间
                TrainType.SetActive(true);
                HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
            }
        }
        else
        {
            txt_Point.text = "漫游";
        }
    }

    private void OnDisable()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
    }

    public void ChickTrainTime(int RoleIndex)
    {
        RoleTrainHelper role = HallRoleMgr.instance.GetTrainRole(RoleIndex);
        if (role.role != currentRole)
        {
            return;
        }
        string time = SystemTime.instance.TimeNormalized(role.time);
        txt_Time.text = time;
    }
}
