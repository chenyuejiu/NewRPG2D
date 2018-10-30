﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using UnityEngine.SceneManagement;
using Assets.Script.Battle.BattleData;

public class LocalStartFight : TSingleton<LocalStartFight>
{
    public List<RoleDetailData> FightRoleData = new List<RoleDetailData>();
    public MapLevelData MapData;

    public void UpdateInfo(List<RoleDetailData> roleData, MapLevelData mapData)
    {
        FightRoleData = roleData;
        this.MapData = mapData;
        SceneManager.LoadScene("Scene_Test");
    }
}
