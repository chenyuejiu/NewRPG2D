﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ServerBuildData
{
    public int id;//房间ID
    public Vector2 buildingPoint;//房间位置
    public float Stock = 0;
    public bool levelUp = false;

    public ServerBuildData() { }
    public ServerBuildData(int id, Vector2 point, float Stock)
    {
        this.id = id;
        this.buildingPoint = point;
        this.Stock = Stock;
    }
}

public class ServerHallRoleData
{
    public HallRoleData data;
}

public class ServerRoleData
{
    public int sex;//性别
    public int[] Skin;//皮肤ID

}

[System.Serializable]
public class LocalBuildingData
{
    public int id;//房间ID
    public Vector2 buildingPoint;//房间位置
    public BuildingData buildingData;//房间ID
    public float Stock = 0;
    public bool ConstructionType = false;
    public HallRoleData[] roleData;

    public float AllRoleProduction()
    {
        float index = 0;
        for (int i = 0; i < roleData.Length; i++)
        {
            if (roleData[i] != null)
            {
                switch (buildingData.RoomName)
                {
                    case BuildRoomName.Gold:
                        index += roleData[i].GoldProduce;
                        break;
                    case BuildRoomName.Food:
                        index += roleData[i].FoodProduce;
                        break;
                    case BuildRoomName.Mana:
                        index += roleData[i].ManaProduce;
                        break;
                    case BuildRoomName.Wood:
                        index += roleData[i].WoodProduce;
                        break;
                    case BuildRoomName.Iron:
                        index += roleData[i].IronProduce;
                        break;
                    default:
                        break;
                }
            }
        }
        return index;
    }
    //public RoleAttribute BuildingRoleHelper()
    //{
    //    switch (buildingData.RoomName)
    //    {
    //        case BuildRoomName.Gold:
    //            return RoleAttribute.Gold;
    //        case BuildRoomName.Food:
    //            return RoleAttribute.Food;
    //        case BuildRoomName.Mana:
    //            return RoleAttribute.Mana;
    //        case BuildRoomName.Wood:
    //            return RoleAttribute.Wood;
    //        case BuildRoomName.Iron:
    //            return RoleAttribute.Iron;
    //        case BuildRoomName.Barracks:
    //            return RoleAttribute.DPS;
    //        default:
    //            break;
    //    }
    //    return RoleAttribute.Nothing;
    //}
    public int ScreenAllYeild(RoleAttribute type, bool isUp)
    {
        Debug.Log("角色产量筛选");
        int index = 0;
        float temp = 0;
        if (isUp)
        {
            temp = 0;
        }
        else
        {
            temp = 100;
        }
        //RoleAttribute type = BuildingRoleHelper();
        if (type == RoleAttribute.Nothing)
        {
            return roleData.Length - 1;
        }
        for (int i = 0; i < roleData.Length; i++)
        {
            if (isUp)
            {
                float temp_1 = ScreenProduceHelper(type, roleData[i]);
                if (temp < temp_1)
                {
                    temp = temp_1;
                    index = i;
                }
            }
            else
            {
                float temp_1 = ScreenProduceHelper(type, roleData[i]);
                if (temp > temp_1)
                {
                    temp = temp_1;
                    index = i;
                }

            }
        }
        return index;
    }
    public float ScreenProduceHelper(RoleAttribute type, HallRoleData data)
    {
        switch (type)
        {
            case RoleAttribute.Gold:
                return data.GoldProduce;
            case RoleAttribute.Food:
                return data.FoodProduce;
            case RoleAttribute.Mana:
                return data.ManaProduce;
            case RoleAttribute.ManaSpeed:
                return data.ManaSpeed;
            case RoleAttribute.Wood:
                return data.WoodProduce;
            case RoleAttribute.Iron:
                return data.IronProduce;
            case RoleAttribute.DPS:
                return data.DPS;
            case RoleAttribute.HP:
                return data.HP;
            default:
                break;
        }
        return 0;
    }
    public LocalBuildingData() { }
    public LocalBuildingData(int id, Vector2 point, BuildingData data, int maxRole)
    {
        this.id = id;
        this.buildingPoint = point;
        this.buildingData = data;
        roleData = new HallRoleData[maxRole];
    }
    public LocalBuildingData(int id, Vector2 point, BuildingData data, float Stock, HallRoleData[] roleData)
    {
        this.id = id;
        this.buildingPoint = point;
        this.buildingData = data;
        this.Stock = Stock;
        this.roleData = roleData;
    }
}

/// <summary>
/// 墙面信息
/// </summary>
public class BuildPoint
{
    public BuildingType pointType;//当前位置类型
    public Transform pointWall;//当前位置墙体引用
    public RoomMgr roomMgr;//当前位置房间信息
    public BuildTip tip;//当前位置提示框信息
}

/// <summary>
/// 空位信息
/// </summary>
[System.Serializable]
public class EmptyPoint
{
    public Vector2 startPoint;//起点位置
    public int endPoint;//结束位置
    public int emptyNumber;//空位数量
    public RoomMgr roomData;//链接的建筑信息

    public EmptyPoint()
    {
        //startPoint = new Vector2(6, 1);
        //endPoint = 16;
        //emptyNumber = 9;
    }

    public EmptyPoint(Vector2 startPoint, int endPoint, int emptyNumber, RoomMgr roomData)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.emptyNumber = emptyNumber;
        this.roomData = roomData;
    }
}

[System.Serializable]
public class EditMergeRoomData
{
    public LocalBuildingData room_1;
    public LocalBuildingData room_2;
    public LocalBuildingData room_3;
    public LocalBuildingData mergeRoom;

    public EditMergeRoomData() { }
}


