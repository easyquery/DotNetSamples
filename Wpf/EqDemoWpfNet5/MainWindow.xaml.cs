using System;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Korzh.DbUtils;

using EasyData.Export;
using EasyData.Export.Excel;
using EasyData.Export.Csv;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Wpf;
using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.EntityFrameworkCore;

using EqDemo.Models;

namespace EqDemo {

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _NavigationFrame.Navigate(new MainPage());
        }

    }
}
