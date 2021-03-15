﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace R1Engine {
    public class Unity_Object_GameloftRK : Unity_Object_3D {
        public Unity_Object_GameloftRK(Unity_ObjectManager_GameloftRK objManager, Gameloft_RK_Level.TrackObject obj, Gameloft_RK_Level.ObjectType type) {
            ObjManager = objManager;
            Object = obj;
            ObjType = type;
            PuppetIndex = type.PuppetIndex;
            AnimIndex = type.AnimationIndex;
            PaletteIndex = type.PaletteIndex;
        }

        // For custom objects
        public Unity_Object_GameloftRK(Unity_ObjectManager_GameloftRK objManager, int puppetIndex, int animIndex, int paletteIndex, string objectName, int? objectGroupIndex = null) {
            ObjManager = objManager;
            PuppetIndex = puppetIndex;
            AnimIndex = animIndex;
            PaletteIndex = paletteIndex;
            ObjectGroupIndex = objectGroupIndex;
            ObjectName = objectName;
        }

        public Unity_ObjectManager_GameloftRK ObjManager { get; }
        public Gameloft_RK_Level.TrackObject Object { get; set; }
        public Gameloft_RK_Level.TrackObjectInstance Instance { get; set; }
        public Gameloft_RK_Level.ObjectType ObjType { get; set; }

        public int PuppetIndex { get; set; }
        public int AnimIndex { get; set; }

		public override bool IsEditor => ObjectName == "Player";

		public override int? ObjectGroupIndex { get; }


        public override Vector3 Position { get; set; }
        public override short XPosition { get; set; }
        public override short YPosition { get; set; }

        public override string DebugText => $"ObjectType: {Object?.ObjectType}{Environment.NewLine}" +
            $"ObjType: {Instance?.ObjType}{Environment.NewLine}" +
            $"TrackObjIndex: {Instance?.TrackObjectIndex}{Environment.NewLine}";


        public override R1Serializable SerializableData => Object;
        public override ILegacyEditorWrapper LegacyWrapper => new LegacyEditorWrapper(this);

        public override string PrimaryName => $"Type_{Object?.ObjectType.ToString() ?? ObjectName}";
        public override string SecondaryName => PuppetData?.Name;
        public Unity_ObjectManager_GameloftRK.PuppetData PuppetData => ObjManager.Puppets.ElementAtOrDefault(PuppetIndex);

        public override bool FlipHorizontally => Instance?.FlipX ?? false;
        public override bool FlipVertically => false;

        public override bool CanBeLinkedToGroup => false;
		public override bool CanBeLinked => false;
        public string ObjectName { get; set; }

		public override Unity_ObjAnimation CurrentAnimation => PuppetData?.Puppet?.Animations?.ElementAtOrDefault(AnimationIndex ?? -1);
        public override int AnimSpeed => CurrentAnimation?.AnimSpeed ?? 0;
        public override int? GetAnimIndex => OverrideAnimIndex ?? AnimIndex;
        public int PaletteIndex { get; set; } = 0;
        protected override int GetSpriteID => PuppetIndex;
        public override IList<Sprite> Sprites => PuppetData?.Puppet?.Sprites[PaletteIndex];


        private class LegacyEditorWrapper : ILegacyEditorWrapper {
            public LegacyEditorWrapper(Unity_Object_GameloftRK obj) {
                Obj = obj;
            }

            private Unity_Object_GameloftRK Obj { get; }

            public ushort Type {
                get => (ushort)Obj.PuppetIndex;
                set => Obj.PuppetIndex = (short)value;
            }

            public int DES {
                get => Obj.PuppetIndex;
                set => Obj.PuppetIndex = (short)value;
            }

            public int ETA {
                get => Obj.PuppetIndex;
                set => Obj.PuppetIndex = (short)value;
            }

            public byte Etat {
                get => (byte)Obj.PaletteIndex;
                set => Obj.PaletteIndex = value;
            }

            public byte SubEtat {
                get => (byte)Obj.AnimIndex;
                set => Obj.AnimIndex = value;
            }

            public int EtatLength => Obj.PuppetData?.Puppet?.Sprites?.Length ?? 0;
            public int SubEtatLength => Obj.PuppetData?.Puppet?.Animations?.Length ?? 0;

            public byte OffsetBX { get; set; }

            public byte OffsetBY { get; set; }

            public byte OffsetHY { get; set; }

            public byte FollowSprite { get; set; }

            public uint HitPoints { get; set; }

            public byte HitSprite { get; set; }

            public bool FollowEnabled { get; set; }
        }
        #region UI States

        protected int UIStates_PuppetIndex { get; set; } = -2;
        protected override bool IsUIStateArrayUpToDate => PuppetIndex == UIStates_PuppetIndex;

		protected class GameloftRK_UIState : UIState
        {
            public int PaletteIndex { get; set; }

            public GameloftRK_UIState(string displayName, int animIndex, int paletteIndex) : base(displayName, animIndex) {
                PaletteIndex = paletteIndex;   
            }

            public override void Apply(Unity_Object obj)
            {
                ((Unity_Object_GameloftRK)obj).AnimIndex = (short)AnimIndex;
                ((Unity_Object_GameloftRK)obj).PaletteIndex = PaletteIndex;
            }

            public override bool IsCurrentState(Unity_Object obj)
            {
                return AnimIndex == ((Unity_Object_GameloftRK)obj).AnimIndex && PaletteIndex == ((Unity_Object_GameloftRK)obj).PaletteIndex;
            }
        }

        protected override void RecalculateUIStates()
        {
            UIStates_PuppetIndex = PuppetIndex;

            List<UIState> uiStates = new List<UIState>();
            int count = (PuppetData?.Puppet?.Animations?.Length ?? 0);
            int paletteCount = (PuppetData?.Puppet?.Sprites?.Length ?? 1);

            for (int i = 0; i < count; i++) {
                for (int p = 0; p < paletteCount; p++) {
                    uiStates.Add(new GameloftRK_UIState($"Animation {i}-{p}", animIndex: i, p));
                }
            }

            UIStates = uiStates.ToArray();
        }
        
        #endregion
    }
}