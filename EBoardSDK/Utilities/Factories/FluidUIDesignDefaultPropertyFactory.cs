// <copyright file="FluidUIDesignDefaultPropertyFactory.cs" company=".">
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
namespace EBoardSDK.Utilities.Factories;

using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class FluidUIDesignDefaultPropertyFactory
{
    public static SolidColorBrush BackgroundDefaultSolidColorBrush => new SolidColorBrush(Colors.White);

    public static SolidColorBrush ForegroundDefaultSolidColorBrush => new SolidColorBrush(Colors.DarkGray);

    public static SolidColorBrush BorderDefaultSolidColorBrush => new SolidColorBrush(Colors.Black);

    public static SolidColorBrush HighlightDefaultSolidColorBrush => new SolidColorBrush(Colors.DarkGoldenrod);

    public static SolidColorBrush SelectionFallbackDefaultSolidColorBrush => new SolidColorBrush(Colors.WhiteSmoke);

    public static SolidColorBrush TransparentSolidColorBrush => new SolidColorBrush(Colors.Transparent);

    public static double DefaultOpacity => 1.0;

    public static Brush CreateImageBrush(string imagePath)
    {
        var brush = new ImageBrush();

        if (imagePath == null || imagePath == string.Empty)
        {
            return BackgroundDefaultSolidColorBrush;
        }

        try
        {
            brush = new ImageBrush(new BitmapImage(
                new Uri(imagePath, UriKind.Absolute)));
        }
        catch (Exception)
        {
            return BackgroundDefaultSolidColorBrush;
        }

        return brush;
    }

    public static SolidColorBrush GetSolidColorBrush(Color color) => new SolidColorBrush(color);
}

// EOF