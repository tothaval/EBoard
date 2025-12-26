// <copyright file="FontManagement.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIFont;

using EBoardSDK.Interfaces.FluidUIText;
using System;
using System.Text.Json.Serialization;
using System.Windows;

using FontFamily = System.Windows.Media.FontFamily;

public class FontManagement : IFluidUIFontModel
{
    private string fontfamilyName = "Times New Roman";
    private FontFamily fontFamily = new FontFamily("Times New Roman");
    private double fontSize = 15.0;
    private FontWeight fontWeight = FontWeights.Normal;

    public FontManagement()
    {
    }

    public event Action? PropertyChangedEvent;

    public double FontSize
    {
        get
        {
            return this.fontSize;
        }

        set
        {
            if (this.fontSize != value)
            {
                this.fontSize = value;

                //this.PropertyChangedEvent?.Invoke();
            }
        }
    }

    public string FontFamilyName
    {
        get
        {
            return this.fontfamilyName;
        }

        set
        {
            if (this.fontfamilyName != value)
            {
                this.fontfamilyName = value;

                //this.PropertyChangedEvent?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public FontFamily FontFamily
    {
        get
        {
            return this.fontFamily;
        }

        set
        {
            if (this.fontFamily != value)
            {
                this.fontFamily = value;
                this.FontFamilyName = value.Source;

                //this.PropertyChangedEvent?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public FontWeight FontWeight
    {
        get
        {
            return this.fontWeight;
        }

        set
        {
            if (this.fontWeight != value)
            {
                this.fontWeight = value;
                this.FontWeightValue = value.ToOpenTypeWeight();

                //this.PropertyChangedEvent?.Invoke();
            }
        }
    }

    public int FontWeightValue { get; set; }

    public int FontSizeDisplay => (int)this.FontSize;

    public void Dispose()
    {
        //this.PropertyChangedEvent = null;
    }

    public void SetInitialValues()
    {
        //this.FontFamily = new FontFamily("Verdana");
        //this.FontFamilyName = this.FontFamily.Name;
        //this.FontSize = 15.0;
        //this.FontWeight = default;

        //this.FontWeight = FontWeight.FromOpenTypeWeight(this.FontWeightValue);

        this.FontFamily = new FontFamily(this.FontFamilyName);
    }
}

// EOF