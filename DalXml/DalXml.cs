﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    sealed class DalXml : IDal
    {
        private static string DataDirectory = @"data";

        private static string configPath = Path.Combine(DataDirectory , @"ConfigXML.xml");
        private static string baseStationsPath = Path.Combine(DataDirectory,  @"BaseStations.xml");
        private static string dronesPath = Path.Combine(DataDirectory, @"Drones.xml");
        private static string parcelsPath = Path.Combine(DataDirectory,  @"Parcels.xml");
        private static string customersPath = Path.Combine(DataDirectory,  @"Customers.xml");
        private static string droneChargesPath = Path.Combine(DataDirectory, @"DroneCharges.xml");

        public static readonly DalXml instance = new DalXml();
        public static DalXml Instance { get => instance; }
        static DalXml() { } // static ctor to ensure instance init is done just before first usage


        //private static string configPath = Path.Combine(solutionDirectory, "DalXML", "data");

        private DalXml()
        {
            DataSource.Initialize();
            //List<DroneCharge> droneCharge = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            //foreach (var item in droneCharge)
            //{
            //    //UpdatePluseChargeSlots(item.StationId);
            //}
            //droneCharge.Clear();
            //XMLTools.SaveListToXmlSerializer(droneCharge, droneChargesPath);
        }

        public double[] GetElectricity()
        {
            var temp1 = XMLTools.LoadListFromXmlElement(configPath);
            var temp2 = temp1.Element("electricityRates").Elements();
            var temp3 = temp2.Select(e => Convert.ToDouble(e.Value)).ToArray();
            return temp3;
            //return XMLTools.LoadListFromXmlElement(Path.Combine(configPath, configFilename)).Element("electricityRates").Elements()
            //    .Select(e => Convert.ToDouble(e.Value)).ToArray();
        }

        #region Get Entity
        public Station GetStation(int requestedId)
        {
            XElement baseStationXml = XMLTools
                .LoadListFromXmlElement(baseStationsPath)
                .Elements().FirstOrDefault(bs => bs.Element("Id").Value == $"{requestedId}");
            return XMLTools.LoadListFromXmlSerializer<Station>(baseStationsPath).Find(bs => bs.Id == requestedId);
        }



        public Drone GetDrone(int requestedId)
        {
            return XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath).Find(drone => drone.Id == requestedId);
        }

        public Customer GetCustomer(int requestedId)
        {
            return XMLTools.LoadListFromXmlSerializer<Customer>(customersPath).Find(customer => customer.Id == requestedId);
        }

        public Parcel GetParcel(int requestedId)
        {
            return XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).Find(parcel => parcel.Id == requestedId);
        }

        public DroneCharge GetDroneCharge(int droneId)
        {
            XElement droneChargeXml = XMLTools
                .LoadListFromXmlElement(droneChargesPath)
                .Elements().FirstOrDefault(dc => dc.Element("DroneId").Value == $"{droneId}");
            return XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath).Find(dc => dc.DroneId == droneId);
        }
        #endregion

        #region Get collections
        public IEnumerable<Station> GetBaseStations()
        {
            XElement baseStationsXML = XMLTools.LoadListFromXmlElement(baseStationsPath);

            List<Station> baseStations = new();
            foreach (var baseStationElement in baseStationsXML.Elements())
            {
                baseStations.Add(
                    new Station()
                    {
                        Id = int.Parse(baseStationElement.Element("Id").Value),
                        AvailableChargeSlots = int.Parse(baseStationElement.Element("AvailableChargeSlots").Value),
                        Name = baseStationElement.Element("Name").Value,
                        Latitude = double.Parse(baseStationElement.Element("Latitude").Value),
                        Longitude = double.Parse(baseStationElement.Element("Longitude").Value),
                    });
            }

            return baseStations;
        }

        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null) =>
            predicate == null ?
               XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath) :
            XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).Where(predicate);

        public IEnumerable<Station> GetStations(Func<Station, bool> predicate = null) =>
            predicate == null ?
                XMLTools.LoadListFromXmlSerializer<Station>(baseStationsPath) :
                XMLTools.LoadListFromXmlSerializer<Station>(baseStationsPath).Where(predicate);

        public IEnumerable<Drone> GetDrones(Func<Drone, bool> predicate = null) =>
            predicate == null ?
                XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath) :
                XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath).Where(predicate);

        public IEnumerable<Customer> GetCustomers(Func<Customer, bool> predicate = null) =>
            predicate == null ?
                XMLTools.LoadListFromXmlSerializer<Customer>(customersPath) :
                XMLTools.LoadListFromXmlSerializer<Customer>(customersPath).Where(predicate);

        public IEnumerable<DroneCharge> GetDroneCharges(Func<DroneCharge, bool> predicate = null) =>
            predicate == null ?
                XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath) :
                XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath).Where(predicate);
        #endregion


        #region Add
        public void AddStation(Station addBaseStation)
        {
            XElement stations = XMLTools.LoadListFromXmlElement(baseStationsPath);
            stations.Add(
                new XElement("Station",
                    new XElement("Id", addBaseStation.Id),
                    new XElement("Name", addBaseStation.Name),
                    new XElement("Longitude", addBaseStation.Longitude),
                    new XElement("Latitude", addBaseStation.Latitude),
                    new XElement("AvailableChargeSlots", addBaseStation.AvailableChargeSlots)
                )
            );
        }

        public void AddDrone(Drone droneDO)
        {
            XElement drones = XMLTools.LoadListFromXmlElement(dronesPath);
            drones.Add(
                new XElement("Drone",
                    new XElement("Id", droneDO.Id),
                    new XElement("MaxWeight", droneDO.MaxWeight),
                    new XElement("Model", droneDO.Model)
                )
            );
        }

        public void AddCustomer(Customer customerDo)
        {
            List<Customer> customers = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (customers.FindIndex(x => x.Id == customerDo.Id) != -1) //this customer exists
                throw new IdAlreadyExistsException("This customer already exists");
            customers.Add(customerDo);
            XMLTools.SaveListToXmlSerializer(customers, customersPath);
        }

        public int AddParcel(Parcel parcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            if (parcels.FindIndex(x => x.Id == parcel.Id) != -1) //this parcel exists
                throw new IdAlreadyExistsException("This parcel already exists");
            parcels.Add(parcel);
            XMLTools.SaveListToXmlSerializer(parcels, parcelsPath);
            return parcel.Id;
        }

        //public void AddDroneCharge(int droneId, int baseStationId)
        //{
        //    XElement droneCharges = XMLTools.LoadListFromXmlElement(droneChargesPath);
        //    droneCharges.Add(
        //        new XElement("DroneCharge",
        //            new XElement("DroneId", droneId),
        //            new XElement("StationId", baseStationId),
        //            new XElement("ChargingTime", DateTime.Now.ToString("O"))
        //            )
        //        );
        //}
        #endregion
        #region Update Functions
        public void UpdateDrone(Drone d)
        {
            XElement drones = XMLTools.LoadListFromXmlElement(dronesPath);
            XElement removeElement = (from dr in drones.Elements()
                                      where dr.Element("Id").Value == $"{d.Id}"
                                      select dr).FirstOrDefault();
            removeElement.Remove();
            drones.Add(
           new XElement("Drone",
               new XElement("Id", d.Id),
               new XElement("MaxWeight", d.MaxWeight),
               new XElement("Model", d.Model)
           )
       );
        }
        public void UpdateCustomer(Customer c)
        {
            XElement customers = XMLTools.LoadListFromXmlElement(customersPath);
            XElement removeElement = (from cr in customers.Elements()
                                      where cr.Element("Id").Value == $"{c.Id}"
                                      select cr).FirstOrDefault();
            removeElement.Remove();
            customers.Add(
           new XElement("Customer",
               new XElement("Id", c.Id),
               new XElement("Name", c.Name),
               new XElement("Phone", c.Phone),
               new XElement("Longitude", c.Longitude),
               new XElement("Latitude", c.Latitude)
           )
       );
        }
        public void UpdateStation(Station s)
        {
            XElement stations = XMLTools.LoadListFromXmlElement(baseStationsPath);
            XElement removeElement = (from st in stations.Elements()
                                      where st.Element("Id").Value == $"{s.Id}"
                                      select st).FirstOrDefault();
            removeElement.Remove();
            stations.Add(
           new XElement("Station",
               new XElement("Id", s.Id),
               new XElement("Name", s.Name),
               new XElement("Longitude", s.Longitude),
               new XElement("Latitude", s.Latitude),
               new XElement("AvailableChargeSlots", s.AvailableChargeSlots)
           )
       );
        }
        #endregion
        #region  Actions on Parcel 
        public void MatchDroneToParcel(Parcel p, Drone d)
        {
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);
            XElement parcel = (from prcl in parcels.Elements()
                               where Convert.ToInt32(prcl.Element("Id").Value) == p.Id
                               select prcl).FirstOrDefault();
            parcel.Element("DroneId").Value = d.Id.ToString();

            XMLTools.SaveListToXmlElement(parcels, parcelsPath);
        }

        public void PickUpParcel(Parcel p)
        {
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);
            XElement parcel = (from prcl in parcels.Elements()
                               where Convert.ToInt32(prcl.Element("Id").Value) == p.Id
                               select prcl).FirstOrDefault();
            parcel.Element("PickedUp").Value = DateTime.Now.ToString("O"); // datetime iso-8601 format

            XMLTools.SaveListToXmlElement(parcels, parcelsPath);
        }

        public void ParcelDelivered(Parcel p)
        {
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);
            XElement parcel = (from prcl in parcels.Elements()
                               where Convert.ToInt32(prcl.Element("Id").Value) == p.Id
                               select prcl).FirstOrDefault();
            parcel.Element("Delivered").Value = DateTime.Now.ToString("O"); // datetime iso-8601 format

            XMLTools.SaveListToXmlElement(parcels, parcelsPath);
        }
        #endregion

        #region  charging drone and release
        /// <summary>
        /// get the drone for charge and the station and update the available charge slots and the list of charge slots
        /// </summary>
        /// <param name="Station s"></param>
        /// <param name="Drone d"></param>
        public void SendDroneToCharge(Station s, Drone d)
        {
            s.AvailableChargeSlots--;
            DroneCharge dc = new DroneCharge()
            {
                DroneId = d.Id,
                StationId = s.Id,
                ChargingTime = DateTime.Now

            };
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            droneCharges.Add(dc);
            XMLTools.SaveListToXmlSerializer<DroneCharge>(droneCharges, droneChargesPath);


            //XElement baseStations = XMLTools.LoadListFromXmlElement(baseStationsPath);
            //XElement baseStation = (from bs in baseStations.Elements()
            //                        where bs.Element("Id").Value == $"{s.Id}"
            //                        select bs).FirstOrDefault();
            //int availableChargingPorts = Convert.ToInt32(baseStation.Element("AvailableChargeSlots").Value);
            //--availableChargingPorts; //update the available charge slots
            //baseStation.Element("AvailableChargeSlots").Value = availableChargingPorts.ToString();

            //XMLTools.SaveListToXmlElement(baseStations, baseStationsPath);

            ////add one drone to the list of drone charges
            //XElement droneCharges = XMLTools.LoadListFromXmlElement(droneChargesPath);
            //droneCharges.Add(
            //    new XElement("DroneCharge",
            //        new XElement("DroneId", d.Id),
            //        new XElement("StationId", s.Id),
            //        new XElement("ChargingTime", DateTime.Now.ToString("O"))
            //        )
            //    );

        }

        public void ReleaseDroneFromCharge(Station s, Drone d)
        {
            XElement baseStations = XMLTools.LoadListFromXmlElement(baseStationsPath);
            XElement baseStation = (from bs in baseStations.Elements()
                                    where bs.Element("Id").Value == $"{s.Id}"
                                    select bs).FirstOrDefault();
            int availableChargingPorts = Convert.ToInt32(baseStation.Element("AvailableChargeSlots").Value);
            ++availableChargingPorts;
            baseStation.Element("AvailableChargeSlots").Value = availableChargingPorts.ToString();

            XMLTools.SaveListToXmlElement(baseStations, baseStationsPath);

            XElement droneCharges = XMLTools.LoadListFromXmlElement(droneChargesPath);
            XElement removeElement = (from dc in droneCharges.Elements()
                                      where dc.Element("DroneId").Value == $"{d.Id}"
                                      select dc).FirstOrDefault();
            removeElement.Remove();
            XMLTools.SaveListToXmlElement(droneCharges, droneChargesPath);
        }
        #endregion
    }
}