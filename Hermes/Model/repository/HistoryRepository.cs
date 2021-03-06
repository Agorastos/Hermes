﻿using Hermes.Model.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
/*
 * History Repository:
 * HistoryRepository class is responsible for 
 * getting every listing history page needs from the database
 */
namespace Hermes.Model
{
    class HistoryRepository
    {
        public List<Listing> GetUserHistory(int userId)
        {
            if (Singleton.GetInstance().OpenConnection() == true)
            {
                string query = "SELECT DISTINCT * FROM View_history VH JOIN Listings L ON L.listingID = VH.ListingID left outer join Listings_Icons on Listings_Icons.listingID=L.listingID WHERE VH.userID=" + userId + " ORDER BY VH.date DESC";

                MySqlCommand cmd = new MySqlCommand(query, Singleton.GetInstance().GetConnection());
                MySqlDataReader dataReader = cmd.ExecuteReader();

                List<Listing> listing = new List<Listing>();

                while (dataReader.Read())
                {
                    Listing tmp = new Listing(dataReader.GetInt32("listingID"), dataReader.GetString("listingName"), dataReader.GetString("listingDescription"), Convert.ToBoolean(dataReader.GetInt32("activeListing")), dataReader.GetInt32("listingRegion"), dataReader.GetInt32("listViews"), dataReader.GetInt32("subCategoryListing"), Convert.ToBoolean(dataReader.GetInt16("premiumListing")), dataReader.GetDateTime("creationDate"), dataReader.GetInt32("price"), dataReader.GetBoolean("types"));

                    if (!dataReader.IsDBNull(15))
                    {
                        byte[] b = (byte[])dataReader.GetValue(15);

                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(b);
                        bitmapImage.EndInit();

                        tmp.Image = bitmapImage;
                    }
                    else
                    {
                        tmp.Image = new BitmapImage(new Uri("pack://application:,,,/error.jpg"));
                    }

                    listing.Add(tmp);
                }

                dataReader.Close();
                dataReader.Dispose();
                cmd.Dispose();

                Singleton.GetInstance().CloseConnection();

                return listing;
            }
            else
            {
                return null;
            }
        }
    }
}
