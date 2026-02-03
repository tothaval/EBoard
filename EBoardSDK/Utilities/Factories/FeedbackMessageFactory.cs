// <copyright file="FeedbackMessageFactory.cs" company=".">
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

using EBoardConfigManager.Enums;

public static class FeedbackMessageFactory
{
    public static EboardFeedbackMessage Determine(EBoardTaskResult criteriaMatchOutput, Result criteria, Result compareMe, string? message = null)
    {
        if (criteria.Equals(compareMe))
        {
            return GetMessage(criteriaMatchOutput, message);
        }

        return GetMessage(EBoardTaskResult.Unknown, message);
    }

    public static EboardFeedbackMessage EmptyCallSuccess(string calledFunctionName)
    {
        return GetMessage(EBoardTaskResult.Success, $"empty call of {calledFunctionName}");
    }

    public static EboardFeedbackMessage Exception(string? message = null)
    {
        return GetMessage(EBoardTaskResult.Exception, message);
    }

    public static EboardFeedbackMessage Failure(string? message = null)
    {
        return GetMessage(EBoardTaskResult.Failure, message);
    }

    public static EboardFeedbackMessage Success(string? message = null)
    {
        return GetMessage(EBoardTaskResult.Success, message);
    }

    public static EboardFeedbackMessage Unknown(string? message = null)
    {
        return GetMessage(EBoardTaskResult.Unknown, message);
    }

    private static EboardFeedbackMessage GetMessage(EBoardTaskResult eBoardTaskResult, string? message)
    {
        return new EboardFeedbackMessage()
        {
            TaskResult = eBoardTaskResult,
            ResultMessage = message ?? string.Empty,
        };
    }
}

// EOF