namespace PdfHelpers.Resize
{
    public class PdfScalingOptions
    {
        public static PdfScalingOptions Default = new PdfScalingOptions();
        public static PdfScalingOptions DisableScaling = new PdfScalingOptions() { EnableScaling = false };

        public bool EnableScaling { get; set; } = true;
        public PdfResizeScalingMode PdfContentScalingMode { get; set; } = PdfResizeScalingMode.ScaleAlways;
        public bool MaintainAspectRatio { get; set; } = true;
        public bool EnableDynamicLandscapeOrientation { get; set; } = true;
        public bool EnableDynamicRotationHandling { get; set; } = true;
        public bool EnableContentCentering { get; set; } = true;
        public bool EnableCompression { get; set; } = true;
        public bool EnableUnethicalReading { get; set; } = true;
    }
}
