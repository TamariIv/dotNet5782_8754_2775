using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    sealed class DalXml : IDal
    {
        private readonly string configPath = "dal-config.xml";
        private readonly string baseStationsPath = "BaseStations.xml";
        private readonly string dronesPath = "Drones.xml";
        private readonly string parcelsPath = "Parcels.xml";
        private readonly string customersPath = "Customers.xml";
        private readonly string droneChargesPath = "DroneCharges.xml";
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        static DalXml() { } // static ctor to ensure instance init is done just before first usage
        private DalXml() { }

        public double[] GetElectricity()
        {
            return XMLTools.LoadListFromXmlElement(configPath).Element("GetElectricity").Elements()
                .Select(e => Convert.ToDouble(e.Value)).ToArray();
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
                    new XElement("Latitude",addBaseStation.Latitude),
                    new XElement("AvailableChargeSlots",addBaseStation.AvailableChargeSlots)
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

        public void AddDroneCharge(int droneId, int baseStationId)
        {
            XElement droneCharges = XMLTools.LoadListFromXmlElement(droneChargesPath);
            droneCharges.Add(
                new XElement("DroneCharge",
                    new XElement("DroneId", droneId),
                    new XElement("StationId", baseStationId),
                    new XElement("ChargingTime", DateTime.Now.ToString("O"))
                    )
                );
        }
        #endregion

        #region  Actions on Parcel 
        public void MatchDroneToParcel(Parcel p,Drone d)
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
            XElement baseStations = XMLTools.LoadListFromXmlElement(baseStationsPath);
            XElement baseStation = (from bs in baseStations.Elements()
                                    where bs.Element("Id").Value == $"{s.Id}"
                                    select bs).FirstOrDefault();
            int availableChargingPorts = Convert.ToInt32(baseStation.Element("AvailableChargeSlots").Value);
            --availableChargingPorts; //update the available charge slots
            baseStation.Element("AvailableChargeSlots").Value = availableChargingPorts.ToString();

            XMLTools.SaveListToXmlElement(baseStations, baseStationsPath);

            //add one drone to the list of drone charges
            XElement droneCharges = XMLTools.LoadListFromXmlElement(droneChargesPath);
            droneCharges.Add(
                new XElement("DroneCharge",
                    new XElement("DroneId", d.Id),
                    new XElement("StationId", s.Id),
                    new XElement("ChargingTime", DateTime.Now.ToString("O"))
                    )
                );
        }

        public void ReleaseDroneFromCharge(Station s,Drone d)
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