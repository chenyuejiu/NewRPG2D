﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicWorkShopInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Num;

    public Image slider;

    private List<UIWorkShopTypeGrid> roleGrids = new List<UIWorkShopTypeGrid>();

    protected override void Awake()
    {
        base.Awake();
        txt_Tip_2.text = LanguageDataMgr.instance.GetString("WorkShopTip");
    }
    protected override void UpdateInfo(RoomMgr roomMgr)
    {
        int num = ChickRoleNumber(roleGrids);
        txt_Tip_2.gameObject.SetActive(num == 0 ? false : true);

    }
}
