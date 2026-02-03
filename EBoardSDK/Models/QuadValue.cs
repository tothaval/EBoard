// <copyright file="QuadValue.cs" company=".">
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
namespace EBoardSDK.Models;

public class QuadValue<T>
{
    public T Value1 { get; set; }

    public T Value2 { get; set; }

    public T Value3 { get; set; }

    public T Value4 { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadValue{T}"/> class.
    /// </summary>
    /// <param name="value"></param>
    public QuadValue(T value)
    {
        this.Value1 = value;
        this.Value2 = value;
        this.Value3 = value;
        this.Value4 = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadValue{T}"/> class.
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="v4"></param>
    public QuadValue(T v1, T v2, T v3, T v4)
    {
        this.Value1 = v1;
        this.Value2 = v2;
        this.Value3 = v3;
        this.Value4 = v4;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadValue{T}"/> class.
    /// </summary>
    public QuadValue()
    {
        this.Value1 = Activator.CreateInstance<T>();
        this.Value2 = Activator.CreateInstance<T>();
        this.Value3 = Activator.CreateInstance<T>();
        this.Value4 = Activator.CreateInstance<T>();
    }

    public void SetValueToAll(T value)
    {
        this.Value1 = value;
        this.Value2 = value;
        this.Value3 = value;
        this.Value4 = value;
    }
}

// EOF