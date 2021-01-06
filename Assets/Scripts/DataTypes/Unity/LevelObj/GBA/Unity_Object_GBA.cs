﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace R1Engine
{
    public class Unity_Object_GBA : Unity_Object
    {
        public Unity_Object_GBA(GBA_Actor actor, Unity_ObjectManager_GBA objManager)
        {
            // Set properties
            Actor = actor;
            ObjManager = objManager;
            InitialXPos = actor.XPos;
            InitialYPos = actor.YPos;
        }

        public GBA_Actor Actor { get; }

        protected short InitialXPos { get; }
        protected short InitialYPos { get; }

        public Unity_ObjectManager_GBA ObjManager { get; }

        public GBA_Action Action => ModelData?.Actions.ElementAtOrDefault(Actor.ActionIndex);
        public Unity_ObjectManager_GBA.ModelData ModelData => ObjManager.ActorModels.ElementAtOrDefault(ActorModelIndex);

        public override ObjectType Type => Actor.Type == GBA_Actor.ActorType.Waypoint ? ObjectType.Waypoint : Actor.Type == GBA_Actor.ActorType.Captor ? ObjectType.Trigger : ObjectType.Object;

        public int ActorModelIndex
        {
            get => Actor.ActorModel == null ? -1 : ObjManager.GraphicsDataLookup.TryGetItem(Actor.Index_ActorModel, -1);
            set 
            {
                if (Actor.ActorModel == null)
                    return;

                if (value != ActorModelIndex) {
                    Actor.ActionIndex = 0;
                    OverrideAnimIndex = null;
                    Actor.Index_ActorModel = (byte)ObjManager.ActorModels[value].Index;
                }
            }
        }


        public override short XPosition
        {
            get => Actor.XPos;
            set
            {
                var change = XPosition - value;
                Actor.XPos = value;

                Actor.BoxMinX = (short)(Actor.BoxMinX - change);
                Actor.BoxMaxX = (short)(Actor.BoxMaxX - change);
            }
        }

        public override short YPosition
        {
            get => Actor.YPos;
            set
            {
                var change = YPosition - value;
                Actor.YPos = value;
                Actor.BoxMinY = (short)(Actor.BoxMinY - change);
                Actor.BoxMaxY = (short)(Actor.BoxMaxY - change);
            }
        }

        public override string DebugText => String.Empty;

        public override IEnumerable<int> Links
        {
            get
            {
                if (Actor.Link_0 != 0xFF)
                    yield return Actor.Link_0;

                if (Actor.Link_1 != 0xFF)
                    yield return Actor.Link_1;

                if (Actor.Link_2 != 0xFF)
                    yield return Actor.Link_2;

                if (Actor.Link_3 != 0xFF)
                    yield return Actor.Link_3;

                if (Actor.CaptorData != null)
                    foreach (var d in Actor.CaptorData.DataEntries)
                    {
                        if (d.Type == 0)
                        {
                            yield return d.LinkedActor;
                        }
                    }

                if (Actor.Milan_Links != null)
                    foreach (var l in Actor.Milan_Links)
                        yield return l.LinkedActor;
            }
        }

        public override R1Serializable SerializableData => Actor;

        public override ILegacyEditorWrapper LegacyWrapper => new LegacyEditorWrapper(this);

        public override bool IsAlways => Actor.Type == GBA_Actor.ActorType.AlwaysActor || Actor.Type == GBA_Actor.ActorType.MainActor;
        public override bool IsEditor => Actor.Type == GBA_Actor.ActorType.Captor || Actor.Type == GBA_Actor.ActorType.Waypoint;
        public override bool CanBeLinked => true;

        public override string PrimaryName
        {
            get
            {
                if (Actor.Type == GBA_Actor.ActorType.Unk)
                    return null;

                if (Actor.Type == GBA_Actor.ActorType.Captor)
                    return $"Captor_{(int)Actor.CaptorID}";

                if (ModelData?.Name != null)
                    return ModelData.Name;

                if (ObjManager.Context.Settings.GBA_IsMilan)
                    return $"Actor";

                return $"ID_{Actor.ActorID}";
            }
        }
        public override string SecondaryName
        {
            get
            {
                if (Actor.Type == GBA_Actor.ActorType.Captor && !ObjManager.Context.Settings.GBA_IsMilan)
                    return $"Trigger ({Actor.CaptorID})";

                if (ObjManager.Context.Settings.Game == Game.GBA_Rayman3)
                    return $"{(GBA_R3_ActorID) Actor.ActorID}";

                return null;
            }
        }

        public override bool FlipHorizontally => Action?.Flags.HasFlag(GBA_Action.ActorStateFlags.HorizontalFlip) ?? false;
        public override bool FlipVertically => Action?.Flags.HasFlag(GBA_Action.ActorStateFlags.VerticalFlip) ?? false;

        private Unity_ObjAnimationCollisionPart[] objCollision;
        private GBA_Action prevAction;
        public override Unity_ObjAnimationCollisionPart[] ObjCollision {
            get {
                if (objCollision == null || prevAction != Action)
                {
                    prevAction = Action;

                    if (Actor.Type == GBA_Actor.ActorType.Captor && !ObjManager.Context.Settings.GBA_IsMilan)
                        objCollision = new Unity_ObjAnimationCollisionPart[]
                        {
                            new Unity_ObjAnimationCollisionPart()
                            {
                                XPosition = Actor.BoxMinX - XPosition,
                                YPosition = Actor.BoxMinY - YPosition,
                                Width = Actor.BoxMaxX - Actor.BoxMinX,
                                Height = Actor.BoxMaxY - Actor.BoxMinY,
                                Type = Actor.CaptorID == GBA_Actor.CaptorType.Player
                                    ? Unity_ObjAnimationCollisionPart.CollisionType.TriggerBox
                                    : Unity_ObjAnimationCollisionPart.CollisionType.HitTriggerBox
                            }
                        };
                    else if (ObjManager.Context.Settings.EngineVersion == EngineVersion.GBA_BatmanVengeance) {
                        objCollision = new Unity_ObjAnimationCollisionPart[]
                        {
                            new Unity_ObjAnimationCollisionPart
                            {
                                XPosition = Action?.Hitbox_X1 ?? 0,
                                YPosition = Action?.Hitbox_Y1 ?? 0,
                                Width = (Action?.Hitbox_X2 - Action?.Hitbox_X1) ?? 0,
                                Height = (Action?.Hitbox_Y2 - Action?.Hitbox_Y1) ?? 0,
                                Type = Unity_ObjAnimationCollisionPart.CollisionType.VulnerabilityBox
                            }
                        };
                        /*if (State != null && State.Flags.HasFlag(GBA_Action.ActorStateFlags.HorizontalFlip)) {
                            objCollision[0].XPosition = -objCollision[0].XPosition;
                        }*/
                    } else
                        objCollision = new Unity_ObjAnimationCollisionPart[0];
                }
                return objCollision;
            }
        }

        public override Unity_ObjAnimation CurrentAnimation => ModelData?.Graphics.Animations.ElementAtOrDefault(AnimationIndex ?? -1);
        public override int AnimSpeed => CurrentAnimation?.AnimSpeed ?? CurrentAnimation?.AnimSpeeds?.ElementAtOrDefault(AnimationFrame) ?? 0;

        public override int? GetAnimIndex => OverrideAnimIndex ?? Action?.AnimationIndex ?? Actor.ActionIndex;
        protected override int GetSpriteID => ActorModelIndex;
        public override IList<Sprite> Sprites => ModelData?.Graphics.Sprites;

        private class LegacyEditorWrapper : ILegacyEditorWrapper
        {
            public LegacyEditorWrapper(Unity_Object_GBA obj)
            {
                Obj = obj;
            }

            private Unity_Object_GBA Obj { get; }

            public ushort Type
            {
                get => Obj.Actor.ActorID;
                set => Obj.Actor.ActorID = (byte)value;
            }

            public int DES
            {
                get => Obj.ActorModelIndex;
                set => Obj.ActorModelIndex = value;
            }

            public int ETA
            {
                get => Obj.ActorModelIndex;
                set => Obj.ActorModelIndex = value;
            }

            public byte Etat { get; set; }

            public byte SubEtat
            {
                get => Obj.Actor.ActionIndex;
                set => Obj.Actor.ActionIndex = value;
            }

            public int EtatLength => 0;
            public int SubEtatLength => Obj.ModelData?.Actions?.Length > 0 ? Obj.ModelData?.Actions?.Length ?? 0 : Obj.ModelData?.Graphics.Animations.Count ?? 0;

            public byte OffsetBX { get; set; }

            public byte OffsetBY { get; set; }

            public byte OffsetHY { get; set; }

            public byte FollowSprite { get; set; }

            public uint HitPoints { get; set; }

            public byte HitSprite { get; set; }

            public bool FollowEnabled { get; set; }
        }


		#region UI States
		protected int UIStates_GraphicsDataIndex { get; set; } = -2;
        protected override bool IsUIStateArrayUpToDate => ActorModelIndex == UIStates_GraphicsDataIndex;

        protected class GBA_UIState : UIState {
            public GBA_UIState(string displayName, byte stateIndex) : base(displayName) {
                StateIndex = stateIndex;
            }
            public GBA_UIState(string displayName, int animIndex) : base(displayName, animIndex) {}

            public byte StateIndex { get; }

            public override void Apply(Unity_Object obj) {
                if (IsState) {
                    (obj as Unity_Object_GBA).Actor.ActionIndex = StateIndex;
                    obj.OverrideAnimIndex = null;
                } else {
                    obj.OverrideAnimIndex = AnimIndex;
                }
            }

            public override bool IsCurrentState(Unity_Object obj) {

                if (obj.OverrideAnimIndex.HasValue)
                    return !IsState && AnimIndex == obj.OverrideAnimIndex;
                else
                    return IsState && StateIndex == (obj as Unity_Object_GBA).Actor.ActionIndex;
            }
        }

        protected override void RecalculateUIStates() {
            UIStates_GraphicsDataIndex = ActorModelIndex;
            var states = ModelData?.Actions;
            var anims = ModelData?.Graphics.Animations;
            HashSet<int> usedAnims = new HashSet<int>();
            List<UIState> uiStates = new List<UIState>();
            if (states != null) {
                for (byte i = 0; i < states.Length; i++) {
                    uiStates.Add(new GBA_UIState($"State {i} (Animation {states[i].AnimationIndex})", stateIndex: i));
                    usedAnims.Add(states[i].AnimationIndex);
                }
            }
            if (anims != null) {
                for (int i = 0; i < anims.Count; i++) {
                    if (usedAnims.Contains(i)) continue;
                    uiStates.Add(new GBA_UIState($"Animation {i}", animIndex: i));
                }
            }

            UIStates = uiStates.ToArray();
        }
		#endregion
	}
}