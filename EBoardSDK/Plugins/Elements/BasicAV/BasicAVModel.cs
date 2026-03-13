// <copyright file="BasicAVModel.cs" company=".">
// Stephan Kammel
// </copyright>
using CommunityToolkit.Mvvm.ComponentModel;

/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may 
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial 
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardSDK.Plugins.Elements.BasicAV;

using System;
using System.Xml.Serialization;

[Serializable]
public class BasicAVModel
{
    [XmlIgnore]
    private readonly BasicAVMainViewModel basicAVMainViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAVModel"/> class.
    /// </summary>
    public BasicAVModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAVModel"/> class.
    /// </summary>
    /// <param name="basicAVMainViewModel"></param>
    public BasicAVModel(BasicAVMainViewModel basicAVMainViewModel)
    {
        this.basicAVMainViewModel = basicAVMainViewModel;

        this.Filename = basicAVMainViewModel.FileName ?? string.Empty;

        this.Filepath = basicAVMainViewModel.Filepath ?? string.Empty;

        this.PlayTimeSpan = basicAVMainViewModel.PlayTimeSpan;

        this.Volume = basicAVMainViewModel.Volume;

        this.ActivateReplay = basicAVMainViewModel.ActivateReplay;
        this.DestructOnEnd = basicAVMainViewModel.DestructOnEnd;
        this.IsGrouped = basicAVMainViewModel.IsGrouped;
        this.ShowDestructToggleButton = basicAVMainViewModel.ShowDestructToggleButton;
        this.ShowGroupToggleButton = basicAVMainViewModel.ShowGroupToggleButton;
        this.UnlinkOnEnd = basicAVMainViewModel.UnlinkOnEnd;
    }

    public string Filename { get; set; } = string.Empty;

    public string Filepath { get; set; } = string.Empty;

    public bool ActivateReplay { get; set; } = false;

    public bool DestructOnEnd { get; set; } = false;

    public bool IsGrouped { get; set; } = false;

    public bool ShowDestructToggleButton { get; set; } = false;

    public bool ShowGroupToggleButton { get; set; } = false;

    public bool UnlinkOnEnd { get; set; } = false;

    public double PlayTimeSpan { get; set; } = 0.0;

    public double Volume { get; set; } = 0.0;
}

// EOF