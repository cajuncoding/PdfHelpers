namespace PdfHelpers.Resize
{
    public class PdfScalingOptions
    {
        public static PdfScalingOptions Default = new PdfScalingOptions();

        public PdfResizeScalingMode PdfContentScalingMode { get; set; } = PdfResizeScalingMode.ScaleAlways;
        public bool MaintainAspectRatio { get; set; } = true;
        public bool EnableDynamicLandscapeOrientation { get; set; } = true;
        public bool EnableDynamicRotationHandling { get; set; }
        public bool EnableContentCentering { get; set; } = true;
    }
}
