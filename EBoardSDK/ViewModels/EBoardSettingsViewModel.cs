// <copyright file="EBoardSettingsViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class EBoardSettingsViewModel : ObservableObject // so machen das die Settings für das Eboard auch die Oberfläche der SettingsView definieren.
    {
        [ObservableProperty]
        private int arrangementOffsetValue;

        [ObservableProperty]
        private int arrangementRotationValue;

        private EBoardViewModel eBoardViewModel;

        public EBoardSettingsViewModel(EBoardViewModel eBoardViewModel)
        {
            this.eBoardViewModel = eBoardViewModel;
        }

        public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

        // in anderes Modul auslagern, hier einstellen, anderswo das arrangement von selektionen setzen
        // da könnte ich hier alle Settings reinpacken, also auch aus dem eboardbrowser ggf raus kopieren
        [RelayCommand]
        private void ArrangeGroupAsLine()
        {
            this.eBoardViewModel?.ArrangeGroupAsLine();
        }

        [RelayCommand]
        private void ArrangeGroupAsSquare()
        {
            this.eBoardViewModel?.ArrangeGroupAsSquare();
        }

        [RelayCommand]
        private void ArrangeGroupAsRandomMatrix10x10()
        {
            this.eBoardViewModel?.ArrangeGroupAsRandomMatrix10x10();
        }
    }
}

// EOF