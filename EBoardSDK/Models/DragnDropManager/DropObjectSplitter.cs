// <copyright file="DropObjectSplitter.cs" company=".">
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
namespace EBoardSDK.Models.DragnDropManager;

using EBoardSDK.Enums;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Windows;

internal class DropObjectSplitter
{
    internal List<EboardFeedbackMessage> ProcessDropFiles(string[] files, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, Point? coords = null, BrushTargets? brushTarget = null)
    {
        var processingResults = new List<EboardFeedbackMessage>();

        foreach (var item in files)
        {
            var fileInfo = new FileInfo(item);

            if (fileInfo.Exists)
            {
                var filetype = new SDKDataManager().FilenameCheck(fileInfo);

                if (string.IsNullOrWhiteSpace(fileInfo.Extension))
                {
                    continue;
                }

                switch (filetype)
                {
                    case SupportedFileTypeCategories.Eboard:
                        // TODO search for active screen and correct model type and insert into free slot,
                        // if none can be found, create new one and insert

                        // TODO extension enum
                        continue;
                    case SupportedFileTypeCategories.FluidUI:
                        var fluidUIresult = new FluidUIDOPManager().ProcessDrop(fileInfo, dropTargetContextAreaViewModel, mainViewModel);
                        processingResults.Add(fluidUIresult);
                        continue;
                    case SupportedFileTypeCategories.Image:
                        var imageresult = new ImageFileDOPManager().ProcessBrushDrop(fileInfo, dropTargetContextAreaViewModel, mainViewModel, brushTarget);
                        processingResults.Add(imageresult);
                        continue;
                    case SupportedFileTypeCategories.Plugin:
                        // TODO if possible and allowed, install plugin and instantiate one instance
                        continue;
                    case SupportedFileTypeCategories.Media:
                        var mediaresult = new MediaFileDOPManager().ProcessDrop(fileInfo, dropTargetContextAreaViewModel, mainViewModel, coords);
                        processingResults.Add(mediaresult);
                        continue;
                    case SupportedFileTypeCategories.Text:
                        var textresult = new TextFileDOPManager().ProcessDrop(fileInfo, dropTargetContextAreaViewModel, mainViewModel, coords);
                        processingResults.Add(textresult);
                        continue;
                    case SupportedFileTypeCategories.Unknown:
                        var linkresult = new LinkDOPManager().ProcessDrop(fileInfo, dropTargetContextAreaViewModel, mainViewModel, coords);
                        processingResults.Add(linkresult);
                        continue;
                    default:
                        continue;
                }
            }

            var result = new LinkDOPManager().ProcessDrop(item, dropTargetContextAreaViewModel, mainViewModel, coords);
            processingResults.Add(result);
        }

        return processingResults;
    }
}

// EOF