﻿using System;

namespace R1Engine
{
    public class Unity_Object_GBACrashIsometric_MultiplayerCrown : Unity_Object_BaseGBACrashIsometric
    {
        public Unity_Object_GBACrashIsometric_MultiplayerCrown(GBACrash_Isometric_Position obj, Unity_ObjectManager_GBACrashIsometric objManager) : base(objManager)
        {
            Object = obj;
        }

        public override void UpdateAnimIndex() => ObjAnimIndex = 32;

        public GBACrash_Isometric_Position Object { get; }

        public override string DebugText => String.Empty;

        public override FixedPointInt XPos
        {
            get => Object.XPos;
            set => Object.XPos = value;
        }
        public override FixedPointInt YPos
        {
            get => Object.YPos;
            set => Object.YPos = value;
        }

        public override R1Serializable SerializableData => Object;

        public override string PrimaryName => $"Crown";
        public override string SecondaryName => null;
    }
}