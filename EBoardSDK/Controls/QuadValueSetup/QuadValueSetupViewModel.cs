// <copyright file="QuadValueSetupViewModel.cs" company=".">
// Stephan Kammel
// </copyright>
namespace EBoardSDK.Controls.QuadValueSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Models;

public partial class QuadValueSetupViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private QuadValue<int> quadValue;

    private int all = 0;
    private int topLeft = 0;
    private int topRight = 0;
    private int bottomLeft = 0;
    private int bottomRight = 0;

    private readonly BrushManagement brushManagement;

    public BrushManagement BrushManagement => this.brushManagement;

    private Action okAction;

    public QuadValueSetupViewModel(QuadValue<int> thebeforethickness, Action okResult, BrushManagement brushManagement)
    {
        this.brushManagement = brushManagement;

        this.okAction = okResult;

        this.quadValue = thebeforethickness;

        var c = thebeforethickness;

        if (c.Value1 == c.Value2 && c.Value1 == c.Value3 && c.Value1 == c.Value4)
        {
            this.all = c.Value1;

        }
        else
        {
            var cmid = ((int)c.Value1 + (int)c.Value2 + (int)c.Value3 + (int)c.Value4) / 4;

            this.all = cmid;
        }

        this.topLeft = (int)c.Value1;
        this.topRight = (int)c.Value2;
        this.bottomRight = (int)c.Value3;
        this.BottomLeft = (int)c.Value4; // on purpose, to trigger property changed only once

        this.BrushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    public string QuadValueString => $"{this.QuadValue.Value1},{this.QuadValue.Value2},{this.QuadValue.Value3},{this.QuadValue.Value4}";

    public int All
    {
        get
        {
            return this.all;
        }

        set
        {
            if (this.all != value)
            {
                this.all = value;

                this.topLeft = value;
                this.topRight = value;
                this.bottomRight = value;
                this.bottomLeft = value;

                this.QuadValue = this.BuildThickness(this.All);

                this.OnPropertyChanged(nameof(this.QuadValueString));

                this.OnPropertyChanged(nameof(this.QuadValue));
                this.OnPropertyChanged(nameof(this.All));
                this.OnPropertyChanged(nameof(this.TopLeft));
                this.OnPropertyChanged(nameof(this.TopRight));
                this.OnPropertyChanged(nameof(this.BottomRight));
                this.OnPropertyChanged(nameof(this.BottomLeft));
            }
        }
    }


    public int TopLeft
    {
        get
        {
            return this.topLeft;
        }

        set
        {
            if (this.topLeft != value)
            {
                this.topLeft = value;

                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.TopLeft));
            }
        }
    }


    public int TopRight
    {
        get
        {
            return this.topRight;
        }

        set
        {
            if (this.topRight != value)
            {
                this.topRight = value;
                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.TopRight));
            }
        }
    }


    public int BottomLeft
    {
        get
        {
            return this.bottomLeft;
        }

        set
        {
            if (this.bottomLeft != value)
            {
                this.bottomLeft = value;
                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.BottomLeft));
            }
        }
    }


    public int BottomRight
    {
        get
        {
            return this.bottomRight;
        }

        set
        {
            if (this.bottomRight != value)
            {
                this.bottomRight = value;
                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.BottomRight));
            }
        }
    }

    public void Dispose()
    {
        this.BrushManagement.PropertyChangedEvent -= this.BrushManagement_PropertyChangedEvent;

        GC.SuppressFinalize(this);
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void Ok()
    {
        this.okAction?.Invoke();
    }


    private QuadValue<int> BuildThickness(int v) => new QuadValue<int>(v);

    private QuadValue<int> BuildThickness()
    {
        return new QuadValue<int>(this.TopLeft, this.TopRight, this.bottomRight, this.bottomLeft);
    }
}

// EOF