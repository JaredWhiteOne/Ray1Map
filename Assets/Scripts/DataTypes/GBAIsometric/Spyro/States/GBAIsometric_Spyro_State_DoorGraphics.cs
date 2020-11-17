﻿namespace R1Engine
{
    public class GBAIsometric_Spyro_State_DoorGraphics : R1Serializable
    {
        public uint ID { get; set; }
        public ushort AnimationGroupIndex { get; set; }
        public ushort AnimSetIndex { get; set; }
        public byte[] UnkBytes { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ID = s.Serialize<uint>(ID, name: nameof(ID));
            AnimationGroupIndex = s.Serialize<ushort>(AnimationGroupIndex, name: nameof(AnimationGroupIndex));
            AnimSetIndex = s.Serialize<ushort>(AnimSetIndex, name: nameof(AnimSetIndex));
            UnkBytes = s.SerializeArray<byte>(UnkBytes, 8, name: nameof(UnkBytes));
        }
    }
}