﻿using ControlSystemMessage.Models;
using ControlSystemMessage.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControlSystemMessage.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SourceListPage : ContentPage
	{
        private MessagesViewModel messagesViewModel;
        private string selectedArea;
        public SourceListPage()
        {
            InitializeComponent();
            messagesViewModel = new MessagesViewModel();
            this.BindingContext = messagesViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData("");
        }
        private async void LoadData(string SearchText)
        {
            if (selectedArea == null) {
                List<MessagesModel> lstAreas = await App.Database.GetMessagesByQuery("SELECT Area from Messages  GROUP BY Area");
                List<string> areas = new List<string>
                {
                    "Select"
                };
                foreach (MessagesModel msg in lstAreas)
                {
                    areas.Add(msg.Area);
                }
                messagesViewModel.Area = areas;

                List<string> System = new List<string>
                {
                    "Select"
                };
                messagesViewModel.System = System;

                Device.BeginInvokeOnMainThread(() =>
                {
                    AreaPicker.SelectedIndex = 0;
                    SystemPicker.SelectedIndex = 0;
                });
                
            //if (lstAreas.Count > 0) {
            //    selectedArea = lstAreas[0].Area;
            //}
            }
            string query = "SELECT msg.* , (Select count(*) from Messages Where Source = msg.Source And Area = msg.Area And IsRead =0) as count from Messages as msg Where (Source LIKE '" + SearchText + "%'  OR NotificationText like '%"+SearchText+"%') GROUP BY Area , Source";
            if (selectedArea != null && !selectedArea.Equals("Select")) {
                query = "SELECT msg.* , (Select count(*) from Messages Where Source = msg.Source And Area = msg.Area And IsRead =0) as count from Messages as msg Where Area ='" + selectedArea + "' And (Source LIKE '" + SearchText + "%' OR NotificationText like '%"+SearchText+"%') GROUP BY Source";
            }
 
            List<MessagesModel> lstMsgs = await App.Database.GetMessagesByQuery(query);
            messagesViewModel.MsgList = new ObservableCollection<MessagesModel>(lstMsgs);
        }
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var SelectedIndex = picker.SelectedIndex;
            if (SelectedIndex != -1)
            {
                selectedArea =  picker.Items[SelectedIndex];
                LoadData("");
            }
            
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Messages item)
            {
                Navigation.PushAsync(new DetailedListPage(item.Source,item.Area));
                ((ListView)sender).SelectedItem = null;
            }
        }
        public void SearchText(string Text) {
            LoadData(Text);
        }
    }
}