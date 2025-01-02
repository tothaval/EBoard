/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementViewModel 
 * 
 *  view model for ElementView, which offers some basic dragmove functionality,
 *  element placement properties within EBoardView canvas and basic content management
 */

using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using EBoard.Interfaces;
using EBoard.Views;

namespace EBoard.ViewModels
{
    public class ElementViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private ElementView _ElementView;
        public ElementView ElementView => _ElementView;


        private EBoardViewModel _EBoardViewModel;
        public EBoardViewModel EBoardViewModel => _EBoardViewModel;

        private IUIManager _ContentContainer;
        public IUIManager ContentContainer => _ContentContainer;

        private ContentViewModel _ContentViewModel;
        public ContentViewModel ContentViewModel => _ContentViewModel;

        private ShapeViewModel _ShapeViewModel;
        public ShapeViewModel ShapeViewModel => _ShapeViewModel;


        private PlacementManagement _PlacementManagement;
        public PlacementManagement PlacementManager
        {
            get { return _PlacementManagement; }
            set
            {
                _PlacementManagement = value;
                OnPropertyChanged(nameof(PlacementManager));
            }
        }


        private int _RotationAngle;
        public int RotationAngleValue
        {
            get { return _RotationAngle; }
            set
            {
                _RotationAngle = value;

                UpdateRotation(value);

                ChangeSelection_RotationAngleValue(value);

                OnPropertyChanged(nameof(RotationAngleValue));
            }
        }


        private RotateTransform _RotateTransform;
        public RotateTransform RotateTransformValue
        {
            get { return _RotateTransform; }
            set
            {
                _RotateTransform = value;

                _PlacementManagement.Angle = RotationAngleValue;

                OnPropertyChanged(nameof(PlacementManager));
                OnPropertyChanged(nameof(RotateTransformValue));
            }
        }


        private Point _TransformOriginPoint;
        public Point TransformOriginPoint
        {
            get { return _TransformOriginPoint; }
            set
            {
                _TransformOriginPoint = value;
                OnPropertyChanged(nameof(TransformOriginPoint));
            }
        }


        private int _CornerRadius;
        public int CornerRadiusValue
        {
            get { return _CornerRadius; }
            set
            {
                _CornerRadius = value;

                UpdateCornerRadius(value);

                ChangeSelection_CornerRadiusValue(value);

                OnPropertyChanged(nameof(CornerRadiusValue));
            }
        }


        private int _HeightValue;
        public int HeightValue
        {
            get { return _HeightValue; }
            set
            {
                _HeightValue = value;

                TransformOriginPoint = new Point(0, 0);

                if (IsContent && ContentViewModel != null)
                {
                    ContentViewModel.Height = value;
                    OnPropertyChanged(nameof(ContentViewModel));
                }

                if (IsShape && ShapeViewModel != null)
                {
                    ShapeViewModel.Height = value;
                    OnPropertyChanged(nameof(ShapeViewModel));
                }

                ChangeSelection_HeightValue(value);


                OnPropertyChanged(nameof(HeightValue));
            }
        }


        private int _WidthValue;
        public int WidthValue
        {
            get { return _WidthValue; }
            set
            {
                _WidthValue = value;

                TransformOriginPoint = new Point(0, 0);

                UpdateContentWidth(value);

                ChangeSelection_WidthValue(value);

                OnPropertyChanged(nameof(WidthValue));
            }
        }




        private string _EID;
        /// <summary>
        /// Element ID, created upon first creation,
        /// built using $"Element_{DateTime().Ticks}"        
        /// </summary>
        public string EID => _EID;


        private bool _IsShape = false;
        public bool IsShape
        {
            get { return _IsShape; }
            set
            {
                _IsShape = value;
                OnPropertyChanged(nameof(IsShape));
            }
        }


        private bool _IsContent = false;
        public bool IsContent
        {
            get { return _IsContent; }
            set
            {
                _IsContent = value;
                OnPropertyChanged(nameof(IsContent));
            }
        }


        private int _ZIndexValue = 0;
        public int ZIndexValue
        {
            get { return _ZIndexValue; }
            set
            {
                _ZIndexValue = value;

                PlacementManager.Z = value;

                ChangeSelection_ZIndexValue(value);

                OnPropertyChanged(nameof(PlacementManager.Z));
                OnPropertyChanged(nameof(ZIndexValue));
            }
        }


        private int _ZMinimumValue;
        public int ZMinimumValue
        {
            get { return _ZMinimumValue; }
            set
            {
                _ZMinimumValue = value;
                OnPropertyChanged(nameof(ZMinimumValue));
            }
        }


        private int _ZMaximumValue;
        public int ZMaximumValue
        {
            get { return _ZMaximumValue; }
            set
            {
                _ZMaximumValue = value;
                OnPropertyChanged(nameof(ZMaximumValue));
            }
        }


        private double _XPosition;
        public double XPosition
        {
            get { return _XPosition; }
            set
            {
                _XPosition = value;
                OnPropertyChanged(nameof(XPosition));
            }
        }


