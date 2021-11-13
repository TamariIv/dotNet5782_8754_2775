using System;

namespace BL
{
    public class BlObject
    {
        IDAL.DO.IDal dal;
        public BlObject()
        {
            dal = new DalObject.DalObject();
        }
    }
}
