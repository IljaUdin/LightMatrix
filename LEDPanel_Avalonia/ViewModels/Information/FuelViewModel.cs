using LEDPanel_Avalonia.Model;

namespace LEDPanel_Avalonia.ViewModels.Information
{
    internal class FuelViewModel : ViewModelBase
    {
        public MainModel mainModel { get; }
        public TableConfiguration tableConfiguration { get; }

        public FuelViewModel(MainModel mainModel, TableConfiguration tableConfiguration)
        {
            this.mainModel = mainModel;
            this.tableConfiguration = tableConfiguration;
        }
    }
}
