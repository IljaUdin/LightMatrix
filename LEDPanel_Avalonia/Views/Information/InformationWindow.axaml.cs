using Avalonia;
using Avalonia.Controls;

namespace LEDPanel_Avalonia.Views.Information
{
    public partial class InformationWindow : Window
    {
        public InformationWindow()
        {
            InitializeComponent();

            this.Topmost = true;

            // ������������� ������� ���� ��� ��� �������������
            this.Opened += (sender, e) =>
            {
                this.Position = new PixelPoint(0, 0);
            };

            // ������� ��������� ����� ����
            this.SystemDecorations = SystemDecorations.None;
        }
    }
}
