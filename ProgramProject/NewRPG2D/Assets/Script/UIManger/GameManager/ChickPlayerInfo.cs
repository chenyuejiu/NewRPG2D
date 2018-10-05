﻿/*
 * 这是一个本地玩家参数单例
 * 用于整合玩家全部资源
 * 整合建筑类型和数量
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class ChickPlayerInfo : TSingleton<ChickPlayerInfo>
{
    public Dictionary<BuildRoomName, LocalBuildingData[]> dic = new Dictionary<BuildRoomName, LocalBuildingData[]>();
    private Dictionary<int, RoomMgr> buildNumber = new Dictionary<int, RoomMgr>();//房间序号 用于储存施工中的房间
    private List<LocalBuildingData> AllBuilding = new List<LocalBuildingData>();//储存全部已经建造的房间
    private Dictionary<int, LocalBuildingData> production = new Dictionary<int, LocalBuildingData>();//产出房间
    private List<LocalBuildingData> storage;
    public int buildingIdIndex = 0;

    private int EventKey = 0;

    /// <summary>
    /// 将建筑数量和信息格式化
    /// </summary>
    public void ChickBuilding()
    {
        dic.Clear();
        var index = BuildingDataMgr.instance.GetBuilding();
        foreach (var item in index)
        {
            if (item.Key == BuildRoomName.Stairs)
            {
                dic.Add(item.Key, new LocalBuildingData[30]);
                break;
            }
            dic.Add(item.Key, new LocalBuildingData[item.Value.Length]);
        }
        Debug.Log(dic);
    }

    /// <summary>
    /// 获取服务器上的建筑数据后 将其转为本地信息
    /// </summary>
    /// <param name="s_BuildData"></param>
    public void ChickBuildDic(List<LocalBuildingData> s_BuildData)
    {
        for (int i = 0; i < s_BuildData.Count; i++)
        {
            for (int j = 0; j < dic[s_BuildData[i].buildingData.RoomName].Length; j++)
            {
                if (dic[s_BuildData[i].buildingData.RoomName][j] == null)
                {
                    dic[s_BuildData[i].buildingData.RoomName][j] = s_BuildData[i];
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 有新的建筑 给字典添加信息
    /// </summary>
    public void ChickBuildDicAdd(LocalBuildingData data)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i] == null)
            {
                Debug.Log("有空位 添加");
                dic[data.buildingData.RoomName][i] = data;
                HallEventManager.instance.SendEvent<RoomType>(HallEventDefineEnum.ChickBuild, data.buildingData.RoomType);
                if (ChickStorage(data))
                {
                    ThisStorage(data);
                }
                return;
            }
        }
        Debug.LogError("房间数量超限");
    }

    /// <summary>
    /// 建筑升级或者改变 改变字典中的信息
    /// </summary>
    /// <param name="data">原数据</param>
    /// <param name="changeData">改变后的数据</param>
    public LocalBuildingData ChickBuildDicChange(LocalBuildingData data, BuildingData changeData)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i].id == data.id)
            {
                dic[data.buildingData.RoomName][i].buildingData = changeData;
                return dic[data.buildingData.RoomName][i];
            }
        }
        Debug.LogError("没有找到匹配的 信息错误");
        return null;
    }

    /// <summary>
    /// 删除建筑信息
    /// </summary>
    public void ChickBuildMerge(ServerBuildData data)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i] != null
                && dic[data.buildingData.RoomName][i].buildingData == data.buildingData
                && dic[data.buildingData.RoomName][i].buildingPoint == data.buildingPoint)
            {
                Debug.Log("找到了 删除");
                dic[data.buildingData.RoomName][i] = null;
                return;
            }
        }
        Debug.LogError("没有找到要删除的建筑 :" + data.buildingData.RoomName);
    }

    /// <summary>
    /// 检查是否是生产类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool ChickProduction(LocalBuildingData data)
    {
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.Gold:
                return true;
            case BuildRoomName.Food:
                return true;
            case BuildRoomName.Mana:
                return true;
            case BuildRoomName.Wood:
                return true;
            case BuildRoomName.Iron:
                return true;
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// 检查是否是储存类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool ChickStorage(LocalBuildingData data)
    {
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.GoldSpace:
                return true;
            case BuildRoomName.FoodSpace:
                return true;

            case BuildRoomName.ManaSpace:
                return true;

            case BuildRoomName.WoodSpace:
                return true;

            case BuildRoomName.IronSpace:
                return true;
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// 给主菜单建造房间提供当前房间数量
    /// </summary>
    /// <returns></returns>
    public int[] GetBuildiDicInfo(BuildingData data)
    {
        int[] index = new int[2];
        PlayerData player = GetPlayerData.Instance.GetData();
        if (data.RoomName != BuildRoomName.Stairs)
        {
            for (int i = 0; i < data.UnlockLevel.Length; i++)
            {
                if (data.UnlockLevel[i] <= player.MainHallLevel)
                {
                    index[1]++;//获取当前等级的可建造数
                }
            }
        }
        else
        {
            index[1] = 4 + player.MainHallLevel;
        }
        for (int i = 0; i < dic[data.RoomName].Length; i++)
        {
            if (dic[data.RoomName][i] != null)
            {
                index[0]++;//获取该类建筑已建造数量
                if (dic[data.RoomName][i].buildingData.RoomType == RoomType.Production)
                {
                    switch (dic[data.RoomName][i].buildingData.RoomSize)
                    {
                        case 6: index[0]++; break;
                        case 9: index[0] += 2; break;
                        default:
                            break;
                    }
                }
            }
        }
        return index;
    }

    /// <summary>
    /// 获取某种建筑的全部产量
    /// </summary>
    /// <returns></returns>
    public int GetAllYield(BuildRoomName name)
    {
        int index = 0;
        for (int i = 0; i < dic[name].Length; i++)
        {
            if (dic[name][i] != null)
            {
                //这里应该计算房间内人物节能加房间默认产量
                index += (int)dic[name][i].buildingData.Param1;
            }
        }
        return index;
    }

    /// <summary>
    /// 获取某类资源的总值
    /// </summary>
    /// <returns></returns>
    public int GetAllStock(BuildRoomName name)
    {
        int index = 0;
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (name)
        {
            case BuildRoomName.Gold:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    index = player.Gold;
                    return index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.Food:
                if (dic[BuildRoomName.FoodSpace][0] == null)
                {
                    index = player.Food;
                    return index;
                }
                index = (int)dic[BuildRoomName.FoodSpace][0].Stock + player.Food;
                break;
            case BuildRoomName.Wood:
                if (dic[BuildRoomName.WoodSpace][0] == null)
                {
                    index = player.Wood;
                }
                index = (int)dic[BuildRoomName.WoodSpace][0].Stock + player.Wood;
                break;
            case BuildRoomName.Mana:
                if (dic[BuildRoomName.ManaSpace][0] == null)
                {
                    index = player.Mana;
                    return index;
                }
                index = (int)dic[BuildRoomName.ManaSpace][0].Stock + player.Mana;
                break;
            case BuildRoomName.Iron:
                if (dic[BuildRoomName.IronSpace][0] == null)
                {
                    index = player.Iron;
                    return index;
                }
                index = (int)dic[BuildRoomName.IronSpace][0].Stock + player.Iron;
                break;
            default:
                break;
        }
        return index;
    }

    /// <summary>
    /// 使用某类资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void UseAllStock(BuildRoomName name, int index)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (name)
        {
            case BuildRoomName.Gold:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.GoldSpace:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.Food:
                if (dic[BuildRoomName.FoodSpace][0] == null)
                {
                    player.Food -= index;
                }
                if (dic[BuildRoomName.FoodSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.FoodSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.FoodSpace][0].Stock;
                    dic[BuildRoomName.FoodSpace][0].Stock = 0;
                    player.Food -= index;
                }
                index = (int)dic[BuildRoomName.FoodSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.FoodSpace:

                break;
            case BuildRoomName.Mana:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.ManaSpace:
                break;
            case BuildRoomName.Wood:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.WoodSpace:
                break;
            case BuildRoomName.Iron:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.IronSpace:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取服务器上数据转成本地数据
    /// </summary>
    /// <param name="allBuidling"></param>
    public void SetAllBuilding(List<ServerBuildData> allBuidling)
    {
        for (int i = 0; i < allBuidling.Count; i++)
        {
            int index = MainCastle.instance.AddBuilding(allBuidling[i]);
            LocalBuildingData data = new LocalBuildingData(index, allBuidling[i].buildingPoint, allBuidling[i].buildingData);
            AllBuilding.Add(data);
        }
    }



    /// <summary>
    /// 新建建筑 添加建筑
    /// </summary>
    /// <param name="data"></param>
    public void AddBuilding(LocalBuildingData data)
    {
        AllBuilding.Add(data);
        ChickBuildDicAdd(data);
    }

    /// <summary>
    /// 删除建筑
    /// </summary>
    /// <param name="data"></param>
    public void RemoveBuilding(LocalBuildingData data)
    {
        if (ChickProduction(data))
        {
            ClostProduction(data);
        }
        bool isTrue = AllBuilding.Remove(data);
        if (isTrue == false)
        {
            Debug.LogError("没找到要删除的建筑 :" + data.buildingData.RoomName);
        }
    }

    /// <summary>
    /// 房间合并
    /// </summary>
    /// <param name="data_1">合并一号</param>
    /// <param name="data_2">合并二号</param>
    /// <param name="data_3">合并结果</param>
    public void MergeRoom(LocalBuildingData data_1, LocalBuildingData data_2, LocalBuildingData data_3)
    {
        RemoveBuilding(data_1);
        RemoveBuilding(data_2);
        AddBuilding(data_3);
        Debug.Log("合并后房间总数据" + AllBuilding.Count);
    }

    /// <summary>
    /// 建造模式数据保存
    /// </summary>
    /// <param name="datas"></param>
    public void SaveEditModInfo(List<LocalBuildingData> datas)
    {

    }

    /// <summary>
    /// 计时器
    /// </summary>
    /// <param name="data"></param>
    /// <param name="time"></param>
    public int Timer(RoomMgr data, int time)
    {
        int index = CTimerManager.instance.AddListener(1f, time, ChickTime);
        buildNumber.Add(index, data);
        LocalServer.instance.Timer(data, time);
        return index;
    }

    /// <summary>
    /// 计时器返回的信息 本地返回信息只是用于显示计时器信息
    /// </summary>
    /// <param name="key"></param>
    public void ChickTime(int key)
    {
        buildNumber[key].TimerCallBack();
    }

    /// <summary>
    /// 删除这个计时事件
    /// </summary>
    /// <param name="sequenceTime"></param>
    public void RemoveThisTime(int sequenceTime)
    {
        CTimerManager.instance.RemoveLister(sequenceTime);
    }

    /// <summary>
    /// 新建生产房间 添加生产事件
    /// </summary>
    /// <param name="data"></param>
    public void ThisProduction(LocalBuildingData data)
    {
        int index = CTimerManager.instance.AddListener(1f, -1, ChickProduction);
        production.Add(index, data);
    }

    /// <summary>
    /// 事件 CallBack
    /// </summary>
    /// <param name="key"></param>
    public void ChickProduction(int key)
    {
        float number = production[key].buildingData.Param1 / 60 / 60;
        production[key].Stock += number * 30f;
        if (production[key].Stock > production[key].buildingData.Param2 * 0.005f)
        {
            MainCastle.instance.FindRoom(production[key].id);
        }
        if (EventKey == key)
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.ChickStock);
        }
    }

    /// <summary>
    /// 删除这个事件
    /// </summary>
    /// <param name="data"></param>
    public void ClostProduction(LocalBuildingData data)
    {
        foreach (var item in production)
        {
            if (item.Value == data)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                production.Remove(item.Key);
                return;
            }
        }
        Debug.LogError("删除产出事件时没有找到对象");
    }

    /// <summary>
    /// 获取该生产类房间资源
    /// </summary>
    /// <param name="data"></param>
    public void GetProductionStock(LocalBuildingData data)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        LocalBuildingData space = null;
        int playerIndex = 0;
        int playerSpace = 0;
        float allStock = 0;
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.Gold:
                space = dic[BuildRoomName.GoldSpace][0];
                playerIndex = player.Gold;
                playerSpace = player.GoldSpace;
                allStock = data.Stock;
                player.Gold += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Gold);
                break;
            case BuildRoomName.Food:
                space = dic[BuildRoomName.FoodSpace][0];
                playerIndex = player.Food;
                playerSpace = player.FoodSpace;
                allStock = data.Stock;
                player.Food += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Food);
                break;
            case BuildRoomName.Mana:
                space = dic[BuildRoomName.ManaSpace][0];
                playerIndex = player.Mana;
                playerSpace = player.ManaSpace;
                allStock = data.Stock;
                player.Mana += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Mana);
                break;
            case BuildRoomName.Wood:
                space = dic[BuildRoomName.WoodSpace][0];
                playerIndex = player.Wood;
                playerSpace = player.WoodSpace;
                allStock = data.Stock;
                player.Wood += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Wood);
                break;
            case BuildRoomName.Iron:
                space = dic[BuildRoomName.IronSpace][0];
                playerIndex = player.Iron;
                playerSpace = player.IronSpace;
                allStock = data.Stock;
                player.Iron += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Iron);
                break;
            default:
                break;
        }
    }
    private int GetProductionStockHpr(LocalBuildingData data, LocalBuildingData space, int playerIndex, int playerSpace)
    {
        float allStock = data.Stock;
        if (space != null)
        {
            float index = (space.buildingData.Param2 - space.Stock);//剩余空间
            if (index < 0)
            {
                Debug.LogError("库存出错");
                return 0;
            }
            if (index - allStock > 0)//如果仓库容量足够
            {
                space.Stock += (int)data.Stock;
                data.Stock = 0;
                return 0;
            }
            else
            {
                data.Stock = ((int)allStock - index);
                allStock = data.Stock;
            }
        }
        //如果没有仓库或者仓库容量不足
        int temp = playerSpace - playerIndex;
        if (temp - allStock > 0)
        {
            //playerIndex += (int)data.Stock;
            int number = (int)data.Stock;
            data.Stock = 0;
            return number;
        }
        else
        {
            data.Stock = ((int)allStock - temp);
            //playerIndex += temp;
            return temp;
        }
    }

    /// <summary>
    /// 新建仓库 检查库存
    /// </summary>
    /// <param name="data"></param>
    public void ThisStorage(LocalBuildingData data)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.GoldSpace:
                player.Gold = ChickStock(data, player.Gold);
                break;
            case BuildRoomName.FoodSpace:
                player.Food = ChickStock(data, player.Food);
                break;
            case BuildRoomName.ManaSpace:
                player.Mana = ChickStock(data, player.Mana);
                break;
            case BuildRoomName.WoodSpace:
                player.Wood = ChickStock(data, player.Wood);
                break;
            case BuildRoomName.IronSpace:
                player.Iron = ChickStock(data, player.Iron);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 检查房间库存
    /// </summary>
    /// <param name="data">仓库</param>
    /// <param name="number">转入数值</param>
    /// <returns></returns>
    public int ChickStock(LocalBuildingData data, int number)
    {
        data.Stock += number;
        float index = data.Stock - data.buildingData.Param2;
        if (index > 0)
        {
            data.Stock = data.buildingData.Param2;
            return (int)index;
        }
        return 0;
    }

    /// <summary>
    /// 获取某建筑的库存值
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public int ThisRoomStock(RoomMgr room)
    {
        AllBuilding.IndexOf(room.currentBuildData);
        return 0;
    }

    /// <summary>
    /// 用于确认是否有该类型仓库
    /// </summary>
    public bool ThisRoomStock()
    {
        return true;
    }

    /// <summary>
    /// 获取房间事件
    /// </summary>
    /// <param name="data"></param>
    public void GetRoomEvent(LocalBuildingData data)
    {
        foreach (var item in production)
        {
            if (item.Value == data)
            {
                EventKey = item.Key;
                return;
            }
        }
        Debug.LogError("没有找到需要监听的对象");
    }

    /// <summary>
    /// 删除房间事件
    /// </summary>
    public void RemoveRoomEvent()
    {
        EventKey = 0;
    }

    /// <summary>
    /// 给予所有已建造房间
    /// </summary>
    /// <returns></returns>
    public List<LocalBuildingData> GetAllBuilding()
    {
        return AllBuilding;
    }

    /// <summary>
    /// 用于国王大厅升级
    /// </summary>
    /// <param name="data">升级后的信息</param>
    public Dictionary<ThroneInfoType, List<BuildingData>> ThroneLeveUpRoomInfo(BuildingData data)
    {
        List<BuildingData> allBuiliding = BuildingDataMgr.instance.AllRoomData();
        int id = data.NextLevelID;
        BuildingData newData = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(id);
        Dictionary<ThroneInfoType, List<BuildingData>> temp = new Dictionary<ThroneInfoType, List<BuildingData>>();
        temp.Add(ThroneInfoType.Build, new List<BuildingData>());
        temp.Add(ThroneInfoType.Upgraded, new List<BuildingData>());

        for (int i = 0; i < allBuiliding.Count; i++)
        {
            if (allBuiliding[i].RoomName == BuildRoomName.ThroneRoom)
            {
                continue;
            }
            //如果这个房间需要登记等于当前等级，且无法拆分 避免可拆分的房间重复出现
            if (allBuiliding[i].NeedLevel == newData.Level
                && allBuiliding[i].SplitID == 0)
            {
                temp[ThroneInfoType.Upgraded].Add(allBuiliding[i]);
            }
            if (allBuiliding[i].UnlockLevel == null)
            {
                continue;
            }
            //可建造
            for (int j = 0; j < allBuiliding[i].UnlockLevel.Length; j++)
            {
                if (allBuiliding[i].UnlockLevel[j] == newData.Level)
                {
                    temp[ThroneInfoType.Build].Add(allBuiliding[i]);
                }
            }
        }
        return temp;
    }
}
