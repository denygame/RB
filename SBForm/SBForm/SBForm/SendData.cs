using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBForm
{
    public class SendData
    {
        private string dcBus, dcStation, dcDistance;

        public string DcBus
        {
            get { return dcBus; }
            set { dcBus = value; }
        }

        public string DcStation
        {
            get { return dcStation; }
            set { dcStation = value; }
        }

        public string DcDistance
        {
            get { return dcDistance; }
            set { dcDistance = value; }
        }

        public int DemCombo
        {
            get{return demCombo;}
            set{demCombo = value;}
        }

        private int demCombo;
       
    }
}
