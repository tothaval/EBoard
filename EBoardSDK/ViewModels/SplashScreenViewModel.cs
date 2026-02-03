// <copyright file="SplashScreenViewModel.cs" company=".">
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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text;
using EBoardSDK.Windows;

/// <summary>
/// This view model serves as data context for the eboard <see cref="SplashScreen"/>,
/// that is shown upon program start and displays program execution progress to the user.
/// </summary>
public partial class SplashScreenViewModel : ObservableObject
{
    private readonly StringBuilder stringBuilder = new ();

    [ObservableProperty]
    private string logMessage = string.Empty;

    [ObservableProperty]
    private string titleMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="SplashScreenViewModel"/> class.
    /// </summary>
    public SplashScreenViewModel()
    {
        this.TitleMessage = "eboard startup screen";

        this.WriteLog($"eboard starts...");
    }

    /// <summary>
    /// Takes in a string and writes it to<see cref="SplashScreenViewModel.LogMessage"/>.
    /// </summary>
    /// <param name="logMessage">The information that should be displayed to the user.</param>
    /// <returns>Returns true if string is not null or empty.</returns>
    public bool WriteLog(string logMessage)
    {
        if (string.IsNullOrWhiteSpace(logMessage))
        {
            return false;
        }

        var time = DateTime.UtcNow;

        this.stringBuilder.AppendLine($"### {time.Year}|{time.Month:#00}|{time.Day:00}:{time.Hour:#.##}:{time.Minute:##}:{time.Second:##}:::\t{logMessage} ###");

        this.LogMessage = this.stringBuilder.ToString();

        return true;
    }
}

// EOF