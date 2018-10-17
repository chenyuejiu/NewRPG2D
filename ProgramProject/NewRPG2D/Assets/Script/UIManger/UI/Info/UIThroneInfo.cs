﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIThroneInfo : UIRoomInfo
{
    private Dictionary<ThroneInfoType, List<BuildingData>> dic;

    public Text txt_Tip_1;
    public Text txt_Tip_2;

    public GameObject grid;
    public Transform gridPoint;

    private int level = 0;
    private List<UIThroneLevelUpGrid> grids = new List<UIThroneLevelUpGrid>();

    protected override void UpdateInfo(RoomMgr roomMgr)
    {
        txt_Tip_1.text = "升级解锁";
        txt_Tip_2.text = "升级国王大厅来解锁新的建筑和建筑等级";
        if (roomMgr.BuildingData.Level == level)
        {
            return;
        }
        dic = ChickPlayerInfo.instance.ThroneLeveUpRoomInfo(roomMgr.BuildingData);
        if (dic == null) return;
        ChickGrid();
        level = roomMgr.BuildingData.Level;
    }

    /// <summary>
    /// 检查可升级可建造的建筑信息
    /// </summary>
    private void ChickGrid()
    {
        int index = dic[ThroneInfoType.Build].Count + dic[ThroneInfoType.Upgraded].Count;
        if (grids.Count < index)
        {
            for (int i = 0; i < index; i++)
            {
                GameObject go = Instantiate(grid, gridPoint) as GameObject;
                UIThroneLevelUpGrid data = go.GetComponent<UIThroneLevelUpGrid>();
                grids.Add(data);
            }
        }
        int index_1 = 0;
        for (int i = 0; i < dic[ThroneInfoType.Build].Count; i++)
        {
            grids[index_1].gameObject.SetActive(true);
            grids[index_1].UpdateInfo(dic[ThroneInfoType.Build][i], ThroneInfoType.Build);
            index_1++;
        }
        for (int i = 0; i < dic[ThroneInfoType.Upgraded].Count; i++)
        {
            grids[index_1].gameObject.SetActive(true);
            grids[index_1].UpdateInfo(dic[ThroneInfoType.Upgraded][i], ThroneInfoType.Upgraded);
            index_1++;
        }
        for (int i = index_1; i < grids.Count; i++)
        {
            grids[i].gameObject.SetActive(false);
        }
    }
}
