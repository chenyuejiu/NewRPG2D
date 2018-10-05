﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIProdLevelUp : UILevelUp
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_Tip_5;

    public Text txt_Yield;
    public Text txt_YieldUp;
    public Text txt_Stock;
    public Text txt_Stock_2;
    public Text txt_StockUp;
    public Text txt_StockUp_2;
    public RectTransform Type_1;
    public RectTransform Type_2;
    public RectTransform Type_3;

    protected override void Init(RoomMgr data)
    {
        base.Init(data);
        UIReset();

        BuildingData d1 = data.currentBuildData.buildingData;
        BuildingData d2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(d1.NextLevelID);

        switch (data.BuildingData.RoomName)
        {
            case BuildRoomName.Gold:
                UpdateInfo_1(data);
                break;
            case BuildRoomName.GoldSpace:
                UpdateInfo_2(data);
                break;
            case BuildRoomName.Food:
                UpdateInfo_1(data);
                break;
            case BuildRoomName.FoodSpace:
                UpdateInfo_2(data);
                break;
            case BuildRoomName.Wood:
                UpdateInfo_1(data);
                break;
            case BuildRoomName.WoodSpace:
                UpdateInfo_2(data);
                break;
            case BuildRoomName.Mana:
                UpdateInfo_1(data);
                break;
            case BuildRoomName.ManaSpace:
                UpdateInfo_2(data);
                break;
            case BuildRoomName.Iron:
                UpdateInfo_1(data);
                break;
            case BuildRoomName.IronSpace:
                UpdateInfo_2(data);
                break;
            default:
                break;
        }
        switch (data.BuildingData.RoomType)
        {
            case RoomType.Nothing:
                break;
            case RoomType.Production:
                break;
            case RoomType.Training:
                UpdateInfo_2(data);
                break;
            case RoomType.Support:
                break;
            case RoomType.Max:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 界面1 仅用于生产类
    /// </summary>
    /// <param name="data"></param>
    private void UpdateInfo_1(RoomMgr data)
    {
        Type_1.anchoredPosition = Vector3.zero;

        txt_Tip_1.text = "升级增加";
        txt_Tip_2.text = "每小时产量";
        txt_Tip_3.text = "房间容量";
        txt_Tip_4.text = "升级增加产量和容量";

        PlayerData playerData = GetPlayerData.Instance.GetData();
        BuildingData b_Data_1;//当前房间信息
        BuildingData b_Data_2;//下一级房间信息
        b_Data_1 = data.BuildingData;
        txt_Yield.text = b_Data_1.Param1.ToString("#0");
        txt_Stock.text = b_Data_1.Param2.ToString("#0");
        if (b_Data_1.NextLevelID == 0)//如果满级了
        {
            txt_YieldUp.text = "+" + 0;
            txt_StockUp.text = "+" + 0;
            return;
        }
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(b_Data_1.NextLevelID);
        txt_YieldUp.text = "+" + (b_Data_2.Param1 - b_Data_1.Param1);
        txt_StockUp.text = "+" + (b_Data_2.Param2 - b_Data_1.Param2);

    }

    /// <summary>
    /// 界面2 通用性强 用于储存类 训练类 起居室等
    /// </summary>
    /// <param name="data"></param>
    private void UpdateInfo_2(RoomMgr data)
    {
        Type_2.anchoredPosition = Vector3.zero;

        txt_Tip_5.text = "容量";
        if (data.RoomName == BuildRoomName.Barracks)
        {
            txt_Tip_5.text = "可携带人数";
        }

        Type_3.anchoredPosition = Vector3.zero;

        PlayerData playerData = GetPlayerData.Instance.GetData();
        BuildingData b_Data_1;//当前房间信息
        BuildingData b_Data_2;//下一级房间信息
        b_Data_1 = data.BuildingData;
        txt_Stock_2.text = b_Data_1.Param2.ToString("#0");
        if (b_Data_1.NextLevelID == 0)//如果满级了
        {
            txt_StockUp_2.text = "+" + 0;
            return;
        }
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(b_Data_1.NextLevelID);
        txt_StockUp_2.text = "+" + (b_Data_2.Param2 - b_Data_1.Param2);
    }

    private void ChickTip_4(BuildingData data)
    {
        switch (data.RoomType)
        {
            case RoomType.Production:
                txt_Tip_4.text = "升级可增加";
                return;
            case RoomType.Training:
                Type_3.anchoredPosition = Vector3.zero;
                txt_Tip_4.text = "将城堡内的居民移动至该房间，提升居民的     等级。\n\n     能够影响战斗表现。";
                return;
            default:
                break;
        }
        switch (data.RoomName)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                break;
            case BuildRoomName.GoldSpace:
                break;
            case BuildRoomName.Food:
                break;
            case BuildRoomName.FoodSpace:
                break;
            case BuildRoomName.Mana:
                break;
            case BuildRoomName.ManaSpace:
                break;
            case BuildRoomName.Wood:
                break;
            case BuildRoomName.WoodSpace:
                break;
            case BuildRoomName.Iron:
                break;
            case BuildRoomName.IronSpace:
                break;
            case BuildRoomName.FighterRoom:
                break;
            case BuildRoomName.Kitchen:
                break;
            case BuildRoomName.Mint:
                break;
            case BuildRoomName.Laboratory:
                break;
            case BuildRoomName.Crafting:
                break;
            case BuildRoomName.Foundry:
                break;
            case BuildRoomName.LivingRoom:
                break;
            case BuildRoomName.TrophyRoom:
                break;
            case BuildRoomName.Hospital:
                break;
            case BuildRoomName.ClanHall:
                break;
            case BuildRoomName.MagicWorkShop:
                break;
            case BuildRoomName.MagicLab:
                break;
            case BuildRoomName.WeaponsWorkShop:
                break;
            case BuildRoomName.ArmorWorkShop:
                break;
            case BuildRoomName.GemWorkSpho:
                break;
            case BuildRoomName.Stairs:
                break;
            case BuildRoomName.ThroneRoom:
                break;
            case BuildRoomName.Barracks:
                break;
            case BuildRoomName.MaxRoom:
                break;
            default:
                break;
        }
    }

    protected override void ClosePage()
    {
        UIPanelManager.instance.ClosePage<UIProdLevelUp>();
    }

    private void UIReset()
    {
        Type_1.anchoredPosition = Vector3.up * 2000;
        Type_2.anchoredPosition = Vector3.up * 2000;
        Type_3.anchoredPosition = Vector3.up * 2000;

    }
}
