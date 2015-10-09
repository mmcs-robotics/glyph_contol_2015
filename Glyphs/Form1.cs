using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.GlyphRecognition;

namespace Glyphs
{
    public partial class Form1 : Form
    {
        #region Ctor

        // object used for synchronization
        private readonly object _sync = new object();

        private readonly GlyphImageProcessor _imageProcessor;
        private readonly GlyphDatabase _glyphDatabase;

        public Form1()
        {
            InitializeComponent();

            AppGlobals.Form = this;

            var databaseBuilder = new GlyphDatabaseBuilder();
            _glyphDatabase = databaseBuilder.Database;

            _imageProcessor = new GlyphImageProcessor(_glyphDatabase);

            Task.Run(() =>
            {
                var engine = new RobotEngine();
                AppGlobals.Robot = new Robot(engine);
            });
        }

        #endregion

        // Open local video capture device
        private void localVideoCaptureDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new VideoCaptureDeviceForm();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // open it
                OpenVideoSource(form.VideoDevice);
            }
        }

        // Open video source
        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // reset glyph processor
            lock (_sync)
            {
                _imageProcessor.Reset();
            }

            // stop current video source
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();

            // start new video source
            videoSourcePlayer.VideoSource = new AsyncVideoSource(source);
            videoSourcePlayer.Start();

            this.Cursor = Cursors.Default;
        }

        // On new video frame
        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                // convert image to RGB if it is grayscale
                var filter = new GrayscaleToRGB();

                var temp = filter.Apply(image);
                image.Dispose();
                image = temp;
            }

            lock (_sync)
            {
                var glyphs = _imageProcessor.ProcessImage(image);
                if (AppGlobals.Robot != null)
                    RunRobot(glyphs);
            }
        }

        // Robot control
        private int _noActionCount;
        private string _prevAction;

        private void RunRobot(List<ExtractedGlyphData> glyphData)
        {
            if (glyphData.Count == 0)
            {
                if (_noActionCount++ == 5)
                    AppGlobals.Robot.Idle();

                return;
            }

            var action = glyphData[0].RecognizedGlyph.Name;
            if (_prevAction != action)
            {
                AppGlobals.Robot.Idle();
                DoAction(action);
            }

            _prevAction = action;
            _noActionCount = 0;
        }

        private void DoAction(string name)
        {
            switch (name.ToLower())
            {
                case "вперед": 
                    AppGlobals.Robot.Run(1);
                    break;
                case "назад":
                    AppGlobals.Robot.Run(-1);
                    break;
                case "влево":
                    AppGlobals.Robot.Turn(1);
                    break;
                case "вправо":
                    AppGlobals.Robot.Turn(-1);
                    break;
                case "бум":
                    AppGlobals.Robot.Shoot();
                    break;
            }
        }

        // Export glyphs
        private void exportGlyphDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            var imageFormat = ImageFormat.Jpeg;
            var imageWidth = 600;

            var path = fbd.SelectedPath;
            foreach (var glyph in _glyphDatabase)
            {
                var fileName = string.Format("{0}/{1}.{2}", path, glyph.Name, imageFormat);
                glyph.ToImage(imageWidth).Save(fileName, imageFormat);
            }

            // opens the folder in explorer
            Process.Start("explorer.exe", path);
        }

        // Logging
        delegate void AppendLogCallback(string text);
        public void AppendLog(string text)
        {
            if (listBox1.InvokeRequired)
            {
                var d = new AppendLogCallback(AppendLog);
                Invoke(d, text);
            }
            else
            {
                listBox1.Items.Add(text);

                // scroll
                var visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
            }
        }
    }
}
