﻿namespace R1Engine
{
    public class Common_AnimationPart {
        /// <summary>
        /// The image index from the available sprites
        /// </summary>
        public int ImageIndex { get; set; }

        /// <summary>
        /// The x position
        /// </summary>
        public int XPosition { get; set; }

        /// <summary>
        /// The y position
        /// </summary>
        public int YPosition { get; set; }

        /// <summary>
        /// Indicates if the layer is flipped horizontally
        /// </summary>
        public bool IsFlippedHorizontally { get; set; }

        /// <summary>
        /// Indicates if the layer is flipped vertically
        /// </summary>
        public bool IsFlippedVertically { get; set; }
    }
}