        private double _YPosition;
        public double YPosition
        {
            get { return _YPosition; }
            set
            {
                _YPosition = value;
                OnPropertyChanged(nameof(YPosition));
            }
        }

        public Point MoveDiff { get; set; }

        #endregion


        #region Commands

        public ICommand ImageCommand { get; }

        public ICommand MoveTenCommand { get; }

        public ICommand ResetBackgroundCommand { get; }


        public ICommand RightClickCommand { get; }

        #endregion


        public ElementViewModel(EBoardViewModel eBoardViewModel)
        {
            IsContent = false;
            IsShape = false;

            //LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);

            ImageCommand = new ImageCommand(this);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            _EBoardViewModel = eBoardViewModel;

            PlacementManager = new PlacementManagement();

            XPosition = PlacementManager.Position.X;
            YPosition = PlacementManager.Position.Y;

            RotationAngleValue = (int)PlacementManager.Angle;
            ZIndexValue = PlacementManager.Z;

            CalibrateZSliderValues(_EBoardViewModel.EBoardDepth);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eBoardViewModel"></param>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        /// <param name="z">depth</param>
        /// <param name="elementHeaderText">element header</param>
        /// <param name="brush">background brush</param>
        /// <param name="control">element content</param>
        public ElementViewModel(
            EBoardViewModel eBoardViewModel,
            ElementDataSet elementDataSet
            )
        {

            IsContent = false;
            IsShape = false;


            //LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);
            ImageCommand = new ImageCommand(this);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            PlacementManager = new PlacementManagement();


            _EBoardViewModel = eBoardViewModel;

            _EID = elementDataSet.EID;

            // need to look into string format again. on implementation, i failed to apply a no digit value to the label stringformat,
            // tried {}{0:F0} and some others, since it didn't work for whatever reason, i made them ints to circumvent the issue for now.
            // better solution for permanent use would be to use doubles and limit the digits on output. gonna try this again sometime, but it has no priority
            HeightValue = (int)elementDataSet.BorderDataSet.Height;
            WidthValue = (int)elementDataSet.BorderDataSet.Width;


            if (elementDataSet.PlacementDataSet != null)
            {
                RotationAngleValue = (int)elementDataSet.PlacementDataSet.Angle;
                PlacementManager.Position = elementDataSet.PlacementDataSet.Position;
                PlacementManager.Z = elementDataSet.PlacementDataSet.Z;

                XPosition = PlacementManager.Position.X;
                YPosition = PlacementManager.Position.Y;

                ZIndexValue = PlacementManager.Z;
            }

            if (_EID == null || _EID.Equals("-1"))
            {
                DateTime dateTime = DateTime.Now;

                _EID = $"Element_{dateTime.Ticks}";
            }


            if (elementDataSet.ElementContent != null)
            {
                if (elementDataSet.ElementContent.ContentIsUserControlAndNotShape)
                {
                    _ContentViewModel = new ContentViewModel(elementDataSet, this);
                    IsContent = true;

                    _ContentContainer = _ContentViewModel;

                    ResetBackgroundCommand = new ResetBackgroundCommand(_ContentViewModel);

                    CornerRadiusValue = (int)elementDataSet.BorderDataSet.CornerRadius.TopLeft;

                    OnPropertyChanged(nameof(ContentViewModel));
                }
                else
                {
                    _ShapeViewModel = new ShapeViewModel(elementDataSet, this);
                    IsShape = true;

                    _ContentContainer = _ShapeViewModel;

                    ResetBackgroundCommand = new ResetBackgroundCommand(_ShapeViewModel);

                    OnPropertyChanged(nameof(ShapeViewModel));
                }


                CalibrateZSliderValues(_EBoardViewModel.EBoardDepth);
            }
        }

        // Methods
        #region Methods
         

        internal void ApplyBackgroundBrush(Brush brush)
        {
            ContentContainer.ApplyBackgroundBrush(brush);
        }


        // in order to apply the value onto every selected element without triggering the value change 
        // and ChangeSelection_VALUE everytime in every element insance, use the ApplyVALUE method 
        public void Apply_CornerRadiusValue(int cornerRadius)
        {
            _CornerRadius = cornerRadius;

            UpdateCornerRadius(cornerRadius);

            OnPropertyChanged(nameof(CornerRadiusValue));
        }        


        private void ChangeSelection_CornerRadiusValue(int cornerRadius)
        {
            _EBoardViewModel.ChangeSelection_CornerRadius(this, cornerRadius);
        }


        public void Apply_HeightValue(int heightValue)
        {
            _HeightValue = heightValue;

            UpdateContentHeight(heightValue);

            OnPropertyChanged(nameof(HeightValue));
        }


        private void ChangeSelection_HeightValue(int heightValue)
        {
            _EBoardViewModel.ChangeSelection_Height(this, heightValue);
        }


        public void ApplyRotationAngleValue(int rotationAngleValue)
        {
            _RotationAngle = rotationAngleValue;

            UpdateRotation(rotationAngleValue);

            OnPropertyChanged(nameof(rotationAngleValue));
        }


        private void ChangeSelection_RotationAngleValue(int rotationAngleValue)
        {
            _EBoardViewModel.ChangeSelection_RotationAngle(this, rotationAngleValue);
        }


        public void ApplyWidthValue(int widthValue)
        {
            _WidthValue = widthValue;

            UpdateContentWidth(widthValue);

            OnPropertyChanged(nameof(WidthValue));
        }


        private void ChangeSelection_WidthValue(int widthValue)
        {
            _EBoardViewModel.ChangeSelection_WidthValue(this, widthValue);
        }


        public void ApplyZIndexValue(int zIndexValue)
        {
            _ZIndexValue = zIndexValue;

            PlacementManager.Z = zIndexValue;

            OnPropertyChanged(nameof(ZIndexValue));
            OnPropertyChanged(nameof(PlacementManager.Z));
        }


        internal void BeginMovement(ElementViewModel elementViewModel)
        {

            XPosition = Canvas.GetLeft(ElementView.VisualParent); // - elementViewModel.ElementView.Position.X;
            YPosition = Canvas.GetTop(ElementView.VisualParent); // - elementViewModel.ElementView.Position.Y;

            //prevZ = PlacementManager.Z;

            //ZIndexValue = 1000;

            OnPropertyChanged(nameof(PlacementManager.Z));

            double xdiff, ydiff;

            xdiff = XPosition - elementViewModel.ElementView.Position.X;
            ydiff = YPosition - elementViewModel.ElementView.Position.Y;

            MoveDiff = new Point(xdiff, ydiff);
        }


        private void ChangeSelection_ZIndexValue(int zIndexValue)
        {
            _EBoardViewModel.ChangeSelection_ZIndex(this, zIndexValue);
        }


        public void CalibrateZSliderValues(int eboardDepth)
        {
            if (eboardDepth >= 0)
            {
                ZMinimumValue = 0;
                ZMaximumValue = eboardDepth;

                if (eboardDepth == 0)
                {
                    ZMaximumValue = 1;
                }
            }
            else if (eboardDepth < 0)
            {
                ZMinimumValue = eboardDepth;
                ZMaximumValue = 0;
            }

            OnPropertyChanged(nameof(ZMinimumValue));
            OnPropertyChanged(nameof(ZMaximumValue));
        }


        public void MoveXY(ElementViewModel elementViewModel, Point newPosition)
        {
            
            if (_ElementView != null)
            {
                //XPosition = Canvas.GetLeft(_ElementView.VisualParent);
                //YPosition = Canvas.GetTop(_ElementView.VisualParent);
                //PlacementManager.Position = new Point(XPosition, YPosition);

                //OnPropertyChanged(nameof(PlacementManager));

                double x, y;

                x = newPosition.X - MoveDiff.X;
                y = newPosition.Y - MoveDiff.Y;


                Canvas.SetLeft(ElementView.VisualParent, x);
                Canvas.SetTop(ElementView.VisualParent, y);

                //MoveDiff = new Point(x, y);
            }

        }


        internal void StopMovement()
        {
            //XPosition = Canvas.GetLeft(_ElementView);
            //YPosition = Canvas.GetTop(_ElementView);

            //PlacementManager.Position = new Point(XPosition, YPosition);

            //ZIndexValue = prevZ;

            //OnPropertyChanged(nameof(PlacementManager));
        }


        private void RemoveElement(object s)
        {
            _EBoardViewModel.RemoveElement(this);
        }


        public void SetView(ElementView elementView)
        {
            _ElementView = elementView;
        }


        private void UpdateContentHeight(int height)
        {
            if (IsContent && ContentViewModel != null)
            {
                ContentViewModel.Height = height;

                OnPropertyChanged(nameof(ContentViewModel));
            }

            if (IsShape && ShapeViewModel != null)
            {
                ShapeViewModel.Height = height;
                OnPropertyChanged(nameof(ShapeViewModel));
            }
        }


        private void UpdateContentWidth(int width)
        {
            if (IsContent && ContentViewModel != null)
            {
                ContentViewModel.Width = width;

                OnPropertyChanged(nameof(ContentViewModel));
            }

            if (IsShape && ShapeViewModel != null)
            {
                ShapeViewModel.Width = width;
                OnPropertyChanged(nameof(ShapeViewModel));
            }
        }


        private void UpdateCornerRadius(int value)
        {
            if (IsContent && ContentViewModel != null)
            {
                ContentViewModel.CornerRadiusValue = value;
            }
        }


        private void UpdateRotation(int rotationAngle)
        {
            RotateTransformValue = new RotateTransform(rotationAngle * -1);

            TransformOriginPoint = new Point(0.5, 0.5);
        }

        internal void WasLastActive()
        {
            _EBoardViewModel.MoveLastClickedElement(this);
        }

        #endregion

    }
}
// EOF