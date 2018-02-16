using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace mc21.svr_viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void OnStart(object sender, StartupEventArgs e)
        {
            // это моделька - коллекция объектов класса "TestData"
            Model.Main_Model m = new Model.Main_Model();

            // вью модель
            ViewModel.Main_ViewModel vm = new ViewModel.Main_ViewModel(m);

            // вью
            View.MainView v = new View.MainView();

            // даем окну датаклнтекст
            v.DataContext = vm;

            // отображаем вью
            v.Show();
        }

    }
}
