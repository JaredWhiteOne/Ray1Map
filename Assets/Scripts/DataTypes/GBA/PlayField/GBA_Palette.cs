﻿namespace R1Engine {
    /// <summary>
    /// Palette block for GBA
    /// </summary>
    public class GBA_Palette : GBA_BaseBlock {
        public uint Length { get; set; }
        public ARGBColor[] Palette { get; set; }

        public override void SerializeBlock(SerializerObject s) {
            Length = s.Serialize<uint>(Length, name: nameof(Length));

            if (s.GameSettings.EngineVersion == EngineVersion.GBA_SplinterCell_NGage) {
                Palette = s.SerializeObjectArray<ARGB1444Color>((ARGB1444Color[])Palette, Length, name: nameof(Palette));
            } else {
                Palette = s.SerializeObjectArray<ARGB1555Color>((ARGB1555Color[])Palette, Length, name: nameof(Palette));
            }
        }
    }
}