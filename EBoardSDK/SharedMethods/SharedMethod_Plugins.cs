// <copyright file="SharedMethod_Plugins.cs" company=".">
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
namespace EBoardSDK.SharedMethods;

using System.IO;
using System.Xml.Serialization;

public class SharedMethod_Plugins
{
    public Task<T>? DeserializeConfigFiles<T>(string filepath)
    {
        var xmlSerializer = new XmlSerializer(typeof(T));

        using (var reader = new StreamReader(filepath))
        {
            try
            {
                var member = (T)xmlSerializer.Deserialize(reader)!;

                if (member != null)
                {
                    return Task.FromResult(member);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }
    }

    public async Task<EBoardFeedbackMessage> SerializeConfigFiles<T>(T dataModel, string targetFilePath)
    {
        try
        {
            // serialize content
            var xmlSerializer = new XmlSerializer(typeof(T));

            await using (var writer = new StreamWriter(targetFilePath))
            {
                xmlSerializer.Serialize(writer, dataModel);
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message, Exception = ex };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"data serialized to: {targetFilePath}" };
    }
}

// EOF