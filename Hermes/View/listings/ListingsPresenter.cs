﻿using Hermes.Model;
using Hermes.Model.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Hermes.View.listings
{
    class ListingsPresenter
    {
        private readonly IListingsView _view;
        private readonly ListingRepository _repository;

        public ListingsPresenter(IListingsView view)
        {
            _view = view;
            _repository = new ListingRepository();
        }

        public void GetListings()
        {
            List<Listing> list = _repository.GetListings(0, "creationDate");

            if(list != null && list.Count > 0)
            {
                _view.Listings = list;
            }
        }

        private void GetSortListing(string field)
        {
            List<Listing> list = _repository.GetSortedListings(field);

            if(list != null && list.Count > 0)
            {
                _view.Listings = list;
            }
        }

        public User GetUploader(int id)
        {
            return _repository.GetUploader(id);
        }

        public User GetCurrentUser()
        {
            return (User) MemoryCache.Default["User"];
        }

        public String SortListing(int option)
        {
            switch(option)
            {
                case 1:
                    return "price";
                    break;
                case 2:
                    return "listViews";
                    break;
                default:
                    return "creationDate";
                    break;
            }
        }

        public void AddToFavourites(int listingId)
        {
            User user = GetCurrentUser();

            if(user != null)
            {
                Favourite favourite = new Favourite(listingId, user.Id);

                _repository.AddToFavourite(favourite);
            }
            else
            {
                _view.Navigate = true;
            }
        }

        public void GetSearchResults(string query)
        {
            List<Listing> list = _repository.GetSearchResult(query);

            if (list != null)
            {
                _view.Listings = list;
            }
        }

        public void GetFilteredListings(List<string> catIds, int category, int order)
        {
            List<Listing> list = _repository.FilteredListings(catIds, category, SortListing(order));

            if (list != null)
            {
                _view.Listings = list;
            }
        }

        public void PriceFilteredListings(List<string> catIds, int priceOption, int category, int order)
        {
            switch (priceOption)
            {
                case 1:
                    GetPriceFilteredListings(catIds, "<=", 100, category, SortListing(order));
                    break;
                case 2:
                    GetPriceFilteredListings(catIds, ">", 100, category, SortListing(order));
                    break;
                default:
                    GetPriceFilteredListings(catIds, "=", 0, category, SortListing(order));
                    break;
            }
        }

        public void DynamicPriceFilteredListings(List<string> catIds, float price, int category, int order)
        {
            GetPriceFilteredListings(catIds, ">=", price, category, SortListing(order));
        }

        private void GetPriceFilteredListings(List<string> catIds, string comparisonOperator, float price, int category, string order)
        {
            List<Listing> list = _repository.PriceFilteredListings(catIds, comparisonOperator, price, category, order);

            if (list != null)
            {
                _view.Listings = list;
            }
        }

        public void DateFilteredListings(List<string> catIds, int dateOption, int category, int order)
        {
            switch (dateOption)
            {
                case 1:
                    GetDateFilteredListings(catIds, "MONTH", category, SortListing(order));
                    break;
                case 2:
                    GetDateFilteredListings(catIds, "YEAR", category, SortListing(order));
                    break;
                default:
                    GetDateFilteredListings(catIds, "WEEK", category, SortListing(order));
                    break;
            }
        }

        public void DateAndPriceFilteredListings(List<string> catIds, int priceOption, int dateOption, int category, int order)
        {
            string date = "";

            switch (dateOption)
            {
                case 1:
                    date = "MONTH";
                    break;
                case 2:
                    date = "YEAR";
                    break;
                default:
                    date = "WEEK";
                    break;
            }
             
            switch (priceOption)
            {
                case 1:
                    GetDateAndPriceFilteredListings(catIds, "<=", 100, date, category, SortListing(order));
                    break;
                case 2:
                    GetDateAndPriceFilteredListings(catIds, ">", 100, date, category, SortListing(order));
                    break;
                default:
                    GetDateAndPriceFilteredListings(catIds, "=", 0, date, category, SortListing(order));
                    break;
            }
        }

        private void GetDateAndPriceFilteredListings(List<string> catIds, string comparisonOperator, float price, string dateOption, int category, string order)
        {
            List<Listing> list = _repository.GetDateAndPriceFilteredListings(catIds, comparisonOperator, price, dateOption, category, order);
            
            if (list != null)
            {
                _view.Listings = list;
            }
        }

        private void GetDateFilteredListings(List<string> catIds, string dateOption, int category, string order)
        {
            List<Listing> list = _repository.GetDateFilteredListings(catIds, dateOption, category, order);

            if (list != null)
            {
                _view.Listings = list;
            }
        }

        public void IncreaseView(int id)
        {
            _repository.IncreaseView(id);
        }

        public void AddToHistory(int listingId)
        {
            User user = GetCurrentUser();

            if(user != null)
            {
                _repository.AddToHistory(listingId, user.Id);
            }
        }

        public void ChangeCategory(int category, int order)
        {
            List<Listing> list = _repository.GetListings(category, SortListing(order));

            if (list != null)
            {
                _view.Listings = list;
            }
        }

        public List<SubCategory> GetSubcategoriesFromSpecificCategory(int category)
        {
            if (category != 0)
            {
                List<SubCategory> subCategories = _repository.GetSubcategoriesFromSpecificCategory(category);

                return subCategories;
            }
            return null;
        }
    }
}
