﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Battle.BattleData;
using Assets.Script.Battle.BattleData.ReadData;

public class UILessonEnemy : MonoBehaviour
{
    public Image Icon;

    public void UpdateInfo(int enemyID)
    {
        Debug.Log("怪物ID :" + enemyID);
        RolePropertyData roleData = RolePropertyCsvDataMgr.instance.GetDataByItemId<RolePropertyData>(enemyID);
        Icon.sprite = GetSpriteAtlas.insatnce.GetEnemyIcon(roleData.SpriteName);
    }
}
