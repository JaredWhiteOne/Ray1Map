﻿using System;
using System.Collections.Generic;
using System.Linq;
using BinarySerializer.PS1;
using UnityEngine;

namespace Ray1Map
{
    public class PS1VRAMAnimation
    {
        public PS1VRAMAnimation(RectInt region, byte[][] frames, int speed, bool pingPong)
        {
            UsesSingleRegion = true;
            Region = region;
            RawFrames = frames;
            Speed = speed;
            PingPong = pingPong;
        }
        public PS1VRAMAnimation(PS1_VRAMRegion region, byte[][] frames, int speed, bool pingPong)
        {
            UsesSingleRegion = true;
            Region = RectIntFromVRAMRegion(region);
            RawFrames = frames;
            Speed = speed;
            PingPong = pingPong;
        }
        public PS1VRAMAnimation(PS1_VRAMRegion region, Func<byte[]>[] frames, int speed, bool pingPong)
        {
            UsesSingleRegion = true;
            Region = RectIntFromVRAMRegion(region);
            GeneratedFrames = frames;
            Speed = speed;
            PingPong = pingPong;
        }
        public PS1VRAMAnimation(RectInt[] regions, byte[][] frames, int speed, bool pingPong)
        {
            UsesSingleRegion = false;
            Regions = regions;
            RawFrames = frames;
            Speed = speed;
            PingPong = pingPong;
        }
        public PS1VRAMAnimation(IEnumerable<PS1_VRAMRegion> regions, byte[][] frames, int speed, bool pingPong)
        {
            UsesSingleRegion = false;
            Regions = regions.Select(RectIntFromVRAMRegion).ToArray();
            RawFrames = frames;
            Speed = speed;
            PingPong = pingPong;
        }
        public PS1VRAMAnimation(PS1_TIM[] timFiles, int speed, bool pingPong)
        {
            UsesSingleRegion = false;
            Regions = timFiles.Select(x => RectIntFromVRAMRegion(x.Region)).ToArray();
            RawFrames = timFiles.Select(x => x.ImgData).ToArray();
            Speed = speed;
            PingPong = pingPong;
        }

        private byte[][] RawFrames { get; }
        private Func<byte[]>[] GeneratedFrames { get; }

        public bool UsesSingleRegion { get; }
        public RectInt Region { get; }
        public RectInt[] Regions { get; }
        public int Speed { get; }
        public bool PingPong { get; }
        private int? _key;
        public int Key => _key ??= Speed | (PingPong ? 1 : 0) << 15 | (FramesLength << 16);

        public int FramesLength => RawFrames?.Length ?? GeneratedFrames.Length;
        public int ActualLength => PingPong ? FramesLength + (FramesLength - 2) : FramesLength;

        private static RectInt RectIntFromVRAMRegion(PS1_VRAMRegion region) => new RectInt(region.XPos * 2, region.YPos, region.Width * 2, region.Height);

        public bool Overlaps(RectInt region)
        {
            if (UsesSingleRegion)
                return Region.Overlaps(region);
            else
                return Regions.Any(x => x.Overlaps(region));
        }

        public byte[] GetFrame(int frame) => RawFrames?[frame] ?? GeneratedFrames[frame]();
    }
}