/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RelayCommand 
 * 
 *  keeps Command logic inside a viewmodel
 */


namespace EBoard.Commands
{
    class RelayCommand : BaseCommand
    {

        // event properties & fields
        #region event properties

        private Predicate<object> _CanExecute { get; set; }


        public event EventHandler? CanExecuteChanged;


        private Action<object> _Execute { get; set; }

        #endregion event properties


        // constructors
        #region constructors

        public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod)
        {
            _Execute = ExecuteMethod;
            _CanExecute = CanExecuteMethod;
        }

        #endregion constructors


        // methods
        #region methods

        public bool CanExecute(object? parameter)
        {
            return _CanExecute(parameter);
        }


        public override void Execute(object? parameter)
        {
            _Execute(parameter);
        }

        #endregion methods


    }
}
// EOF