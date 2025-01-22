using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EBoard.Plugins.Elements.StandardText
{
    [Serializable]
    public class StandardTextModel
    {
        [XmlIgnore]
        private StandardTextViewModel standardTextViewModel;

        public string Text { get; set; }

        public StandardTextModel()
        {
                
        }

        public StandardTextModel(StandardTextViewModel standardTextViewModel)
        {
            this.standardTextViewModel = standardTextViewModel;

            Text = standardTextViewModel.Text;
        }
    }
}
