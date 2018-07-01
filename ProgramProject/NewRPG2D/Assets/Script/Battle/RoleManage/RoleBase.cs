﻿using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using Assets.Script.Utility;
using DragonBones;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Script.Battle
{
    public struct RoleInfo
    {
        public int InstanceId;
        public int TeamId;
        public int RoleId;
        public RoleTypeEnum RoleType;
    }

    public abstract class RoleBase
    {
        public int InstanceId { get; private set; }
        public int RoleId { get; private set; }
        public UnityArmatureComponent RoleAnimator { get; private set; }
        public int TeamId { get; private set; }
        public RoleTypeEnum RoleType { get; private set; }
        public Transform RoleTransform { get; private set; }
        public int AnimationId { get; private set; }
        public SkillSlotTypeEnum CurrentSlot { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsCanControl { get; private set; }

        public SearchTarget RoleSearchTarget { get; private set; }
        public RoleRender MonoRoleRender { get; private set; }
        public RoleAnimationSys RoleAnimation { get; private set; }
        public MoveMoment RoleMoveMoment { get; private set; }
        public ValueProperty RolePropertyValue { get; private set; }
        public DamageMoment RoleDamageMoment { get; private set; }
        public SkillCompoment RoleSkill { get; private set; }
        public FsmStateMachine<RoleBase> RoleActionMachine;
        public Dictionary<int, FsmState<RoleBase>> RoleActionStateDic; //存放动作状态

        protected RoleData CurrentRoleData;
        private int[] AttackSkillIdArray;

        public void SetRoleInfo(RoleInfo info, RoleRender roleRender)
        {
            MonoRoleRender = roleRender;
            InitComponent();
            InitFSM();
            InitData();
            SetRoleInfo(info);
            InitSkill();
        }

        public virtual void SetRoleInfo(RoleInfo info)
        {
            InstanceId = info.InstanceId;
            TeamId = info.TeamId;
            RoleType = info.RoleType;
            RoleId = info.RoleId;
            CurrentRoleData = RoleDataMgr.instance.GetXmlDataByItemId<RoleData>(RoleId);
        }

        public void InitComponent()
        {
            RoleAnimator = MonoRoleRender.roleAnimation;
            RoleTransform = MonoRoleRender.transform;
        }

        public void InitData()
        {
            RoleAnimation = new RoleAnimationSys();
            RoleAnimation.SetCurrentRole(this);
            RoleMoveMoment = new MoveMoment();
            RoleMoveMoment.SetCurrentRole(this);
            RolePropertyValue = new ValueProperty();
            RolePropertyValue.SetCurrentRole(this);
            //RoleProperty.InitRoleValue(100f, 100f);
            RolePropertyValue.SetMoveSeed(10f);
            RoleDamageMoment = new DamageMoment();
            RoleDamageMoment.SetCurrentRole(this);
            RoleSearchTarget = new SearchTarget();
            RoleSearchTarget.SetCurrentRole(this);
            RoleSkill = new SkillCompoment();
            RoleSkill.SetCurrentRole(this);

            AttackSkillIdArray = new int[3];
        }

        public virtual void InitFSM()
        {
            RoleActionStateDic = new Dictionary<int, FsmState<RoleBase>>(10);
            RoleActionMachine = new FsmStateMachine<RoleBase>(this);
        }

        private void InitSkill()
        {
            RoleSkill.InitSkill(SkillSlotTypeEnum.NormalAttack, CurrentRoleData.NormalAttackId);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill1, CurrentRoleData.SkillDataId01);
            RoleSkill.InitSkill(SkillSlotTypeEnum.Skill2, CurrentRoleData.SkillDataId02);
            CurrentSlot = SkillSlotTypeEnum.NormalAttack;
        }

        public void SetRoleActionState(RoleActionEnum state)
        {
            if (RoleActionStateDic.ContainsKey((int)state))
            {
                RoleActionMachine.ChangeState(RoleActionStateDic[(int)state]);
            }
        }

        public void SetAnimationId(int animationId)
        {
            AnimationId = animationId;
            RoleAnimation.ChangeAniamtionNameById(AnimationId);
        }

        public void SetAttackSkillId(int[] attackSkillId)
        {
            for (int i = 0; i < attackSkillId.Length; i++)
            {
                AttackSkillIdArray[i] = attackSkillId[i];
            }
        }

        public int SetCurrentAttackSkillId(int attackIndex)
        {
            CurrentAttackSkillId = AttackSkillIdArray[attackIndex];
            if (attackIndex++ >= AttackSkillIdArray.Length)
            {
                attackIndex = 0;
            }
            return attackIndex;
        }

        public virtual void UpdateLogic(float deltaTime)
        {
            if (RoleMoveMoment != null) { RoleMoveMoment.Update(deltaTime); }
            if (RoleActionMachine != null) RoleActionMachine.Update(deltaTime);
        }

        public abstract void FixedUpdateLogic(float deltaTime);
        public abstract void Dispose();
    }
}