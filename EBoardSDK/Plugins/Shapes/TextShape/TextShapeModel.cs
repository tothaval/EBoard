// <copyright file="TextShapeModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Shapes.TextShape;

using EBoardSDK.Models;
using System.Text.Json.Serialization;

public class TextShapeModel
{
    [JsonIgnore]
    private TextShapeViewModel textShapeViewModel;

    public bool TextEntered { get; set; } = false;

    public string TextString { get; set; } = "enter text";

    public double ScaleX { get; set; } = 1.0;

    public double ScaleY { get; set; } = 1.0;

    public FluidUIContext FluidUIContext { get; set; } = new FluidUIContext();

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeModel"/> class.
    /// </summary>
    public TextShapeModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeModel"/> class.
    /// </summary>
    /// <param name="textShapeViewModel"></param>
    public TextShapeModel(TextShapeViewModel textShapeViewModel)
    {
        this.textShapeViewModel = textShapeViewModel;

        this.TextEntered = textShapeViewModel.TextEntered;
        this.TextString = textShapeViewModel.TextString;
        this.ScaleX = textShapeViewModel.ScaleX;
        this.ScaleY = textShapeViewModel.ScaleY;

        this.FluidUIContext = (FluidUIContext)textShapeViewModel.ViewModel.FluidUI;
    }
}

// EOF