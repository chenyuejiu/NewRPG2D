﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildFood : RoomMgr, IProduction
{
    //暂无角色先用默认产能代替
    public float Yield { get { return buildingData.Param1; } }
    public float Stock { get { return stock; } set { stock = value; } }

    public override void GetNumber(int number)
    {
        base.GetNumber(number);
    }

    public override void ProductionType()
    {
        base.ProductionType();
    }

    public override void RoomAwake()
    {
        if (roomFunc == false)
        {
            return;
        }
        LocalServer.instance.GetNumber(this);
    }

    public override void ThisRoomFunc()
    {


        #region 逻辑改到父类中运行
        //if (roomFunc == false)
        //{
        //    return;
        //}
        //bool isTrue = LocalServer.instance.SetNumber(this);

        //if (isTrue)
        //{
        //    //如果数量小于1 那么关闭提示框 关闭收获提示
        //    if (stock <= 1)
        //    {
        //        roomProp.SetActive(false);
        //        isHarvest = false;
        //    }
        //    //显示动画并刷新数字
        //    else
        //    {
        //        roomProp.SetActive(true);
        //        isHarvest = true;
        //    }
        //    HallEventManager.instance.SendEvent(HallEventDefineEnum.FoodSpace);
        //}
        //else
        //{
        //    //仓库已满
        //    Debug.Log("仓库已满");
        //}
        #endregion
    }

    #region 该逻辑改到父类中运行
    //public void GetNumber(int number)
    //{
    //    if (roomFunc == false)
    //    {
    //        return;
    //    }
    //    //显示可获取
    //    isHarvest = true;
    //    roomProp.SetActive(true);
    //} 
    #endregion
}
