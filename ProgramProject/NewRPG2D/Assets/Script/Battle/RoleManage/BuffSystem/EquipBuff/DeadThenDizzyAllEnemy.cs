﻿/// <summary>
///当前攻击目标被击杀时，x秒内造成的伤害提高y%
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class DeadThenDizzyAllEnemy : TargetDeadTrigger
    {
        private float duration;


        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param1;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                DizzyAllEnemy(duration);
                return true;
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
