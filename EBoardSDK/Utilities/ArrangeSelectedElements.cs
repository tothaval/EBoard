// <copyright file="ArrangeSelectedElements.cs" company=".">
// Stephan Kammel
// </copyright>
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
namespace EBoardSDK.Utilities;

using EBoardSDK.ViewModels;
using System.Linq;
using System.Windows;

/// <summary>
/// TODO: fix selected elements group actions throughout the project.
/// </summary>
public class ArrangeSelectedElements
{

    // auch auslagern, vielleicht in eine Hilfsklasse, die Platzierungen behandelt
    public void ArrangeGroupAsLine(ScreenViewModel eBoardViewModel)
    {
        var elements = eBoardViewModel.Elements;
        var eBoardSettingsViewModel = eBoardViewModel.MainViewModel.ScreenControlViewModel;

        var group = elements.Where(e => e.IsSelected).ToList();

        var selectionArrangementOrigin = new Point(25, 25);

        var rotation = eBoardSettingsViewModel.ArrangementRotationValue;
        var offset = eBoardSettingsViewModel.ArrangementOffsetValue;
        var counter = 1 * group.Count;

        foreach (ElementViewModel item in group)
        {
            //switch (rotation)
            //{
            //    case 0:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)(selectionArrangementOrigin.X + (counter * offset));
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)selectionArrangementOrigin.Y;
            //        break;

            //    case 45:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)(selectionArrangementOrigin.X + (counter * offset));
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(selectionArrangementOrigin.Y + (counter * offset));
            //        break;

            //    case -45:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)(selectionArrangementOrigin.X - (counter * offset));
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(selectionArrangementOrigin.Y + (counter * offset));
            //        break;

            //    case 90:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)selectionArrangementOrigin.X;
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(selectionArrangementOrigin.Y + (counter * offset));
            //        break;

            //    case -90:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)selectionArrangementOrigin.X;
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(selectionArrangementOrigin.Y - (counter * offset));
            //        break;

            //    default:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)selectionArrangementOrigin.X;
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)selectionArrangementOrigin.Y;
            //        break;
            //}

            counter--;
        }
    }

    public void ArrangeGroupAsLine(ScreenViewModel eBoardViewModel, ElementViewModel originElement)
    {
        var elements = eBoardViewModel.Elements;
        var eBoardSettingsViewModel = eBoardViewModel.MainViewModel.ScreenControlViewModel;

        var group = elements.Where(e => e.IsSelected && !e.Equals(originElement)).ToList();

        var rotation = eBoardSettingsViewModel.ArrangementRotationValue;
        var offset = eBoardSettingsViewModel.ArrangementOffsetValue;
        var counter = 1 * group.Count;

        foreach (ElementViewModel item in group)
        {
            //switch (rotation)
            //{
            //    case 0:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition + (counter * (originElement.ElementView.ActualWidth + offset)));

            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition;
            //        break;

            //    case 45:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition + (counter * (originElement.ElementView.ActualWidth + offset)));

            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition + (counter * (originElement.ElementView.ActualHeight + offset)));
            //        break;

            //    case -45:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition - (counter * (originElement.ElementView.ActualWidth + offset)));

            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition + (counter * (originElement.ElementView.ActualHeight + offset)));
            //        break;

            //    case 90:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition;

            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition + (counter * (originElement.ElementView.ActualHeight + offset)));
            //        break;

            //    case -90:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition;

            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = (int)(originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition - (counter * (originElement.ElementView.ActualHeight + offset)));
            //        break;

            //    default:
            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition = originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition;

            //        item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition = originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition;
            //        break;
            //}

            //// draw it again, so that it is correctly arranged as on top
            //originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition--;
            //originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition--;

            //originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.XPosition++;
            //originElement.FluidUIMenuViewModel.FluidUIStandSetupViewModel.YPosition++;

            //counter--;
            //}
        }
    }

    public void ArrangeGroupAsSquare(ScreenViewModel eBoardViewModel, ElementViewModel? originElement = null)
    {
        var elements = eBoardViewModel.Elements;
        var eBoardSettingsViewModel = eBoardViewModel.MainViewModel.ScreenControlViewModel;

        var group = elements.Where(e => e.IsSelected).ToList();

        if (originElement != null)
        {
            group.Remove(originElement);
        }

        var rotation = eBoardSettingsViewModel.ArrangementRotationValue;
        var offset = eBoardSettingsViewModel.ArrangementOffsetValue;
        var counter = 1 * group.Count;

        foreach (ElementViewModel item in group)
        {
        }
    }

    public void ArrangeGroupAsRandomMatrix10x10(ScreenViewModel eBoardViewModel, ElementViewModel? originElement = null)
    {
        var elements = eBoardViewModel.Elements;
        var eBoardSettingsViewModel = eBoardViewModel.MainViewModel.ScreenControlViewModel;

        var group = elements.Where(e => e.IsSelected).ToList();

        if (originElement != null)
        {
            group.Remove(originElement);
        }

        var rotation = eBoardSettingsViewModel.ArrangementRotationValue;
        var offset = eBoardSettingsViewModel.ArrangementOffsetValue;
        var counter = 1 * group.Count;

        foreach (ElementViewModel item in group)
        {
        }
    }
}

// EOF