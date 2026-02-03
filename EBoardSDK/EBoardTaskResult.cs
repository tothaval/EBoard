// <copyright file="EBoardTaskResult.cs" company=".">
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
namespace EBoardSDK;

/// <summary>
/// This enum displays a spectrum of potential outcomes
/// for tasks that are executed by the eboard prototype.
///
/// Its values are:
/// <see cref="EBoardTaskResult.Success"/> = 0,
/// <see cref="EBoardTaskResult.Failure"/> = 1,
/// <see cref="EBoardTaskResult.Exception"/> = 2,
/// <see cref="EBoardTaskResult.Unknown"/> = 7.
/// </summary>
public enum EBoardTaskResult
{
    /// <summary>
    /// Represents that the task finished with the desired
    /// or expected outcome.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Represents that the task failed somehow, but that
    /// the result is neither unexpected nor undesired.
    /// </summary>
    Failure = 1,

    /// <summary>
    /// Represents an unexpected task execution result that
    /// requires special handling or even fixing.
    /// </summary>
    Exception = 2,

    /// <summary>
    /// Represents an unknown outcome. This can indicate
    /// either missing spectrum values in this enum or
    /// simply that logic forbid it.
    /// </summary>
    Unknown = 7,
}

// EOf