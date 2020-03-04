namespace PdfResizeHelper.Parameters
{
    public class PdfScalingOptions
    {
        public static PdfScalingOptions Default = new PdfScalingOptions();

        public PdfScalingOptions(
            PdfResizeScalingMode scalingMode = PdfResizeScalingMode.ScaleAlways, 
            bool maintainAspectRatio = true, 
            bool enableDynamicLandscapeOrientation = true,
            bool enableContentCentering = true
        )
        {
            PdfContentScalingMode = scalingMode;
            MaintainAspectRatio = maintainAspectRatio;
            EnableDynamicLandscapeOrientation = enableDynamicLandscapeOrientation;
            EnableContentCentering = enableContentCentering;
        }

        public PdfResizeScalingMode PdfContentScalingMode { get; set; }
        public bool MaintainAspectRatio { get; set; }
        public bool EnableDynamicLandscapeOrientation { get; set; }
        public bool EnableContentCentering { get; set; }
    }
}
