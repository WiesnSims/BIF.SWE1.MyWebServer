﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyWebServer.Database
{
    class SensorTemperatureDB
    {
        private const string CONNECTION_STRING = "Server=127.0.0.1;Database=sensortemperaturedb;Uid=root;Pwd=;";

        public SensorTemperatureDB()
        {
        }

        public void InitializeDatabase()
        {
            using (MySqlConnection db = new MySqlConnection(CONNECTION_STRING))
            {
                db.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT COUNT(*) FROM Temperatures WHERE YEAR(time) BETWEEN 2008 AND 2018", db);
                int count;
                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    rd.Read();
                    count = rd.GetInt32(0);
                }

                if (count < 10000)
                {
                    //Insert test data:
                    DateTime time = new DateTime(2008, 1, 1, 0, 0, 0);
                    double lastTemp = TemperatureTestData.GetRandomTemperature();

                    while (time < DateTime.Now)
                    {
                        cmd = new MySqlCommand(@"INSERT INTO `Temperatures` (`Time`, `Temperature`) VALUES (@time, @temp)", db);
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.Parameters.AddWithValue("@temp", lastTemp);
                        int rows = cmd.ExecuteNonQuery();

                        //Add 3 hours and generate new random temperature:
                        time = time.AddHours(3);
                        lastTemp = TemperatureTestData.GetRandomTemperature(lastTemp);
                    }
                }
            }
        }

        public Dictionary<DateTime, double> GetTemperaturesOfTimespan(DateTime from, DateTime until)
        {
            var temperatures = new Dictionary<DateTime, double>();

            try
            {
                using (MySqlConnection db = new MySqlConnection(CONNECTION_STRING))
                {
                    db.Open();

                    MySqlCommand cmd = new MySqlCommand(@"SELECT Time, Temperature FROM Temperatures WHERE
                    YEAR(Time) BETWEEN YEAR(@from) AND YEAR(@until) AND
                    MONTH(Time) BETWEEN MONTH(@from) AND MONTH(@until) AND
                    DAY(Time) BETWEEN DAY(@from) AND DAY(@until)
                    ORDER BY Time", db);
                    cmd.Parameters.AddWithValue("@from", from);
                    cmd.Parameters.AddWithValue("@until", until);

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            temperatures.Add(rd.GetDateTime(0), rd.GetDouble(1));
                        }
                    }
                }
            }
            catch
            {
                //Because DB does not exist on jenkins.....
            }

            return temperatures;
        }

        public void InsertTemperature(DateTime time, double temperature)
        {
            try
            {
                using (MySqlConnection db = new MySqlConnection(CONNECTION_STRING))
                {
                    db.Open();
                    MySqlCommand cmd = new MySqlCommand(@"INSERT INTO `Temperatures` (`Time`, `Temperature`) VALUES (@time, @temp)", db);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@temp", temperature);
                    int rows = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                ConsoleWrite.Red("Error inserting testdata.");
            }
        }
    }
}
