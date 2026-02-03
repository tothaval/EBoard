// <copyright file="FluidUIFontModel.cs" company=".">
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

using EBoardSDK.Interfaces.FluidUIFont;
using System;
using System.Text.Json.Serialization;
using System.Windows;

using FontFamily = System.Windows.Media.FontFamily;

/// <summary>
/// Provides properties for FluidUI-Font logic
/// and is the model class for state serialization
/// or deserialization.
/// </summary>
public class FluidUIFontModel : IFluidUIFontModel
{
    private string fontfamilyName = "Times New Roman";
    private FontFamily fontFamily = new FontFamily("Times New Roman");
    private FontWeight fontWeight = FontWeights.Normal;
    private int fontWeightValue = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIFontModel"/> class.
    /// </summary>
    public FluidUIFontModel()
    {
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    /// <inheritdoc/>
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
            }
        }
    }

    /// <inheritdoc/>
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

                this.FontFamily = new FontFamily(this.FontFamilyName);
            }
        }
    }

    /// <inheritdoc/>
    public double FontSize { get; set; } = 15.0;

    /// <inheritdoc/>
    public int FontSizeDisplay => (int)this.FontSize;

    /// <inheritdoc/>
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

                if (this.FontWeightValue != value.ToOpenTypeWeight())
                {
                    this.FontWeightValue = value.ToOpenTypeWeight();
                }
            }
        }
    }

    /// <inheritdoc/>
    public int FontWeightValue
    {
        get
        {
            return this.fontWeightValue;
        }

        set
        {
            if (this.fontWeightValue != value)
            {
                this.fontWeightValue = value;

                if (this.FontWeight.ToOpenTypeWeight() != value)
                {
                    this.FontWeight = FontWeight.FromOpenTypeWeight(value);
                }
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.FontFamily = new FontFamily("Verdana");
        this.FontFamilyName = this.FontFamily.Source;
        this.FontSize = 15.0;
        this.FontWeight = FontWeights.Normal;
        this.FontWeightValue = this.FontWeight.ToOpenTypeWeight();
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.InvokePropertyChangedEvent();
    }

    private void InvokePropertyChangedEvent()
    {
        this.PropertyChangedEvent?.Invoke();
    }
}

// EOF