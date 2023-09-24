using SCKK_APP_2023.Commands;
using SCKK_APP_2023.Services;
using SCKK_APP_2023.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Microsoft.Extensions.Configuration;

namespace SCKK_APP_2023.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        public ISeries[] Series { get; set; } = new ISeries[]
        {
            new PieSeries<double> { Values = new List<double> { 12 } , Name="Elfogadott", Fill = new SolidColorPaint(SKColors.SeaGreen)},
            new PieSeries<double> { Values = new List<double> { 1 }, Name="Lemondott", Fill = new SolidColorPaint(SKColors.IndianRed) },
            new PieSeries<double> { Values = new List<double> { 5 }, Name="Egyperces", Fill = new SolidColorPaint(SKColors.Yellow) },
        };

        private readonly AccountStore _accountStore;
        private readonly IConfiguration _configuration;

        public string LoginName => _accountStore.CurrentAccount?.LoginName!;

        public HomeViewModel(AccountStore accountStore, IConfiguration configuration)
        {
            _configuration = configuration;
            _accountStore = accountStore;

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(LoginName));
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
            base.Dispose();
        }
    }
}
