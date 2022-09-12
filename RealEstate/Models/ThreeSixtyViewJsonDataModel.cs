using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models
{
    public class ThreeSixtyViewJsonDataModel
    {
        public string OriginalFileName { get; set; }
        public string Title => OriginalFileName;
        public string SceneId { get; set; }
        public string PanoramaUrl { get; set; }
        public string OutputFolder { get; set; }
        public string InputFile { get; set; }
        public string BasePath { get; set; }
        public string Extension { get; set; }
        public int TileResolution { get; set; }
        public int MaxLevel { get; set; }
        public int CubeResolution { get; set; }
        public bool IsMultiResType => CubeResolution > 0 || MaxLevel > 0 || TileResolution > 0;
    }
}