// <copyright file="FluidUIDataBlockManager.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIDataBlock;

using EBoardSDK.Controls.FluidUIDataBlock.FluidUIIndexText;
using EBoardSDK.Controls.FluidUIDataBlock.FluidUIKeyText;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.FluidUIDataBlock;
using EBoardSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// This class is tasked with manipulation of <see cref="IFluidUIDataBlockModel"/> instances
/// and has getter and setter methods that get values or apply changes. It requires an
/// instance of a <see cref="FluidUIBaseViewModel"/> and will manipulate the font model
/// within its <see cref="IFluidUIContext"/>.
/// </summary>
internal class FluidUIDataBlockManager : IFluidUIManager
{
    private FluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDataBlockManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUIDataBlockManager(FluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    /// <summary>
    /// Resets FluidUI-DataBlock properties to initial values
    /// and updates EboardFluidUIBaseViewModel.
    /// </summary>
    /// <param name="calledByIFluidUIContextManager"></param>
    public void Reset(bool calledByIFluidUIContextManager = false)
    {
        this.viewModel.FluidUI.DataBlock?.SetInitialValues();

        this.ClearIndexTextList();
        this.ClearKeyTextList();
        this.ClearTextAndTitle();
        this.SetInitialIndexTextQuadValues();
        this.SetInitialKeyTextQuadValues();
        this.SetShowToolTip(true);

        if (!calledByIFluidUIContextManager)
        {
            this.viewModel.SetFluidUIByUser(this.viewModel.FluidUI);
        }

        this.viewModel.UpdateDataBlock();
    }

    internal void ClearIndexTextList()
    {
        this.viewModel.FluidUI.DataBlock?.IndexTextList.Clear();

        this.viewModel.UpdateDataBlock();
    }

    internal void ClearKeyTextList()
    {
        this.viewModel.FluidUI.DataBlock?.KeyTextList.Clear();

        this.viewModel.UpdateDataBlock();
    }

    internal void ClearTextAndTitle()
    {
        this.SetupTitleAndText(string.Empty, string.Empty);
    }

    internal ObservableCollection<FluidUIIndexTextViewModel>? DeleteFluidUIIndexText(FluidUIIndexTextViewModel fluidUIIndexTextViewModel)
    {
        var index = fluidUIIndexTextViewModel.Index;
        var time = fluidUIIndexTextViewModel.Time;
        var text = fluidUIIndexTextViewModel.Text;

        if (this.viewModel.FluidUI.DataBlock != null && this.viewModel.FluidUI.DataBlock.IndexTextList.Any(x => (x.Index == index && x.Time.Equals(time))))
        {
            var model = this.viewModel.FluidUI.DataBlock.IndexTextList.Where(x => (x.Index == index && x.Time.Equals(time))).FirstOrDefault();

            if (model != null)
            {
                this.viewModel.FluidUI.DataBlock.IndexTextList.Remove(model);

                this.viewModel.UpdateDataBlock();

                var fluidUIIndexTexts = new ObservableCollection<FluidUIIndexTextViewModel>();

                foreach (var item in this.viewModel.FluidUI.DataBlock.IndexTextList)
                {
                    fluidUIIndexTexts.Add(new FluidUIIndexTextViewModel(this.viewModel, item));
                }

                return fluidUIIndexTexts;
            }
        }

        return null;
    }

    internal ObservableCollection<FluidUIKeyTextViewModel>? DeleteFluidUKeyText(FluidUIKeyTextViewModel fluidUIIndexTextViewModel)
    {
        var key = fluidUIIndexTextViewModel.Key;
        var time = fluidUIIndexTextViewModel.Time;
        var text = fluidUIIndexTextViewModel.Text;

        if (this.viewModel.FluidUI.DataBlock != null && this.viewModel.FluidUI.DataBlock.KeyTextList.Any(x => (x.Key == key && x.Time.Equals(time))))
        {
            var model = this.viewModel.FluidUI.DataBlock.KeyTextList.Where(x => (x.Key == key && x.Time.Equals(time))).FirstOrDefault();

            if (model != null)
            {
                this.viewModel.FluidUI.DataBlock.KeyTextList.Remove(model);

                this.viewModel.UpdateDataBlock();

                var fluidUIKeyTexts = new ObservableCollection<FluidUIKeyTextViewModel>();

                foreach (var item in this.viewModel.FluidUI.DataBlock.KeyTextList)
                {
                    fluidUIKeyTexts.Add(new FluidUIKeyTextViewModel(this.viewModel, item));
                }

                return fluidUIKeyTexts;
            }
        }

        return null;
    }

    internal FluidUIIndexTextViewModel? GetFluidUIIndexTextViewModel()
    {
        if (this.viewModel.FluidUI.DataBlock != null && this.viewModel.FluidUI.DataBlock.IndexTextList != null && this.viewModel.FluidUI.DataBlock.IndexTextList.Count > 0)
        {
            var fluidtext = this.viewModel.FluidUI.DataBlock.IndexTextList.LastOrDefault();

            if (fluidtext != null)
            {
                return new FluidUIIndexTextViewModel(this.viewModel, fluidtext);
            }
        }

        return null;
    }

    internal List<FluidUIIndexText>? GetIndexTextList()
    {
        return this.viewModel.FluidUI.DataBlock?.IndexTextList;
    }

    internal List<FluidUIKeyText>? GetKeyTextList()
    {
        return this.viewModel.FluidUI.DataBlock?.KeyTextList;
    }

    internal ObservableCollection<FluidUIIndexTextViewModel>? GetFluidUIIndexTextViewModelObservableCollection()
    {
        if (this.viewModel.FluidUI.DataBlock != null && this.viewModel.FluidUI.DataBlock.IndexTextList.Count > 0)
        {
            var collection = new ObservableCollection<FluidUIIndexTextViewModel>();

            foreach (var item in this.viewModel.FluidUI.DataBlock.IndexTextList)
            {
                collection.Add(new FluidUIIndexTextViewModel(this.viewModel, item));
            }

            return collection;
        }

        return null;
    }

    internal FluidUIKeyTextViewModel? GetFluidUIKeyTextViewModel()
    {
        this.AddFluidUIKeyTextToList();

        if (this.viewModel.FluidUI.DataBlock != null && this.viewModel.FluidUI.DataBlock.KeyTextList != null && this.viewModel.FluidUI.DataBlock.KeyTextList.Count > 0)
        {
            var fluidUIKeyText = this.viewModel.FluidUI.DataBlock.KeyTextList.LastOrDefault();

            if (fluidUIKeyText != null)
            {
                return new FluidUIKeyTextViewModel(this.viewModel, fluidUIKeyText);
            }
        }

        return null;
    }

    internal ObservableCollection<FluidUIKeyTextViewModel>? GetFluidUIKeyTextViewModelObservableCollection()
    {
        if (this.viewModel.FluidUI.DataBlock != null && this.viewModel.FluidUI.DataBlock.KeyTextList.Count > 0)
        {
            var collection = new ObservableCollection<FluidUIKeyTextViewModel>();

            foreach (var item in this.viewModel.FluidUI.DataBlock.KeyTextList)
            {
                collection.Add(new FluidUIKeyTextViewModel(this.viewModel, item));
            }

            return collection;
        }

        return null;
    }

    internal int GetNewIndexCount()
    {
        return this.AddFluidUIIndexTextToList();
    }

    internal ObservableCollection<QuadFluidUIIndexTextViewModel> GetIndexTextQuadViewModels()
    {
        var quads = new ObservableCollection<QuadFluidUIIndexTextViewModel>();

        quads.Add(new QuadFluidUIIndexTextViewModel(this.viewModel, QuadValueTargets.A));
        quads.Add(new QuadFluidUIIndexTextViewModel(this.viewModel, QuadValueTargets.B));
        quads.Add(new QuadFluidUIIndexTextViewModel(this.viewModel, QuadValueTargets.C));
        quads.Add(new QuadFluidUIIndexTextViewModel(this.viewModel, QuadValueTargets.D));

        return quads;
    }

    internal ObservableCollection<QuadFluidUIKeyTextViewModel> GetKeyTextQuadViewModels()
    {
        var quads = new ObservableCollection<QuadFluidUIKeyTextViewModel>();

        quads.Add(new QuadFluidUIKeyTextViewModel(this.viewModel, QuadValueTargets.A));
        quads.Add(new QuadFluidUIKeyTextViewModel(this.viewModel, QuadValueTargets.B));
        quads.Add(new QuadFluidUIKeyTextViewModel(this.viewModel, QuadValueTargets.C));
        quads.Add(new QuadFluidUIKeyTextViewModel(this.viewModel, QuadValueTargets.D));

        return quads;
    }

    internal bool GetShowToolTip()
    {
        return this.viewModel.FluidUI.DataBlock?.ShowToolTip ?? true;
    }

    internal string GetText()
    {
        return this.viewModel.FluidUI.DataBlock?.Text ?? "text";
    }

    internal string GetTitle()
    {
        return this.viewModel.FluidUI.DataBlock?.Title ?? "title";
    }

    internal void SetInitialIndexTextQuadValues()
    {
        this.viewModel.FluidUI.DataBlock?.IndexTextQuadValue1.SetValueToAll(new());
        this.viewModel.FluidUI.DataBlock?.IndexTextQuadValue2.SetValueToAll(new());
        this.viewModel.FluidUI.DataBlock?.IndexTextQuadValue3.SetValueToAll(new());
        this.viewModel.FluidUI.DataBlock?.IndexTextQuadValue4.SetValueToAll(new());

        this.viewModel.UpdateDataBlock();
    }

    internal void SetInitialKeyTextQuadValues()
    {
        this.viewModel.FluidUI.DataBlock?.KeyTextQuadValueA.SetValueToAll(new());
        this.viewModel.FluidUI.DataBlock?.KeyTextQuadValueB.SetValueToAll(new());
        this.viewModel.FluidUI.DataBlock?.KeyTextQuadValueC.SetValueToAll(new());
        this.viewModel.FluidUI.DataBlock?.KeyTextQuadValueD.SetValueToAll(new());

        this.viewModel.UpdateDataBlock();
    }

    internal void SetShowToolTip(bool showToolTip)
    {
        if (this.viewModel.FluidUI.DataBlock != null)
        {
            this.viewModel.FluidUI.DataBlock.ShowToolTip = showToolTip;

            this.viewModel.UpdateDataBlock();
        }
    }

    internal void SetText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            text = string.Empty;
        }

