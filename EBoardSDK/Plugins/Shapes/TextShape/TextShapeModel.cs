// <copyright file="TextShapeModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Shapes.TextShape;
using EBoardSDK.Models;
using System.Text.Json.Serialization;

<<<<<<< Updated upstream
public class TextShapeModel
{
    [JsonIgnore]
    private TextShapeViewModel textShapeViewModel;
=======
/// <summary>
/// Serializable data model for <see cref="te"/>.
/// </summary>
public class TextShapeModel : ShapeModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeModel"/> class.
    /// </summary>
    public TextShapeModel()
    {
        this.HasStroke = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeModel"/> class.
    /// </summary>
    /// <param name="textShapeViewModel">Desired is the instance that has to be stored.</param>
    public TextShapeModel(TextShapeViewModel textShapeViewModel)
        : base(textShapeViewModel)
    {
        this.TextEntered = textShapeViewModel.TextEntered;
        this.TextString = textShapeViewModel.TextString;

        this.HasStroke = false;
    }
>>>>>>> Stashed changes

    public bool TextEntered { get; set; } = false;

    public string TextString { get; set; } = "enter text";
<<<<<<< Updated upstream

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
=======
>>>>>>> Stashed changes
}

// EOF