﻿using UnityEngine;
using Assets.Script.Battle;
using Spine.Unity;
using UnityEngine.Rendering;
using Transform = UnityEngine.Transform;

public class RoleRender : MonoBehaviour
{
    public SkeletonAnimation roleAnimation;
    [SerializeField] private SortingGroup currentSortingGroup;
    [SerializeField] private GameObject blueSelectObj, redSelectObj;
    public RoleBase CurrentRole { get; private set; }
    private Transform _trans;
    private Transform _renderTrans;
    private bool bCanContorl;

    // Use this for initialization
    private void Awake()
    {
        _trans = transform;
        EventManager.instance.AddListener<int>(EventDefineEnum.ClickMyRole, OnClickMyRole);
        EventManager.instance.AddListener<RoleBase>(EventDefineEnum.ClickEnemyRole, OnClickEnemyRole);
    }

    private void OnDestory()
    {
        EventManager.instance.RemoveListener<int>(EventDefineEnum.ClickMyRole, OnClickMyRole);
        EventManager.instance.RemoveListener<RoleBase>(EventDefineEnum.ClickEnemyRole, OnClickEnemyRole);
    }

    private void Update()
    {
        SetRendererSorintRender();
    }

    public void SetRoleBaseInfo(RoleBase roleBase)
    {
        CurrentRole = roleBase;
        SetIsControlState(roleBase.IsCanControl);
    }

    public void SetIsControlState(bool canControl)
    {
        bCanContorl = canControl;
    }

    public void Move(Vector3 movePostion)
    {
        CurrentRole.RoleMoveMoment.SetTargetPositionAndChangeActionState(movePostion);
    }

    public void ChangeTarget(RoleBase targetRole)
    {
        DebugHelper.LogFormat("target name == {0}, team id = {1}, currentRole = {2} team id = {3} ", targetRole.MonoRoleRender.name, targetRole.TeamId, name, CurrentRole.TeamId);
        if (targetRole.TeamId == CurrentRole.TeamId)
        {
            return;
        }

        CurrentRole.RoleSearchTarget.SetTarget(targetRole);
        DebugHelper.Log(" attack  " + targetRole.InstanceId);
    }

    private void SetRendererSorintRender()
    {
        currentSortingGroup.sortingOrder = (int)((6 - transform.position.y) * 1000);
    }


    private void OnClickMyRole(int instanceId)
    {
        if (CurrentRole.InstanceId == instanceId)
        {
            blueSelectObj.SetActive(true);
            redSelectObj.SetActive(false);
        }
        else
        {
            blueSelectObj.SetActive(false);
            redSelectObj.SetActive(false);
        }
    }

    private void OnClickEnemyRole(RoleBase roleInfo)
    {
        if (roleInfo == null)
        {
            blueSelectObj.SetActive(false);
            redSelectObj.SetActive(false);
        }
        else
        {
            if (CurrentRole.InstanceId == roleInfo.InstanceId)
            {
                blueSelectObj.SetActive(false);
                redSelectObj.SetActive(true);
            }
            else
            {
                blueSelectObj.SetActive(false);
                redSelectObj.SetActive(false);
            }
        }
    }


}


