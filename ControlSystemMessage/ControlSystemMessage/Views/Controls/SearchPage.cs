﻿using System.Windows.Input;
using Xamarin.Forms;

namespace ControlSystemMessage.Views.Controls
{
    public class SearchPage : ContentPage
    {
        public static readonly BindableProperty SearchPlaceHolderTextProperty = BindableProperty.Create(nameof(SearchPlaceHolderText), typeof(string), typeof(SearchPage), string.Empty);
        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(nameof(SearchText), typeof(string), typeof(SearchPage), string.Empty);
        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(SearchPage));
        public static readonly BindableProperty ShowSearchProperty = BindableProperty.Create(nameof(ShowSearch), typeof(bool), typeof(SearchPage), true);

        public bool ShowSearch
        {
            get
            {
                return (bool)GetValue(ShowSearchProperty);
            }
            set
            {
                SetValue(ShowSearchProperty, value);
                OnPropertyChanged("Page");
            }
        }
        public string SearchPlaceHolderText
        {
            get
            {
                return (string)GetValue(SearchPlaceHolderTextProperty);
            }
            set
            {
                SetValue(SearchPlaceHolderTextProperty, value);
            }
        }

        public string SearchText
        {
            get
            {
                return (string)GetValue(SearchTextProperty);
            }
            set
            {
                SetValue(SearchTextProperty, value);
                var CurrentPage = (App.Current.MainPage as NavigationPage).CurrentPage;
                if (CurrentPage is DetailedListPage)
                {
                    (CurrentPage as DetailedListPage).Search(value);
                }
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return (ICommand)GetValue(SearchCommandProperty);
            }
            set
            {
                SetValue(SearchCommandProperty, value);
            }
        }
    }
}

