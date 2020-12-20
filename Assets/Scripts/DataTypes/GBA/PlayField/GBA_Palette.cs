﻿namespace R1Engine {
    /// <summary>
    /// Palette block for GBA
    /// </summary>
    public class GBA_Palette : GBA_BaseBlock {
        public uint MadTrax_Uint_00 { get; set; }
        public uint MadTrax_Uint_04 { get; set; }
        public ushort Length { get; set; }
        public ushort UnknownUshort { get; set; }
        public BaseColor[] Palette { get; set; }

        public override void SerializeBlock(SerializerObject s) 
        {
            if (s.GameSettings.EngineVersion <= EngineVersion.GBA_R3_MadTrax)
                s.Goto(ShanghaiOffsetTable.GetPointer(0));

            Length = s.Serialize<ushort>(Length, name: nameof(Length));
            UnknownUshort = s.Serialize<ushort>(UnknownUshort, name: nameof(UnknownUshort));

            if (s.GameSettings.EngineVersion <= EngineVersion.GBA_R3_MadTrax)
                s.Goto(ShanghaiOffsetTable.GetPointer(1));

            if (s.GameSettings.EngineVersion == EngineVersion.GBA_SplinterCell_NGage) {
                Palette = s.SerializeObjectArray<BGRA4441Color>((BGRA4441Color[])Palette, Length, name: nameof(Palette));
            } else {
                Palette = s.SerializeObjectArray<RGBA5551Color>((RGBA5551Color[])Palette, Length, name: nameof(Palette));
            }
        }

        public override long GetShanghaiOffsetTableLength => 2;
    }
}