        if (this.viewModel.FluidUI.DataBlock != null)
        {
            this.viewModel.FluidUI.DataBlock.Text = text;

            this.viewModel.UpdateDataBlock();
        }
    }

    internal void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = string.Empty;
        }

        if (this.viewModel.FluidUI.DataBlock != null)
        {
            this.viewModel.FluidUI.DataBlock.Title = title;

            this.viewModel.UpdateDataBlock();
        }
    }

    internal void SetupTitleAndText(string title, string text)
    {
        this.SetTitle(title);
        this.SetText(text);
    }

    internal void UndoLastFluidUIChange()
    {
        this.viewModel.ResetFluidUIToPrevious();
    }

    private int AddFluidUIIndexTextToList()
    {
        var index = 0;

        if (this.viewModel.FluidUI.DataBlock != null)
        {
            if (this.viewModel.FluidUI.DataBlock.IndexTextList == null)
            {
                this.viewModel.FluidUI.DataBlock.IndexTextList = new List<FluidUIIndexText>();
            }

            if (this.viewModel.FluidUI.DataBlock.IndexTextList.Count == 0)
            {
                index = this.viewModel.FluidUI.DataBlock.IndexTextList.Count;
            }
            else if (this.viewModel.FluidUI.DataBlock.IndexTextList.Count > 0)
            {
                var lastEntry = this.viewModel.FluidUI.DataBlock.IndexTextList.LastOrDefault();

                if (lastEntry != null)
                {
                    index = lastEntry.Index + 1;
                }
            }

            var indexText = new FluidUIIndexText() { Index = index, Time = DateTime.Now };

            this.viewModel.FluidUI.DataBlock.IndexTextList.Add(indexText);

            this.viewModel.UpdateDataBlock();

            return this.viewModel.FluidUI.DataBlock.IndexTextList.Count();
        }

        return 0;
    }

    private void AddFluidUIKeyTextToList(FluidUIKeyText? fluidUIKeyText = null)
    {
        if (fluidUIKeyText == null)
        {
            fluidUIKeyText = new FluidUIKeyText() { Key = "key", Time = DateTime.Now };
        }

        this.viewModel.FluidUI.DataBlock?.KeyTextList.Add(fluidUIKeyText);

        this.viewModel.UpdateDataBlock();
    }
}

// EOF