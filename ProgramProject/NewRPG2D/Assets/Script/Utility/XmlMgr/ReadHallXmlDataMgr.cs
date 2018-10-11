﻿
//功能： 读取的Xml数据管理
//创建者: 胡海辉
//创建时间：


using System;
using UnityEngine;
using System.Xml;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Utility
{

    public class ReadHallXmlDataMgr
    {

        public static XmlData GetXmlData(XmlName name)
        {
            switch (name)
            {
                case XmlName.BuildingData:
                    return new BuildingData();
                case XmlName.TrainData:
                    return new TrainData();
                case XmlName.MagicData:
                    return new MagicData();
                case XmlName.ChildData:
                    return new ChildData();
                default: return new XmlData();
            }
        }
    }
}
