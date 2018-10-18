﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildLivingRoom : RoomMgr
{
    public override void AddRole(HallRole role)
    {
        if (currentBuildData.roleData == null)
        {
            Debug.LogError("房间角色空间出错");
        }
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] != null
                && currentBuildData.roleData[i].sexType != role.RoleData.sexType)
            {
                if (i % 2 == 0 && currentBuildData.roleData[i + 1] == null)
                {
                    currentBuildData.roleData[i + 1] = role.RoleData;
                    Vector3 point = new Vector3(transform.position.x + (1.2f * (i + 2)), transform.position.y + 0.3f, role.transform.position.z);
                    role.transform.position = point;
                    role.ChangeType(RoomName);
                    if (role.RoleData.currentRoom != null)
                    {
                        role.RoleData.currentRoom.RemoveRole(role);
                    }
                    role.RoleData.currentRoom = this;
                    HallRoleMgr.instance.LoveStart(currentBuildData.roleData[i], role.RoleData);
                    return;
                }
                else if (i % 2 != 0 && currentBuildData.roleData[i - 1] == null)
                {
                    currentBuildData.roleData[i - 1] = role.RoleData;
                    Vector3 point = new Vector3(transform.position.x + (1.2f * (i)), transform.position.y + 0.3f, role.transform.position.z);
                    role.transform.position = point;
                    role.ChangeType(RoomName);
                    if (role.RoleData.currentRoom != null)
                    {
                        role.RoleData.currentRoom.RemoveRole(role);
                    }
                    role.RoleData.currentRoom = this;
                    HallRoleMgr.instance.LoveStart(currentBuildData.roleData[i], role.RoleData);
                    return;
                }
            }
        }
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] == null)
            {
                currentBuildData.roleData[i] = role.RoleData;
                Vector3 point = new Vector3(transform.position.x + (1.2f * (i + 1)), transform.position.y + 0.3f, role.transform.position.z);
                role.transform.position = point;
                role.ChangeType(RoomName);
                if (role.RoleData.currentRoom != null)
                {
                    role.RoleData.currentRoom.RemoveRole(role);
                }
                role.RoleData.currentRoom = this;
                role.RoleData.LoveType = RoleLoveType.WaitFor;
                return;
            }
        }

        //这边筛选出属性较低的更换位置
        int index = currentBuildData.ScreenAllYeild(NeedAttribute, false);
        HallRole oldRole = HallRoleMgr.instance.GetRole(currentBuildData.roleData[index]);
        if (role.RoleData.currentRoom != null)
        {
            Debug.Log("切换房间 调换角色");
            HallRoleMgr.instance.RoleChangeRoom(role, oldRole);
        }
        else
        {
            Debug.Log("当前角色原房间为空 进入漫游状态");
            role.RoleData.currentRoom.RemoveRole(role);
            HallRoleData data = currentBuildData.roleData[index];
            HallRole roleTemp = HallRoleMgr.instance.GetRole(data);
            roleTemp.ChangeType(BuildRoomName.Nothing);
            currentBuildData.roleData[index] = role.RoleData;
        }
    }

    public void ThisRoomFunc(HallRoleData data_1, HallRoleData data_2)
    {

    }

}
