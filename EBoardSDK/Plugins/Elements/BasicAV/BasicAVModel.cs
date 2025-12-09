namespace EBoardSDK.Plugins.Elements.BasicAV
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class BasicAVModel
    {
        [XmlIgnore]
        private readonly BasicAVMainViewModel basicAVMainViewModel;

        public BasicAVModel()
        {
        }

        public BasicAVModel(BasicAVMainViewModel basicAVMainViewModel)
        {
            this.basicAVMainViewModel = basicAVMainViewModel;

            this.Filename = basicAVMainViewModel.FileName ?? string.Empty;

            this.Filepath = basicAVMainViewModel.Filepath ?? string.Empty;

            this.PlayTimeSpan = basicAVMainViewModel.PlayTimeSpan;

            this.Volume = basicAVMainViewModel.Volume;
        }

        public string Filename { get; set; } = string.Empty;

        public string Filepath { get; set; } = string.Empty;

        public double PlayTimeSpan { get; set; } = 0.0;

        public double Volume { get; set; } = 0.0;
    }
}