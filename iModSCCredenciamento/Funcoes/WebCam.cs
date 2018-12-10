using System.Drawing;
using WebCam_Capture;
using Image = System.Windows.Controls.Image;

namespace iModSCCredenciamento.Funcoes
{
    class WebCam
    {
        private WebCamCapture webcam;
        private Image _FrameImage;
        private int FrameNumber = 30;
        public void InitializeWebCam(ref Image ImageControl)
        {

            webcam = new WebCamCapture();
            webcam.FrameNumber = 0ul;
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.ImageCaptured += webcam_ImageCaptured;
            _FrameImage = ImageControl;
        }

        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            _FrameImage.Source = Helper.LoadBitmap((Bitmap)e.WebCamImage);
        }

        public void Start()
        {
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.Start(0);
        }

        public void Stop()
        {
            webcam.Stop();
        }

        public void Continue()
        {
            // change the capture time frame
            webcam.TimeToCapture_milliseconds = FrameNumber;

            // resume the video capture from the stop
            webcam.Start(webcam.FrameNumber);
        }

        public void ResolutionSetting()
        {
            webcam.Config();
        }

        public void AdvanceSetting()
        {
            webcam.Config2();
        }

    }
}
