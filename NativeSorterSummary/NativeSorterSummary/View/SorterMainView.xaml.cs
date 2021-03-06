﻿using NativeSorterSummary.Model;
using NativeSorterSummary.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NativeSorterSummary.View
{
    public partial class SorterMainView : ContentPage
    {
        private List<BuildingViewModel> _buildings = new List<BuildingViewModel>();
        private List<SorterViewModel> _sorters = new List<SorterViewModel>();
        private List<SorterSummaryViewModel> _sorterSummary = new List<SorterSummaryViewModel>();
        public SorterMainView()
        {
            InitializeComponent();
            this.btnGo.Clicked += BtnGo_Clicked;
            this.lblStatus.Text = "Loading...";
            _sorters = new SorterModel().GetList();
            foreach(SorterViewModel item in _sorters)
                this.picker.Items.Add(item.SORTER_NAME);
            this.picker.SelectedIndex = 0;
            this.picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
            this.lblStatus.Text = "Load Complete!";
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.picker.SelectedIndex > -1)
            {
                _sorterSummary = new SorterModel().GetSummary(picker.Items[picker.SelectedIndex]);
            }
        }

        private void BtnGo_Clicked(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Loading...";
            this.lblResult.Text = new SorterModel().GetString();
            this.lblStatus.Text = "Finished";
        }
    }
}
